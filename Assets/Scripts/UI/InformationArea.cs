using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationArea : MonoBehaviour
{
    [SerializeField] Image _informationImage;
    [SerializeField] Text _informationName;
    [SerializeField] Transform _productArea;
    [SerializeField] ProductButton _productButtonPrefab;
    Transform[] _childArr = { };

    private void OnDisable()
    {
        Flush();
    }

    public void Flush()
    {
        for (int i = 0; i < _childArr.Length; i++)
        {
            Destroy(_childArr[i].gameObject);
        }
        _childArr = new Transform[0];
    }

    public void SetInformationArea(Sprite sprite, string name, IProduct[] products = null)
    {
        _informationImage.sprite = sprite;
        _informationName.text = name;
        if (products == null) return;

        _childArr = new Transform[products.Length];
        for (int i = 0; i < products.Length; i++)
        {
            var button = Instantiate(_productButtonPrefab, _productArea);
            button.Name = products[i].GetName;
            button.SetProductImage(products[i].GetSprite);

            _childArr[i] = button.transform;
        }
    }
}
