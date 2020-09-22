using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//creates a trader to buy and sell items from

namespace Engine.Models
{
    //creates trader entity with inheritance from livingentity and basenotification classes
    public class Trader : LivingEntity
    {
        public Trader(string name) : base(name, 9999, 9999, 9999)
        {
        }
    }
}
