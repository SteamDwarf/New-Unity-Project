using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapDrawer : MonoBehaviour
{
    private DungeonGenerator dG;

    public GameObject playerPrefab;
    public GameObject wallPrefab;
    public GameObject frontDoorPrefab;
    public GameObject topDoorPrefab;
    public GameObject backStone;

    public Transform mapTransform;
    public List<GameObject> floorPrefab;
    public List<GameObject> easyEnemies;
    public List<GameObject> hardEnemies;
    public List<GameObject> spawnedEnemies;
    public List<GameObject> momentaryPotions;
    public List<GameObject> continiousPotions;
    public List<GameObject> spawnedPotions;

    public float mapk;
    public void DrawMap()
    {
        dG = GetComponent<DungeonGenerator>();
        mapTransform = new GameObject("Map").transform;


        for (int y = 0; y < dG.mapHeight - 1; y++)
        {
            for (int x = 0; x < dG.mapWidth - 1; x++)
            {
                if (MapManager.map[x, y] == null)
                {
                    GameObject toInstantiate = backStone;
                    GameObject instance = Instantiate(toInstantiate, new Vector3(x * mapk, y * mapk, 0), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(mapTransform);
                    continue;
                }
                else if (MapManager.map[x, y].type == "Wall")
                {
                    GameObject toInstantiate = wallPrefab;
                    GameObject instance = Instantiate(toInstantiate, new Vector3(x * mapk, y * mapk, 0), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(mapTransform);

                }
                else if (MapManager.map[x, y].type == "Floor")
                {
                    int randomNum = Random.Range(0, floorPrefab.Count - 1);
                    GameObject toInstantiate = floorPrefab[randomNum];
                    GameObject instance = Instantiate(toInstantiate, new Vector3(x * mapk, y * mapk, 0), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(mapTransform);
                }

                if(MapManager.map[x, y].furniture == "DoorFront")
                {
                    GameObject toInstantiate = frontDoorPrefab;
                    GameObject instance = Instantiate(toInstantiate, new Vector3(x * mapk, y * mapk, 0), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(mapTransform);
                }
                else if(MapManager.map[x, y].furniture == "DoorTop")
                {
                    GameObject toInstantiate = topDoorPrefab;
                    GameObject instance = Instantiate(toInstantiate, new Vector3(x * mapk, y * mapk, 0), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(mapTransform);
                }

                if (MapManager.map[x, y].hasPlayer == true)
                {
                    GameObject playerSpawn = GameObject.Instantiate(playerPrefab, new Vector3(x * mapk, y * mapk, 0), Quaternion.identity);
                }

                if (MapManager.map[x, y].hasEnemy)
                {
                    int enemyInd;
                    GameObject enemy;
                    if (MapManager.map[x, y].typeEnemy == "Easy")
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
                    
                    //Vector2Int spawnPoint = new Vector2Int(x, y);
                    GameObject enemyToInst = Instantiate(enemy, new Vector3(x * mapk, y * mapk, 0), Quaternion.identity);
                    MapManager.map[x, y].enemy = enemyToInst;
                    
                    //GameObject enemy = MapManager.map[x, y].enemy;

                }


                if(MapManager.map[x, y].typeItem != null)
                {
                    int potionInd;
                    GameObject potion;

                    if (MapManager.map[x, y].typeItem == "MomentaryPotion")
                    {
                        potionInd = Random.Range(0, momentaryPotions.Count);
                        potion = momentaryPotions[potionInd];
                    }
                    else 
                    {
                        potionInd = Random.Range(0, continiousPotions.Count);
                        potion = continiousPotions[potionInd];
                    }

                    GameObject potionToInst = Instantiate(potion, new Vector3(x * mapk, y * mapk, 0), Quaternion.identity);

                    MapManager.map[x, y].item = potionToInst;
                    spawnedPotions.Add(potion);
                }
            }
        }
    }
}
