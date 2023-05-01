using System.Text;
using Helpers.Collections;

namespace LD52
{
    public class QuestDiaryEntry : DiaryEntry
    {
        public string Description;
        
        string[] optimisticPhrases = {
            "The future is bright and full of hope.",
            "Every challenge is an opportunity.",
            "Dream big and work hard to achieve it.",
            "Success is right around the corner.",
            "There's always a silver lining.",
            "Together, we can overcome anything.",
            "Positive thoughts create positive results.",
            "Tomorrow is a new day to shine.",
            "Kindness is contagious; spread it around.",
            "With determination, anything is possible."
        };

        
        public QuestDiaryEntry(Quest quest)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Took a delivery from {quest.GetFromLocalizedName()} to {quest.GetToLocalizedName()}.");
            stringBuilder.Append(optimisticPhrases.RandomElement());

            Description = stringBuilder.ToString();
        }

        public override string ToDescription => Description;
    }
}