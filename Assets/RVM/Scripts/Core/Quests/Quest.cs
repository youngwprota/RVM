using RVM.Scripts.Core.NPC;
using RVM.Scripts.Core.Quests.Utils;
using UnityEngine;

namespace RVM.Scripts.Core.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Quests/Quest")]
    public class Quest : ScriptableObject
    {
        public string title;
        [TextArea] public string description;
        
        public QuestStatus questStatus;
        public QuestObjective questObjective;
        public QuestRequirements questRequirements;
        public QuestReward questReward;
        
        public string questGiverID;
        public string questReceiverID;
        
        public bool available = false;
    }
}
