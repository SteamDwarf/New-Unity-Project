using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AnimationState : AnimationUnit
{
    public bool isMovingState;
    public override AnimationUnit Clone() {
        AnimationState clone = new AnimationState();
        clone.animationName = this.animationName;
        clone.isState = true;
        clone.isMixing = this.isMixing;

        return clone;
    }
}
