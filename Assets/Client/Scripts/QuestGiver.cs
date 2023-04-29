using Fusion;

namespace LD52
{
    public class QuestGiver : NetworkBehaviour
    {
        [Networked]
        public NetworkArray<ItemView> PossibleItems { get; set; }
        public string LocalizedName;
    }
}