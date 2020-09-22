using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//class with common properties for all living entities in game

namespace Engine.Models
{
    public abstract class LivingEntity : BaseNotificationClass
    {
        //name, hp, and gold properties used by all entities
        private string _name;
        private int _currentHP;
        private int _maxHP;
        private int _gold;
        private int _level;
        private GameItem _currentWeapon;
        private GameItem _currentConsumable;

        public string name
        {
            get { return _name; }
            private set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public int currentHP
        {
            get { return _currentHP; }
            private set
            {
                _currentHP = value;
                OnPropertyChanged();
            }
        }
        public int maxHP
        {
            get { return _maxHP; }
            protected set
            {
                _maxHP = value;
                OnPropertyChanged();
            }
        }
        public int gold
        {
            get { return _gold; }
            private set
            {
                _gold = value;
                OnPropertyChanged();
            }
        }

        public int level
        {
            get { return _level; }
            protected set
            {
                _level = value;
                OnPropertyChanged();
            }
        }

        public GameItem currentWeapon
        {
            get { return _currentWeapon; }
            set
            {
                if(_currentWeapon != null)
                {
                    _currentWeapon.action.OnActionPerformed -= RaiseActionPerformedEvent;
                }

                _currentWeapon = value;

                if(_currentWeapon != null)
                {
                    _currentWeapon.action.OnActionPerformed += RaiseActionPerformedEvent;
                }

                OnPropertyChanged();
            }
        }

        public GameItem currentConsumable
        {
            get { return _currentConsumable; }
            set
            {
                if(_currentConsumable != null)
                {
                    _currentConsumable.action.OnActionPerformed -= RaiseActionPerformedEvent;
                }

                _currentConsumable = value;
                if(_currentConsumable != null)
                {
                    _currentConsumable.action.OnActionPerformed += RaiseActionPerformedEvent;
                }

                OnPropertyChanged();
            }
        }

        //creates inventory for game entities
        public ObservableCollection<GameItem> inventory { get; }

        //creates inventory for stacked items, weapons, and consumables
        public ObservableCollection<GroupedInventoryItem> groupedInventory { get; }

        public List<GameItem> Weapons => inventory.Where(i => i.category == GameItem.ItemCategory.Weapon).ToList();

        public List<GameItem> Consumables => inventory.Where(i => i.category == GameItem.ItemCategory.Consumable).ToList();

        public bool hasConsumable => Consumables.Any();

        //checks if entity hp is 0
        public bool isDead => currentHP <= 0;

        public event EventHandler<string> OnActionPerformed;

        //event for if entity hp is 0
        public event EventHandler OnKilled;

        //constructor for livingentity. child classes such as player/monster/trader have access
        //to class and its properties
        protected LivingEntity(string entityName, int maxHealth, int currentHealth, int gld, int lvl = 1)
        {
            name = entityName;
            maxHP = maxHealth;
            currentHP = currentHealth;
            gold = gld;
            level = lvl;

            inventory = new ObservableCollection<GameItem>();
            groupedInventory = new ObservableCollection<GroupedInventoryItem>();
        }

        public void useCurrentWeapon(LivingEntity target)
        {
            currentWeapon.PerformAction(this, target);
        }

        public void useCurrentConsumable()
        {
            currentConsumable.PerformAction(this, this);
            RemoveItemFromInventory(currentConsumable);
        }

        //functions to encapsulate changing properties' values
        public void takeDamage(int hpDamage)
        {
            currentHP -= hpDamage;

            if (isDead)
            {
                currentHP = 0;
                RaiseOnKilledEvent();
            }
        }

        public void Heal(int hpHeal)
        {
            currentHP += hpHeal;

            if (currentHP > maxHP)
            {
                currentHP = maxHP;
            }
        }

        public void fullHeal()
        {
            currentHP = maxHP;
        }

        public void receiveGold(int amountGold)
        {
            gold += amountGold;
        }

        public void spendGold(int amountGold)
        {
            if (amountGold > gold)
            {
                throw new ArgumentOutOfRangeException($"{name} only has {gold} Macca, and cannot spend it.");
            }
            gold -= amountGold;
        }

        //add items to entity's inventory
        public void AddItemToInventory(GameItem item)
        {
            inventory.Add(item);

            //checks if item to add is unique
            if (item.isUnique)
            {
                groupedInventory.Add(new GroupedInventoryItem(item, 1));
            }
            else
            {
                //first time a player gets a non-unique item, adds item w/ quantity of 0
                //and is incremented by one. runs increment if player already has at least one of
                //the non-unique item
                if (!groupedInventory.Any(gi => gi.item.itemTypeID == item.itemTypeID))
                {
                    groupedInventory.Add(new GroupedInventoryItem(item, 0));
                }
                groupedInventory.First(gi => gi.item.itemTypeID == item.itemTypeID).quantity++;
            }

            OnPropertyChanged(nameof(Weapons));
            OnPropertyChanged(nameof(Consumables));
            OnPropertyChanged(nameof(hasConsumable));
        }

        //remove single item from entity's inventory
        public void RemoveItemFromInventory(GameItem item)
        {
            inventory.Remove(item);

            //ternary operator goes checks whether to remove by first or second option
            GroupedInventoryItem itemToRemove = item.isUnique ?
                groupedInventory.FirstOrDefault(gi => gi.item == item) :
                groupedInventory.FirstOrDefault(gi => gi.item.itemTypeID == item.itemTypeID);

            //checks if quantity of item to remove is 1 or more
            //removes item from inventory entirely if 1 and decrements item if more than 1
            if (itemToRemove != null)
            {
                if (itemToRemove.quantity == 1)
                {
                    groupedInventory.Remove(itemToRemove);
                }
                else
                {
                    itemToRemove.quantity--;
                }
            }

            OnPropertyChanged(nameof(Weapons));
            OnPropertyChanged(nameof(Consumables));
            OnPropertyChanged(nameof(hasConsumable));
        }

        //removes multiple items from entity's inventory
        public void RemoveItemsFromInventory(List<ItemQuantity> itemQuantities)
        {
            foreach (ItemQuantity itemQuantity in itemQuantities)
            {
                for (int i = 0; i < itemQuantity.quantity; i++)
                {
                    RemoveItemFromInventory(inventory.First(item => item.itemTypeID == itemQuantity.itemID));
                }
            }
        }

        //checks if player has all required items for completing a quest or crafting a recipe
        public bool HasAllReqItems(List<ItemQuantity> items)
        {
            foreach(ItemQuantity item in items)
            {
                if(inventory.Count(i => i.itemTypeID == item.itemID) < item.quantity)
                {
                    return false;
                }
            }
            return true;
        }

        //lets gamesession know an entity hp is at 0
        private void RaiseOnKilledEvent()
        {
            OnKilled?.Invoke(this, new System.EventArgs());
        }

        //lets gamesession know an action is being performed
        private void RaiseActionPerformedEvent(object sender, string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }
    }
}
