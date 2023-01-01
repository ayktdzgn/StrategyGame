using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            InitializeBuilding();
        });
    }

    public void SetButton(Sprite sprite, string name)
    {
        _image.sprite = sprite;
        _text.text = name;

        _buildingName = name;
    }

    public void InitializeBuilding()
    {
        GameController.Instance.BuildingController.CarryBuilding(_buildingName);
    }
}
