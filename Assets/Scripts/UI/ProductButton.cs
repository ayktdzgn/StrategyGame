using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Text))]
public class ProductButton : MonoBehaviour
{
    [SerializeField] Text _productName;
    Image _image;
    string _name;

    public string Name {
        get => _name;
        set {
            _productName.text = value; 
            _name = value; }
    }

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void SetProductImage(Sprite sprite)
    {
        _image.sprite = sprite;
    }
}
