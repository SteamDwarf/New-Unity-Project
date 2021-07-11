public enum TileType {
    wall, floor
}
public enum EnemyType {
    simpleEnemy, hardEnemy
}
public enum ItemType {
    momentaryPotion, continiousPotion, oneTargetThrowingItem
}
public enum FurnitureType {
    doorTop, doorFront
}
public enum WallDirection {
    north, south, west, east
}
public enum TypeFeature {
    corridor, room
}
public enum CellType {
    inventory, hotBar
}
public enum PotionType {
    health, strength, stamina, speed
}
public enum MousePressed {
    down, enter, up
}
public enum GameState {
    menuState, inGameState, inventoryState, invetoryItemInteraction
}
public enum ItemUseType {
    getEffectUse, chooseTargetUse
}
public enum ItemDamageType {
    target, area
}
public enum RarityType {
    unique, rare, unusual, common
}
public enum PlayerFaceTo {
    front, back, left, right
}