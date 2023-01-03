using Core.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Text))]
    public class ProductButton : MonoBehaviour
    {
        [SerializeField] protected Text _productName;
        protected Button _button;
        protected Image _image;
        protected string _name;

        private IProduct _product;
        private ISelectable _selectedObject;

        public string Name
        {
            get => _name;
            set
            {
                _productName.text = value;
                _name = value;
            }
        }

        public ISelectable SelectedObject { get => _selectedObject; set => _selectedObject = value; }
        public IProduct Product
        {
            get => _product;
            set
            {
                _product = value;
                Name = value.GetName;
                SetProductImage(value.GetSprite);
            }
        }

        private void Awake()
        {
            _image = GetComponent<Image>();
            _button = GetComponent<Button>();
        }

        //Set Button's image
        private void SetProductImage(Sprite sprite)
        {
            _image.sprite = sprite;
        }
    }
}
