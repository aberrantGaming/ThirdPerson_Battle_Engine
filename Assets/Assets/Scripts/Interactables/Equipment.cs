using UnityEngine;
using Attributes;
using Managers;

namespace Interactables.Items
{ 
    [CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
    public class Equipment : Item
    {

        public EquipmentSlot equipSlot;

        public RawBonus BonusHealth = new RawBonus(1);
        public RawBonus BonusEnergy = new RawBonus(1);

        public RawBonus BonusBody = new RawBonus(1);
        public RawBonus BonusMind = new RawBonus(1);
        public RawBonus BonusSoul = new RawBonus(1);
        
        public override void Use()
        {
            base.Use();

            EquipmentManager.instance.Equip(this);
            RemoveFromInventory();
        }

    }

    public enum EquipmentSlot { Head, Chest, Arms, Legs, Boots, Weapon }
}