using System.Collections;
using TMPro;
using UnityEngine;

namespace LD52
{
    public class WinScreen : MonoBehaviour
    {
        public TMP_Text Timer;
        private float TimeLeft;
        public GameObject Root;
        public Camera Camera;
        public float Delay = 5;

        public void Show(float timeTillRestart)
        {
            TimeLeft = timeTillRestart;
            gameObject.SetActive(true);
            Root.SetActive(false);
            if (Camera.main)
            {
                Camera.main.gameObject.SetActive(false);
            }
            Camera.gameObject.SetActive(true);
            UI.Instance.HUD.gameObject.SetActive(false);
            StartCoroutine(ShowRoot());
        }

        private IEnumerator ShowRoot()
        {
            yield return new WaitForSeconds(Delay);
            Root.gameObject.SetActive(true);
        }

        private void Update()
        {
            TimeLeft -= Time.deltaTime;
            TimeLeft = Mathf.Max( 0, TimeLeft);
            Timer.text = $"Restart game in {TimeLeft:F1}s";
        }
    }
}