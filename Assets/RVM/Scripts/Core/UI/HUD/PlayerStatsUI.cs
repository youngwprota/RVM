using System;
using RVM.Scripts.Core.Quests;
using TMPro;
using UnityEngine;

namespace RVM.Scripts.Core.UI.HUD
{
    public class PlayerStatsUI : MonoBehaviour
    {
        public TMP_Text goldValue;
        public TMP_Text experienceValue;
        
        // TODO: Need PlayerStatsController 
        public int playerGold = 0;
        public int playerExperience = 0;

        public void Start()
        {
            QuestManager.Instance.OnQuestReceived += UpdateStatsUI;
        }

        public void OnDisable()
        {
            QuestManager.Instance.OnQuestReceived -= UpdateStatsUI;
        }
        
        // TODO: Need PlayerStatsController 
        private void UpdateStatsUI(Quest quest)
        {
            playerGold += quest.questReward.gold;
            playerExperience += quest.questReward.experience;
            
            goldValue.text = playerGold.ToString();
            experienceValue.text = playerExperience.ToString();
        }
    }
}