using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//manages items and quantities to be turned in for quests

namespace Engine.Models
{
    public class ItemQuantity
    {
        public int itemID { get; }
        public int quantity { get; }

        public ItemQuantity(int itmID, int itmQuantity)
        {
            itemID = itmID;
            quantity = itmQuantity;
        }
    }
}
