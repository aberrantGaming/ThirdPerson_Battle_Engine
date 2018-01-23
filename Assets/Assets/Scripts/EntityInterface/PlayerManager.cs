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

            Health_CurValue = Stats.Health_MaxValue;
            Energy_CurValue = Stats.Energy_MaxValue;
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
            Stats.Body.AddRawBonus(Investiture.BonusBody);
            Stats.Mind.AddRawBonus(Investiture.BonusMind);
            Stats.Soul.AddRawBonus(Investiture.BonusSoul);            
            Stats.Health.AddRawBonus(Investiture.BonusHealth);
            Stats.Energy.AddRawBonus(Investiture.BonusEnergy);

            Stats.CalculateAll();
        }

        private void RemoveInvestitureBonus()
        {
            Stats.Body.RemoveRawBonus(Investiture.BonusBody);
            Stats.Mind.RemoveRawBonus(Investiture.BonusMind);
            Stats.Soul.RemoveRawBonus(Investiture.BonusSoul);
            Stats.Health.RemoveRawBonus(Investiture.BonusHealth);
            Stats.Energy.RemoveRawBonus(Investiture.BonusEnergy);

            Stats.CalculateAll();
        }

        private void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
        {
            if (newItem != null)
            {
                Stats.Body.AddRawBonus(newItem.BonusBody);
                Stats.Mind.AddRawBonus(newItem.BonusMind);
                Stats.Soul.AddRawBonus(newItem.BonusSoul);
                Stats.Health.AddRawBonus(newItem.BonusHealth);
                Stats.Energy.AddRawBonus(newItem.BonusEnergy);
                Stats.CalculateAll();
            }

            if (oldItem != null)
            {
                Stats.Body.RemoveRawBonus(oldItem.BonusBody);
                Stats.Mind.RemoveRawBonus(oldItem.BonusMind);
                Stats.Soul.RemoveRawBonus(oldItem.BonusSoul);
                Stats.Health.RemoveRawBonus(oldItem.BonusHealth);
                Stats.Energy.RemoveRawBonus(oldItem.BonusEnergy);
                Stats.CalculateAll();
            }
        }
    }
}