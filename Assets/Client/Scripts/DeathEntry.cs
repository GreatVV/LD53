using System;

namespace LD52
{
    public class DeathEntry : DiaryEntry
    {
        public DateTime DeathTime;
        public string KilledBy;

        public override string ToDescription => $"Killed by {KilledBy} on {DeathTime.ToShortDateString()}";
    }
}