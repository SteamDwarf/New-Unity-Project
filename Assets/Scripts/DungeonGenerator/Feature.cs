using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feature
{
    public List<Vector2Int> positions {get; private set;}
    public Wall[] walls {get; private set;}
    public TypeFeature type {get; private set;}
    public int width {get; private set;}
    public int height {get; private set;}
    public int enemyInRoom {get; private set;}
    public int itemInRoom {get; private set;}

    public Feature(int width, int height, TypeFeature type) {
        this.width = width;
        this.height = height;
        this.type = type;
        this.positions = new List<Vector2Int>();
        this.walls =  new Wall[4];
    }
    public void SetWall(Wall[] walls) {
        this.walls = walls;
    }
    public void IncreaseEnemyCount(int num) {
        this.enemyInRoom += num;
    }
    public void IncreaseItemCount(int num) {
        this.itemInRoom += num;
    }
    public void SetPosition(Vector2Int position) {
        positions.Add(position);
    }
}
