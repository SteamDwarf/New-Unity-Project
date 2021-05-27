using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public Vector2Int position;

    public bool isVisible;
    public bool isExplore;

    SpriteRenderer sprite;

    public void Initialize(Vector2Int pos)
    {
        position = pos;

        sprite = GetComponent<SpriteRenderer>();
        sprite.color = Color.clear;
    }
}
