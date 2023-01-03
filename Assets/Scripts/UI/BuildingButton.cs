using Core.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Button))]
    public class BuildingButton : MonoBehaviour
    {
        [SerializeField] Text _text;
        Image _image;
        string _buildingName;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }
        //AddListener to button for spawn building from factory
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                InitializeBuilding();
            });
        }
        /// <summary>
        /// Set button image and text name
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="name"></param>
        public void SetButton(Sprite sprite, string name)
        {
            _image.sprite = sprite;
            _text.text = name;

            _buildingName = name;
        }
        //Spawn building from factory and carry it for plant
        public void InitializeBuilding()
        {
            GameController.Instance.BuildingController.CarryBuilding(_buildingName);
        }
    }
}
