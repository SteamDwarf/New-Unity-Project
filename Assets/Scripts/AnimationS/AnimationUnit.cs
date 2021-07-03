using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationUnit : ScriptableObject
{
    public string animationName;
    public bool isState;
    public bool isMixing;

    public abstract AnimationUnit Clone();
}
