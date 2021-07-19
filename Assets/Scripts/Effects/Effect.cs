using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    private Attribute attribute;
    private AttributeType attributeType;
    private AttributeValueType valueType;
    private float increase;
    private MathActions magnification;

    public Effect(AttributeType attributeType, AttributeValueType valueType, MathActions magnification = MathActions.addition) {
        this.attributeType = attributeType;
        this.valueType = valueType;
        this.magnification = magnification;
    }

    public void SetAttribute(Attribute attribute) {
        this.attribute = attribute;
    }
    public void SetIncrease(float increase) {
        this.increase = increase;
    }
    public void StartEffect() {
        if(magnification == MathActions.addition) {
            if(valueType == AttributeValueType.curValue) {
                attribute.curValue += increase;
                return;
            }
            if(valueType == AttributeValueType.maxValue) {
                attribute.curMaxValue += increase;
                attribute.curValue = attribute.curMaxValue;
                return;
            }
            return;
        }
        if(magnification == MathActions.multiplication) {
            if(valueType == AttributeValueType.curValue) {
                attribute.curValue *= increase;
                return;
            }
            if(valueType == AttributeValueType.maxValue) {
                attribute.curMaxValue *= increase;
                attribute.curValue = attribute.curMaxValue;
                return;
            }
            return;
        }
    }
    public void EndEffect() {
        if(magnification == MathActions.addition) {
            if(valueType == AttributeValueType.curValue) {
                attribute.curValue -= increase;
                return;
            }
            if(valueType == AttributeValueType.maxValue) {
                attribute.curMaxValue -= increase;
                attribute.curValue = attribute.curMaxValue;
                return;
            }
            return;
        }
        if(magnification == MathActions.multiplication) {
            if(valueType == AttributeValueType.curValue) {
                attribute.curValue /= increase;
                return;
            }
            if(valueType == AttributeValueType.maxValue) {
                attribute.curMaxValue /= increase;
                attribute.curValue = attribute.curMaxValue;
                return;
            }
            return;
        }

        Debug.Log(attribute.curValue);
    }
}
