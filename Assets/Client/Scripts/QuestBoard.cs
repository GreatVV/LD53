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

            for (var index = 0; index < questManager.PossibleQuests.Count; index++)
            {
                var quest = questManager.PossibleQuests[index];
                var instance = Instantiate(QuestBoardViewPrefabs[index % QuestBoardViewPrefabs.Length], Root);
                instance.Set(quest, quester);
            }
        }
    }
}