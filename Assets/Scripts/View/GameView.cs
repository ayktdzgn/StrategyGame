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

    private void InformationAreaStatus(bool status)
    {
        _informationArea.gameObject.SetActive(status);
    }

    private void GetSelectedObjectData(object sender, Message<OnSelectEvent<ISelectable>> e)
    {
        if(e.GenericMessage.selectedObject == null) { InformationAreaStatus(false); return; }

        IProduct[] products = null;
        if (e.GenericMessage.selectedObject is IProducer)
            products = ((IProducer)e.GenericMessage.selectedObject).Products;

        PassInformationAreaData(e.GenericMessage.selectedObject, products);
    }

    private void PassInformationAreaData(ISelectable selectable, IProduct[] products = null)
    {
        _informationArea.gameObject.SetActive(true);
        _informationArea.Flush();
        _informationArea.SetInformationArea(selectable,products);
    }

    private void CreateBuildingsButton()
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
