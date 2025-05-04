using UnityEngine;

namespace RVM.Scripts.Core.UI
{
    public class Popup : MonoBehaviour
    {
        public string popupName;

        public void Show()
        {
            gameObject.SetActive(true);
        }
        
                
        public virtual void Hide()
        {
            UIManager.Instance.lastClosedPopup = this;
            gameObject.SetActive(false);
        }

    }
}