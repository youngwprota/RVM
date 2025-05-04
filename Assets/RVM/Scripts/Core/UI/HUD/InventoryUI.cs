using System.Collections.Generic;
using RVM.Scripts.Core.Items;
using RVM.Scripts.Core.Player.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RVM.Scripts.Core.UI.HUD
{
    public class InventoryUI : MonoBehaviour
    {
        public  List<Image> _slotsImages;
        public List<TMP_Text> _slotsCountValue;
        
        public Inventory inventory;
        
        private void OnEnable()
        {
            if (inventory != null)
            {
                inventory.OnInventoryChanged += UpdateInventoryUI;
            }
        }

        private void OnDisable()
        {
            if (inventory != null)
            {
                inventory.OnInventoryChanged -= UpdateInventoryUI;
            }
        }

        private void Start()
        {
            UpdateInventoryUI();
        }

        private void UpdateInventoryUI()
        {
            for (int i = 0; i < _slotsImages.Count; i++)
            {
                _slotsImages[i].sprite = null;
                _slotsImages[i].enabled = false;
                _slotsCountValue[i].text = "";
            }

            int slotIndex = 0;
            foreach (var item in inventory.inventory)
            {
                if (slotIndex >= _slotsImages.Count) break;

                var itemIcon = GetItemIcon(item.Key);
            
                _slotsImages[slotIndex].sprite = itemIcon;
                _slotsImages[slotIndex].enabled = true;
            
                if (item.Value > 1)
                {
                    _slotsCountValue[slotIndex].text = item.Value.ToString();
                }
                else
                {
                    _slotsCountValue[slotIndex].text = "";
                }

                slotIndex++;
            }
        }
        
        private Sprite GetItemIcon(string itemID)
        {
            var itemIcon = Resources.Load<Sprite>($"Icons/{itemID}");
            return itemIcon;
        }

        
    }
}