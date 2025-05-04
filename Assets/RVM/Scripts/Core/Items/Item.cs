using System;
using System.Collections.Generic;
using RVM.Scripts.Core.Interfaces;
using RVM.Scripts.Core.Player.Utils;
using RVM.Scripts.Core.Quests;
using UnityEngine;

namespace RVM.Scripts.Core.Items
{
    public class Item : MonoBehaviour, IItem
    {
        public string itemID;
        public bool interactable;

        public bool isQuestItem;
        public Quest _quest;

        public void Interact(Inventory inventory)
        {
            if (!interactable)
                return;

            inventory.AddItem(itemID);

            gameObject.SetActive(false);

            if (isQuestItem)
            {
                if (_quest != null)
                {
                    QuestManager.Instance.OnCollectQuestItemCollected(itemID, inventory.GetItemCount(itemID), _quest);
                }
            }
        }

        public void Start()
        {
            QuestManager.Instance.OnGiveCollectQuest += HandleCollectQuest;
            QuestManager.Instance.OnQuestItemCollected += HandleQuestItemCollected;
            QuestManager.Instance.OnReceiveCollectQuest += HandleReceivedCollectQuest;
        }

        public void OnDestroy()
        {
            QuestManager.Instance.OnGiveCollectQuest -= HandleCollectQuest;
            QuestManager.Instance.OnQuestItemCollected -= HandleQuestItemCollected;
            QuestManager.Instance.OnReceiveCollectQuest -= HandleReceivedCollectQuest;
        }

        private void HandleCollectQuest(Quest quest)
        {
            foreach (var questItem in quest.questObjective.Items)
            {
                if (questItem.Key == itemID)
                {
                    isQuestItem = true;
                    _quest = quest;
                    interactable = true;
                    gameObject.layer = LayerMask.NameToLayer("Interactable");
                }
            }
        }

        private void HandleQuestItemCollected(string itemId)
        {
            if (itemId == itemID)
                interactable = false;
        }

        private void HandleReceivedCollectQuest(Quest quest)
        {
            foreach (var kvp in quest.questObjective.Items)
            {
                if (kvp.Key == itemID)
                {
                    isQuestItem = false;
                    _quest = null;
                    gameObject.layer = LayerMask.NameToLayer("Default");
                }
            }
            gameObject.SetActive(true);
        }
    }
}