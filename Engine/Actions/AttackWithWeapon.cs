using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Actions
{
    public class AttackWithWeapon : BaseAction, IAction
    {
        private readonly int _maximumDamage;
        private readonly int _minimumDamage;
      
        //constructor accepts weapon paramaters to attack with
        public AttackWithWeapon(GameItem itemInUse, int minimumDamage, int maximumDamage) : base(itemInUse)
        {
            //check if item to attack with is a weapon, has a higher min dmg of 0, and greater or equal max dmg to min dmg
            if(itemInUse.category != GameItem.ItemCategory.Weapon)
            {
                throw new ArgumentException($"{itemInUse.name} is not a weapon.");
            }

            if(_minimumDamage < 0)
            {
                throw new ArgumentException("minimumDamage must be 0 or higher.");
            }

            if(_maximumDamage < _minimumDamage)
            {
                throw new ArgumentException("maxiumDamage must be >= minimumDamage.");
            }

            _minimumDamage = minimumDamage;
            _maximumDamage = maximumDamage;
        }

        //executes attack
        public void Execute(LivingEntity actor, LivingEntity target)
        {
            int damage = RandomNumberGenerator.numberBetween(_minimumDamage, _maximumDamage);

            //actor and target checks whether entity initiating attack is player or monster
            string actorName = (actor is Player) ? "You" : $"The {actor.name}";
            string targetName = (target is Player) ? "you" : $"the {target.name}";

            if(damage == 0)
            {
                ReportResult($"{actorName} dealt 0 damage to {targetName}.");
            }
            else
            {
                ReportResult($"{actorName} hit {targetName} for {damage} damage.");
                target.takeDamage(damage);
            }
        }
    }
}
