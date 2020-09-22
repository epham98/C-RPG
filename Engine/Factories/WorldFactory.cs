using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    internal static class WorldFactory
    {
        //creates locations and puts them in the game world
        internal static World CreateWorld()
        {
            World newWorld = new World();

            newWorld.addLocation(-2, -1, "Isamu's Field",
                "Rows of corn grow here, with giant rats hiding among them.",
                "FarmFields.png");

            newWorld.locationAt(-2, -1).AddMonster(2, 100);

            newWorld.addLocation(-1, -1, "Isamu's House",
                "This is the house of your neighbor, Isamu.",
                "Farmhouse.png");

            newWorld.locationAt(-1, -1).traderHere = TraderFactory.GetTraderByName("Isamu");

            newWorld.addLocation(0, -1, "Home", 
                "This is your home.", 
                "Home.png");

            newWorld.addLocation(-1, 0, "Trading Shop",
                "The shop of Chiaki, the trader.",
                "Trader.png");

            newWorld.locationAt(-1, 0).traderHere = TraderFactory.GetTraderByName("Chiaki");

            newWorld.addLocation(0, 0, "Town square",
                "You see a fountain here.",
                "TownSquare.png");

            newWorld.addLocation(1, 0, "Town Gate",
                "There is a gate here, protecting the town from demons.",
                "TownGate.png");

            newWorld.addLocation(2, 0, "Dark Forest",
                "The trees in this forest are covered with a terrifying presence.",
                "SpiderForest.png");

            newWorld.locationAt(2, 0).AddMonster(3, 100);

            newWorld.addLocation(0, 1, "Herbalist's hut",
                "You see a small hut, with plants drying from the roof.",
                "HerbalistsHut.png");

            newWorld.locationAt(0, 1).traderHere = TraderFactory.GetTraderByName("Hikawa");

            newWorld.locationAt(0, 1).questAvailable.Add(QuestFactory.getQuestByID(1));

            newWorld.addLocation(0, 2, "Herbalist's garden",
                "There are many plants here, with slimes hiding behind them.",
                "HerbalistsGarden.png");

            newWorld.locationAt(0, 2).AddMonster(1, 100);

            return newWorld;
        }
    }
}
