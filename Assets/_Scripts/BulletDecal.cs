using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDecal : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        Sprite sprite = sprites[Random.Range(0, sprites.Length - 1)];
        
        var croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        var pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                (int)sprite.textureRect.y,
                                                (int)sprite.textureRect.width,
                                                (int)sprite.textureRect.height);
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();
        rend.material.SetTexture("_MainTex", croppedTexture);
    }
}
