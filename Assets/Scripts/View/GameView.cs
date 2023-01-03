using Core.Controllers;
using Core.Factory;
using Core.Interfaces;
using Core.PublishSubscribe;
using Core.UI;
using Core.Objects;
using UnityEngine;

namespace Core.View
{
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

        //Bind with Game controller
        public void Bind(GameController gameController)
        {
            _gameController = gameController;

            onObjectSelectSubscriber = new Subscriber<OnSelectEvent<ISelectable>>(gameController.InputController.OnObjectSelected);
            onObjectSelectSubscriber.Publisher.MessagePublisher += GetSelectedObjectData;
        }
        //Set information area's active
        private void InformationAreaStatus(bool status)
        {
            _informationArea.gameObject.SetActive(status);
        }
        //If selected object is producer than products datas will pass to information area
        private void GetSelectedObjectData(object sender, Message<OnSelectEvent<ISelectable>> e)
        {
            if (e.GenericMessage.selectedObject == null) { InformationAreaStatus(false); return; }

            IProduct[] products = null;
            if (e.GenericMessage.selectedObject is IProducer producer)
                products = producer.Products;

            PassInformationAreaData(e.GenericMessage.selectedObject, products);
        }
        //Set information area active and cleare previous datas and set new datas
        private void PassInformationAreaData(ISelectable selectable, IProduct[] products = null)
        {
            _informationArea.gameObject.SetActive(true);
            _informationArea.Flush();
            _informationArea.SetInformationArea(selectable, products);
        }
        //Create building buttons to production menu
        private void CreateBuildingsButton()
        {
            var factory = BuildingFactory.Instance;

            for (int i = 0; i < factory.FactorySettingsArr.Length; i++)
            {
                var building = factory.FactorySettingsArr[i].productPrefab;
                var sprite = building.GetComponent<SpriteRenderer>().sprite;
                var button = Instantiate(_buildingButtonPrefab);
                button.SetButton(sprite, building.name);

                _infiniteScroll.AddNewElement(button.transform);
            }
        }
    }
}
