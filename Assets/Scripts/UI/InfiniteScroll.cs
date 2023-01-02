using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class InfiniteScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IScrollHandler
    {
        //Scroll Rect
        [SerializeField] private float _itemSpacing;
        [SerializeField] private float _horizontalMargin, _verticalMargin;
        [SerializeField] private float _outOfBoundsThreshold;

        ScrollRect _scrollRect;
        RectTransform _contentRectTransform;
        RectTransform[] _rtChildren;
        private float _width, _height;
        private float _childWidth, _childHeight;

        Vector2 _lastDragPosition;
        bool _isPositiveDrag;

        private void Awake()
        {
            _scrollRect = GetComponent<ScrollRect>();
            _contentRectTransform = GetComponent<RectTransform>();

            _contentRectTransform = _scrollRect.content;
            _scrollRect.movementType = ScrollRect.MovementType.Unrestricted;

            if (_scrollRect.horizontal && _scrollRect.vertical)
            {
                throw new System.InvalidOperationException("InfiniteScroll can not be vertical and horizontal at the same time!");
            }
        }

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            _rtChildren = new RectTransform[_contentRectTransform.childCount];

            for (int i = 0; i < _contentRectTransform.childCount; i++)
            {
                _rtChildren[i] = _contentRectTransform.GetChild(i) as RectTransform;
            }

            // Subtract the margin from both sides.
            _width = _contentRectTransform.rect.width - (2 * _horizontalMargin);

            // Subtract the margin from the top and bottom.
            _height = _contentRectTransform.rect.height - (2 * _verticalMargin);

            //If object count less than view size, increase object size
            if ((_height / _rtChildren.Length) >= _rtChildren[0].rect.height + 2 * _itemSpacing)
            {
                for (int i = 0; i < _rtChildren.Length; i++)
                {
                    var size = (_height / _rtChildren.Length) - 2 * _itemSpacing;
                    _rtChildren[i].sizeDelta = new Vector2(size, size);
                }
            }

            _childWidth = _rtChildren[0].rect.width;
            _childHeight = _rtChildren[0].rect.height;

            if (_scrollRect.vertical)
                InitializeContentVertical();
            else
                InitializeContentHorizontal();
        }

        public void AddNewElement(Transform element)
        {
            element.SetParent(_contentRectTransform);
            Init();
        }

        private void InitializeContentHorizontal()
        {
            float originX = 0 - (_width * 0.5f);
            float posOffset = _childWidth * 0.5f;
            for (int i = 0; i < _rtChildren.Length; i++)
            {
                Vector2 childPos = _rtChildren[i].localPosition;
                childPos.x = originX + posOffset + i * (_childWidth + _itemSpacing);
                _rtChildren[i].localPosition = childPos;
            }
        }

        private void InitializeContentVertical()
        {
            float originY = _height / 2;
            float posOffset = _childHeight * 0.5f;

            for (int i = 0; i < _rtChildren.Length; i++)
            {
                Vector2 childPos = _rtChildren[i].localPosition;
                childPos = Vector2.zero;
                childPos.y = originY - (posOffset - _itemSpacing) - i * (_childHeight + 2 * _itemSpacing);
                _rtChildren[i].localPosition = childPos;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _lastDragPosition = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_scrollRect.vertical)
            {
                _isPositiveDrag = eventData.position.y > _lastDragPosition.y;
            }
            else if (_scrollRect.horizontal)
            {
                _isPositiveDrag = eventData.position.x > _lastDragPosition.x;
            }

            _lastDragPosition = eventData.position;
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (_scrollRect.vertical)
            {
                _isPositiveDrag = eventData.scrollDelta.y > 0;
            }
            else
            {
                _isPositiveDrag = eventData.scrollDelta.y < 0;
            }
        }

        public void OnViewScroll()
        {
            if (_scrollRect.vertical)
            {
                HandleVerticalScroll();
            }
            else
            {
                HandleHorizontalScroll();
            }
        }

        private void HandleVerticalScroll()
        {
            int currItemIndex = _isPositiveDrag ? 0 : _scrollRect.content.childCount - 1;
            var currItem = _scrollRect.content.GetChild(currItemIndex);

            if (!ReachedThreshold(currItem))
            {
                return;
            }

            int endItemIndex = _isPositiveDrag ? _scrollRect.content.childCount - 1 : 0;
            Transform endItem = _scrollRect.content.GetChild(endItemIndex);
            Vector2 newPos = endItem.position;

            if (_isPositiveDrag)
            {
                newPos.y = endItem.position.y - _childHeight - 2 * _itemSpacing;
            }
            else
            {
                newPos.y = endItem.position.y + _childHeight + 2 * _itemSpacing;
            }

            currItem.position = newPos;
            currItem.SetSiblingIndex(endItemIndex);
        }

        private void HandleHorizontalScroll()
        {
            int currItemIndex = _isPositiveDrag ? _scrollRect.content.childCount - 1 : 0;
            var currItem = _scrollRect.content.GetChild(currItemIndex);
            if (!ReachedThreshold(currItem))
            {
                return;
            }

            int endItemIndex = _isPositiveDrag ? 0 : _scrollRect.content.childCount - 1;
            Transform endItem = _scrollRect.content.GetChild(endItemIndex);
            Vector2 newPos = endItem.position;

            if (_isPositiveDrag)
            {
                newPos.x = endItem.position.x - _childWidth * 1.5f + _itemSpacing;
            }
            else
            {
                newPos.x = endItem.position.x + _childWidth * 1.5f - _itemSpacing;
            }

            currItem.position = newPos;
            currItem.SetSiblingIndex(endItemIndex);
        }

        private bool ReachedThreshold(Transform item)
        {
            if (_scrollRect.vertical)
            {
                float posYThreshold = transform.position.y + _height * 0.5f + _outOfBoundsThreshold - (_childHeight / 2);
                float negYThreshold = transform.position.y - _height * 0.5f - _outOfBoundsThreshold + (_childHeight / 2);
                return _isPositiveDrag ? item.position.y - _childHeight * 0.5f > posYThreshold :
                    item.position.y + _childWidth * 0.5f < negYThreshold;
            }
            else
            {
                float posXThreshold = transform.position.x + _width * 0.5f + _outOfBoundsThreshold - (_childWidth / 2);
                float negXThreshold = transform.position.x - _width * 0.5f - _outOfBoundsThreshold + (_childWidth / 2);
                return _isPositiveDrag ? item.position.x - _childWidth * 0.5f > posXThreshold :
                    item.position.x + _childWidth * 0.5f < negXThreshold;
            }
        }
    }
}
