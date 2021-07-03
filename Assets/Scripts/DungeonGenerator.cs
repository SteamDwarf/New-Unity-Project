using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonGenerator : MonoBehaviour
{
    private MapDrawer MD;
    private GameManager GM;
    //private new CameraScript camera;

    public int mapWidth;
    public int mapHeight;

    public int widthMinRoom;
    public int widthMaxRoom;
    public int heightMinRoom;
    public int heightMaxRoom;

    public int minCorridorLength;
    public int maxCorridorLength;
    public int maxFeatures;
    

    public int maxEnemiesPerRoom;
    public int minEnemiesPerRoom;
    public int maxPotionInRoom;
    public int minPotionInRoom;

    private int aliveEnemiesCount;
    private int countFeatures;

    public float mapk;

    public List<Feature> allFeatures;
    public List<Feature> allRooms;

    public Vector2Int playerSpawnCordinates;

    //public GameObject playerPrefab;
    /*public GameObject playerPrefab;
    public GameObject wallPrefab;
    public Transform mapTransform;
    public List<GameObject> floorPrefab;*/


    public void InitializeDungeon()
    {
        MapManager.map = new Tile[mapWidth, mapHeight];
        MD = GetComponent<MapDrawer>();
        GM = GetComponent<GameManager>();
        aliveEnemiesCount = 0;
        mapk = MD.mapk;
        //camera = GetComponent<CameraScript>();
    }

    public void GenerateDungeon()
    {
        GenerateFeature("Room", new Wall(), true);

        for (int i = 0; i < 500; i++)
        {
            Feature originFeature;

            if (allFeatures.Count == 1)
            {
                originFeature = allFeatures[0];
            }
            else
            {
                originFeature = allFeatures[Random.Range(0, allFeatures.Count - 1)];
            }

            Wall wall = ChoseWall(originFeature);
            if (wall == null) continue;

            string type;

            if (originFeature.type == "Room")
            {
                type = "Corridor";
            }
            else
            {
                if (Random.Range(0, 100) < 90)
                {
                    type = "Room";
                }
                else
                {
                    type = "Corridor";
                }
            }

            GenerateFeature(type, wall);

            if (countFeatures >= maxFeatures) break;
        }

        SpawnPlayer();
        SpawnEnemies();
        SpawnPotions();
        MD.DrawMap();
    }

    void GenerateFeature(string type, Wall wall, bool isFirst = false)
    {
        //Создаем помещение
        Feature room = new Feature();
        room.positions = new List<Vector2Int>();

        int roomWidth = 0;
        int roomHeight = 0;

        //В зависимости от типа помещения определяем длину и высоту
        if (type == "Room")
        {
            roomWidth = Random.Range(widthMinRoom, widthMaxRoom);
            roomHeight = Random.Range(heightMinRoom, heightMaxRoom);
        }
        else
        {
            switch (wall.direction)
            {
                case "South":
                    roomWidth = 3;
                    roomHeight = Random.Range(minCorridorLength, maxCorridorLength);
                    break;
                case "North":
                    roomWidth = 3;
                    roomHeight = Random.Range(minCorridorLength, maxCorridorLength);
                    break;
                case "West":
                    roomWidth = Random.Range(minCorridorLength, maxCorridorLength);
                    roomHeight = 3;
                    break;
                case "East":
                    roomWidth = Random.Range(minCorridorLength, maxCorridorLength);
                    roomHeight = 3;
                    break;

            }
        }

        //Определяем начальные точки генерации помещения
        int xStartingPoint;
        int yStartingPoint;

        if (isFirst)
        {
            xStartingPoint = mapWidth / 2;
            yStartingPoint = mapHeight / 2;
        }
        else
        {
            int id;
            if (wall.positions.Count == 3) id = 1;
            else id = Random.Range(1, wall.positions.Count - 2);

            xStartingPoint = wall.positions[id].x;
            yStartingPoint = wall.positions[id].y;
        }

        Vector2Int lastWallPosition = new Vector2Int(xStartingPoint, yStartingPoint);

        //Также в зависимости от того первая это помещение или нет, сдвигаем на определенное расстояние
        if (isFirst)
        {
            xStartingPoint -= Random.Range(1, roomWidth);
            yStartingPoint -= Random.Range(1, roomHeight);
        }
        else
        {
            switch (wall.direction)
            {
                case "South":
                    if (type == "Room") xStartingPoint -= Random.Range(1, roomWidth - 2);
                    else xStartingPoint--;
                    yStartingPoint -= Random.Range(1, roomHeight - 2);
                    break;
                case "North":
                    if (type == "Room") xStartingPoint -= Random.Range(1, roomWidth - 2);
                    else xStartingPoint--;
                    yStartingPoint++;
                    break;
                case "West":
                    xStartingPoint -= roomWidth;
                    if (type == "Room") yStartingPoint -= Random.Range(1, roomHeight - 2);
                    else yStartingPoint--;
                    break;
                case "East":
                    xStartingPoint++;
                    if (type == "Room") yStartingPoint -= Random.Range(1, roomHeight - 2);
                    else yStartingPoint--;
                    break;
            }
        }

        //Проверка на границу карты
        if (!CheckIfHasSpace(new Vector2Int(xStartingPoint, yStartingPoint), new Vector2Int(xStartingPoint + roomWidth - 1, yStartingPoint + roomHeight - 1)))
        {
            return;
        }

        //Создаются стены
        room.walls = new Wall[4];

        for (int i = 0; i < room.walls.Length; i++)
        {
            room.walls[i] = new Wall();
            room.walls[i].positions = new List<Vector2Int>();
            room.walls[i].length = 0;

            //Для каждой стены указываются направления
            switch (i)
            {
                case 0:
                    room.walls[i].direction = "South";
                    break;
                case 1:
                    room.walls[i].direction = "North";
                    break;
                case 2:
                    room.walls[i].direction = "West";
                    break;
                case 3:
                    room.walls[i].direction = "East";
                    break;
            }
        }

        for (int y = 0; y < roomHeight; y++)
        {
            for (int x = 0; x < roomWidth; x++)
            {
                //Заполняется список координат комнаты
                Vector2Int position = new Vector2Int();
                position.x = xStartingPoint + x;
                position.y = yStartingPoint + y;

                room.positions.Add(position);

                //В карту передаем координаты тайла комнаты
                MapManager.map[position.x, position.y] = new Tile();
                MapManager.map[position.x, position.y].xPosition = position.x;
                MapManager.map[position.x, position.y].yPosition = position.y;

                //В карту передается тип тайла комнаты
                if (y == 0)
                {
                    room.walls[0].positions.Add(position);
                    room.walls[0].length++;
                    MapManager.map[position.x, position.y].type = "Wall";
                }
                if (y == (roomHeight - 1))
                {
                    room.walls[1].positions.Add(position);
                    room.walls[1].length++;
                    MapManager.map[position.x, position.y].type = "Wall";
                }
                if (x == 0)
                {
                    room.walls[2].positions.Add(position);
                    room.walls[2].length++;
                    MapManager.map[position.x, position.y].type = "Wall";
                }
                if (x == (roomWidth - 1))
                {
                    room.walls[3].positions.Add(position);
                    room.walls[3].length++;
                    MapManager.map[position.x, position.y].type = "Wall";
                }
                if (MapManager.map[position.x, position.y].type != "Wall")
                {
                    MapManager.map[position.x, position.y].type = "Floor";
                }
            }
        }

        //В карту передается тип двери комнаты
        if (!isFirst)
        {
            MapManager.map[lastWallPosition.x, lastWallPosition.y].type = "Floor";
            switch (wall.direction)
            {
                case "South":
                    MapManager.map[lastWallPosition.x, lastWallPosition.y - 1].type = "Floor";
                    if(type == "Room")
                        MapManager.map[lastWallPosition.x, lastWallPosition.y - 1].furniture = "DoorFront";
                    break;
                case "North":
                    MapManager.map[lastWallPosition.x, lastWallPosition.y + 1].type = "Floor";
                    if (type == "Room")
                        MapManager.map[lastWallPosition.x, lastWallPosition.y + 1].furniture = "DoorFront";
                    break;
                case "West":
                    MapManager.map[lastWallPosition.x - 1, lastWallPosition.y].type = "Floor";
                    if (type == "Room")
                        MapManager.map[lastWallPosition.x - 1, lastWallPosition.y].furniture = "DoorTop";
                    break;
                case "East":
                    MapManager.map[lastWallPosition.x + 1, lastWallPosition.y].type = "Floor";
                    if (type == "Room")
                        MapManager.map[lastWallPosition.x + 1, lastWallPosition.y].furniture = "DoorTop";
                    break;
            }
        }

        room.width = roomWidth;
        room.height = roomHeight;
        room.type = type;
        allFeatures.Add(room);
        countFeatures++;
    }

    bool CheckIfHasSpace(Vector2Int start, Vector2Int end)
    {
        for (int y = start.y; y <= end.y; y++)
        {
            for (int x = start.x; x <= end.x; x++)
            {
                if (x < 0 || y < 0 || x >= mapWidth || y >= mapHeight) return false;
                if (MapManager.map[x, y] != null) return false;
            }
        }

        return true;
    }

    Wall ChoseWall(Feature feature)
    {
        for (int i = 0; i < 10; i++)
        {
            int id = Random.Range(0, 100) / 25;
            if (!feature.walls[id].hasFeature)
            {
                return feature.walls[id];
            }
        }
        return null;
    }

    public void GetRooms()
    {
        foreach (Feature feature in allFeatures)
        {
            if (feature.type == "Room")
            {
                allRooms.Add(feature);
            }
        }
    }

    public void SpawnPlayer()
    {
        GetRooms();
        Feature room = allRooms[0];

        for (int i = 0; i < room.positions.Count; i++)
        {
            Vector2Int cordinates = room.positions[i];

            if (MapManager.map[cordinates.x, cordinates.y].type == "Floor")
            {
                playerSpawnCordinates = cordinates;
                MapManager.map[cordinates.x, cordinates.y].hasPlayer = true;
                room.hasPlayer = true;
                //camera.MoveCamera(new Vector2(cordinates.x, cordinates.y));
                break;

                /*playerSpawnCordinates = cordinates;
                GameObject player = GameObject.Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                player.GetComponent<PlayerMovement>().playerPosition = cordinates;
                MapManager.map[cordinates.x, cordinates.y].hasPlayer = true;
                room.hasPlayer = true;
                GetComponent<GameManager>().player = player.GetComponent<PlayerMovement>();
                */
            }
        }
    }

    public void SpawnEnemies()
    {   
        for (int i = 1; i < allRooms.Count; i++)
        {
            int enemiesSpawn = Random.Range(minEnemiesPerRoom, maxEnemiesPerRoom + 1);
            Feature room = allRooms[i];

            for (int j = room.enemyInRoom; j < enemiesSpawn;)
            {
                int tileInd = Random.Range(0, room.positions.Count - 1);
                int typeEnemy = Random.Range(0, 5);
                //int enemyInd = Random.Range(0, enemies.Count);

                Vector2Int spawnPoint = room.positions[tileInd];

                if (MapManager.map[spawnPoint.x, spawnPoint.y].type == "Wall" || MapManager.map[spawnPoint.x, spawnPoint.y].furniture != null)
                    continue;
                else if (MapManager.map[spawnPoint.x, spawnPoint.y].hasEnemy)
                    continue;
                else
                {
                    MapManager.map[spawnPoint.x, spawnPoint.y].hasEnemy = true;
                    //spawnedEnemies.Add(enemies[enemyInd]);

                    if (typeEnemy == 4 && room.enemyInRoom >= 2)
                    {
                        MapManager.map[spawnPoint.x, spawnPoint.y].typeEnemy = "Hard";
                        room.enemyInRoom += 2;
                        j += 2;
                        aliveEnemiesCount++;
                    }
                    else
                    {
                        MapManager.map[spawnPoint.x, spawnPoint.y].typeEnemy = "Easy";
                        room.enemyInRoom++;
                        j++;
                        aliveEnemiesCount++;
                    } 
                }
            }
        }

        GM.aliveEnemies = aliveEnemiesCount;
        Debug.Log(GM.aliveEnemies);
    }

    public void SpawnPotions()
    {
        for (int i = 0; i < allRooms.Count; i++)
        {
            int potionSpawnCount = Random.Range(minPotionInRoom, maxPotionInRoom + 1);
            Feature room = allRooms[i];

            for (int j = room.potionInRoom; j < potionSpawnCount;)
            {
                int tileInd = Random.Range(0, room.positions.Count - 1);

                Vector2Int spawnPoint = room.positions[tileInd];

                if (MapManager.map[spawnPoint.x, spawnPoint.y].type == "Wall" || MapManager.map[spawnPoint.x, spawnPoint.y].furniture != null)
                    continue;
                else if (MapManager.map[spawnPoint.x, spawnPoint.y].typeItem != null)
                    continue;
                else if (MapManager.map[spawnPoint.x, spawnPoint.y].hasPlayer)
                    continue;
                else
                {
                    int lucky = Random.Range(1, 4);

                    if (lucky < 3)
                        MapManager.map[spawnPoint.x, spawnPoint.y].typeItem = "MomentaryPotion";
                    else if (lucky == 3)
                        MapManager.map[spawnPoint.x, spawnPoint.y].typeItem = "ContiniousPotion";

                    room.potionInRoom++;
                    j++;
                }
            }


        }
    }

}
