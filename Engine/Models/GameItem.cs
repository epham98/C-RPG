using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Actions;

//class for game items

namespace Engine.Models
{
    public class GameItem
    {
        public ItemCategory category { get; }
        public int itemTypeID { get; }
        public string name { get; }
        public int price { get; }
        public bool isUnique { get; }
        public IAction action { get; set; }

        public enum ItemCategory
        {
            Misc,
            Weapon,
            Consumable
        }

        public GameItem(ItemCategory itemCat, int itmID, string itmName, int itmPrice, bool unique = false, IAction act = null)
        {
            category = itemCat;
            itemTypeID = itmID;
            name = itmName;
            price = itmPrice;
            isUnique = unique;
            action = act;
        }

        public void PerformAction(LivingEntity actor, LivingEntity target)
        {
            action?.Execute(actor, target);
        }

        //clone function
        //instantiates new game item w/ same property values to create clone
        public GameItem Clone()
        {
            return new GameItem(category, itemTypeID, name, price, isUnique, action);
        }
    }
}
