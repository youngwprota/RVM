using System.Collections.Generic;
using System.Linq;
using RVM.Scripts.Core.Quests;
using UnityEngine;

namespace RVM.Scripts.Core.NPC
{
    public class NpcManager : MonoBehaviour
    {
        public static NpcManager Instance;

        private Dictionary<string, Npc> NpcDictionary;
        public List<string> npcsID;
        public List<Npc> npcs;

        private Dictionary<string, QuestNpc> QuestNpcsDictionary;
        public List<string> QuestNpcsID;
        public List<Npc> QuestNpcs;
        
        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            InitializeNpcDictionary();
            InitializeQuestNpcDictionary();
        }

        private void InitializeNpcDictionary()
        {
            NpcDictionary = new Dictionary<string, Npc>();
    
            if (npcsID.Count != npcs.Count)
            {
                Debug.LogError("Количество ID NPC не совпадает с количеством NPC объектов");
                return;
            }
    
            for (var i = 0; i < npcsID.Count; i++)
            {
                NpcDictionary[npcsID[i]] = npcs[i];
            }
        }

        private void InitializeQuestNpcDictionary()
        {
            QuestNpcsDictionary = new Dictionary<string, QuestNpc>();

            if (QuestNpcsID.Count != QuestNpcs.Count)
            {
                Debug.LogError("Количество ID QuestNPC не совпадает с количеством QuestNPC объектов");
                return;
            }
            
            for (var i = 0; i < QuestNpcsID.Count; i++)
            {
                if (QuestNpcs[i] is QuestNpc questNpc)
                {
                    QuestNpcsDictionary[QuestNpcsID[i]] = questNpc;
                }
            }
        }

        public Npc GetNpcByID(string npcID)
        {
            return NpcDictionary.GetValueOrDefault(npcID);
        }
        
        public QuestNpc GetQuestNpcByID(string npcID)
        {
            return QuestNpcsDictionary.GetValueOrDefault(npcID);
        }
    }
}