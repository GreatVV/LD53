using UnityEngine;

namespace LD52
{
    public class QuestBoard : MonoBehaviour
    {
        public Transform Root;
        public QuestBoardView QuestBoardViewPrefab;

        public void Show(QuestManager questManager, Quester quester)
        {
            gameObject.SetActive(true);
            
            Root.DestroyChildren();

            foreach (var quest in questManager.PossibleQuests)
            {
                var instance = Instantiate(QuestBoardViewPrefab, Root);
                instance.Set(quest, quester);
            }
        }
    }
}