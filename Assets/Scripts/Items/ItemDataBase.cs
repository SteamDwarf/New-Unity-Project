using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ItemDataBase
{
    static Dictionary<int, Dictionary<string, object>> data = new Dictionary<int, Dictionary<string, object>>() {
        {1, new Dictionary<string, object>() {
            {"id", 1},
            {"itemName", "Зелье лечения"},
            {"description", "Это зелье содержит в себе живительную силу. Восстанавливают игроку жизни."},
            {"useType", ItemUseType.getEffectUse},
            {"rarity", RarityType.unusual},
            {"sprite", "Sprites/Items/Potions/HealthPotion"},
            {"prefab", "Prefabs/Items/Potions/HealthPotion"}
        }},
        {2, new Dictionary<string, object>() {
            {"id", 2},
            {"itemName", "Зелье скорости"},
            {"description", "Хотите жить? Тогда бегите! Бегите! Пока не поздно... Зелье увеличивает скорость."},
            {"useType", ItemUseType.getEffectUse},
            {"rarity", RarityType.rare},
            {"sprite", "Sprites/Items/Potions/SpeedPotion"},
            {"prefab", "Prefabs/Items/Potions/SpeedPotion"}
        }},
        {3, new Dictionary<string, object>() {
            {"id", 3},
            {"itemName", "Зелье выносливости"},
            {"description", "Устали от охоты на монстров, опускаются руки? Тогда этот напиток то, что вам надо! Увеличивает максимальное количество выносливости."},
            {"useType", ItemUseType.getEffectUse},
            {"rarity", RarityType.rare},
            {"sprite", "Sprites/Items/Potions/StaminaPotion"},
            {"prefab", "Prefabs/Items/Potions/StaminaPotion"}
        }},
        {4, new Dictionary<string, object>() {
            {"id", 4},
            {"itemName", "Зелье сил"},
            {"description", "Скорее всего это зелье каждый день пил Добрыня Никитич! Увеличивает силу игрока."},
            {"useType", ItemUseType.getEffectUse},
            {"rarity", RarityType.rare},
            {"sprite", "Sprites/Items/Potions/StrengthPotion"},
            {"prefab", "Prefabs/Items/Potions/StrengthPotion"}
        }},
        {26, new Dictionary<string, object>() {
            {"id", 26},
            {"itemName", "Боевой топор"},
            {"description", "Нет ничего лучше, чем топор, который можно кинуть в монстра и сразить его наповал!"},
            {"useType", ItemUseType.chooseTargetUse},
            {"rarity", RarityType.unusual},
            {"sprite", "Sprites/Items/ThrowingItems/CombatAxe"},
            {"prefab", "Prefabs/Items/ThrowingItems/CombatAxe"}
        }},
        {27, new Dictionary<string, object>() {
            {"id", 27},
            {"itemName", "Метательный кинжал"},
            {"description", "Быстрый, резкий, как кинжал дерзкий!"},
            {"useType", ItemUseType.chooseTargetUse},
            {"rarity", RarityType.common},
            {"sprite", "Sprites/Items/ThrowingItems/Dagger"},
            {"prefab", "Prefabs/Items/ThrowingItems/Dagger"}
        }},
        {28, new Dictionary<string, object>() {
            {"id", 28},
            {"itemName", "Бомбочка"},
            {"description", "Вас окружило полчище монстров? Не беда! Эта малышка поможет вам справиться с ними! Только не подорвите себя!"},
            {"useType", ItemUseType.chooseTargetUse},
            {"rarity", RarityType.unusual},
            {"sprite", "Sprites/Items/ThrowingItems/Bomb_v2"},
            {"prefab", "Prefabs/Items/ThrowingItems/Bomb"}
        }}
    };

    public static Dictionary<string, object> GetItemInformation(int id) {
        return data[id];
    } 

    public static List<int> GetAllID() {
        List<int> allId = new List<int>();

        foreach (var item in data){
            allId.Add(item.Key);
        }

        return allId;
    }
}
