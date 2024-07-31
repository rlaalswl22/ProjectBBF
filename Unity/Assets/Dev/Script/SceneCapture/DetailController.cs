using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using MyBox;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DetailController : MonoBehaviour
{
    [SerializeField] private string _detailsPath;
    

    private const string PATRTERN = @"^MapDetail_\d+_\d+\.png$";
    
    

    [ButtonMethod]
    private void ApplyDetails()
    {
        var sprites = LoadSprites();
        var doubleList = SortSprites(sprites);
        
        
        
    }

    private List<List<Sprite>> SortSprites(List<Sprite> list)
    {
        Regex regex = new Regex(PATRTERN);

        OrderedDictionary dict = new OrderedDictionary();

        foreach (var sprite in list)
        {
            Match match = regex.Match(sprite.name);
            if (match.Success == false) continue;
            if (match.Groups.Count != 2) continue;

            int y = Convert.ToInt32(match.Groups[1]);
            int x= Convert.ToInt32(match.Groups[0]);
            
            if (dict.Contains(y) == false)
            {
                var targetList = new List<(int, Sprite)>();
                dict.Add(y,targetList);
                targetList.Add((x, sprite));
            }
            else
            {
                var targetList = dict[y] as  List<(int, Sprite)>;
                targetList.Add((x, sprite));;
            }
        }

        foreach (List<(int, Sprite)> targetList  in dict.Values)
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
        var loadedObj = AssetDatabase.LoadAllAssetsAtPath(_detailsPath);
        Regex regex = new Regex(PATRTERN);

        List<Sprite> sprites = new List<Sprite>(10);
        
        foreach (var obj in loadedObj)
        {
            if (obj is not Sprite sprite) continue;
            if(regex.IsMatch(obj.name) == false) continue;
            
            sprites.Add(sprite);
        }

        return sprites;
    }
}
