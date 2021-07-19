using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGetEffect
{
    void GetEffect(EffectClass effectClass, EffectType effectType, float increase, float time);
}
