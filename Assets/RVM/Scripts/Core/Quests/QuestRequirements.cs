using UnityEngine.Serialization;

namespace RVM.Scripts.Core.Quests
{
    [System.Serializable]
    public class QuestRequirements
    {
        public Quest[] doneQuests;
        public bool completed = false;
    }
}