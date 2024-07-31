using System;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Cysharp.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;


public class SceneCapture : MonoBehaviour
{
    [SerializeField] private Vector2 _offset;
    [SerializeField] private Camera _camera;
    [SerializeField] private string _filePath;
    [SerializeField] private string _uniqueFileName;
    [SerializeField] private Vector2Int _resoultion;

    [SerializeField] private Vector2Int _iteration;

    [SerializeField] private int _captureDelay = 100;

    [ButtonMethod]
    private void Capture()
    {
        _camera.farClipPlane = 999999f;
        _camera.nearClipPlane = -999999f;
        /*
         * a = resolutions
         * 0.5 : 100 = x : a
         * 100x = 0.5a
         * x = 0.005a
         * a = 1024, x = 5.12
         * */
        _camera.orthographicSize = (float)_resoultion.x * 0.005f;
        _camera.aspect = 1f;
        _camera.transform.position = new Vector3(_offset.x + _camera.orthographicSize,
            _offset.y + -_camera.orthographicSize, -10f);

        RenderTexture renderTexture = _camera.targetTexture;
        _camera.targetTexture = null;
        renderTexture.Release();
        renderTexture.width = _resoultion.x;
        renderTexture.height = _resoultion.y;
        if (renderTexture.Create() == false) Debug.LogError("SceneCapture에서 RenderTexture의 resizing을 실패함.");
        _camera.targetTexture = renderTexture;


        // 에디터가 이미 Play 모드에 있는지 확인합니다
        if (!EditorApplication.isPlaying)
        {
            // Play 모드로 전환합니다
            EditorApplication.isPlaying = true;
        }
    }

    private void Awake()
    {
        UniTask.Create(async () =>
        {
            await UniTask.Delay(300);

            for (int i = 0; i < _iteration.y; i++)
            {
                var backupPos = _camera.transform.position;
                for (int j = 0; j < _iteration.x; j++)
                {
                    await UniTask.Delay(_captureDelay);
                    _camera.transform.position += new Vector3(_camera.orthographicSize * 2f, 0f, 0f);

                    var texture2d = CopyRenderToTexture2D();
                    Gamma2Linear(ref texture2d);
                    SaveCapturedTexture(texture2d, $"Map_{_uniqueFileName}_{j}_{i}");
                }

                _camera.transform.position = backupPos;
                _camera.transform.position += new Vector3(0f, -_camera.orthographicSize * 2f, 0f);
            }


            EditorApplication.isPlaying = false;
        });
    }

    private Texture2D CopyRenderToTexture2D()
    {
        Texture2D texture2D = null;


        RenderTexture renderTexture = _camera.targetTexture;
        RenderTexture originalRenderTexture = _camera.targetTexture;
        _camera.targetTexture = renderTexture;

        RenderTexture.active = renderTexture;
        if (texture2D == null || texture2D.width != renderTexture.width || texture2D.height != renderTexture.height)
        {
            texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        }

        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = null;
        _camera.targetTexture = originalRenderTexture;

        return texture2D;
    }

    private void Gamma2Linear(ref Texture2D texture)
    {
        if (texture == null)
        {
            Debug.LogError("Texture2D is null!");
            return;
        }

        Color[] pixels = texture.GetPixels();

        Color[] linearPixels = new Color[pixels.Length];

        for (int i = 0; i < pixels.Length; i++)
        {
            linearPixels[i] = sRGBToLinear(pixels[i]);
        }

        texture.SetPixels(linearPixels);
        texture.Apply();
    }

    private Color sRGBToLinear(Color color)
    {
        return new Color(
            Mathf.Pow(color.r, 1f / 2.2f),
            Mathf.Pow(color.g, 1f / 2.2f),
            Mathf.Pow(color.b, 1f / 2.2f),
            color.a
        );
    }

    private Texture2D CropTexture(Texture2D original, int x, int y, int width, int height)
    {
        // 원본 텍스처의 지정된 영역에서 픽셀을 가져옴
        Color[] pixels = original.GetPixels(x, y, width, height);

        // 새로운 텍스처를 생성하고, 해당 픽셀을 설정
        Texture2D croppedTexture = new Texture2D(width, height);
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();

        return croppedTexture;
    }

    private void SaveCapturedTexture(Texture2D texture2D, string fileName)
    {
        byte[] pngData = texture2D.EncodeToPNG();

        string target = "Assets";

        var newFilePath = _filePath;
        if (_filePath.StartsWith(target))
        {
            newFilePath = newFilePath.Substring(target.Length);
        }

        string path = Application.dataPath + newFilePath + "\\" + fileName + ".png";
        try
        {
            if (pngData != null)
            {
                File.WriteAllBytes(path, pngData);
                Debug.Log("Texture2D saved as PNG at: " + path);
            }
            else
            {
                Debug.LogError("Failed to encode Texture2D to PNG.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }


    private Color[,] _colors;

    private void OnValidate()
    {
        _camera.orthographicSize = (float)_resoultion.x * 0.005f;

        _colors = new Color[_iteration.x, _iteration.y];

        for (int i = 0; i < _iteration.y; i++)
        {
            for (int j = 0; j < _iteration.x; j++)
            {
                Color color = UnityEngine.Random.ColorHSV();
                color.a = 0.8f;
                _colors[j, i] = color;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        var color = Color.red;
        color.a = 0.5f;
        Gizmos.color = color;
        Gizmos.DrawWireCube(new Vector3(
                _offset.x + _camera.orthographicSize * _iteration.x,
                _offset.y + _camera.orthographicSize * -_iteration.y,
                -10f),
            new Vector3(
                _camera.orthographicSize * _iteration.x * 2f,
                _camera.orthographicSize * _iteration.y * 2f,
                1f
            ));

        Vector3 pos = new Vector3(_offset.x + _camera.orthographicSize, _offset.y + -_camera.orthographicSize, -10f);

        for (int i = 0; i < _iteration.y; i++)
        {
            var backupPos = pos;
            for (int j = 0; j < _iteration.x; j++)
            {
                Gizmos.color = _colors[j, i];
                Gizmos.DrawCube(pos, new Vector3(_camera.orthographicSize * 2f, _camera.orthographicSize * 2f, 1f));
                pos += new Vector3(_camera.orthographicSize * 2f, 0f, 0f);
            }

            pos = backupPos;
            pos += new Vector3(0f, -_camera.orthographicSize * 2f, 0f);
        }
    }
}

#endif