using System;
using System.Collections.Generic;
using System.Linq;
using RVM.Scripts.Core.Interfaces;
using RVM.Scripts.Core.Quests;
using RVM.Scripts.Core.UI;
using UnityEngine;

namespace RVM.Scripts.Core.NPCS
{
    public class QuestReceiver : MonoBehaviour
    {
        public NPC npc;
        public List<Quest> _readyQuests = new List<Quest>();
        
        public GameObject questAvailableIcon;
        public SpriteRenderer questAvailableIconSprite;
        
        public void Start()
        {
            QuestManager.Instance.OnQuestBring += HandleQuestBring;
            UpdateQuests();
            UpdateQuestIcon();
        }

        public void OnDestroy()
        {
            QuestManager.Instance.OnQuestBring -= HandleQuestBring;
        }

        public void UpdateQuestIcon()
        {
            questAvailableIcon.SetActive(false);

            if (_readyQuests.Count > 0)
            {
                questAvailableIcon.SetActive(true);
                questAvailableIconSprite.color = Color.yellow;
            }

        }

        public void UpdateQuests()
        {
            _readyQuests.Clear();
            foreach (var quest in QuestManager.Instance.readyQuests)
            {
                foreach (var obj in quest.questObjectives)
                {
                    if (obj.questReceiver == npc && !_readyQuests.Contains(quest))
                    {
                        _readyQuests.Add(quest);
                    }
                }
            }
        }

        // public void Interact()
        // {
        //     if (_readyQuests.Count > 0)
        //     {
        //         QuestManager.Instance.lastQuest = _readyQuests[0];
        //         UIManager.Instance.ShowPopup("BringQuestPopup");
        //     }
        // }
        
        private void HandleQuestBring(Quest quest)
        {
            _readyQuests.Remove(quest);
            UpdateQuestIcon();
            var giver = FindObjectsOfType<QuestGiver>().FirstOrDefault(r => r.npc == quest.questGiver);
            
            if (giver != null)
            {
                // giver._doneQuests.Add(quest);
                giver._readyQuests.Remove(quest);
            }
        }
    }
}