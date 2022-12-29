using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationArea : MonoBehaviour
{
    [SerializeField] Image _informationImage;
    [SerializeField] Text _informationName;
    [SerializeField] Transform _productArea;

    public void SetInformationArea(Sprite sprite, string name, IProduct[] products = null)
    {
        _informationImage.sprite = sprite;
        _informationName.text = name;
    }
}
