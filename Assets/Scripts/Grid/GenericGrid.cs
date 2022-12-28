using System;
using UnityEngine;

namespace Core.Grid
{

public class GenericGrid<T>
{
    IPublisher<OnGridObjectChangedEventArgs> OnGridObjectChanged = new Publisher<OnGridObjectChangedEventArgs>();

    private int _width;
    private int _height;
    private float _cellSize;
    private Vector2 _originPosition;
    private T[,] _gridArray;

    public GenericGrid(int width, int height, float cellSize, Vector2 originPosition, Func<GenericGrid<T>, int, int, T> createGridObject)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _originPosition = originPosition;

        _gridArray = new T[width, height];

        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < _gridArray.GetLength(1); y++)
            {
                _gridArray[x, y] = createGridObject(this, x, y);
            }
        }
    }

    public int GetWidth()
    {
        return _width;
    }

    public int GetHeight()
    {
        return _height;
    }

    public float GetCellSize()
    {
        return _cellSize;
    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(x, y) * _cellSize + _originPosition;
    }

    public void GetXY(Vector2 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
        y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
    }

    public void SetGridObject(int x, int y, T value)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
        {
            _gridArray[x, y] = value;
            if (OnGridObjectChanged != null)
                OnGridObjectChanged.Publish(new OnGridObjectChangedEventArgs(x, y));
        }
    }

    public void TriggerGridObjectChanged(int x, int y)
    {
        if (OnGridObjectChanged != null)
            OnGridObjectChanged.Publish(new OnGridObjectChangedEventArgs(x, y));
    }

    public void SetGridObject(Vector2 worldPosition, T value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public void GetGridObjectNo(Vector2 worldPosition, out int x, out int y)
    {
        int width, height;
        GetXY(worldPosition, out width, out height);
        x = width;
        y = height;
    }

    public T GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
        {
            return _gridArray[x, y];
        }
        else
        {
            return default(T);
        }
    }

    public T GetGridObject(Vector2 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    public Vector2 GetGridObjectPositions(Vector2 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetWorldPosition(x, y);
    }

}

public class OnGridObjectChangedEventArgs
{
    public int x;
    public int y;

    public OnGridObjectChangedEventArgs(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}


}
//Subscriber<OnGridObjectChangedEventArgs> subscriber;

//void ABC()
//{
//    subscriber = new Subscriber<OnGridObjectChangedEventArgs>(OnGridObjectChanged); // Subscribe to new Publisher
//    subscriber.Publisher.MessagePublisher += Test;

//    OnGridObjectChanged.Publish(new OnGridObjectChangedEventArgs(3, 3));
//}


//void Test(object sender, Message<OnGridObjectChangedEventArgs> e)
//{

//}