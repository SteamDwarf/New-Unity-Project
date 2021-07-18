using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EffectDataBase
{
    static Dictionary<EffectType, Dictionary<string, string>> data = new Dictionary<EffectType, Dictionary<string, string>>() {
        {EffectType.currentSpeed, new Dictionary<string, string>() {
            {"Image", "Sprites/Effects/CurrentSpeed"},
            {"Description", "Скорость x1.3"}
        }},
        {EffectType.currentStrength, new Dictionary<string, string>() {
            {"Image", "Sprites/Effects/CurrentStrength"},
            {"Description", "Сила +2"}
        }},
        {EffectType.maxStamina, new Dictionary<string, string>() {
            {"Image", "Sprites/Effects/MaxStamina"},
            {"Description", "Выносливость х2"}
        }}
    };

    public static Dictionary<string, string> GetEffectInformation(EffectType effectType) {
        return data[effectType];
    }
}
