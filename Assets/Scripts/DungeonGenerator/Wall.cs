using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall
{
    public List<Vector2Int> positions {get; private set;}
    public WallDirection direction {get; private set;}
    public int length {get; private set;}
    public bool hasFeature {get; private set;}
    public Feature connectedFeature {get; private set;}

    public Wall(WallDirection direction, int length) {
        this.direction = direction;
        this.length = length;
        this.positions = new List<Vector2Int>();
    }
    public void ConnectFeature (Feature feature) {
        this.hasFeature = true;
        this.connectedFeature = feature;
    }
    public void SetPosition(Vector2Int position) {
        this.positions.Add(position);
    }
}
