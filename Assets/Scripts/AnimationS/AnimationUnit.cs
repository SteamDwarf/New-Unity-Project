using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AnimationUnit : ScriptableObject
{
    public string animationName;
    public bool isState;
    public bool isMixing;
    public float time;
    public bool isMovingState;
    private bool isPlaying;
   

    //public abstract void Play(Animator animator);
}
