using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Engine.Factories;
using Engine.EventArgs;

//manages the play session while the player runs the game

namespace Engine.ViewModels
{
    public class GameSession : BaseNotificationClass
    {
        public event EventHandler<GameMessageEventArgs> OnMessageRaised;

        private Player _currentPlayer;
        private Location _currentLocation;
        private Monster _currentMonster;
        private Trader _currentTrader;

        //creates properties to hold world, player, location, etc.
        public World currentWorld { get; }
        public Player currentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                if (_currentPlayer != null)
                {
                    _currentPlayer.OnActionPerformed -= OnCurrentPlayerPerformedAction;
                    _currentPlayer.OnLevelUp -= OnPlayerLevelUp;
                    _currentPlayer.OnKilled -= OnCurrentPlayerKilled;
                }

                _currentPlayer = value;

                if (_currentPlayer != null)
                {
                    _currentPlayer.OnActionPerformed += OnCurrentPlayerPerformedAction;
                    _currentPlayer.OnLevelUp += OnPlayerLevelUp;
                    _currentPlayer.OnKilled += OnCurrentPlayerKilled;
                }
            }
        }

        public Location currentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;

                //uses nameof to ensure it calls proper objects/functions
                //uses property changed event handlers from base notification class
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasLocationToNorth));
                OnPropertyChanged(nameof(HasLocationToEast));
                OnPropertyChanged(nameof(HasLocationToWest));
                OnPropertyChanged(nameof(HasLocationToSouth));

                CompleteQuestAtLoc();
                GivePlayerQuestAtLoc();
                GetMonsterAtLoc();
                currentTrader = currentLocation.traderHere;
            }
        }

        //holds monster at location so player can battle the monster
        public Monster currentMonster
        {
            get { return _currentMonster; }
            set
            {
                if (_currentMonster != null)
                {
                    _currentMonster.OnActionPerformed -= OnCurrentMonsterPerformedAction;
                    _currentMonster.OnKilled -= OnCurrentMonsterKilled;
                }

                _currentMonster = value;

                if (_currentMonster != null)
                {
                    _currentMonster.OnActionPerformed += OnCurrentMonsterPerformedAction;
                    _currentMonster.OnKilled += OnCurrentMonsterKilled;

                    RaiseMessage("");
                    RaiseMessage($"A {currentMonster.name} approaches!");
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasMonster));
            }
        }

        public Trader currentTrader
        {
            get { return _currentTrader; }
            set
            {
                _currentTrader = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasTrader));
            }
        }

        //runs constructor to create new properties and put it in the session
        public GameSession()
        {
            currentPlayer = new Player("Demi-Fiend", "Fighter", 0, 10, 10, 1000);

            if (!currentPlayer.Weapons.Any())
            {
                currentPlayer.AddItemToInventory(ItemFactory.createItem(1001));
            }

            currentPlayer.AddItemToInventory(ItemFactory.createItem(2001));
            currentPlayer.AddItemToInventory(ItemFactory.createItem(2001));
            currentPlayer.LearnRecipe(RecipeFactory.RecipeByID(1));
            currentPlayer.LearnRecipe(RecipeFactory.RecipeByID(2));
            currentPlayer.AddItemToInventory(ItemFactory.createItem(3001));
            currentPlayer.AddItemToInventory(ItemFactory.createItem(3002));
            currentPlayer.AddItemToInventory(ItemFactory.createItem(3002));


            //goes to static worldfactory class and uses static createworld function
            //and instantiates new world object and sets it as current world property
            currentWorld = WorldFactory.CreateWorld();

            currentLocation = currentWorld.locationAt(0, 0);
        }

        //moves coordinates of player
        //prevents bad input by using guard clauses to check for valid location
        public void MoveNorth()
        {
            if (HasLocationToNorth)
            {
                currentLocation = currentWorld.locationAt(currentLocation.xCoord, currentLocation.yCoord + 1);
            }         
        }
        public void MoveEast()
        {
            if (HasLocationToEast)
            {
                currentLocation = currentWorld.locationAt(currentLocation.xCoord + 1, currentLocation.yCoord);
            }
        }
        public void MoveWest()
        {
            if (HasLocationToWest)
            {
                currentLocation = currentWorld.locationAt(currentLocation.xCoord - 1, currentLocation.yCoord);
            }
        }
        public void MoveSouth()
        {
            if (HasLocationToSouth)
            {
                currentLocation = currentWorld.locationAt(currentLocation.xCoord, currentLocation.yCoord - 1);
            }
        }

        //checks if there is a valid cooridnate for player to move to from the current coordinate
        public bool HasLocationToNorth =>
            currentWorld.locationAt(currentLocation.xCoord, currentLocation.yCoord + 1) != null;
        public bool HasLocationToEast =>
            currentWorld.locationAt(currentLocation.xCoord + 1, currentLocation.yCoord) != null;
        public bool HasLocationToWest =>
            currentWorld.locationAt(currentLocation.xCoord - 1, currentLocation.yCoord) != null;
        public bool HasLocationToSouth =>
            currentWorld.locationAt(currentLocation.xCoord, currentLocation.yCoord - 1) != null;
        //lets location know if it has a monster
        public bool HasMonster => currentMonster != null;
        //lets location know if it has a trader
        public bool HasTrader => currentTrader != null;

        //if player goes to a location with a quest, gives player a quest at corresponding location
        //with requirements and rewards
        private void GivePlayerQuestAtLoc()
        {
            foreach(Quest quest in currentLocation.questAvailable)
            {
                if(!currentPlayer.quests.Any(q => q.playerQuest.ID == quest.ID))
                {
                    currentPlayer.quests.Add(new QuestStatus(quest));

                    RaiseMessage("");
                    RaiseMessage($"You received the \"{quest.name}\" quest.");
                    RaiseMessage(quest.desc);

                    RaiseMessage("Return with:");
                    foreach(ItemQuantity itemQuantity in quest.itemsToComplete)
                    {
                        RaiseMessage($" {itemQuantity.quantity} {ItemFactory.createItem(itemQuantity.itemID).name}");
                    }

                    RaiseMessage("And you will receive:");
                    RaiseMessage($" {quest.rewardEXP} EXP");
                    RaiseMessage($"{quest.rewardGold} Macca");
                    foreach(ItemQuantity itemQuantity in quest.rewardItem)
                    {
                        RaiseMessage($" {itemQuantity.quantity} {ItemFactory.createItem(itemQuantity.itemID).name}");
                    }
                }
            }
        }

        //function to mark a quest as complete and give appropriate rewards
        private void CompleteQuestAtLoc()
        {
            foreach(Quest quest in currentLocation.questAvailable)
            {
                QuestStatus questToComplete = currentPlayer.quests.FirstOrDefault(q => q.playerQuest.ID == quest.ID && !q.isCompleted);
                if (questToComplete != null)
                {
                    if (currentPlayer.HasAllReqItems(quest.itemsToComplete))
                    {
                        //remove required items from player inventory
                        currentPlayer.RemoveItemsFromInventory(quest.itemsToComplete);
                        RaiseMessage("");
                        RaiseMessage($"You completed the \"{quest.name}\" quest!");

                        //give player quest rewards
                        RaiseMessage($"You got {quest.rewardEXP} EXP.");
                        currentPlayer.AddExperience(quest.rewardEXP);

                        RaiseMessage($"You got {quest.rewardGold} Macca.");
                        currentPlayer.receiveGold(quest.rewardGold);

                        foreach (ItemQuantity itemQuantity in quest.rewardItem)
                        {
                            GameItem rewardItem = ItemFactory.createItem(itemQuantity.itemID);

                            RaiseMessage($"You got a {rewardItem.name}.");
                            currentPlayer.AddItemToInventory(rewardItem);
                        }

                        //mark quest as complete
                        questToComplete.isCompleted = true;
                    }
                }
            }
        }

        private void GetMonsterAtLoc()
        {
            currentMonster = currentLocation.GetMonster();
        }

        public void AttackCurrentMonster()
        {
            if(currentMonster == null)
            {
                return;
            }

            if (currentPlayer.currentWeapon == null)
            {
                RaiseMessage("You must select an attack.");
                return;
            }

            //execute attack command from attackwithweapon class
            currentPlayer.useCurrentWeapon(currentMonster);

            //if monster is killed, get another monster to fight
            if (currentMonster.isDead)
            {
                GetMonsterAtLoc();
            }
            //initiate monster's attack if still alive
            else
            {
                currentMonster.useCurrentWeapon(currentPlayer);
            }
        }

        //uses consumable item in inventory
        public void useCurrentConsumable()
        {
            if (currentPlayer.currentConsumable != null)
            {
                currentPlayer.useCurrentConsumable();
            }
        }

        //checks if player has all required items for recipe uses them to create new item
        public void craftItem(Recipe recipe)
        {
            if(currentPlayer.HasAllReqItems(recipe.ingredients))
            {
                currentPlayer.RemoveItemsFromInventory(recipe.ingredients);

                foreach(ItemQuantity itemQuantity in recipe.outputItems)
                {
                    for(int i = 0; i < itemQuantity.quantity; i++)
                    {
                        GameItem outputItem = ItemFactory.createItem(itemQuantity.itemID);
                        currentPlayer.AddItemToInventory(outputItem);
                        RaiseMessage($"You crafted a {outputItem.name}.");
                    }
                }
            }
            else
            {
                RaiseMessage("You do not have the required ingredients:");
                foreach(ItemQuantity itemQuantity in recipe.ingredients)
                {
                    RaiseMessage($"{itemQuantity.quantity} {ItemFactory.itemName(itemQuantity.itemID)}");
                }
            }
        }

        private void OnCurrentPlayerPerformedAction(object sender, string result)
        {
            RaiseMessage(result);
        }

        private void OnCurrentMonsterPerformedAction(object sender, string result)
        {
            RaiseMessage(result);
        }

        private void OnCurrentPlayerKilled(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage("");
            RaiseMessage("You have been defeated!");

            currentLocation = currentWorld.locationAt(0, -1);
            currentPlayer.fullHeal();
        }

        private void OnCurrentMonsterKilled(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage("");
            RaiseMessage($"You defeated the {currentMonster.name}!");

            RaiseMessage($"You got {currentMonster.rewardEXP} EXP.");
            currentPlayer.AddExperience(currentMonster.rewardEXP);

            RaiseMessage($"You got {currentMonster.gold} Macca.");
            currentPlayer.receiveGold(currentMonster.gold);

            foreach (GameItem gameItem in currentMonster.inventory)
            {
                RaiseMessage($"You got a {gameItem.name}.");
                currentPlayer.AddItemToInventory(gameItem);
            }
        }

        private void OnPlayerLevelUp(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage($"You are now level {currentPlayer.level}!");
        }

        //looks at onmessageraised and passes itself and new gamemessageeventargs to display message
        private void RaiseMessage(string message)
        {
            OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message));
        }       
    }
}