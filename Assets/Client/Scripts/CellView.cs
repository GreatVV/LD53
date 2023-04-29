using UnityEngine;

namespace LD52
{
    public class CellView : MonoBehaviour
    {
        public Vector2Int Position;
        public GameObject Taken;

        public void SetTaken(bool isTaken)
        {
            Taken.SetActive(isTaken);
        }
    }
}