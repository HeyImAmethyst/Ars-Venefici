using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Util
{
    public static class Utils
    {
        public static T RequireNotNull<T>(T arg)
        {
            if (arg == null)
            {
                throw new ArgumentNullException();
            }
            return arg;
        }

        public static T GetFirstMatching<T>(List<T> list, Predicate<T> predicate)
        {
            foreach (T t in list)
            {
                if (predicate.Invoke(t)) return t;
            }

            return default;
        }

        public static T GetLastMatching<T>(List<T> list, Predicate<T> predicate)
        {
            List<T> l = new List<T>(list);
            l.Reverse();

            return GetFirstMatching(l, predicate);
        }

        public static Vector2 ConvertToTilePos(int x ,int y)
        {
            return new Vector2(x / Game1.tileSize, y / Game1.tileSize);
        }

        public static Vector2 ConvertToTilePos(Vector2 pos)
        {
            return new Vector2(pos.X / Game1.tileSize, pos.Y / Game1.tileSize);
        }
    }
}
