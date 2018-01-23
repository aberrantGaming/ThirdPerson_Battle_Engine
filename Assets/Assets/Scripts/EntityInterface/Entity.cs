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
        
        #region Public Getters
        public int Body_Value { get { return Body.CalculateValue(); } }
        public int Mind_Value { get { return Mind.CalculateValue(); } }
        public int Soul_Value { get { return Soul.CalculateValue(); } }
        public int Health_MaxValue { get { return Health.CalculateValue(); } }
        public int Energy_MaxValue { get { return Energy.CalculateValue(); } }
        #endregion

        #region Base Attributes
        public Attribute Body;
        public Attribute Mind;
        public Attribute Soul;
        #endregion

        #region Dependant Attributes
        public HealthPoints Health;
        public EnergyPoints Energy;
        public AttackCombatValue ACV;
        public DefenseCombatValue DCV;
        #endregion     

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
        public EntityStats Stats = new EntityStats();
        protected int Health_CurValue, Energy_CurValue;

        public abstract void Init();

        public abstract void TakeDamage(int damageTaken);
        protected abstract void EntityDeath();
    }
}
