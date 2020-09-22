using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

//creates recipes and items from recipes

namespace Engine.Factories
{
    public static class RecipeFactory
    {
        //list of known recipes in game
        private static readonly List<Recipe> _recipes = new List<Recipe>();

        //populates _recipes list
        static RecipeFactory()
        {
            Recipe prayerBead = new Recipe(1, "Prayer Bead");
            prayerBead.addIngredient(3001, 1);
            prayerBead.addIngredient(3002, 2);
            prayerBead.addOutputItem(2001, 1);

            Recipe beadString = new Recipe(2, "Bead String");
            beadString.addIngredient(2001, 3);
            beadString.addOutputItem(2002, 1);

            _recipes.Add(prayerBead);
            _recipes.Add(beadString);
        }

        public static Recipe RecipeByID(int id)
        {
            return _recipes.FirstOrDefault(x => x.ID == id);
        }
    }
}
