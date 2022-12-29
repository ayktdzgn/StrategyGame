using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Building : MonoBehaviour , ISelectable
{
    [SerializeField] int _width;
    [SerializeField] int _height;

    protected string _name;
    protected Sprite _sprite;

    public int Width => _width;
    public int Height => _height;

    public Sprite GetSprite { get => _sprite; }
    public string GetName { get => _name; }

    public virtual void Awake()
    {
        _name = gameObject.name;
        _sprite = GetComponent<SpriteRenderer>().sprite;
    }
}
