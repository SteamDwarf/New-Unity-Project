//SetingsEnums
public enum AudioSettingsEnum {
    none, musicVolume, playerSoundVolume, enemySoundVolume, environmentSoundVolume
}



//Game enums
public enum TileType {
    wall, floor
}
public enum EnemyType {
    simpleEnemy, hardEnemy
}
public enum EnemyBehaviourState {
    playerClose, canAttack, recharging, attacking, lowResources, fullResources
}//Пока не используется
public enum AttributeType {
    health, stamina, strength, speed
}
public enum AttributeValueType {
    maxValue, curValue
}
public enum EffectClass {
    none, baf, debaf
}
public enum EffectType {
    none, maxStamina, currentSpeed, currentStrength
}
public enum ItemType {
    momentaryPotion, continiousPotion, oneTargetThrowingItem
}//Уже не используется
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
/* public enum PotionType {
    health, strength, stamina, speed
} */
public enum MousePressed {
    down, enter, up
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
public enum MathActions {
    division, substraction, addition, multiplication
}