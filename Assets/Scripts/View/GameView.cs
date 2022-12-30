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
        _informationArea.gameObject.SetActive(false);
    }

    public void Bind(GameController gameController)
    {
        _gameController = gameController;
 
        onObjectSelectSubscriber = new Subscriber<OnSelectEvent<ISelectable>>(gameController.InputController.OnObjectSelected);
        onObjectSelectSubscriber.Publisher.MessagePublisher += GetSelectedObjectData;


    }

    void InformationAreaStatus(bool status)
    {
        _informationArea.gameObject.SetActive(status);
    }

    void GetSelectedObjectData(object sender, Message<OnSelectEvent<ISelectable>> e)
    {
        if(e.GenericMessage.selectedObject == null) { InformationAreaStatus(false); return; }

        IProduct[] products = null;
        if (e.GenericMessage.selectedObject is IProductable)
            products = ((IProductable)e.GenericMessage.selectedObject).Products;

        PassInformationAreaData(e.GenericMessage.selectedObject.GetSprite,e.GenericMessage.selectedObject.GetName, products);
    }

    void PassInformationAreaData(Sprite sprite, string name, IProduct[] products = null)
    {
        _informationArea.gameObject.SetActive(true);
        _informationArea.Flush();
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
