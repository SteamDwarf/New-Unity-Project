using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class EffectDataBase
{
    static Dictionary<EffectType, Dictionary<string, object>> bafs = new Dictionary<EffectType, Dictionary<string, object>>() {
        {EffectType.currentSpeed, new Dictionary<string, object>() {
            {"Image", "Sprites/Effects/CurrentSpeed"},
            {"Description", "Скорость +4"},
            {"Color", new Color32(112, 188, 180, 255)},
            {"Effect", new Effect(AttributeType.speed, AttributeValueType.curValue, MathActions.multiplication)}
        }},
        {EffectType.currentStrength, new Dictionary<string, object>() {
            {"Image", "Sprites/Effects/CurrentStrength"},
            {"Description", "Сила +2"},
            {"Color", new Color32(188, 130, 112, 255)},
            {"Effect", new Effect(AttributeType.strength, AttributeValueType.curValue)}
        }},
        {EffectType.maxStamina, new Dictionary<string, object>() {
            {"Image", "Sprites/Effects/MaxStamina"},
            {"Description", "Выносливость х2"},
            {"Color", new Color32(126, 188, 112, 255)},
            {"Effect", new Effect(AttributeType.stamina, AttributeValueType.maxValue, MathActions.multiplication)}
        }}
    };
    static Dictionary<EffectType, Dictionary<string, object>> debafs = new Dictionary<EffectType, Dictionary<string, object>>() {
        {EffectType.currentSpeed, new Dictionary<string, object>() {
            {"Image", "Sprites/Effects/CurrentSpeed"},
            {"Description", "Скорость x0.75"},
            {"Color", new Color32(112, 188, 180, 255)},
            {"Effect", new Effect(AttributeType.speed, AttributeValueType.curValue, MathActions.multiplication)}
        }},
        {EffectType.currentStrength, new Dictionary<string, object>() {
            {"Image", "Sprites/Effects/CurrentStrength"},
            {"Description", "Сила -2"},
            {"Color", new Color32(188, 130, 112, 255)},
            {"Effect", new Effect(AttributeType.strength, AttributeValueType.curValue)}
        }},
        {EffectType.maxStamina, new Dictionary<string, object>() {
            {"Image", "Sprites/Effects/MaxStamina"},
            {"Description", "Выносливость x0.5"},
            {"Color", new Color32(126, 188, 112, 255)},
            {"Effect", new Effect(AttributeType.stamina, AttributeValueType.maxValue, MathActions.multiplication)}
        }}
    };

    public static Dictionary<string, object> GetEffectInformation(EffectClass effectClass, EffectType effectType) {
        if(effectClass == EffectClass.baf) {
            return bafs[effectType];
        } else {
            return debafs[effectType];
        }
    }
}
