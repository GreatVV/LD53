using LeopotamGroup.Globals;

namespace LD52
{
    public static class QuestExtensions
    {
        public static string GetFromLocalizedName(this Quest quest)
        {
            var runner = Service<RuntimeData>.Get().Runner;
            var fromName = "Secret guy";
            if (runner.TryFindObject(quest.From, out var giver))
            {
                fromName = $"<b>{ giver.GetComponent<QuestGiver>().LocalizedName }</b>";
            }

            return fromName;
        }

        public static string GetToLocalizedName(this Quest quest)
        {
            var runner = Service<RuntimeData>.Get().Runner;
            var toName = "Other Secret guy";
            if (runner.TryFindObject(quest.To, out var target))
            {
                toName = $"<b>{ target.GetComponent<QuestTarget>().LocalizedName }</b>";
            }

            return toName;
        }

        public static string GetItemLocalizedName(this Quest quest)
        {
            var staticData = Service<StaticData>.Get();
            var items = staticData.Items;
            if (items.TryGetByItemId(quest.ItemID.ToString(), out var item))
            {
                return $"<b>{ item.ItemDescription.LocalizedName }</b>";
            }

            return "Secret Item";
        } 
        
        public static string ToDescription(this Quest quest)
        {
            var staticData = Service<StaticData>.Get();
            var items = staticData.Items;
            var mass = 0;
            if (items.TryGetByItemId(quest.ItemID.ToString(), out var item))
            {
                mass = item.ItemDescription.Mass;
            }

            var fromName = quest.GetFromLocalizedName();

            var toName = quest.GetToLocalizedName();

            return $@"Take {item.ItemDescription.LocalizedName} from {fromName} and deliver to {toName}. {(mass > 0 ? $"It weights {mass}" : "")}";
        }
    }
}