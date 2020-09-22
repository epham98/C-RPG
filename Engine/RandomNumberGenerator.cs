using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//simple random number generator

namespace Engine
{
    public static class RandomNumberGenerator
    {
        private static readonly Random _generator = new Random();

        public static int numberBetween(int minValue, int maxValue)
        {
            return _generator.Next(minValue, maxValue + 1);
        }        
    }
}
