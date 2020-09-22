using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

//class for creating monster objects for each battle. creating a new monster object each time ensures
//each monster will have its own random loot.

namespace Engine.Factories
{
    public static class MonsterFactory
    {
        public static Monster GetMonster(int monsterID)
        {
            //switch statement uses monsterID value passed into getmonster and looks for matching value
            //to determine which monster will be added
            switch (monsterID)
            {
                case 1:
                    Monster slime = new Monster("Slime", "slime.png", 4, 4, 5, 1);
                    slime.currentWeapon = ItemFactory.createItem(1501);
                    AddLootItem(slime, 9001, 50);
                    AddLootItem(slime, 9002, 75);
                    return slime;

                case 2:
                    Monster rat = new Monster("Rat", "Rat.png", 5, 5, 5, 1);
                    rat.currentWeapon = ItemFactory.createItem(1502);
                    AddLootItem(rat, 9003, 25);
                    AddLootItem(rat, 9004, 75);
                    return rat;

                case 3:
                    Monster jackFrost = new Monster("Frost Mite", "jack frost.png", 10, 10, 10, 3);
                    jackFrost.currentWeapon = ItemFactory.createItem(1503);
                    AddLootItem(jackFrost, 9005, 25);
                    AddLootItem(jackFrost, 9006, 75);
                    return jackFrost;

                //runs if there is no case statement for a matching value
                default:
                    throw new ArgumentException(string.Format("MonsterType '{0}' does not exist", monsterID));
            }
        }

        //makes monster loot random by generating a random number between 1 and 100
        //if number is less than or equal to percentage parameter, loot will be added to monster inventory
        private static void AddLootItem(Monster monster, int itemID, int percentage)
        {
            if (RandomNumberGenerator.numberBetween(1, 100) <= percentage)
            {
                monster.AddItemToInventory(ItemFactory.createItem(itemID));
            }
        }
    }
}
