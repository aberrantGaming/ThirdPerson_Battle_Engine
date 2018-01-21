using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attributes;

namespace EntityInterface
{
    [System.Serializable]
    public struct EntityStats
    {
        #region Variables

        public Attribute Body;
        public Attribute Mind;
        public Attribute Soul;
        public HealthPoints Health;
        public EnergyPoints Energy;
        public AttackCombatValue ACV;
        public DefenseCombatValue DCV;
        public int CurrentHealth, CurrentEnergy;

        public int MaxHealth
        {
            get { return Health.CalculateValue(); }
        }
        public int MaxEnergy
        {
            get { return Energy.CalculateValue(); }
        }

    #endregion

        public EntityStats(int baseBody, int baseMind, int baseSoul)
        {
            Body = new Attribute(baseBody);
            Mind = new Attribute(baseMind);
            Soul = new Attribute(baseSoul);

            Health = new HealthPoints(0);
            Health.AddAttribute(Body);
            Health.AddAttribute(Soul);

            Energy = new EnergyPoints(0);
            Energy.AddAttribute(Mind);
            Energy.AddAttribute(Soul);

            ACV = new AttackCombatValue(0);
            ACV.AddAttribute(Body);
            ACV.AddAttribute(Mind);
            ACV.AddAttribute(Soul);

            DCV = new DefenseCombatValue(0);
            DCV.AddAttribute(Body);
            DCV.AddAttribute(Mind);
            DCV.AddAttribute(Soul);

            CurrentHealth = Health.CalculateValue();
            CurrentEnergy = Energy.CalculateValue();

            CalculateAll();            
        }

        public void CalculateAll()
        {
            Body.CalculateValue();
            Mind.CalculateValue();
            Soul.CalculateValue();
            ACV.CalculateValue();
            DCV.CalculateValue();
            Health.CalculateValue();
            Energy.CalculateValue();
        }
    
    }

    public abstract class Entity : MonoBehaviour
    {
        protected EntityStats myStats = new EntityStats();

        public abstract void Init();

        protected abstract void TakeDamage(int damageTaken);
        protected abstract void EntityDeath();
    }
}
