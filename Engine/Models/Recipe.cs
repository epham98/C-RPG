using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//class for inputting and outputting recipe items

namespace Engine.Models
{
    public class Recipe
    {
        //recipe properties
        public int ID { get; }
        public string name { get; }
        //list for ingredients to put in and list for output of combining ingredients
        public List<ItemQuantity> ingredients { get; } = new List<ItemQuantity>();
        public List<ItemQuantity> outputItems { get; } = new List<ItemQuantity>();

        public Recipe(int id, string recipeName)
        {
            ID = id;
            name = recipeName;
        }

        //adds ingredients and output items to list properties
        public void addIngredient(int itemID, int quantity)
        {
            if(!ingredients.Any(x => x.itemID == itemID))
            {
                ingredients.Add(new ItemQuantity(itemID, quantity));
            }
        }

        public void addOutputItem(int itemID, int quantity)
        {
            if (!outputItems.Any(x => x.itemID == itemID))
            {
                outputItems.Add(new ItemQuantity(itemID, quantity));
            }
        }
    }
}
