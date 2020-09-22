using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    //creates monster entity with inheritance from livingentity and basenotification
    //and unique monster properties
    public class Monster : LivingEntity
    {
        public string imgName { get; }
        public int rewardEXP { get; }

        //constructor accepts parameters and populates properties and adds location of image file for monster
        public Monster(string name, string monImg, int maxHP, int currentHP, int monEXP, int gold) :
            base(name, maxHP, currentHP, gold)
        {
            imgName = $"/Engine;component/Images/Monsters/{imgName}";
            rewardEXP = monEXP;
        }
    }
}
