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
            {"sprite", "Sprites/Items/Potions/HealthPotion"},
            {"prefab", "Prefabs/Items/Potions/HealthPotion"}
        }},
        {2, new Dictionary<string, object>() {
            {"id", 2},
            {"itemName", "Зелье скорости"},
            {"description", "Хотите жить? Тогда бегите! Бегите! Пока не поздно... Зелье увеличивает скорость."},
            {"useType", ItemUseType.getEffectUse},
            {"sprite", "Sprites/Items/Potions/SpeedPotion"},
            {"prefab", "Prefabs/Items/Potions/SpeedPotion"}
        }},
        {3, new Dictionary<string, object>() {
            {"id", 3},
            {"itemName", "Зелье выносливости"},
            {"description", "Устали от охоты на монстров, опускаются руки? Тогда этот напиток то, что вам надо! Увеличивает максимальное количество выносливости."},
            {"useType", ItemUseType.getEffectUse},
            {"sprite", "Sprites/Items/Potions/StaminaPotion"},
            {"prefab", "Prefabs/Items/Potions/StaminaPotion"}
        }},
        {4, new Dictionary<string, object>() {
            {"id", 4},
            {"itemName", "Зелье сил"},
            {"description", "Скорее всего это зелье каждый день пил Добрыня Никитич! Увеличивает силу игрока."},
            {"useType", ItemUseType.getEffectUse},
            {"sprite", "Sprites/Items/Potions/StrengthPotion"},
            {"prefab", "Prefabs/Items/Potions/StrengthPotion"}
        }},
        {26, new Dictionary<string, object>() {
            {"id", 26},
            {"itemName", "Боевой топор"},
            {"description", "Нет ничего лучше, чем топор, который можно кинуть в монстра и сразить его наповал!"},
            {"useType", ItemUseType.chooseTargetUse},
            {"sprite", "Sprites/Items/ThrowingItems/CombatAxe"},
            {"prefab", "Prefabs/Items/ThrowingItems/CombatAxe"}
        }},
        {27, new Dictionary<string, object>() {
            {"id", 27},
            {"itemName", "Метательный кинжал"},
            {"description", "Быстрый, резкий, как кинжал дерзкий!"},
            {"useType", ItemUseType.chooseTargetUse},
            {"sprite", "Sprites/Items/ThrowingItems/Dagger"},
            {"prefab", "Prefabs/Items/ThrowingItems/Dagger"}
        }}
    };

    public static Dictionary<string, object> GetItemInformation(int id) {
        return data[id];
    } 
}
