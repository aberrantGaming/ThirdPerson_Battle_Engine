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
        // Set this to provide specific data about the entity's discipline
        public IInvestiture Investiture = new Surgebinding();
        
        private Ability ActiveAbility = null;

        #region Hidden Variables
        [HideInInspector]
        public GameObject EntitySelf { get { return gameObject; } }
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
            myStats = new EntityStats(3, 3, 3);

            if (Investiture != null)
            {
                Investiture.Init();
                ApplyInvestitureBonus();

                SetOrder(SurgebindingOrder.Windrunners);
                Debug.Log("Discipline: " + Investiture.DisciplineName + "; Order: " + Investiture.OrderName);
            }

            Health_CurValue = myStats.Health_MaxValue;
            Energy_CurValue = myStats.Energy_MaxValue;
        }

        protected void AttributeInit()
        {
            ApplyInvestitureBonus();
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

        private void SetOrder(SurgebindingOrder newOrder)
        {
            RemoveInvestitureBonus();

            if (newOrder == SurgebindingOrder.Windrunners)
                Investiture = new Windrunner();
            //else if (newOrder == SurgebindingOrder.Skybreakers)
            //    Investiture = new Skybreaker();
            //else if (newOrder == SurgebindingOrder.Dustbringers)
            //    Investiture = new Dustbringer();
            //else if (newOrder == SurgebindingOrder.Edgedancers)
            //    Investiture = new Edgedancer();
            //else if (newOrder == SurgebindingOrder.Truthwatchers)
            //    Investiture = new Truthwatcher();
            //else if (newOrder == SurgebindingOrder.Elsecallers)
            //    Investiture = new Willshaper();
            //else if (newOrder == SurgebindingOrder.Stonewards)
            //    Investiture = new Stonewards();
            //else if (newOrder == SurgebindingOrder.Bondsmiths)
            //    Investiture = new Bondsmith();

            Investiture.Init();

            ApplyInvestitureBonus();
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