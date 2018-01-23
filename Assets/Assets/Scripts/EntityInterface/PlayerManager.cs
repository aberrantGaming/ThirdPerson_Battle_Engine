using Attributes;
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
        public Surgebinder Investiture;
        
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
                Investiture = Surgebinder.CreateInstance<Surgebinder>();

            myStats = new EntityStats(3, 3, 3);

            Investiture.Init();
            ApplyInvestitureBonus();
            Debug.Log("Initializing complete. ChosenOrder: " + Investiture.CharacterOrder);

            Health_CurValue = myStats.Health_MaxValue;
            Energy_CurValue = myStats.Energy_MaxValue;
        }

        protected void AttributeInit()
        {
            ApplyInvestitureBonus();
        }

        public virtual void FireAbility()
        {
            //if (ActiveAbility != null)
            //    ActiveAbility.TriggerAbility();
        }

        public virtual void CancelAbility()
        {
            //ActiveAbility = null;
        }

        public virtual void SetAbility(int index)
        {
            //ActiveAbility = currentAbilities[index];
        }

        private void ApplyInvestitureBonus()
        {
            myStats.Body.AddRawBonus(Investiture.BonusBody);
            myStats.Mind.AddRawBonus(Investiture.BonusMind);
            myStats.Soul.AddRawBonus(Investiture.BonusSoul);            
            myStats.Health.AddRawBonus(Investiture.BonusHealth);
            myStats.Energy.AddRawBonus(Investiture.BonusEnergy);

            myStats.CalculateAll();
        }

        private void RemoveInvestitureBonus()
        {
            myStats.Body.RemoveRawBonus(Investiture.BonusBody);
            myStats.Mind.RemoveRawBonus(Investiture.BonusMind);
            myStats.Soul.RemoveRawBonus(Investiture.BonusSoul);
            myStats.Health.RemoveRawBonus(Investiture.BonusHealth);
            myStats.Energy.RemoveRawBonus(Investiture.BonusEnergy);

            myStats.CalculateAll();
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