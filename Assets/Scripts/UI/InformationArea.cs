using Core.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
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
        //Clear button from information area's product section
        public void Flush()
        {
            for (int i = 0; i < _childArr.Length; i++)
            {
                Destroy(_childArr[i].gameObject);
            }
            _childArr = new Transform[0];
        }
        /// <summary>
        /// Set information Sprite and Text, If it has products, instantiate them
        /// </summary>
        /// <param name="selectable">Get selectable for show at Information area</param>
        /// <param name="products">Create new product buttons</param>
        public void SetInformationArea(ISelectable selectable, IProduct[] products = null)
        {
            _informationImage.sprite = selectable.GetSprite;
            _informationName.text = selectable.GetName;
            if (products == null) return;

            _childArr = new Transform[products.Length];
            for (int i = 0; i < products.Length; i++)
            {
                var button = Instantiate(_productButtonPrefab, _productArea);
                button.Product = products[i];
                button.SelectedObject = selectable;

                _childArr[i] = button.transform;
            }
        }
    }
}
