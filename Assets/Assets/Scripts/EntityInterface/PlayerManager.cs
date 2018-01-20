using Attributes;
using Characters;
using Interactables.Items;
using Managers;
using Skills;
using UnityEngine;

namespace EntityInterface
{
    public class PlayerManager : EntityManager
    {
        #region Singleton
        public static PlayerManager instance;

        private void Awake()
        {
            instance = this;
        }
        #endregion

        #region Variables
        public int BodyBaseValue = 3;
        public int MindBaseValue = 3;
        public int SoulBaseValue = 3;

        public Surgebinder PlayerClass;

        protected Ability[] currentAbilities;


        private Ability ActiveAbility = null;
        #endregion

        public override void Init()
        {
            EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;

            AbilityInit();
            AttributeInit();
        }

        protected void AbilityInit()
        {
            currentAbilities = new Ability[4];
            currentAbilities[0] = PlayerClass.Passive1;
            currentAbilities[1] = PlayerClass.Ability1;
            currentAbilities[2] = PlayerClass.Ability2;
            currentAbilities[3] = PlayerClass.Ability3;
        }
        
        protected void AttributeInit()
        {
            myStats = new EntityStats(BodyBaseValue, MindBaseValue, SoulBaseValue);

            // Apply raw bonuses from the attached player class
            myStats.Body.AddRawBonus(new RawBonus(PlayerClass.BodyBonus));
            myStats.Mind.AddRawBonus(new RawBonus(PlayerClass.MindBonus));
            myStats.Soul.AddRawBonus(new RawBonus(PlayerClass.SoulBonus));
            myStats.Health.AddRawBonus(new RawBonus(PlayerClass.HealthBonus));
            myStats.Energy.AddRawBonus(new RawBonus(PlayerClass.EnergyBonus));
            myStats.ACV.AddRawBonus(new RawBonus(PlayerClass.AcvBonus));
            myStats.DCV.AddRawBonus(new RawBonus(PlayerClass.DcvBonus));
            myStats.CalculateAll();

            // Heal the player up to their new max health and energy
            myStats.CurrentHealth = myStats.MaxHealth;
            myStats.CurrentEnergy = myStats.MaxEnergy;
        }

        public virtual void FireAbility()
        {
            if (ActiveAbility != null)
                ActiveAbility.Fire();
        }

        public virtual void CancelAbility()
        {
            ActiveAbility = null;
        }

        public virtual void SetAbility(int index)
        {
            ActiveAbility = currentAbilities[index];
        }

        private void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
        {
            if (newItem != null)
            {
                myStats.Body.AddRawBonus(newItem.BonusBody);
                myStats.Mind.AddRawBonus(newItem.BonusMind);
                myStats.Soul.AddRawBonus(newItem.BonusSoul);
                myStats.Health.AddRawBonus(newItem.BonusHealth);
                myStats.Energy.AddRawBonus(newItem.BonusEnergy);
                myStats.CalculateAll();
            }

            if (oldItem != null)
            {
                myStats.Body.RemoveRawBonus(oldItem.BonusBody);
                myStats.Mind.RemoveRawBonus(oldItem.BonusMind);
                myStats.Soul.RemoveRawBonus(oldItem.BonusSoul);
                myStats.Health.RemoveRawBonus(oldItem.BonusHealth);
                myStats.Energy.RemoveRawBonus(oldItem.BonusEnergy);
                myStats.CalculateAll();
            }
        }
    }
}