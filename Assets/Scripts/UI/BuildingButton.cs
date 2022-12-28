using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BuildingButton : MonoBehaviour
{
    [SerializeField] Text _text;
    Image _image;


    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void SetButton(Sprite sprite, string name)
    {
        _image.sprite = sprite;
        _text.text = name;
    }
}
