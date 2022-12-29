using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProductable
{
    public IProduct[] GetProducts { get; }
}
