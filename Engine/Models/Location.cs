using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Factories;

namespace Engine.Models
{
    //creates a location object and function for random monster encounters at location
    public class Location
    {
        public int xCoord { get; }
        public int yCoord { get; }
        public string name { get; }
        public string desc { get; }
        public string imgName { get; }
        public List<Quest> questAvailable { get; } = new List<Quest>();
        public List<MonsterEncounter> monsterHere { get; } = new List<MonsterEncounter>();
        public Trader traderHere { get; set; }

        public Location(int xCoordinate, int yCoordinate, string locName, string locDesc, string image)
        {
            xCoord = xCoordinate;
            yCoord = yCoordinate;
            name = locName;
            desc = locDesc;
            imgName = image;
        }

        public void AddMonster(int monID, int monChance)
        {
            if(monsterHere.Exists(m => m.monsterID == monID))
            {
                //monster has already been added to location, overwrite encounterchance w/ new number
                monsterHere.First(m => m.monsterID == monID).encounterChance = monChance;
            }
            else
            {
                //monster not yet at location, so add monster
                monsterHere.Add(new MonsterEncounter(monID, monChance));
            }
        }

        public Monster GetMonster()
        {
            if(!monsterHere.Any())
            {
                return null;
            }

            //total percentages of all monsters at location
            int totalChance = monsterHere.Sum(m => m.encounterChance);

            //select random number between 1 and total
            int randomNumber = RandomNumberGenerator.numberBetween(1, totalChance);

            //loop thru monster list adding monster's percentage chance of appearing to runningtotal var
            //when random num is lower than running total, monster returns
            int runningTotal = 0;

            foreach(MonsterEncounter monsterEncounter in monsterHere)
            {
                runningTotal += monsterEncounter.encounterChance;

                if(randomNumber <= runningTotal)
                {
                    return MonsterFactory.GetMonster(monsterEncounter.monsterID);
                }
            }

            //if there was a problem, return last monster in list
            return MonsterFactory.GetMonster(monsterHere.Last().monsterID);
        }
    }
}
