using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AnimationActing : AnimationUnit { 
    public float time;
    public override AnimationUnit Clone() {
        AnimationActing clone = new AnimationActing();

        clone.animationName = this.animationName;
        clone.isState = false;
        clone.isMixing = this.isMixing;
        clone.time = this.time;

        return clone;
    }
}
