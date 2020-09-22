using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class GroupedInventoryItem : BaseNotificationClass
    {
        private GameItem _item;
        private int _quantity;

        public GameItem item
        {
            get { return _item; }
            set
            {
                _item = value;
                OnPropertyChanged(nameof(item));
            }
        }
        public int quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(quantity));
            }
        }

        public GroupedInventoryItem(GameItem groupItem, int groupQuantity)
        {
            item = groupItem;
            quantity = groupQuantity;
        }
    }
}
