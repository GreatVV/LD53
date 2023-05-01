using System.Text;
using Helpers.Collections;

namespace LD52
{
    internal class TakeQuestItemEntry : DiaryEntry
    {
        private string _description;
        
        string[] pickupPhrases = {
            "Hello, I'm here to pick up your package.",
            "Good day! I've arrived for the scheduled pickup.",
            "Hi, is this the correct location for the pickup?",
            "Can you please confirm the package details?",
            "Do you have any special handling instructions?",
            "Thank you for having the package ready.",
            "Is there anything fragile inside the package?",
            "Could you please sign the pickup confirmation?",
            "I'll make sure your package arrives safely.",
            "What's the destination address for this package?",
            "Can you please provide the package's dimensions?",
            "Do you have a preferred delivery time?",
            "Is this package part of a multiple-item shipment?",
            "Would you like to add any insurance to this shipment?",
            "Please let me know if you have any concerns.",
            "Have you attached the shipping label to the package?",
            "I'll handle your package with care.",
            "Is there a contact number for the recipient?",
            "Do you need any additional shipping services?",
            "Thank you, I'll take it from here."
        };


        public TakeQuestItemEntry(Quest takenQuest)
        {
            var sb = new StringBuilder();
            sb.AppendLine(
                $"Picked up {takenQuest.GetItemLocalizedName()} from {takenQuest.GetFromLocalizedName()} ");
            sb.Append($"His reaction to \"{pickupPhrases.RandomElement()}\" was priceless");
            
            _description = sb.ToString();
        }

        public override string ToDescription => _description;
    }
}