using System;
using Helpers.Collections;
using LeopotamGroup.Globals;
using UnityEngine;

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
                fromName = $"<color=\"black\">{ giver.GetComponent<QuestGiver>().LocalizedName }</color>";
            }

            return fromName;
        }

        public static string GetToLocalizedName(this Quest quest)
        {
            var runner = Service<RuntimeData>.Get().Runner;
            var toName = "Other Secret guy";
            if (runner.TryFindObject(quest.To, out var target))
            {
                toName = $"<color=\"black\">{ target.GetComponent<QuestTarget>().LocalizedName }</color>";
            }

            return toName;
        }

        public static string GetItemLocalizedName(this Quest quest)
        {
            var staticData = Service<StaticData>.Get();
            var items = staticData.Items;
            if (items.TryGetByItemId(quest.ItemID.ToString(), out var item))
            {
                return $"<color=#444444>{ item.ItemDescription.LocalizedName }</color>";
            }

            return "Secret Item";
        } 
        
        public static Sprite GetItemIcon(this Quest quest)
        {
            var staticData = Service<StaticData>.Get();
            var items = staticData.Items;
            if (items.TryGetByItemId(quest.ItemID.ToString(), out var item))
            {
                return item.ItemDescription.Icon;
            }

            return default;
        } 
        
        static string[] deliveryMissionSentences = {
            "Begin your quest by meeting {0}, who will provide you with the {1} to be delivered to {2}.",
            "Your task is to collect the {1} from {0} and ensure it reaches the hands of {2} without delay.",
            "{0} has entrusted you with the vital mission of delivering the {1} to {2}.",
            "The fate of the {1} lies in your hands; receive it from {0} and journey forth to find {2}.",
            "A legendary {1} must be transferred from {0} to {2}; do not fail this crucial mission.",
            "The task before you is to retrieve the {1} from {0} and make your way to {2} with haste.",
            "Embark on a daring mission to transport the {1} from the possession of {0} to {2}.",
            "The {1} requires safe passage from {0} to {2}; be the hero who ensures its successful delivery.",
            "A perilous journey awaits you as you take the {1} from {0} and venture onward to locate {2}.",
            "The {1} must be transferred from {0} to {2} with great care and urgency; accept this challenge and prove your worth."
        };

        
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

            return string.Format(deliveryMissionSentences.RandomElement(), fromName, quest.GetItemLocalizedName(), toName);
        }
    }
}