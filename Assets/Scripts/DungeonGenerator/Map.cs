using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public Tile[,] map {get; private set;}

    public Map(int width, int height) {
        this.map = new Tile[width, height];
    }
    public Tile[,] GetMap() {
        return this.map;
    }
    public void SetTile(Tile tile) {
        Vector2Int tilePosition = tile.position;
        map[tilePosition.x, tilePosition.y] = tile;
    }
    public Tile GetTile(Vector2Int position) {
        return map[position.x, position.y];
    }
}
