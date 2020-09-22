using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Engine.Actions;
using Engine.Shared;

namespace Engine.Factories
{
    public static class ItemFactory
    {
        //assigns item xml data to a string variable
        private const string GAME_DATA_FILENAME = ".\\GameData\\GameItems.xml";

        //list to contain all game items
        private static readonly List<GameItem> _standardGameItems = new List<GameItem>();

        //initialize and populates game item list from external xml document nodes
        static ItemFactory()
        {
            if(File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                loadItemsFromNodes(data.SelectNodes("/GameItems/Weapons/Weapon"));
                loadItemsFromNodes(data.SelectNodes("/GameItems/HealingItems/HealingItem"));
                loadItemsFromNodes(data.SelectNodes("/GameItems/MiscItems/MiscItem"));

            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
            /**
            OUTDATED BUILD ITEM FUNCTION CALLS 
            
            buildWeapon(1001, "Lunge", 1, 1, 2);
            buildWeapon(1002, "Feral Claw", 5, 1, 3);

            buildWeapon(1501, "Slime ball", 0, 0, 2);
            buildWeapon(1502, "Rat claws", 0, 0, 4);
            buildWeapon(1503, "Frost bite", 0, 0, 4);

            buildHealItem(2001, "Prayer Bead", 5, 2);
            buildHealItem(2002, "Bead String", 15, 10);

            buildMiscItem(3001, "Empty Bead", 1);
            buildMiscItem(3002, "Healing Essence", 1);

            buildMiscItem(9001, "Slime bone", 1);
            buildMiscItem(9002, "Slime drop", 2);
            buildMiscItem(9003, "Rat tail", 1);
            buildMiscItem(9004, "Rat fur", 2);
            buildMiscItem(9005, "Frost fang", 1);
            buildMiscItem(9006, "Frost cloth", 2);
            **/
        }

        //creates game items in-game
        public static GameItem createItem(int itmID)
        {
            return _standardGameItems.FirstOrDefault(item => item.itemTypeID == itmID)?.Clone();
        }        

        //displays item name w/o creating new gameitem object by passing in itemtypeid and getting item's name
        public static string itemName(int itmID)
        {
            return _standardGameItems.FirstOrDefault(i => i.itemTypeID == itmID)?.name ?? "";
        }

        //loads items from GameItems.xml
        private static void loadItemsFromNodes(XmlNodeList nodes)
        {
            if(nodes == null)
            {
                return;
            }

            foreach(XmlNode node in nodes)
            {
                GameItem.ItemCategory itemCategory = determineItemCategory(node.Name);

                //creates the gameitem using extension methods to acquire attributes
                GameItem gameItem = new GameItem(itemCategory,
                                                 node.AttributeAsInt("id"),
                                                 node.AttributeAsString("name"),
                                                 node.AttributeAsInt("price"),
                                                 itemCategory == GameItem.ItemCategory.Weapon);

                //allows item to have attack action and damage properties if identified as weapon
                if(itemCategory == GameItem.ItemCategory.Weapon)
                {
                    gameItem.action = new AttackWithWeapon(gameItem, 
                                                           node.AttributeAsInt("minimumDamage"),
                                                           node.AttributeAsInt("maximumDamage"));
                }
                else if(itemCategory == GameItem.ItemCategory.Consumable)
                {
                    gameItem.action = new Heal(gameItem, node.AttributeAsInt("hpToHeal"));
                }

                _standardGameItems.Add(gameItem);
            }
        }

        //verifies an item's item category
        private static GameItem.ItemCategory determineItemCategory(string itemType)
        {
            switch(itemType)
            {
                case "Weapon":
                    return GameItem.ItemCategory.Weapon;
                case "HealingItem":
                    return GameItem.ItemCategory.Consumable;
                default:
                    return GameItem.ItemCategory.Misc;
            }
        }

        //helper functions are tailored to create specific item types (misc, weapon, healing) - OUTDATED
        /**
        private static void buildMiscItem(int id, string name, int price)
        {
            _standardGameItems.Add(new GameItem(GameItem.ItemCategory.Misc, id, name, price));
        }

        private static void buildWeapon(int id, string name, int price, int minimumDamage, int maximumDamage)
        {
            //instantiates weapon as gameitem object and sets weapon's action property to attackwithweapon object
            GameItem weapon = new GameItem(GameItem.ItemCategory.Weapon, id, name, price, true);

            weapon.action = new AttackWithWeapon(weapon, minimumDamage, maximumDamage);

            _standardGameItems.Add(weapon);
        }

        private static void buildHealItem(int id, string name, int price, int hpToHeal)
        {
            GameItem item = new GameItem(GameItem.ItemCategory.Consumable, id, name, price);
            item.action = new Heal(item, hpToHeal);
            _standardGameItems.Add(item);
        }
        **/
    }
}
