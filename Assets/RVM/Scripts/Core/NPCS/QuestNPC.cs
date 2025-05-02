using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RVM.Scripts.Core.Quests;
using RVM.Scripts.Core.UI;
using RVM.Scripts.Core.Interfaces;

namespace RVM.Scripts.Core.NPCS
{
    public class QuestNPC : MonoBehaviour, IInteractable
    {
        public NPC npc;

        // Состояния квестов
        public List<Quest> _availableQuests = new List<Quest>();
        public List<Quest> _activeQuests = new List<Quest>();
        public List<Quest> _readyQuests = new List<Quest>(); // Квесты, готовые к сдаче

        // UI
        public GameObject questAvailableIcon;
        public GameObject questActiveIcon;
        public SpriteRenderer questAvailableIconSprite;
        public SpriteRenderer questActiveIconSprite;

        private void OnDisable()
        {
            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.OnQuestTaken -= HandleQuestTaken;
                QuestManager.Instance.OnQuestBring -= HandleQuestBring;
            }
        }

        private void Start()
        {
            QuestManager.Instance.OnQuestTaken += HandleQuestTaken;
            QuestManager.Instance.OnQuestBring += HandleQuestBring;
            UpdateQuests();
            UpdateQuestIcon();
        }

        public void UpdateQuests()
        {
            _availableQuests.Clear();
            _activeQuests.Clear();
            _readyQuests.Clear();

            foreach (var quest in QuestManager.Instance.availableQuests.Where(q => q.questGiver == npc))
            {
                _availableQuests.Add(quest);
            }

            foreach (var quest in QuestManager.Instance.activeQuests.Where(q => q.questGiver == npc))
            {
                _activeQuests.Add(quest);
            }

            foreach (var quest in QuestManager.Instance.readyQuests.Where(q => q.questObjectives.Any(o => o.questReceiver == npc)))
            {
                _readyQuests.Add(quest);
            }
        }

        public void UpdateQuestIcon()
        {
            questActiveIcon.SetActive(false);
            questAvailableIcon.SetActive(false);

            if (_availableQuests.Count > 0)
            {
                questAvailableIcon.SetActive(true);
                questAvailableIconSprite.color = Color.yellow;
            }

            if (_activeQuests.Count > 0 && _availableQuests.Count == 0)
            {
                questActiveIcon.SetActive(true);
                questActiveIconSprite.color = Color.grey;
            }
            
            // if (_readyQuests.Count > 0 && _availableQuests.Count == 0)
            // {
            //     questActiveIcon.SetActive(true);
            //     questActiveIconSprite.color = Color.yellow;
            // }
        }

        public void Interact()
        {
            if (_readyQuests.Count > 0)
            {
                QuestManager.Instance.lastQuest = _readyQuests[0];
                UIManager.Instance.ShowPopup("BringQuestPopup");
            }
            else if (_availableQuests.Count > 0)
            {
                QuestManager.Instance.lastQuest = _availableQuests[0];
                UIManager.Instance.ShowPopup("TakeQuestPopup");
            }
        }

        private void HandleQuestTaken(Quest quest)
        {
            if (quest.questGiver != npc) return;
            _availableQuests.Remove(quest);

            foreach (var objective in quest.questObjectives)
            {
                if (objective.type == ObjectiveType.Talk && objective.questReceiver == npc)
                {
                    _readyQuests.Add(quest);
                    
                    var receiver = FindObjectsOfType<QuestNPC>().FirstOrDefault(r => r.npc == objective.questReceiver);
                    receiver?.UpdateQuests();
                    receiver?.UpdateQuestIcon();
                }
                else if (objective.type is ObjectiveType.Collect or ObjectiveType.Explore or ObjectiveType.Kill)
                {
                    _activeQuests.Add(quest);
                }
            }

            UpdateQuestIcon();
        }

        private void HandleQuestBring(Quest quest)
        {
            if (quest.questObjectives.Any(obj => obj.questReceiver == npc))
            {
                _readyQuests.Remove(quest);
                UpdateQuestIcon();
            }
        }
    }
}