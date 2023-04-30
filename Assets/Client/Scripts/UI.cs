using UnityEngine;

namespace LD52
{
    public class UI : MonoBehaviour
    {
        public static UI Instance;

        private void Awake()
        {
            Instance = this;
        }

        public QuestBoard QuestBoard;
        public QuestWindow QuestWindow;
        public InventoryView InventoryView;
        public DiaryView DiaryView;
        public HUD HUD;
    }
}