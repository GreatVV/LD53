using System;
using System.Collections;
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

        private void OnDestroy()
        {
            Instance = default;
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(2);
            Loading.SetActive(false);
            HUD.gameObject.SetActive(true);
        }

        public QuestBoard QuestBoard;
        public QuestWindow QuestWindow;
        public InventoryView InventoryView;
        public DiaryView DiaryView;
        public HUD HUD;
        public GameObject Loading;
        public WinScreen WinScreen;
    }
}