namespace LD52
{
    public static class QuestExtensions
    {
        public static string ToDescription(this Quest quest)
        {
            return $@"Take {quest.ItemID} from {quest.From.LocalizedName} and deliver to {quest.To.LocalizedName}";
        }
    }
}