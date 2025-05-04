using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace RVM.Scripts.Core.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        
        public List<Popup> listPopup = new() ;
        
        public Popup lastClosedPopup;

        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void ShowPopup(string popupName)
        {
            foreach (var popup in listPopup.Where(popup => popup.name == popupName && !popup.gameObject.activeSelf))
            {
                popup.Show();
            }
        }

        
    }
}