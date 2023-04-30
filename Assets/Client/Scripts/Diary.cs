using System;
using System.Collections.Generic;

namespace LD52
{
    public class Diary
    {
        public List<DiaryEntry> Entries = new();
        private int _unreadEntries;

        public static event Action<int> NotificationChanged ;

        public int UnreadEntries
        {
            get => _unreadEntries;
            set
            {
                _unreadEntries = value;
                NotificationChanged?.Invoke(value);
            }
        }

        public void AddEntry(DiaryEntry entry)
        {
            Entries.Add(entry);
            UnreadEntries++;
        }
    }
}