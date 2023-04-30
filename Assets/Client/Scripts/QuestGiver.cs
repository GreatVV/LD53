using Fusion;

namespace LD52
{
    public class QuestGiver : NetworkBehaviour
    {
        [Networked, Capacity(16)]
        public NetworkArray<string> PossibleItems { get; }
        public string LocalizedName;
    }
}