using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureManager : MonoBehaviour
{
    [SerializeField] private GameObject _cubeModelPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Texture2D _baseTexture;
    private Color[] _basePixels;
    private Texture2D _tempTexture;


    private void Awake()
    {
        _tempTexture = Convert2DTexture();
    }

    private void Start()
    {
        GetColorPixels();
    }

    private Texture2D Convert2DTexture()
    {
        Texture mainTexture = _baseTexture;
        Texture2D texture2D = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);

        RenderTexture currentRT = RenderTexture.active;

        RenderTexture renderTexture = new RenderTexture(mainTexture.width, mainTexture.height, 32);
        Graphics.Blit(mainTexture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();
        _basePixels = texture2D.GetPixels();
        return texture2D;
    }

    private void GetColorPixels()
    {
        int _textureSizeX = _tempTexture.width;
        int _textureSizeY = _tempTexture.height;
        //Debug.Log(_textureSizeX);
        //Debug.Log(_textureSizeY);
        int index;
        for (int x = 0; x < _textureSizeX; x++)
        {
            for (int y = 0; y < _textureSizeY; y++)
            {
                index = x * _textureSizeY + y;
                Debug.Log(_basePixels[index].grayscale);
                if(_basePixels[index].grayscale != 0)
                {
                    GenerateModel(_basePixels[index], new Vector3(_spawnPoint.transform.position.x + y, _spawnPoint.transform.position.y + x, 0f));
                }
            }
        }
    }

    private void GenerateModel(Color _color,Vector3 _objectPosition)
    {
         var tempObject = Instantiate(_cubeModelPrefab, _objectPosition , transform.rotation);
        tempObject.GetComponent<MeshRenderer>().material.color = _color;
    }


}
