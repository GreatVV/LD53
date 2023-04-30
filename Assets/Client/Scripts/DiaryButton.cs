using System.Globalization;
using LeopotamGroup.Globals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LD52
{
    public class DiaryButton : MonoBehaviour
    {
        public Button Button;
        public GameObject NotificationRoot;
        public TMP_Text UnreadNotification;

        public void Start()
        {
            Diary.NotificationChanged += OnAmountChanged;
            
            OnAmountChanged(Service<RuntimeData>.Get().Diary.UnreadEntries);
        }

        private void OnDestroy()
        {
            Diary.NotificationChanged -= OnAmountChanged;
        }

        private void OnAmountChanged(int unread)
        {
            NotificationRoot.SetActive(unread > 0);
            UnreadNotification.text = unread.ToString(CultureInfo.InvariantCulture);
        }
    }
}