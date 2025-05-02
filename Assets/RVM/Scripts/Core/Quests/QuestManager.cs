using System.Collections.Generic;
using UnityEngine;

namespace RVM.Scripts.Core.Quests
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance { get; private set; }

        public Quest[] quests;
        public List<Quest> activeQuests;
        public List<Quest> doneQuests;

        public void Awake()
        {
            if (Instance != this && Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void AddQuest()
        {
        }

        public void UpdateQuests()
        {
            
        }

    }
}