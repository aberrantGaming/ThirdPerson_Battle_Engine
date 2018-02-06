using Attributes;
using Interactables.Items;
using Managers;
using Abilities;
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
        public Surgebinder Investiture;

        protected Ability[] Hotbar = new Ability[3];
        protected Ability SelectedAbility;
        
        #region Hidden Variables
        [HideInInspector] public GameObject EntitySelf { get { return gameObject; } }
        #endregion

        #endregion

        private void Start()
        {
            EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
        }
        
        /// <summary>
        /// This method is called from the ThirdPersonInput component
        /// </summary>
        public override void Init()
        {
            if (Investiture == null)
                Investiture = ScriptableObject.CreateInstance<Surgebinder>();

            Stats = new EntityStats(3, 3, 3);

            Investiture.Init();
            ApplyInvestitureBonus();
            Hotbar[0] = Investiture.CharacterAbilities[0];
            Debug.Log("Initializing complete. Current Order: " + Investiture.CharacterOrder);

            Health_CurValue = Stats.HealthMax;
            Energy_CurValue = Stats.EnergyMax;
        }

        protected void AttributeInit()
        {
            ApplyInvestitureBonus();
        }

        public virtual void FireAbility()
        {
            if (SelectedAbility != null)
                SelectedAbility.TriggerAbility();
        }

        public virtual void CancelAbility()
        {
            SelectedAbility = null;
        }

        public virtual void SetAbility(int index)
        {
            SelectedAbility = Hotbar[index];
        }

        private void ApplyInvestitureBonus()
        {
            Stats.BaseBody.AddRawBonus(Investiture.BonusBody);
            Stats.BaseMind.AddRawBonus(Investiture.BonusMind);
            Stats.BaseSoul.AddRawBonus(Investiture.BonusSoul);
        }

        private void RemoveInvestitureBonus()
        {
            Stats.BaseBody.RemoveRawBonus(Investiture.BonusBody);
            Stats.BaseMind.RemoveRawBonus(Investiture.BonusMind);
            Stats.BaseSoul.RemoveRawBonus(Investiture.BonusSoul);
        }

        private void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
        {
            if (newItem != null)
            {
                Stats.BaseBody.AddRawBonus(newItem.BonusBody);
                Stats.BaseMind.AddRawBonus(newItem.BonusMind);
                Stats.BaseSoul.AddRawBonus(newItem.BonusSoul);
            }

            if (oldItem != null)
            {
                Stats.BaseBody.RemoveRawBonus(oldItem.BonusBody);
                Stats.BaseMind.RemoveRawBonus(oldItem.BonusMind);
                Stats.BaseSoul.RemoveRawBonus(oldItem.BonusSoul);
            }
        }
    }
}