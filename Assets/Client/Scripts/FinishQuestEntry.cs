using System.Text;
using Helpers.Collections;

namespace LD52
{
    internal class FinishQuestEntry : DiaryEntry
    {
        private string _description;
        string[] deliveryReactions = {
            "Wow, that was fast!",
            "Thank you for the prompt service.",
            "I appreciate your timely delivery.",
            "Impressive! Arrived earlier than expected.",
            "Great job, well packaged.",
            "This is exactly what I ordered.",
            "Nice work, everything is in order.",
            "I'm happy with my purchase.",
            "Thanks for making this so easy.",
            "The delivery person was very polite.",
            "Good communication, smooth process.",
            "Just what I needed, right on time.",
            "The tracking updates were helpful.",
            "I'll definitely order from you again.",
            "Great value for the price.",
            "Your service never disappoints.",
            "You've exceeded my expectations.",
            "Keep up the good work!",
            "Securely packaged, no damage.",
            "You've earned a loyal customer."
        };

        public FinishQuestEntry(Quest takenQuest)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(
                $"Delivered {takenQuest.GetItemLocalizedName()} to {takenQuest.GetToLocalizedName()}");
            stringBuilder.Append($"He said:\" {deliveryReactions.RandomElement()} \"");

            _description = stringBuilder.ToString();
        }

        public override string ToDescription => _description;
    }
}