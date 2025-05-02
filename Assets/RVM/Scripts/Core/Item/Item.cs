using UnityEngine;

namespace RVM.Scripts.Core.Item
{
    [CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
    public class Item : ScriptableObject
    {
        public string itemID;
        public string itemName;
        public Rarity rarity;
    }
}