using LeopotamGroup.Globals;

namespace LD52
{
    public static class QuestExtensions
    {
        public static string ToDescription(this Quest quest)
        {
            var staticData = Service<StaticData>.Get();
            var items = staticData.Items;
            var mass = 0;
            if (items.TryGetByItemId(quest.ItemID.ToString(), out var item))
            {
                mass = item.ItemDescription.Mass;
            }

            var runner = Service<RuntimeData>.Get().Runner;
            var fromName = "Secret guy";
            if (runner.TryFindObject(quest.From, out var giver))
            {
                fromName = giver.GetComponent<QuestGiver>().LocalizedName;
            }
            
            var toName = "Other Secret guy";
            if (runner.TryFindObject(quest.To, out var target))
            {
                toName = target.GetComponent<QuestTarget>().LocalizedName;
            }

            return $@"Take {item.ItemDescription.LocalizedName} from {fromName} and deliver to {toName}. {(mass > 0 ? $"It weights {mass}" : "")}";
        }
    }
}