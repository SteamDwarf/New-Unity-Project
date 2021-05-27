using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    public string name;
    public float range;
    public int damage;

    public Attack(string name, float range, int damage)
    {
        this.name = name;
        this.range = range;
        this.damage = damage;
    }
}
