using UnityEngine;

namespace RVM.Scripts.Core.NPC
{
    [CreateAssetMenu(fileName = "NPC", menuName = "NPC/NPC", order = 0)]
    public class NPC : ScriptableObject
    {
        public string npcID;
        public string npcName;
    }
}