using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] int _width;
    [SerializeField] int _height;

    public int Width => _width;
    public int Height => _height;
}
