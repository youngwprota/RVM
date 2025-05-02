using RVM.Scripts.Core.Items;
using UnityEditor;

namespace RVM.Scripts.Core.Quests
{
    [System.Serializable]
    public class QuestReward
    {
        public int experience;
        public int gold;
        public Item item;
    }
}