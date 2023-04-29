using UnityEngine;

namespace LD52
{
    public class QuestBoard : MonoBehaviour
    {
        public Transform Root;
        public QuestBoardView[] QuestBoardViewPrefabs;

        public void Show(QuestManager questManager, Quester quester)
        {
            gameObject.SetActive(true);
            
            Root.DestroyChildren();

            foreach (var quest in questManager.PossibleQuests)
            {
                var instance = Instantiate(QuestBoardViewPrefabs[Random.Range(0, QuestBoardViewPrefabs.Length)], Root);
                instance.Set(quest, quester);
            }
        }
    }
}