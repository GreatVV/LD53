using UnityEngine;

namespace LD52
{
    public class QuestWindow : MonoBehaviour
    {
        public RectTransform Root;
        public QuestStatusView Prefab;

        public GameObject NoQuestsRoot;
        
        public void Show(Quester quester)
        {
            gameObject.SetActive(true);
            Root.DestroyChildren();
            
            NoQuestsRoot.SetActive(quester.TakenQuests.Count == 0);
            
            foreach (var q in quester.TakenQuests)
            {
                var instance = Instantiate(Prefab, Root);
                instance.Set(q);
            }
        }
    }
}