using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMobile
{
    public void SetSelectedColor(bool status);
    public void Move(Vector2Int destination);
    public bool IsMooving { get; }
}
