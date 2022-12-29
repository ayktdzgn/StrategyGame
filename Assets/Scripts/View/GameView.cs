using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour
{
    [SerializeField] InfiniteScroll _infiniteScroll;
    [SerializeField] InformationArea _informationArea;

    [SerializeField] BuildingButton _buildingButtonPrefab;
    GameController _gameController;

    Subscriber<OnSelectEvent<ISelectable>> onObjectSelectSubscriber;

    private void Start()
    {
        CreateBuildingsButton();
    }

    public void Bind(GameController gameController)
    {
        _gameController = gameController;
 
        onObjectSelectSubscriber = new Subscriber<OnSelectEvent<ISelectable>>(gameController.InputController.OnObjectSelected);
        onObjectSelectSubscriber.Publisher.MessagePublisher += GetSelectedObjectData;


    }

    void GetSelectedObjectData(object sender, Message<OnSelectEvent<ISelectable>> e)
    {
        PassInformationAreaData(e.GenericMessage.selectedObject.GetSprite,e.GenericMessage.selectedObject.GetName);
    }

    void PassInformationAreaData(Sprite sprite, string name, IProduct[] products = null)
    {
        _informationArea.gameObject.SetActive(true);
        _informationArea.SetInformationArea(sprite,name,products);
    }

    void CreateBuildingsButton()
    {
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
