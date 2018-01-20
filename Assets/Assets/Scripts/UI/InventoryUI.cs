using UnityEngine;
using Managers;

namespace UserInterface
{

    public class InventoryUI : MonoBehaviour
    {

        public Transform itemsParent;
        public GameObject inventoryUI;
        public string inventoryButton = "Inventory";

        InventoryManager inventory;
        InventorySlot[] slots;


        // Use this for initialization
        void Start()
        {
            inventory = InventoryManager.instance;
            inventory.onItemChangedCallback += UpdateUI;

            slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown(inventoryButton))
            {
                inventoryUI.SetActive(!inventoryUI.activeSelf);
            }
        }

        void UpdateUI()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (i < inventory.items.Count)
                {
                    slots[i].AddItem(inventory.items[i]);
                }
                else
                {
                    slots[i].ClearSlot();
                }
            }

        }
    }
}

