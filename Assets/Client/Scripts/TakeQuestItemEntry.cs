using System.Text;
using Helpers.Collections;

namespace LD52
{
    internal class TakeQuestItemEntry : DiaryEntry
    {
        private string _description;
        
        string[] pickupSentences = {
            "When I approached the door, I greeted the client with, 'Hello, I'm here to pick up your package.'",
            "Before leaving, I made sure to ask, 'Do you have any special handling instructions?'",
            "I kindly requested the client to sign the pickup confirmation by saying, 'Could you please sign the pickup confirmation?'",
            "I assured the client by telling them, 'I'll make sure your package arrives safely.'",
            "To double-check, I asked, 'What's the destination address for this package?'",
            "I inquired about the size of the item, 'Can you please provide the package's dimensions?'",
            "I wanted to make sure the delivery time was suitable, so I asked, 'Do you have a preferred delivery time?'",
            "It was important to know if there was more to pick up, so I asked, 'Is this package part of a multiple-item shipment?'",
            "I let the client know they could add extra protection by asking, 'Would you like to add any insurance to this shipment?'",
            "Before heading to my next pickup, I reassured the client by saying, 'Thank you, I'll take it from here.'"
        };



        public TakeQuestItemEntry(Quest takenQuest)
        {
            var sb = new StringBuilder();
            sb.AppendLine(
                $"Picked up {takenQuest.GetItemLocalizedName()} from {takenQuest.GetFromLocalizedName()} ");
            sb.Append($"{pickupSentences.RandomElement()}");
            
            _description = sb.ToString();
        }

        public override string ToDescription => _description;
    }
}