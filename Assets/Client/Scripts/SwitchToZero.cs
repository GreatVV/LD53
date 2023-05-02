using System.Collections;
using Fusion;
using UnityEngine;

namespace LD52
{
    public class SwitchToZero : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return default;
            yield return default;
            yield return default;
            yield return default;

            FindFirstObjectByType<NetworkRunner>().SetActiveScene(0);
        }
    }
}