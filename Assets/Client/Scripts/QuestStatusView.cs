using TMPro;
using UnityEngine;

namespace LD52
{
    public class QuestStatusView : MonoBehaviour
    {
        public TMP_Text Description;
        public TMP_Text Status;
        
        public void Set(Quest quest)
        {
            Description.text = quest.ToDescription();
            Status.text = quest.QuestState.ToString();
        }
    }
}