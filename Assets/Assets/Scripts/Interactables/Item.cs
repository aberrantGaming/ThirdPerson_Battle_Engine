using UnityEngine;
using Managers;

namespace Interactables.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class Item : ScriptableObject
    {

        new public string name = "new item";    //Name of the item

        public Sprite icon = null;              //Item Icon
        public bool isDefaultItem = false;      //Is it the item deafault wear?

        public virtual void Use()
        {
            // use the item
            //something might happen

            Debug.Log("Using " + name);
        }

        public void RemoveFromInventory ()
        {
            InventoryManager.instance.Remove(this);
        }
    }
}
