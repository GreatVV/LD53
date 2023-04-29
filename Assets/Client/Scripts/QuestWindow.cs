using UnityEngine;

namespace LD52
{
    public class QuestWindow : MonoBehaviour
    {
        public RectTransform Root;
        public QuestStatusView Prefab;
        
        
        public void Show(Quester quester)
        {
            gameObject.SetActive(true);
            Root.DestroyChildren();
            
            foreach (var q in quester.TakenQuests)
            {
                var instance = Instantiate(Prefab, Root);
                instance.Set(q);
            }
        }
    }
}