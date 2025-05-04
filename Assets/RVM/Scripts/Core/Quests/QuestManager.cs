using System;
using System.Collections.Generic;
using System.Linq;
using RVM.Scripts.Core.NPC;
using RVM.Scripts.Core.Quests.Utils;
using Unity.VisualScripting;
using UnityEngine;

namespace RVM.Scripts.Core.Quests
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance;

        public List<Quest> allQuests = new List<Quest>();

        public List<Quest> questsAvailableToGive = new List<Quest>();
        public List<Quest> questsUnavailableToGive = new List<Quest>();
        public List<Quest> questsAvailableToReceive = new List<Quest>();
        public List<Quest> questsUnavailableToReceive = new List<Quest>();

        public List<Quest> questsActive = new List<Quest>();
        public List<Quest> questsCompleted = new List<Quest>();
        public List<Quest> questsPrevious = new List<Quest>();

        public Quest lastQuest;

        private readonly Dictionary<Quest, Dictionary<string, int>> _collectQuestDictionary = new Dictionary<Quest, Dictionary<string, int>>();

        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Start()
        {
            InitializeQuestsToGive();
            InitializeQuestsToReceive();

            UpdateNpcQuests();
        }

        private void UpdateNpcQuests()
        {
            foreach (var questNpcID in NpcManager.Instance.QuestNpcsID)
            {
                GetNpcQuestsToGive(questNpcID);
                GetNpcQuestsToReceive(questNpcID);
                NpcManager.Instance.GetQuestNpcByID(questNpcID).UpdateQuestIcon();
            }
        }

        private void GetNpcQuestsToGive(string npcID)
        {
            foreach (var quest in questsAvailableToGive)
            {
                if (quest.questGiverID == npcID && CheckQuestStatus(quest) == QuestStatus.Untaken)
                {
                    if (!NpcManager.Instance.GetQuestNpcByID(npcID).questsAvailableToGive.Contains(quest))
                    {
                        if (NpcManager.Instance.GetQuestNpcByID(npcID).questsUnavailableToGive.Contains(quest))
                        {
                            NpcManager.Instance.GetQuestNpcByID(npcID).questsUnavailableToGive.Remove(quest);
                        }

                        NpcManager.Instance.GetQuestNpcByID(npcID).questsAvailableToGive.Add(quest);
                    }
                }
            }

            foreach (var quest in questsUnavailableToGive)
            {
                if (quest.questGiverID == npcID)
                {
                    if (!NpcManager.Instance.GetQuestNpcByID(npcID).questsUnavailableToGive.Contains(quest))
                        NpcManager.Instance.GetQuestNpcByID(npcID).questsUnavailableToGive.Add(quest);
                }
            }

            foreach (var quest in questsCompleted)
            {
                if (quest.questGiverID == npcID)
                {
                    if (NpcManager.Instance.GetQuestNpcByID(npcID).questsAvailableToGive.Contains(quest))
                    {
                        if (!NpcManager.Instance.GetQuestNpcByID(npcID).questsGiven.Contains(quest))
                        {
                            NpcManager.Instance.GetQuestNpcByID(npcID).questsGiven.Add(quest);
                        }

                        NpcManager.Instance.GetQuestNpcByID(npcID).questsAvailableToGive.Remove(quest);
                    }
                }
            }

            foreach (var quest in questsActive)
            {
                if (quest.questGiverID == npcID)
                {
                    if (NpcManager.Instance.GetQuestNpcByID(npcID).questsAvailableToGive.Contains(quest))
                    {
                        if (!NpcManager.Instance.GetQuestNpcByID(npcID).questsGiven.Contains(quest))
                        {
                            NpcManager.Instance.GetQuestNpcByID(npcID).questsGiven.Add(quest);
                        }

                        NpcManager.Instance.GetQuestNpcByID(npcID).questsAvailableToGive.Remove(quest);
                    }
                }
            }

            foreach (var quest in questsPrevious)
            {
                if (quest.questGiverID == npcID)
                {
                    if (NpcManager.Instance.GetQuestNpcByID(npcID).questsAvailableToGive.Contains(quest))
                    {
                        if (!NpcManager.Instance.GetQuestNpcByID(npcID).questsGiven.Contains(quest))
                        {
                            NpcManager.Instance.GetQuestNpcByID(npcID).questsGiven.Add(quest);
                        }

                        NpcManager.Instance.GetQuestNpcByID(npcID).questsAvailableToGive.Remove(quest);
                    }
                }
            }
        }

        private void GetNpcQuestsToReceive(string npcID)
        {
            foreach (var quest in questsAvailableToReceive)
            {
                if (quest.questReceiverID == npcID && CheckQuestStatus(quest) == QuestStatus.Ready)
                {
                    if (!NpcManager.Instance.GetQuestNpcByID(npcID).questsAvailableToReceive.Contains(quest))
                    {
                        if (NpcManager.Instance.GetQuestNpcByID(npcID).questsUnavailableToReceive.Contains(quest))
                        {
                            NpcManager.Instance.GetQuestNpcByID(npcID).questsUnavailableToReceive.Remove(quest);
                        }

                        NpcManager.Instance.GetQuestNpcByID(npcID).questsAvailableToReceive.Add(quest);
                    }
                }
            }

            foreach (var quest in questsUnavailableToReceive)
            {
                if (quest.questReceiverID == npcID)
                {
                    if (!NpcManager.Instance.GetQuestNpcByID(npcID).questsUnavailableToReceive.Contains(quest))
                        NpcManager.Instance.GetQuestNpcByID(npcID).questsUnavailableToReceive.Add(quest);
                }
            }

            foreach (var quest in questsPrevious)
            {
                if (quest.questReceiverID == npcID)
                {
                    if (NpcManager.Instance.GetQuestNpcByID(npcID).questsAvailableToReceive.Contains(quest))
                    {
                        if (!NpcManager.Instance.GetQuestNpcByID(npcID).questsReceived.Contains(quest))
                        {
                            NpcManager.Instance.GetQuestNpcByID(npcID).questsReceived.Add(quest);
                        }

                        NpcManager.Instance.GetQuestNpcByID(npcID).questsAvailableToReceive.Remove(quest);
                    }
                }
            }
        }

        private void InitializeQuestsToGive()
        {
            foreach (var quest in allQuests)
            {
                if (CheckQuestAvailability(quest) && CheckQuestStatus(quest) == QuestStatus.Untaken)
                {
                    questsAvailableToGive.Add(quest);
                }
                else
                {
                    questsUnavailableToGive.Add(quest);
                }
            }
        }

        private void InitializeQuestsToReceive()
        {
            foreach (var quest in allQuests)
            {
                if (CheckQuestAvailability(quest) && CheckQuestStatus(quest) == QuestStatus.Ready)
                {
                    questsAvailableToReceive.Add(quest);
                }
                else
                {
                    questsUnavailableToReceive.Add(quest);
                }
            }
        }

        private QuestStatus CheckQuestStatus(Quest quest)
        {
            return quest.questStatus;
        }

        private bool CheckQuestAvailability(Quest quest)
        {
            if (quest.questRequirements.doneQuests.Length == 0)
            {
                return true;
            }

            foreach (var doneQuest in quest.questRequirements.doneQuests)
            {
                if (doneQuest.questStatus != QuestStatus.Done)
                {
                    return false;
                }
            }

            return true;
        }

        public void GiveQuestToPlayer(Quest quest)
        {
            if (!questsAvailableToGive.Contains(quest) || questsActive.Contains(quest) ||
                CheckQuestStatus(quest) != QuestStatus.Untaken || !CheckQuestAvailability(quest)) return;

            questsAvailableToGive.Remove(quest);

            switch (quest.questObjective.type)
            {
                case ObjectiveType.Talk:
                    HandleGiveTalkQuest(quest);
                    break;
                case ObjectiveType.Collect:
                    HandGiveCollectQuest(quest);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            UpdateNpcQuests();
        }

        public void ReceiveQuestFromPlayer(Quest quest)
        {
            if (!questsAvailableToReceive.Contains(quest) || !questsCompleted.Contains(quest) ||
                CheckQuestStatus(quest) != QuestStatus.Ready) return;

            questsAvailableToReceive.Remove(quest);
            quest.questStatus = QuestStatus.Done;

            if (!questsPrevious.Contains(quest) && questsCompleted.Contains(quest))
            {
                questsPrevious.Add(quest);
                questsCompleted.Remove(quest);
            }

            if (quest.questObjective.type == ObjectiveType.Collect)
            {
                OnReceiveCollectQuest?.Invoke(quest);
            }
            
            OnQuestReceived?.Invoke(quest);
            UpdateQuestsAvailability();
            UpdateNpcQuests();
        }

        private void UpdateQuestsAvailability()
        {
            foreach (var quest in allQuests)
            {
                var available = true;
                foreach (var doneQuest in quest.questRequirements.doneQuests)
                {
                    if (doneQuest.questStatus != QuestStatus.Done)
                    {
                        available = false;
                    }
                }

                quest.available = available;
                if (available)
                {
                    if (!questsAvailableToGive.Contains(quest) && questsUnavailableToGive.Contains(quest))
                    {
                        questsAvailableToGive.Add(quest);
                        questsUnavailableToGive.Remove(quest);
                    }
                }
            }
        }

        private void HandleGiveTalkQuest(Quest quest)
        {
            quest.questStatus = QuestStatus.Ready;

            questsUnavailableToReceive.Remove(quest);
            questsAvailableToReceive.Add(quest);
            questsCompleted.Add(quest);
        }

        private void HandGiveCollectQuest(Quest quest)
        {
            quest.questStatus = QuestStatus.Active;
            questsActive.Add(quest);


            if (quest.questObjective.itemsID.Count != quest.questObjective.itemsCount.Count)
            {
                Debug.LogError("Количество ID NPC не совпадает с количеством NPC объектов");
                return;
            }

            for (var i = 0; i < quest.questObjective.itemsID.Count; i++)
            {
                quest.questObjective.Items[quest.questObjective.itemsID[i]] = quest.questObjective.itemsCount[i];
            }

            var progress = new Dictionary<string, int>();

            foreach (var kvp in quest.questObjective.Items)
            {
                progress[kvp.Key] = 0;
            }

            _collectQuestDictionary[quest] = progress;


            OnGiveCollectQuest?.Invoke(quest);
        }

        public void OnCollectQuestItemCollected(string itemID, int itemCount, Quest quest)
        {
            if (!_collectQuestDictionary.ContainsKey(quest)) return;
            if (_collectQuestDictionary[quest][itemID] == itemCount) return;

            _collectQuestDictionary[quest][itemID] = itemCount;
            if (_collectQuestDictionary[quest][itemID] == quest.questObjective.Items[itemID])
            {
                OnQuestItemCollected?.Invoke(itemID);
            }

            var allItemsCollected = true;

            foreach (var (targetID, targetCount) in quest.questObjective.Items)
            {
                if (!_collectQuestDictionary[quest].ContainsKey(targetID) || _collectQuestDictionary[quest][targetID] < targetCount)
                {
                    allItemsCollected = false;
                    break;
                }
            }

            if (allItemsCollected)
            {
                quest.questStatus = QuestStatus.Ready;
                
                questsUnavailableToReceive.Remove(quest);
                questsAvailableToReceive.Add(quest);
                
                questsActive.Remove(quest);
                questsCompleted.Add(quest);
                UpdateNpcQuests();
            }
        }

        public event Action<Quest> OnGiveCollectQuest;
        public event Action<Quest> OnReceiveCollectQuest;
        //ItemID required
        public event Action<string> OnQuestItemCollected;
        
        // Calculate rewards
        public event Action<Quest> OnQuestReceived;


    }
}