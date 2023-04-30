using LeopotamGroup.Globals;
using UnityEngine;
using UnityEngine.UI;

namespace LD52
{
    public class DiaryView : MonoBehaviour
    {
        public RectTransform Root;
        public DiaryEntryView Prefab;
        public ScrollRect ScrollRect;

        public void Show(Diary diary)
        {
            gameObject.SetActive(true);
            Root.DestroyChildren();

            foreach (var entry in diary.Entries)
            {
                var diaryEntryView = Instantiate(Prefab, Root);
                diaryEntryView.Set(entry);
            }

            ScrollRect.verticalNormalizedPosition = 1;

            Service<RuntimeData>.Get().Diary.UnreadEntries = 0;
        }
    }
}