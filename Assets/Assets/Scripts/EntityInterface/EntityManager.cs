﻿using UnityEngine;

namespace EntityInterface
{
    [RequireComponent(typeof(Rigidbody))]
    public class EntityManager : Entity
    {
        #region Variables
        #endregion

        public override void Init()
        {
            Stats = new EntityStats(1, 1, 1);

            Health_CurValue = Stats.HealthMax;
            Energy_CurValue = Stats.EnergyMax;
        }

        public override void TakeDamage(int damageTaken)
        {
            damageTaken -= Stats.AttackStrength;
            damageTaken = Mathf.Clamp(damageTaken, 0, int.MaxValue);

            Health_CurValue -= damageTaken;
            Debug.Log(transform.name + " takes " + damageTaken + " damage.");

            if (Health_CurValue <= 0)
                EntityDeath();
        }

        protected override void EntityDeath()
        {
            // Die in Some Way
            // this method is meant to be overridden

            Debug.Log(transform.name + " died.");
        }
    }
}
