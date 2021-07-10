using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapDrawer : MonoBehaviour
{
    private DungeonGenerator dG;
    private Tile[,] map;
    private Vector2Int mapSize;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject frontDoorPrefab;
    [SerializeField] private GameObject topDoorPrefab;
    [SerializeField] private GameObject backStone;

    [SerializeField] private Transform mapTransform;
    [SerializeField] private List<GameObject> floorPrefab;
    [SerializeField] private List<GameObject> easyEnemies;
    [SerializeField] private List<GameObject> hardEnemies;
    [SerializeField] private List<GameObject> spawnedEnemies;
    [SerializeField] private List<GameObject> momentaryPotions;
    [SerializeField] private List<GameObject> continiousPotions;
    [SerializeField] private List<GameObject> oneTargetAmmo;
    //[SerializeField] private List<GameObject> spawnedPotions;

    [SerializeField] private float mapk;
    public void DrawMap()
    {
        dG = GetComponent<DungeonGenerator>();
        mapTransform = new GameObject("Map").transform;
        map = dG.GetMap();
        mapSize = dG.GetMapSize();


        for (int y = 0; y < mapSize.y - 1; y++)
        {
            for (int x = 0; x < mapSize.x - 1; x++)
            {
                if (map[x, y] == null)
                {
                    GameObject toInstantiate = backStone;
                    GameObject instance = Instantiate(toInstantiate, new Vector3(x * mapk, y * mapk, 0), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(mapTransform);
                    continue;
                }
                else if (map[x, y].tileType == TileType.wall)
                {
                    GameObject toInstantiate = wallPrefab;
                    GameObject instance = Instantiate(toInstantiate, new Vector3(x * mapk, y * mapk, 0), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(mapTransform);

                }
                else if (map[x, y].tileType == TileType.floor)
                {
                    int randomNum = Random.Range(0, floorPrefab.Count - 1);
                    GameObject toInstantiate = floorPrefab[randomNum];
                    GameObject instance = Instantiate(toInstantiate, new Vector3(x * mapk, y * mapk, 0), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(mapTransform);
                }

                if(map[x, y].hasFurniture) {
                    if(map[x, y].furnitureType == FurnitureType.doorFront) {
                        GameObject toInstantiate = frontDoorPrefab;
                        GameObject instance = Instantiate(toInstantiate, new Vector3(x * mapk, y * mapk, 0), Quaternion.identity) as GameObject;
                        instance.transform.SetParent(mapTransform);
                    }
                    else if(map[x, y].furnitureType == FurnitureType.doorTop) {
                        GameObject toInstantiate = topDoorPrefab;
                        GameObject instance = Instantiate(toInstantiate, new Vector3(x * mapk, y * mapk, 0), Quaternion.identity) as GameObject;
                        instance.transform.SetParent(mapTransform);
                    }
                }
                

                if (map[x, y].hasPlayer == true)
                {
                    GameObject playerSpawn = GameObject.Instantiate(playerPrefab, new Vector3(x * mapk, y * mapk, 0), Quaternion.identity);
                }

                if (map[x, y].hasEnemy)
                {
                    int enemyInd;
                    GameObject enemy;
                    if (map[x, y].enemyType == EnemyType.simpleEnemy)
                    {
                        enemyInd = Random.Range(0, easyEnemies.Count);
                        enemy = easyEnemies[enemyInd];
                        spawnedEnemies.Add(easyEnemies[enemyInd]);
                    }
                    else
                    {
                        enemyInd = Random.Range(0, hardEnemies.Count);
                        enemy = hardEnemies[enemyInd];
                        spawnedEnemies.Add(hardEnemies[enemyInd]);
                    }
                    
                    GameObject enemyToInst = Instantiate(enemy, new Vector3(x * mapk, y * mapk, 0), Quaternion.identity);
                }


                if(map[x, y].hasItem)
                {
                    int itemInd;
                    GameObject item;

                    if (map[x, y].itemType == ItemType.momentaryPotion){
                        itemInd = Random.Range(0, momentaryPotions.Count);
                        item = momentaryPotions[itemInd];

                    }else if (map[x, y].itemType == ItemType.continiousPotion) {
                        itemInd = Random.Range(0, continiousPotions.Count);
                        item = continiousPotions[itemInd];

                    }else {
                        itemInd = Random.Range(0, oneTargetAmmo.Count);
                        item = oneTargetAmmo[itemInd];
                    }

                    GameObject potionToInst = Instantiate(item, new Vector3(x * mapk, y * mapk, 0), Quaternion.identity);
                    //spawnedPotions.Add(potion);
                }
            }
        }
    }

    public float GetMapScaler() {
        return mapk;
    }
}
