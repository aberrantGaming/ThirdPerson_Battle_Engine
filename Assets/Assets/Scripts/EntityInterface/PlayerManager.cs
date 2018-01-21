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
        public IInvestiture Investiture = new Windrunner();
        
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
            // Run base entity initialization
            base.Init();
            
            if (Investiture != null)
                Investiture.Init();

            Debug.Log("Discipline: " + Investiture.DisciplineName + "; Order: " + Investiture.OrderName);

            AttributeInit();

            //AbilityInit();
        }

        protected void AbilityInit()
        {
            //currentAbilities = new Ability[4];
            //currentAbilities[0] = PlayerClass.Passive1;
            //currentAbilities[1] = PlayerClass.Ability1;
            //currentAbilities[2] = PlayerClass.Ability2;
            //currentAbilities[3] = PlayerClass.Ability3;
        }
        
        protected void AttributeInit()
        {
            // Apply raw bonuses from the attached player class
            myStats.Body.AddRawBonus(Investiture.BonusBody);
            myStats.Mind.AddRawBonus(Investiture.BonusMind);
            myStats.Soul.AddRawBonus(Investiture.BonusSoul);            
            myStats.Health.AddRawBonus(Investiture.BonusHealth);
            myStats.Energy.AddRawBonus(Investiture.BonusEnergy);

            myStats.CalculateAll();

            //// Heal the player up to their new max health and energy
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
            //ActiveAbility = currentAbilities[index];
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