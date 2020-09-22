using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

//has property changed events to be inherited by player.cs and gamesession.cs w/o
//the need to create property changed handlers for said classes

namespace Engine
{
    public class BaseNotificationClass : INotifyPropertyChanged
    {
        //implementation of INotiftyPropertyChanged
        //notifies the user interface that values in player data and location have changed and needs updating
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
