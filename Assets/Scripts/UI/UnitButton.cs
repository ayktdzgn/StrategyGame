using Core.Interfaces;
using UnityEngine;

namespace Core.UI
{
    public class UnitButton : ProductButton
    {
        private void Start()
        {
            _button.onClick.AddListener(() =>
            {
                if (SelectedObject == null) { Debug.Log("Selected object null"); return; }
                if (SelectedObject is IProducer)
                {
                    ((IProducer)SelectedObject).Produce(Product);
                }
            });
        }
    }
}
