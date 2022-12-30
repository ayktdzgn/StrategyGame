using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Entity : MonoBehaviour , ISelectable
{
    [SerializeField] int _width;
    [SerializeField] int _height;

    [SerializeField] string _name;
    [SerializeField] Sprite _sprite;

    public int Width => _width;
    public int Height => _height;

    public Sprite GetSprite { get => _sprite; }
    public string GetName { get => _name; }

    protected SpriteRenderer _spriteRenderer;

    protected void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
