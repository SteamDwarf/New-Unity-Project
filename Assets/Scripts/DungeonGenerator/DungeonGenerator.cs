using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonGenerator : MonoBehaviour
{
    private MapDrawer MD;
    private GameManager GM;
    private Map map;

    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;

    [SerializeField] private int widthMinRoom;
    [SerializeField] private int widthMaxRoom;
    [SerializeField] private int heightMinRoom;
    [SerializeField] private int heightMaxRoom;

    [SerializeField] private int minCorridorLength;
    [SerializeField] private int maxCorridorLength;

    [SerializeField] private int maxFeatures;
    
    [SerializeField] private int maxEnemiesPerRoom;
    [SerializeField] private int minEnemiesPerRoom;
    [SerializeField] private int maxItemInRoom;
    [SerializeField] private int minItemInRoom;
    [SerializeField] private int maxPotionInRoom;
    [SerializeField] private int minPotionInRoom;

    public int aliveEnemiesCount {get; private set;}
    private int countFeatures;

    public List<Feature> allFeatures;
    public List<Feature> allRooms;

    public Vector2Int playerSpawnCordinates;


    public void InitializeDungeon()
    {
        MD = GetComponent<MapDrawer>();
        GM = GetComponent<GameManager>();
        aliveEnemiesCount = 0;
        countFeatures = 0;
        map = new Map(mapWidth, mapHeight);

        allFeatures = new List<Feature>();
        allRooms = new List<Feature>();
    }

    public void GenerateDungeon()
    {
        GenerateFeature(TypeFeature.room, true);

        for (int i = 0; i < 500; i++) {
            TypeFeature type;
            Feature originFeature;

            if (allFeatures.Count == 1) {
                originFeature = allFeatures[0];
            }else {
                originFeature = allFeatures[Random.Range(0, allFeatures.Count - 1)];
            }

            Wall wall = ChoseWall(originFeature);
            if (wall == null) {
                continue;
            }

            if (originFeature.type == TypeFeature.room) {
                type = TypeFeature.corridor;
            }else {
                if (Random.Range(0, 100) < 90){
                    type = TypeFeature.room;
                }else {
                    type = TypeFeature.corridor;
                }
            }

            GenerateFeature(type, false, wall);

            if (countFeatures >= maxFeatures) {
                break;
            }
        }

        SpawnPlayer();
        SpawnEnemies();
        SpawnPotions();
        MD.DrawMap();
    }

    void GenerateFeature(TypeFeature type, bool isFirst = false,  Wall wall = null)
    {
        Feature feature;
        int featureWidth = 0;
        int featureHeight = 0;
        int xStartingPoint;
        int yStartingPoint;
        int id;

        //В зависимости от типа помещения определяем его длину и высоту 
        if (type == TypeFeature.room)
        {
            featureWidth = Random.Range(widthMinRoom, widthMaxRoom);
            featureHeight = Random.Range(heightMinRoom, heightMaxRoom);
        }
        else
        {
            switch (wall.direction)
            {
                case WallDirection.south:
                    featureWidth = 3;
                    featureHeight = Random.Range(minCorridorLength, maxCorridorLength);
                    break;
                case WallDirection.north:
                    featureWidth = 3;
                    featureHeight = Random.Range(minCorridorLength, maxCorridorLength);
                    break;
                case WallDirection.west:
                    featureWidth = Random.Range(minCorridorLength, maxCorridorLength);
                    featureHeight = 3;
                    break;
                case WallDirection.east:
                    featureWidth = Random.Range(minCorridorLength, maxCorridorLength);
                    featureHeight = 3;
                    break;

            }
        }

        //Создаем помещение
        feature = new Feature(featureWidth, featureHeight, type);    

        //Определяем начальные точки генерации помещения
        if (isFirst)
        {
            xStartingPoint = mapWidth / 2;
            yStartingPoint = mapHeight / 2;
        }
        else
        {
            if (wall.positions.Count == 3) {
                id = 1;
            } else id = Random.Range(1, wall.positions.Count - 2);

            xStartingPoint = wall.positions[id].x;
            yStartingPoint = wall.positions[id].y;
        }

        Vector2Int lastWallPosition = new Vector2Int(xStartingPoint, yStartingPoint);

        //Также в зависимости от того первое это помещение или нет, сдвигаем на определенное расстояние
        if (isFirst)
        {
            xStartingPoint -= Random.Range(1, featureWidth);
            yStartingPoint -= Random.Range(1, featureHeight);
        }
        else
        {
            switch (wall.direction)
            {
                case WallDirection.south:
                    if (type == TypeFeature.room) { 
                        xStartingPoint -= Random.Range(1, featureWidth - 2);
                    }else {
                        xStartingPoint--;
                    }
                    yStartingPoint -= Random.Range(1, featureHeight - 2);
                    break;
                case WallDirection.north:
                    if (type == TypeFeature.room) {
                        xStartingPoint -= Random.Range(1, featureWidth - 2);
                    }else {
                        xStartingPoint--;
                    }
                    yStartingPoint++;
                    break;
                case WallDirection.west:
                    xStartingPoint -= featureWidth;
                    if (type == TypeFeature.room) { 
                        yStartingPoint -= Random.Range(1, featureHeight - 2);
                    }
                    else {
                        yStartingPoint--;
                    }
                    break;
                case WallDirection.east:
                    xStartingPoint++;
                    if (type == TypeFeature.room) {
                        yStartingPoint -= Random.Range(1, featureHeight - 2);
                    }
                    else {
                        yStartingPoint--;
                    }
                    break;
            }
        }

        //Проверка на границу карты
        if (!CheckIfHasSpace(new Vector2Int(xStartingPoint, yStartingPoint), new Vector2Int(xStartingPoint + featureWidth - 1, yStartingPoint + featureHeight - 1))) {
            return;
        }

        //Создаются стены     
        feature.SetWall(BuildWalls(featureWidth, featureHeight));

        //Исходя из координат помещения в карту добавляем клетки и определяем их тип
        for (int y = 0; y < featureHeight; y++)
        {
            for (int x = 0; x < featureWidth; x++)
            {
                //Заполняется список координат комнаты
                Tile tile;
                Vector2Int position = new Vector2Int();
                TileType tileType = TileType.floor;
                position.x = xStartingPoint + x;
                position.y = yStartingPoint + y;

                feature.SetPosition(position);

                //В карту передаем координаты тайла комнаты
                //В карту передается тип тайла комнаты
                if (y == 0)
                {
                    feature.walls[0].SetPosition(position);
                    tileType = TileType.wall;
                }
                if (y == (featureHeight - 1))
                {
                    feature.walls[1].SetPosition(position);
                    tileType = TileType.wall;
                }
                if (x == 0)
                {
                    feature.walls[2].SetPosition(position);
                    tileType = TileType.wall;
                }
                if (x == (featureWidth - 1))
                {
                    feature.walls[3].SetPosition(position);
                    tileType = TileType.wall;
                }
                if (tileType != TileType.wall)
                {
                    tileType = TileType.floor;
                }

                tile = new Tile(position, tileType);
                map.SetTile(tile);
            }
        }

        //Устанавливаем двери
        if (!isFirst){
            SetDoors(lastWallPosition, wall, type);
        }

        //Добавляем помещение в список
        allFeatures.Add(feature);

        //Присоединяем помещение к стене
        if(wall != null) {
            wall.ConnectFeature(feature);
        }
        countFeatures++;
    }


    private Wall[] BuildWalls(int featureWidth, int featureHeight) {
        Wall[] featureWalls = new Wall[4];

        for (int i = 0; i < 4; i++)
        {
            Wall newWall;
            int wallLength = 0;
            WallDirection wallDirection = WallDirection.south;

            //Для каждой стены указываются направления
            switch (i)
            {
                case 0:
                    wallDirection = WallDirection.south;
                    wallLength = featureWidth;
                    break;
                case 1:
                    wallDirection = WallDirection.north;
                    wallLength = featureWidth;
                    break;
                case 2:
                    wallDirection = WallDirection.west;
                    wallLength = featureHeight;
                    break;
                case 3:
                    wallDirection = WallDirection.east;
                    wallLength = featureHeight;
                    break;
            }

            newWall = new Wall(wallDirection, wallLength);
            featureWalls[i] = newWall;
        }

        return featureWalls;
    }

    private void SetDoors(Vector2Int lastWallPosition, Wall wall, TypeFeature type) {
        map.map[lastWallPosition.x, lastWallPosition.y].ChangeTileType(TileType.floor);
        switch (wall.direction)
        {
            case WallDirection.south:
                map.map[lastWallPosition.x, lastWallPosition.y - 1].ChangeTileType(TileType.floor);
                if(type == TypeFeature.room){ 
                    map.map[lastWallPosition.x, lastWallPosition.y - 1].SpawnFurniture(FurnitureType.doorFront);
                }
                break;
            case WallDirection.north:
                map.map[lastWallPosition.x, lastWallPosition.y + 1].ChangeTileType(TileType.floor);
                if (type == TypeFeature.room) { 
                    map.map[lastWallPosition.x, lastWallPosition.y + 1].SpawnFurniture(FurnitureType.doorFront);
                }
                break;
            case WallDirection.west:
                map.map[lastWallPosition.x - 1, lastWallPosition.y].ChangeTileType(TileType.floor);
                if (type == TypeFeature.room) { 
                    map.map[lastWallPosition.x - 1, lastWallPosition.y].SpawnFurniture(FurnitureType.doorTop);
                }
                break;
            case WallDirection.east:
                map.map[lastWallPosition.x + 1, lastWallPosition.y].ChangeTileType(TileType.floor);
                if (type == TypeFeature.room) { 
                    map.map[lastWallPosition.x + 1, lastWallPosition.y].SpawnFurniture(FurnitureType.doorTop);
                }
                break;
        }
    }

    private bool CheckIfHasSpace(Vector2Int start, Vector2Int end)
    {
        for (int y = start.y; y <= end.y; y++)
        {
            for (int x = start.x; x <= end.x; x++)
            {
                if (x < 0 || y < 0 || x >= mapWidth || y >= mapHeight) {
                    return false;
                }
                if (map.map[x, y] != null) {
                    return false;
                }
            }
        }

        return true;
    }

    private Wall ChoseWall(Feature feature)
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
            if (feature.type == TypeFeature.room)
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

            if (map.map[cordinates.x, cordinates.y].tileType == TileType.floor)
            {
                playerSpawnCordinates = cordinates;
                map.map[cordinates.x, cordinates.y].SpawnPlayer();
                break;
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

                Vector2Int spawnPoint = room.positions[tileInd];

                if (map.map[spawnPoint.x, spawnPoint.y].tileType == TileType.wall || map.map[spawnPoint.x, spawnPoint.y].hasFurniture)
                    continue;
                else if (map.map[spawnPoint.x, spawnPoint.y].hasEnemy)
                    continue;
                else
                {
                    EnemyType enemy = EnemyType.simpleEnemy;

                    if (typeEnemy == 4 && room.enemyInRoom >= 2)
                    {
                        enemy = EnemyType.hardEnemy;
                        map.map[spawnPoint.x, spawnPoint.y].SpawnEnemy(enemy);
                        room.IncreaseEnemyCount(2);
                        j += 2;
                        aliveEnemiesCount++;
                    }
                    else
                    {
                        enemy = EnemyType.simpleEnemy;
                        map.map[spawnPoint.x, spawnPoint.y].SpawnEnemy(enemy);
                        room.IncreaseEnemyCount(1);
                        j++;
                        aliveEnemiesCount++;
                    } 
                }
            }
        }
    }

    public void SpawnPotions()
    {
        for (int i = 0; i < allRooms.Count; i++)
        {
            int itemSpawnCount = Random.Range(minItemInRoom, maxItemInRoom + 1);
            Feature room = allRooms[i];

            for (int j = room.itemInRoom; j < itemSpawnCount;)
            {
                int tileInd = Random.Range(0, room.positions.Count - 1);

                Vector2Int spawnPoint = room.positions[tileInd];

                if (map.map[spawnPoint.x, spawnPoint.y].tileType == TileType.wall || map.map[spawnPoint.x, spawnPoint.y].hasFurniture)
                    continue;
                else if (map.map[spawnPoint.x, spawnPoint.y].hasItem)
                    continue;
                else if (map.map[spawnPoint.x, spawnPoint.y].hasPlayer)
                    continue;
                else if(map.map[spawnPoint.x, spawnPoint.y].hasEnemy)
                    continue;
                else
                {
                    int lucky = Random.Range(1, 3);

                    if(lucky == 1) {
                        int potLucky = Random.Range(1, 4);

                        if (potLucky < 3) {
                            map.map[spawnPoint.x, spawnPoint.y].SpawnItem(ItemType.momentaryPotion);
                        }   
                        else if (potLucky == 3) {
                            map.map[spawnPoint.x, spawnPoint.y].SpawnItem(ItemType.continiousPotion);
                        }

                    } else if(lucky == 2) {
                        map.map[spawnPoint.x, spawnPoint.y].SpawnItem(ItemType.oneTargetThrowingItem);
                    }
                    room.IncreaseItemCount(1);
                    j++;
                }
            }
        }
    }


    public Tile[,] GetMap() {
        return map.map;
    }

    public Vector2Int GetMapSize() {
        Vector2Int mapSize = new Vector2Int();

        mapSize.x = mapWidth;
        mapSize.y = mapHeight;

        return mapSize;
    }
}
