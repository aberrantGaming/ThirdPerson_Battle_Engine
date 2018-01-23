using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : TimedEffect 
{
    public int Damage;

    protected override void ApplyEffect()
    {
        target.TakeDamage(Damage);
    }
}
