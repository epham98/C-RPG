using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class QuestStatus : BaseNotificationClass
    {
        private bool _isCompleted;

        public Quest playerQuest { get; }
        public bool isCompleted
        {
            get { return _isCompleted; }
            set
            {
                _isCompleted = value;
                OnPropertyChanged(nameof(isCompleted));
            }
        }

        public QuestStatus(Quest quest)
        {
            playerQuest = quest;
            isCompleted = false;
        }
    }
}
