using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//class to execute function of encountering monster at a set chance

namespace Engine.Models
{
    public class MonsterEncounter
    {
        public int monsterID { get; }
        public int encounterChance { get; set; }

        public MonsterEncounter(int monID, int monChance)
        {
            monsterID = monID;
            encounterChance = monChance;
        }
    }
}
