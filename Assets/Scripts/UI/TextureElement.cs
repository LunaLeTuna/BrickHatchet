using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class TextureElement : MonoBehaviour
{
    // public TMP_Text Name;
    // public TMP_Text Timestamp;
    // public TMP_Text Path;
    public GameObject texturelib;
    public Button[] ElementButton;

    public Button buttoninst;

    //public Texture2D importedImage;

    private void GetTextures(){

        string url = SettingsManager.Settings.KEProjectPath;

        string[] files = System.IO.Directory.GetFiles(url+"/textures", "*", System.IO.SearchOption.AllDirectories);

        foreach (string file in files){
            Debug.Log(file);

            Button newbutton = Instantiate(buttoninst);
            byte[] bytes = System.IO.File.ReadAllBytes(file);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);

            newbutton.image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f,0.5f));

            newbutton.transform.SetParent(texturelib.transform, true);
        }
    }

    private void Start() {
        GetTextures();
    }
}
