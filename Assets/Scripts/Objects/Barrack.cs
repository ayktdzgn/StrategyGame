using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrack : Building, IProductable
{
    [SerializeReference] Unit[] _products;
    public IProduct[] GetProducts { get => _products; }
}
