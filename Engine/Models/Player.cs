using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    //create a player object with properties relevant to an RPG game, inherits from livingentity and basenotification
    public class Player : LivingEntity
    {

        private string _charClass;
        private int _EXP;

        //getters and setters update and change properties unique to player object
        public string charClass
        {
            get { return _charClass; }
            set
            {
                _charClass = value;
                OnPropertyChanged();
            }
        }
        public int EXP
        {
            get { return _EXP; }
            private set
            {
                _EXP = value;
                OnPropertyChanged();
                SetLevelAndMaxHP();
            }
        }       

        public ObservableCollection<QuestStatus> quests { get; }

        public ObservableCollection<Recipe> recipes { get; }

        public event EventHandler OnLevelUp;

        public Player(string name, string characterClass, int experiencePoints, int maxHP, int currentHP, int gold) : 
            base(name, maxHP, currentHP, gold)
        {
            charClass = characterClass;
            EXP = experiencePoints;

            quests = new ObservableCollection<QuestStatus>();
            recipes = new ObservableCollection<Recipe>();
        }

        public void AddExperience(int experiencePoints)
        {
            EXP += experiencePoints;
        }

        private void SetLevelAndMaxHP()
        {
            int originalLevel = level;
            level = (EXP / 100) + 1;

            if (level != originalLevel)
            {
                maxHP = level * 10;
                OnLevelUp?.Invoke(this, System.EventArgs.Empty);
            }
        }

        //adds a recipe to player's learned recipes
        public void LearnRecipe(Recipe recipe)
        {
            if(!recipes.Any(r => r.ID == recipe.ID))
            {
                recipes.Add(recipe);
            }
        }
    }
}
