using System;
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
            string status;
            switch (quest.QuestState)
            {
                case QuestState.None:
                case QuestState.ReadyToBeTaken:
                case QuestState.NeedItem:
                    status = "Picking up";
                    break;
                case QuestState.Delivering:
                    status = "Delivering";
                    break;
                case QuestState.Delivered:
                case QuestState.Completed:
                    status = "Completed";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Status.text = status;
        }
    }
}