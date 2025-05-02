using System;
using System.Collections.Generic;
using System.Linq;
using RVM.Scripts.Core.NPCS;
using UnityEngine;

namespace RVM.Scripts.Core.Quests
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance { get; private set; }

        public Quest[] quests;
        public List<Quest> activeQuests;
        public List<Quest> availableQuests;
        public List<Quest> unavailableQuests;
        public List<Quest> readyQuests;
        public List<Quest> doneQuests;

        public Quest lastQuest;

        public void Awake()
        {
            if (Instance != this && Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (var quest in quests)
            {
                if (quest.questStatus == QuestStatus.Active)
                {
                    activeQuests.Add(quest);
                }

                if (quest.questStatus == QuestStatus.Ready)
                {
                    readyQuests.Add(quest);
                }


                if (quest.questRequirements.Length == 0 && quest.questStatus == QuestStatus.Untaken)
                {
                    availableQuests.Add(quest);
                }
                else
                {
                    foreach (var req in quest.questRequirements)
                    {
                        if (quest.questStatus == QuestStatus.Untaken)
                        {
                            if (req.completed)
                            {
                                availableQuests.Add(quest);
                            }
                            else
                            {
                                unavailableQuests.Add(quest);
                            }
                        }
                    }
                }
            }
        }
        
        public void AddQuest(Quest quest)
        {
            availableQuests.Remove(quest);

            foreach (var objective in quest.questObjectives)
            {
                if (objective is { type: ObjectiveType.Talk })
                {
                    quest.questStatus = QuestStatus.Ready;
                    readyQuests.Add(quest);
                }

                if (objective is { type: ObjectiveType.Collect })
                {
                    quest.questStatus = QuestStatus.Active;
                    activeQuests.Add(quest);
                }

                if (objective is { type: ObjectiveType.Explore })
                {
                    quest.questStatus = QuestStatus.Active;
                    activeQuests.Add(quest);
                }

                if (objective is { type: ObjectiveType.Kill })
                {
                    quest.questStatus = QuestStatus.Active;
                    activeQuests.Add(quest);
                }
            }

            OnQuestTaken?.Invoke(quest);
        }

        public void BringQuest(Quest quest)
        {
            if (quest.questStatus != QuestStatus.Ready) return;

            UpdateQuests(quest); 
    
            doneQuests.Add(quest);
            readyQuests.Remove(quest);
            
            OnQuestBring?.Invoke(quest);
        }

        public void UpdateQuests(Quest quest)
        {
            quest.questStatus = QuestStatus.Done;
            for (int i = unavailableQuests.Count - 1; i >= 0; i--)
            {
                var q = unavailableQuests[i];
                foreach (var req in q.questRequirements)
                {
                    bool done = true;
                    foreach (var doneQuest in req.doneQuests)
                    {
                        if (doneQuest.questStatus != QuestStatus.Done)
                        {
                            done = false;
                            break;
                        }
                    }

                    req.completed = done;

                    if (req.completed)
                    {
                        availableQuests.Add(q);
                        unavailableQuests.RemoveAt(i); 
                        break; 
                    }
                }
            }

        }

        public event Action<Quest> OnQuestTaken;
        public event Action<Quest> OnQuestBring;

    }
}