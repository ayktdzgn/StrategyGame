using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    public Sprite GetSprite { get; }
    public string GetName { get;  }
}
