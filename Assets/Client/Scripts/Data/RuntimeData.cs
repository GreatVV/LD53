using Fusion;

namespace LD52
{
    [System.Serializable]
    public class RuntimeData
    {
        public Diary Diary = new Diary();
        public NetworkRunner Runner;
        public Inventory Inventory;
        public Quester Quester;
        public Character PlayerCharacter;
    }
}