using RVM.Scripts.Core.Interfaces;
using UnityEngine;

namespace RVM.Scripts.Core.Quests
{
    public class QuestGiver : MonoBehaviour, IInteractable
    {
        public NPC.NPC npc;

        public Quest[] quests;
        public int currentQuest = 0;

        public void Start()
        {
            foreach (var quest in quests)
            {
                
            }
        }

        public void Update()
        {

        }

        public void Interact()
        {
            Debug.Log("Interact with " + npc.npcName);
        }
    }
}