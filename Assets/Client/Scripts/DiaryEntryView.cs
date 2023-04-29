using TMPro;
using UnityEngine;

namespace LD52
{
    public class DiaryEntryView : MonoBehaviour
    {
        public TMP_Text Text;

        public void Set(DiaryEntry entry)
        {
            Text.text = entry.ToDescription;
        }
    }
}