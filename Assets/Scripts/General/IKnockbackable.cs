using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnockbackable 
{
    void Knockback(Vector2 knockVector, float power);
}
