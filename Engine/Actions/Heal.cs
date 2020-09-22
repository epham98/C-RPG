using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models; 

//heal action to restore entity health

namespace Engine.Actions
{
    public class Heal : BaseAction, IAction
    {
        private readonly int _hpToHeal;

        public Heal(GameItem itemInUse, int hpToHeal) : base(itemInUse)
        {
            //checks if item is not useable for healing
            if(itemInUse.category != GameItem.ItemCategory.Consumable)
            {
                throw new ArgumentException($"{itemInUse.name} is not consumable.");
            }

            _hpToHeal = hpToHeal;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            string actorName = (actor is Player) ? "You" : $"The {actor.name}";
            string targetName = (target is Player) ? "yourself" : $"The {target.name}";

            ReportResult($"{actorName} healed {targetName} for {_hpToHeal} HP.");
            target.Heal(_hpToHeal);
        }
    }
}
