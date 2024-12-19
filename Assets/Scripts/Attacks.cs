using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    public float range;
    public int damage;
    public Attack(float range, int damage)
    {
        this.range = range;
        this.damage = damage;
    }


    public static Attack basicMeleeAttack = new Attack(2, 10);
    public static Attack basicRangedAttack = new Attack(10, 5);
}
