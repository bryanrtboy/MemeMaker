using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakerScript : MonoBehaviour
{
    public RawImage[] m_images;
    public string[] m_paths = new string[]{"Top", "MiddleTop","MiddleBottom", "Bottom"};
    public Object[] textures;
    public Texture2D texture;
    private GameObject go;

    void Start()
    {
        textures = Resources.LoadAll(m_paths[0], typeof(Texture2D));

        foreach (var t in textures)
        {
            Debug.Log(t.name);
        }

        go = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }
    
    public void GenerateRandomCharacter()
    {
        int i = Random.Range(0, textures.Length + 1);
        i -= 1;
        if (i >= 0)
        {
            texture = (Texture2D) textures[i];
        }
        else
        {
            texture = null;
        }
    }
}
