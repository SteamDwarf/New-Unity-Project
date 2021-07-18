using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGetEffect
{
    void GetEffect(AttributeType attribute, AttributeValueType valueType, float increase, float time);
}
