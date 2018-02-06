using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attributes;

namespace EntityInterface
{
    public struct EntityStats
    {
        #region Variables

        #region Properties
        public int Body { get { return baseBody.CalculateValue(); } }
        public int Mind { get { return baseMind.CalculateValue(); } }
        public int Soul { get { return baseSoul.CalculateValue(); } }
        public int ShockValue { get { return shockValue.CalculateValue(); } }
        public int HealthMax { get { return maxHealthPoints.CalculateValue(); } }
        public int EnergyMax { get { return maxEnergyPoints.CalculateValue(); } }
        public int AttackStrength { get { return attackCombatValue.CalculateValue(); } }
        public int DefenseStrength { get { return defenseCombatValue.CalculateValue(); } }
        #endregion

        #region Base Stats
        private Attribute baseBody;  // A measure of the character's physical prowess and health.
        public Attribute BaseBody { get { return baseBody; } }
        private Attribute baseMind;  // A measure of the character's mental capacity and intelligence.
        public Attribute BaseMind { get { return baseMind; } }
        private Attribute baseSoul;  // A measure of the character's spirit and willpower.
        public Attribute BaseSoul { get { return baseSoul; } }
        #endregion

        #region Derived Stats
        private HealthPoints maxHealthPoints;           // A measure of the character's available health
        private EnergyPoints maxEnergyPoints;           // A measure of the character's available energy
        private ShockValue shockValue;                  // A measure of the character's knockback resistance
        private AttackCombatValue attackCombatValue;    // A measure of the character's offense rating
        private DefenseCombatValue defenseCombatValue;  // A measure of the character's defense rating
        #endregion

        #endregion

        public EntityStats(int _baseBody, int _baseMind, int _baseSoul)
        {
            baseBody = new Attribute(_baseBody);
            baseMind = new Attribute(_baseMind);
            baseSoul = new Attribute(_baseSoul);

            maxHealthPoints = new HealthPoints(0);
            maxEnergyPoints = new EnergyPoints(0);
            shockValue = new ShockValue(0);
            attackCombatValue = new AttackCombatValue(0);
            defenseCombatValue = new DefenseCombatValue(0);

            InitializeDerivedStats();
        }
        
        void InitializeDerivedStats()
        {
            maxHealthPoints.AddAttribute(BaseBody);
            maxHealthPoints.AddAttribute(BaseSoul);

            maxEnergyPoints.AddAttribute(BaseMind);
            maxEnergyPoints.AddAttribute(BaseSoul);

            shockValue.AddAttribute(maxHealthPoints);

            attackCombatValue.AddAttribute(BaseBody);
            attackCombatValue.AddAttribute(BaseMind);
            attackCombatValue.AddAttribute(BaseSoul);

            defenseCombatValue.AddAttribute(BaseBody);
            defenseCombatValue.AddAttribute(BaseMind);
            defenseCombatValue.AddAttribute(BaseSoul);
        }

        public void AddBuff(Attribute attribute, RawBonus bonus)
        {
            attribute.AddRawBonus(bonus);
        }

        public void RemoveBuff(Attribute attribute, RawBonus bonus)
        {
            attribute.RemoveRawBonus(bonus);
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
