using System.Collections.Generic;
using System.Linq;
using RVM.Scripts.Core.Interfaces;
using RVM.Scripts.Core.Quests;
using RVM.Scripts.Core.UI;
using UnityEngine;

namespace RVM.Scripts.Core.NPCS
{
    public class QuestGiver : MonoBehaviour, IInteractable
    {
        public NPC npc;
        public GameObject questAvailableIcon;
        public GameObject questActiveIcon;

        public SpriteRenderer questAvailableIconSprite;
        public SpriteRenderer questActiveIconSprite;
        public List<Quest> _availableQuests = new List<Quest>();
        public List<Quest> _unavailableQuests = new List<Quest>();
        public List<Quest> _activeQuests = new List<Quest>();
        public List<Quest> _readyQuests = new List<Quest>();
        public List<Quest> _doneQuests = new List<Quest>();

        public void Start()
        {
            QuestManager.Instance.OnQuestTaken += HandleQuestTaken;
            QuestManager.Instance.OnQuestBring += HandleQuestBring;

            UpdateQuests();
            UpdateQuestIcon();
        }

        private void OnDestroy()
        {
            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.OnQuestTaken -= HandleQuestTaken;
                QuestManager.Instance.OnQuestBring -= HandleQuestBring;

            }
        }

        private void UpdateQuests()
        {
            _availableQuests.Clear();
            _activeQuests.Clear();
            _unavailableQuests.Clear();
            _readyQuests.Clear();
            _doneQuests.Clear();
            
            foreach (var quest in QuestManager.Instance.availableQuests.Where(quest => quest.questGiver == npc))
            {
                _availableQuests.Add(quest);
            }

            foreach (var quest in QuestManager.Instance.activeQuests.Where(quest => quest.questGiver == npc))
            {
                _activeQuests.Add(quest);
            }

            foreach (var quest in QuestManager.Instance.unavailableQuests.Where(quest => quest.questGiver == npc))
            {
                _unavailableQuests.Add(quest);
            }

            foreach (var quest in QuestManager.Instance.readyQuests.Where(quest => quest.questGiver == npc))
            {
                _readyQuests.Add(quest);
            }

            foreach (var quest in QuestManager.Instance.doneQuests.Where(quest => quest.questGiver == npc))
            {
                _doneQuests.Add(quest);
            }
        }

        private void UpdateQuestIcon()
        {
            questActiveIcon.SetActive(false);
            questAvailableIcon.SetActive(false);

            if (_availableQuests.Count > 0)
            {
                questAvailableIcon.SetActive(true);
                questActiveIcon.SetActive(false);

                questAvailableIconSprite.color = Color.yellow;
            }

            if (_availableQuests.Count > 0 && _activeQuests.Count > 0)
            {
                questAvailableIcon.SetActive(true);
                questActiveIcon.SetActive(false);

                questAvailableIconSprite.color = Color.yellow;
            }

            if (_availableQuests.Count == 0 && _activeQuests.Count > 0)
            {
                questActiveIcon.SetActive(true);
                questAvailableIcon.SetActive(false);

                questActiveIconSprite.color = Color.grey;
            }
        }

        public void Interact()
        {
            var reciver = GetComponent<QuestReceiver>();
            if (reciver != null && reciver._readyQuests.Count > 0 )
            {
                QuestManager.Instance.lastQuest = reciver._readyQuests[0];
                UIManager.Instance.ShowPopup("BringQuestPopup");
                return;
            }
            if (_availableQuests.Count > 0)
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
                if (objective is { type: ObjectiveType.Talk })
                {
                    _readyQuests.Add(quest);
                    foreach (var obj in quest.questObjectives)
                    {
                        var receiver = FindObjectsOfType<QuestReceiver>()
                            .FirstOrDefault(r => r.npc == obj.questReceiver);
                        if (receiver != null)
                        {
                            receiver.UpdateQuests();
                            receiver.UpdateQuestIcon();
                        }
                    }
                }

                if (objective is { type: ObjectiveType.Collect })
                {
                    _activeQuests.Add(quest);
                }

                if (objective is { type: ObjectiveType.Explore })
                {
                    _activeQuests.Add(quest);
                }

                if (objective is { type: ObjectiveType.Kill })
                {
                    _activeQuests.Add(quest);
                }
            }


            UpdateQuestIcon();
        }
        
        private void HandleQuestBring(Quest quest)
        {
            UpdateQuests();
            UpdateQuestIcon();
        }

    }
}