using ShoppingAssistantServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAssistantServer.Helpers
{
    public static class Calculator2D
    {

        public static Double GetDistance(Point pos1, Point pos2)
        {
            //calculates the distance between two points described by geographical coordinates
            //uses the haversine formula
            double R = 6371; //Earths radius
            var deltaLat = ToRadians(pos2.X - pos1.X);
            var deltaLong = ToRadians(pos2.Y - pos1.Y);
            var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                          Math.Cos(ToRadians(pos1.X)) * Math.Cos(ToRadians(pos2.X)) *
                          Math.Sin(deltaLong / 2) * Math.Sin(deltaLong / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a));
            Console.WriteLine("---------->Get distance "+R*c);

            return R * c;
        }

        public static Double ToRadians(double deg)
        {
            return (deg * 3.14159265359 / 180.0);
        }

    }
}
