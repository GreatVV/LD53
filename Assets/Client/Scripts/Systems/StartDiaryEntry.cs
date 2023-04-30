using System;

namespace LD52
{
    public class StartDiaryEntry : DiaryEntry
    {
        public DateTime StartTime;

        public StartDiaryEntry()
        {
            StartTime = DateTime.Today;
        }

        public override string ToDescription => $"I started my journey on sunny day of {StartTime.ToShortDateString()}";
    }
}