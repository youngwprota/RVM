using RVM.Scripts.Core.NPCS;
using UnityEngine;

namespace RVM.Scripts.Core.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Quests/Quest")]
    public class Quest : ScriptableObject
    {
        public string questID;
        public string title;
        [TextArea] public string description;

        public QuestStatus questStatus;
        public QuestObjective[] questObjectives;

        public QuestRequirements[] questRequirements;
        public QuestReward questReward;
        public NPC questGiver;
        
    }
}
