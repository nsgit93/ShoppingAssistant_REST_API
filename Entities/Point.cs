using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAssistantServer.Entities
{
    public class Point
    {
        public Double X { get; set; }
        public Double Y { get; set; }
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
