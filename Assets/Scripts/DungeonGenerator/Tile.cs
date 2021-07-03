using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Vector2Int position {get; private set;}
    public TileType tileType {get; private set;}
    public bool hasEnemy {get; private set;}
    public EnemyType enemyType {get; private set;}
    public bool hasItem {get; private set;}
    public ItemType itemType {get; private set;}
    public bool hasFurniture {get; private set;}
    public FurnitureType furnitureType {get; private set;}
    public bool hasPlayer {get; private set;}

    public Tile(Vector2Int position, TileType type) {
        this.position = position;
        this.tileType = type;

        hasEnemy = false;
        hasItem = false;
        hasFurniture = false;
        hasPlayer = false;
    }
    public void SpawnEnemy(EnemyType enemy) {
        this.hasEnemy = true;
        this.enemyType = enemy;
    }
    public void SpawnItem(ItemType item) {
        this.hasItem = true;
        this.itemType = item;
    }
    public void SpawnFurniture(FurnitureType furniture) {
        this.hasFurniture = true;
        this.furnitureType = furniture;

        if(this.tileType == TileType.wall) {
            this.tileType = TileType.floor;
        }
    }

    public void ChangeTileType(TileType type) {
        this.tileType = type;
    }
    public void SpawnPlayer() {
        this.hasPlayer = true;
    }
}
