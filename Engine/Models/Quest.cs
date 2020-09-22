using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//creates a quest and quest properties such as name, requirements, rewards

namespace Engine.Models
{
    public class Quest
    {
        public int ID { get; }
        public string name { get; }
        public string desc { get; }

        public List<ItemQuantity> itemsToComplete { get; }

        public int rewardEXP { get; }
        public int rewardGold { get; }
        public List<ItemQuantity> rewardItem { get; }

        //constructor passes all parameters needed for property values 
        public Quest(int questID, string questName, string questDesc, List<ItemQuantity> questComplete, 
            int questEXP, int questGold, List<ItemQuantity> questItem)
        {
            ID = questID;
            name = questName;
            desc = questDesc;
            itemsToComplete = questComplete;
            rewardEXP = questEXP;
            rewardGold = questGold;
            rewardItem = questItem;
        }
    }
}
