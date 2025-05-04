using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using RVM.Scripts.Core.Items;
using RVM.Scripts.Core.Quests;
using UnityEngine;

namespace RVM.Scripts.Core.Player.Utils
{
    public class Inventory : MonoBehaviour
    {
        public readonly Dictionary<string, int> inventory = new Dictionary<string, int>();
        public int inventoryCapacity = 10;
        public int maxCount = 64;

        public event Action OnInventoryChanged;

        public void Start()
        {
            QuestManager.Instance.OnReceiveCollectQuest += HandleReceivedCollectQuest;
        }

        public void OnDestroy()
        {
            QuestManager.Instance.OnReceiveCollectQuest -= HandleReceivedCollectQuest;
        }

        public void AddItem(string itemID)
        {
            if (!inventory.ContainsKey(itemID) && inventory.Count == inventoryCapacity ||
                inventory.ContainsKey(itemID) && inventory.Count == inventoryCapacity &&
                inventory.GetValueOrDefault(itemID) == maxCount)
                return;

            if (!inventory.ContainsKey(itemID))
            {
                inventory.Add(itemID, 1);
            }
            else
            {
                inventory[itemID]++;
            }
            OnInventoryChanged?.Invoke();
        }

        private void RemoveItem(string itemID)
        {
            if (!inventory.ContainsKey(itemID)) return;
            inventory.Remove(itemID);
        }

        public int GetItemCount(string itemID)
        {
            if (inventory.ContainsKey(itemID))
            {
                return inventory[itemID];
            }
            return 0;
        }
        
        private void HandleReceivedCollectQuest(Quest quest)
        {
            foreach (var kvp in quest.questObjective.Items)
            {
                RemoveItem(kvp.Key);
            }
            OnInventoryChanged?.Invoke();
        }
    }
}