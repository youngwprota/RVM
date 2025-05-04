using System.Collections.Generic;
using UnityEditor;

namespace RVM.Scripts.Core.Quests.Utils
{
    [System.Serializable]
    public class QuestObjective
    {
        public ObjectiveType type;
        
        // ItemID and Amount
        public Dictionary<string, int> Items = new Dictionary<string, int>();
        public List<string> itemsID;
        public List<int> itemsCount;
    }
}