using Fusion;

namespace LD52
{
    public class QuestGiver : NetworkBehaviour
    {
        [Networked]
        public NetworkArray<ItemView> PossibleItems { get; }
        public string LocalizedName;
    }
}