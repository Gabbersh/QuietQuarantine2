using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GetIcon : MonoBehaviour
{
    [SerializeField] Object  gameObject;
    Image image;

    void Start()
    {
        Texture2D icon = AssetPreview.GetMiniThumbnail(gameObject);
        Rect rec = new Rect(0, 0 , icon.width, icon.height);    
        Sprite sprite = Sprite.Create(icon, rec, new Vector2(0.5f,0.5f),1); 
        image = GetComponent<Image>();
        image.sprite = sprite;
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
