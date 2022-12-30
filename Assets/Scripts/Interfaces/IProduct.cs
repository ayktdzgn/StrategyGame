using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProduct
{
    public Sprite GetSprite { get; }
    public string GetName { get; }
}
