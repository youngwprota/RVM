using RVM.Scripts.Core.Items;
using RVM.Scripts.Core.NPCS;

namespace RVM.Scripts.Core.Quests
{
    [System.Serializable]
    public class QuestObjective
    {
        public ObjectiveType type;
        public NPC questReceiver;           
        public Item targetItem;    
        public string targetID;        
        public int requiredAmount;
    }

}