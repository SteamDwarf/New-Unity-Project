using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Attribute : ScriptableObject
{
    public float defaultMaxValue;
    public float curMaxValue;
    public float curValue;
    public float timeEffect;

    public void GetEffect(AttributeValueType valueType, float increase, float time) {
        if(valueType == AttributeValueType.maxValue) {
            if(curMaxValue < defaultMaxValue || curMaxValue > defaultMaxValue) {
                return;
            }

            curMaxValue += increase;
        } else if(valueType == AttributeValueType.curValue) {
            if(curValue < curMaxValue || curValue > curMaxValue) {
                return;
            }
            
            curValue += increase;
        }

        timeEffect = time;
    }
}
