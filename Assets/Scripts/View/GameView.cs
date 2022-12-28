using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour
{
    [SerializeField] InfiniteScroll _infiniteScroll;
    [SerializeField] BuildingButton _buildingButtonPrefab;
    GameController _gameController;

    private void Start()
    {
        CreateBuildingsButton();
    }

    public void Bind(GameController gameController)
    {
        _gameController = gameController;
    }

    void CreateBuildingsButton()
    {
        //List<string> buildings = new List<string>(factoryController.BuildingFactory.PoolDic.Keys);
        //for (int i = 0; i < buildings.Count; i++)
        //{
        //    string building = buildings[0];

        //}
        var factory = BuildingFactory.Instance;

        for (int i = 0; i < factory.FactorySettingsArr.Length; i++)
        {
            var building = factory.FactorySettingsArr[i].productPrefab;
            var sprite = building.GetComponent<SpriteRenderer>().sprite;
            var button = Instantiate(_buildingButtonPrefab);
            button.SetButton(sprite , building.name);

            _infiniteScroll.AddNewElement(button.transform);
        }

    }
}
