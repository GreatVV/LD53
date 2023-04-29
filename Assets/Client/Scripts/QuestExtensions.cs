namespace LD52
{
    public static class QuestExtensions
    {
        public static string ToDescription(this Quest quest)
        {
            //todo get object back from NetworkID
            return $@"Take {quest.ItemID} from {quest.From} and deliver to {quest.To}";
        }
    }
}