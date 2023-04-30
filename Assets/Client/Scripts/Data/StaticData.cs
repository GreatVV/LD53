using Fusion;
using UnityEngine;

namespace LD52
{
    [CreateAssetMenu(menuName = "Game/Static Data")]
    public class StaticData : ScriptableObject
    {
        public Formulas Formulas;
        public NetworkRunner RunnerPrefab;
        public Character Player;
        public UI UI;
    }
}