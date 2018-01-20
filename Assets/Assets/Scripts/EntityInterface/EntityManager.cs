using UnityEngine;

namespace EntityInterface
{
    [RequireComponent(typeof(Rigidbody))]
    public class EntityManager : Entity
    {
        #region Variables
        public GameObject EntitySelf;
    #endregion

        public override void Init()
        {
            myStats = new EntityStats(1,1,1);
        }

        protected override void TakeDamage(int damageTaken)
        {
            damageTaken -= myStats.ACV.CalculateValue();
            damageTaken = Mathf.Clamp(damageTaken, 0, int.MaxValue);

            myStats.CurrentHealth -= damageTaken;
            Debug.Log(transform.name + " takes " + damageTaken + " damage.");

            if (myStats.CurrentHealth <= 0)
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
