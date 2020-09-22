using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

//creates trader objects to add to a list

namespace Engine.Factories
{
    public static class TraderFactory
    {
        private static readonly List<Trader> _traders = new List<Trader>();

        static TraderFactory()
        {
            Trader chiaki = new Trader("Chiaki");
            chiaki.AddItemToInventory(ItemFactory.createItem(1001));

            Trader isamu = new Trader("Isamu");
            isamu.AddItemToInventory(ItemFactory.createItem(1001));

            Trader hikawa = new Trader("Hikawa");
            hikawa.AddItemToInventory(ItemFactory.createItem(1001));

            AddTraderToList(chiaki);
            AddTraderToList(isamu);
            AddTraderToList(hikawa);
        }

        public static Trader GetTraderByName(string name)
        {
            return _traders.FirstOrDefault(t => t.name == name);
        }

        private static void AddTraderToList(Trader trader)
        {
            if(_traders.Any(t => t.name == trader.name))
            {
                throw new ArgumentException($"There is already a trader named \"{trader.name}\".");
            }

            _traders.Add(trader);
        }
    }
}
