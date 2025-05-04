using System;
using System.Collections.Generic;
using RVM.Scripts.Core.Interfaces;
using RVM.Scripts.Core.Quests;
using RVM.Scripts.Core.Quests.Utils;
using RVM.Scripts.Core.UI;
using UnityEngine;

namespace RVM.Scripts.Core.NPC
{
    public class QuestNpc : Npc, IInteractable
    {
        public List<Quest> questsAvailableToGive;
        public List<Quest> questsAvailableToReceive;
        
        public List<Quest> questsUnavailableToGive;
        public List<Quest> questsUnavailableToReceive;

        public List<Quest> questsReceived;
        public List<Quest> questsGiven;

        public Transform giveQuestIcon;
        public Transform receiveQuestIcon;

        
        public void Interact()
        {
            if (questsAvailableToReceive.Count > 0)
            {
                QuestManager.Instance.lastQuest = questsAvailableToReceive[questsAvailableToReceive.Count - 1];
                UIManager.Instance.ShowPopup("ReceiveQuestPopup");
            }
            if (questsAvailableToGive.Count > 0)
            {
                QuestManager.Instance.lastQuest = questsAvailableToGive[questsAvailableToGive.Count - 1];
                UIManager.Instance.ShowPopup("GiveQuestPopup");
            }
        }

        public void UpdateQuestIcon()
        {
            receiveQuestIcon.gameObject.SetActive(false);
            giveQuestIcon.gameObject.SetActive(false);
            
            if (questsAvailableToReceive.Count > 0)
            {
                giveQuestIcon.gameObject.SetActive(false);
                receiveQuestIcon.gameObject.SetActive(true);
            }
            if (questsAvailableToGive.Count > 0)
            {
                receiveQuestIcon.gameObject.SetActive(false);
                giveQuestIcon.gameObject.SetActive(true);
            }
        }
    }
}