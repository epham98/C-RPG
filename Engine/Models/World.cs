using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class World
    {
        //stores and instantiates list of locations
        private List<Location> _locations = new List<Location>();

        //adds location to list
        internal void addLocation(int xCoord, int yCoord, string name, string desc, string imgName)
        {
            _locations.Add(new Location(xCoord, yCoord, name, desc, $"/Engine;component/Images/Locations/{imgName}"));
        }

        //returns a location object from whichever coordinate the player is in
        public Location locationAt(int xCoord, int yCoord)
        {
            //looks in each object in _locations list and assigns it to variable loc to see if it matches criteria
            foreach(Location loc in _locations)
            {
                if(loc.xCoord == xCoord && loc.yCoord == yCoord)
                {
                    return loc;
                }
            }

            return null;
        }
    }
}
