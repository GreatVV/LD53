using NaughtyAttributes;
using UnityEngine;

namespace LD52
{
    [CreateAssetMenu(menuName = "Game/Levels")]
    public class Levels : ScriptableObject
    {
        public int MaxLevel;
        public int[] ExpForLevels;

        public int StartValue;
        public int LevelMultipler;

        public int GetExpForLevel(int level)
        {
            var l = Mathf.Max(level, MaxLevel);
            return ExpForLevels[l];
        }

        public int GetLevel(int exp)
        {
            for(var i = 0; i < ExpForLevels.Length; i++)
            {
                if(exp < ExpForLevels[i]) return i;
            }
            return ExpForLevels[^1];
        }

        [Button]
        public void Calculate()
        {
            ExpForLevels = new int[MaxLevel];
            ExpForLevels[0] = StartValue;
            for(var i = 1; i < MaxLevel; i++)
            {
                ExpForLevels[i] = (2*StartValue + LevelMultipler*(i-1))/2*i;
            }
        }
    }
}