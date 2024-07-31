using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using MyBox;
using Unity.VisualScripting;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public class DetailController : MonoBehaviour
{
    [Serializable]
    public class Item1
    {
        public List<Item2> List = new List<Item2>();
    }

    [Serializable]
    public class Item2
    {
        public SpriteRenderer Renderer;
        public int X;
        public int Y;
    }

    [SerializeField] private string _detailsPath;
    [SerializeField] private string _mapName;

    [SerializeField] private Vector2Int _resolution;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector2Int _iteration;
    [field: SerializeField, SortingLayer] private int _sortingLayer;
    [SerializeField] private int _order;

    [SerializeField] private List<Item1> _rendererList;

    [ButtonMethod]
    private void ApplyDetails()
    {
        var sprites = LoadSprites();
        var doubleList = SortSprites(sprites);

        InitDetailRenderer(doubleList);
        
        Debug.Log("디테일 로드 성공");
    }

    private void InitDetailRenderer(List<List<Sprite>> list)
    {
        if (_rendererList != null)
        {
            _rendererList.ForEach(x => x.List.ForEach(y =>
            {
                if (y.Renderer)
                {
                    DestroyImmediate(y.Renderer.gameObject);
                }
            }));
        }

        _rendererList = new List<Item1>();

        foreach (var inList in list)
        {
            var rendererList = new Item1();
            _rendererList.Add(rendererList);

            foreach (var sprite in inList)
            {
                var matchCollection = Regex.Matches(sprite.name, @"\d+");
                if (matchCollection.Count != 2) continue;

                int y = Convert.ToInt32(matchCollection[1].Value);
                int x = Convert.ToInt32(matchCollection[0].Value);

                var renderer = new GameObject(sprite.name).AddComponent<SpriteRenderer>();
                renderer.sortingOrder = _order;
                renderer.sortingLayerID = _sortingLayer;
                renderer.transform.SetParent(transform);
                renderer.sprite = sprite;
                rendererList.List.Add(new Item2()
                {
                    Renderer = renderer,
                    X = x,
                    Y = y
                });

                renderer.transform.position = new Vector3(
                                                  _resolution.x* x, -_resolution.y * y, 0f) * 0.005f * 2f +
                                              _offset +
                                              new Vector3(_resolution.x, -_resolution.y, 0f) * 0.005f;
            }
        }
    }

    private List<List<Sprite>> SortSprites(List<Sprite> list)
    {
        OrderedDictionary dict = new OrderedDictionary();

        foreach (var sprite in list)
        {
            var matchCollection = Regex.Matches(sprite.name, @"\d+");
            if (matchCollection.Count != 2) continue;

            int y = Convert.ToInt32(matchCollection[1].Value);
            int x = Convert.ToInt32(matchCollection[0].Value);

            if (dict.Contains(y) == false)
            {
                var targetList = new List<(int, Sprite)>();
                dict.Add(y, targetList);
                targetList.Add((x, sprite));
            }
            else
            {
                var targetList = dict[y] as List<(int, Sprite)>;
                targetList.Add((x, sprite));
            }
        }

        foreach (List<(int, Sprite)> targetList in dict.Values)
        {
            targetList.Sort((x, y) =>
            {
                if (x.Item1 > y.Item1) return -1;
                if (x.Item1 < y.Item1) return 1;

                return 0;
            });
        }

        var newList = new List<List<Sprite>>();

        foreach (List<(int, Sprite)> targetList in dict.Values)
        {
            newList.Add(targetList.Select(x => x.Item2).ToList());
        }

        return newList;
    }

    private List<Sprite> LoadSprites()
    {
        List<Sprite> sprites = new List<Sprite>(10);

        for (int i = 0; i < _iteration.y; i++)
        {
            for (int j = 0; j < _iteration.x; j++)
            {
                var obj = AssetDatabase.LoadAssetAtPath<Texture2D>(_detailsPath + $"/Detail_{_mapName}_{j}_{i}.png");
                if (obj is null) continue;

                var spr = ConvertTextureToSprite(obj);
                spr.name = $"Detail_{j}_{i}";
                sprites.Add(spr);
            }
        }

        return sprites;
    }

    private Sprite ConvertTextureToSprite(Texture2D texture)
    {
        // Texture2D를 Sprite로 변환
        return Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );
    }
}
#endif