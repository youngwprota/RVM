using System;
using RVM.Scripts.Core.Quests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RVM.Scripts.Core.UI.Popups
{
    public class BringQuestPopup : Popup
    {
        public TMP_Text questTitle;
        public TMP_Text questDescription;
        public TMP_Text goldValue;
        public TMP_Text experienceValue;

        public void Exit()
        {
            Hide();
        }

        public void BringQuest()
        {
            QuestManager.Instance.BringQuest(QuestManager.Instance.lastQuest);
            Hide();
        }

        public void OnEnable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            questTitle.text = QuestManager.Instance.lastQuest.title;
            questDescription.text = QuestManager.Instance.lastQuest.description;
            goldValue.text = QuestManager.Instance.lastQuest.questReward.gold.ToString();
            experienceValue.text = QuestManager.Instance.lastQuest.questReward.experience.ToString();
            
        }
        
        public void OnDisable()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            questTitle.text = "";
            questDescription.text = "";
            goldValue.text = "";
            experienceValue.text = "";
        }
    }
}