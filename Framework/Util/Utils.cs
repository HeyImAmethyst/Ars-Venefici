using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Util
{
    public static class Utils
    {
        public const float Deg2Rad = (float)(Math.PI * 2F / 360F);
        public const float Rad2Deg = 1F / Deg2Rad;

        public const float kEpsilon = 0.00001F;
        public const float kEpsilonNormalSqrt = 1e-15f;

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

        //---------------------------------------
        // Math Utils
        //---------------------------------------

        public static Vector2 LengthenLine(Vector2 startPoint, Vector2 endPoint, float pixelCount)
        {
            if (startPoint.Equals(endPoint))
                return new Vector2(); // not a line

            Vector2 newEndPoint = endPoint;

            double dx = newEndPoint.X - startPoint.X;
            double dy = newEndPoint.Y - startPoint.Y;

            if (dx == 0)
            {
                // vertical line:
                if (newEndPoint.Y < startPoint.Y)
                    newEndPoint.Y -= pixelCount;
                else
                    newEndPoint.Y += pixelCount;
            }
            else if (dy == 0)
            {
                // horizontal line:
                if (newEndPoint.X < startPoint.X)
                    newEndPoint.X -= pixelCount;
                else
                    newEndPoint.X += pixelCount;
            }
            else
            {
                // non-horizontal, non-vertical line:
                double length = Math.Sqrt(dx * dx + dy * dy);
                double scale = (length + pixelCount) / length;
                dx *= scale;
                dy *= scale;
                newEndPoint.X = startPoint.X + Convert.ToSingle(dx);
                newEndPoint.Y = startPoint.Y + Convert.ToSingle(dy);
            }

            return newEndPoint;
        }

        public static float Angle(Vector2 from, Vector2 to)
        {
            // sqrt(a) * sqrt(b) = sqrt(a * b) -- valid for real numbers
            float denominator = (float)Math.Sqrt(from.LengthSquared() * to.LengthSquared());

            if (denominator < kEpsilonNormalSqrt)
                return 0F;

            float dot = Math.Clamp(Vector2.Dot(from, to) / denominator, -1F, 1F);
            return (float)Math.Acos(dot) * Rad2Deg;
        }

        public static float RemapValue(float value, float initialMin, float initialMax, float outputMin, float outputMax)
        {
            return outputMin + (value - initialMin) * (outputMax - outputMin) / (initialMax - initialMin);
        }

        public static float WrapValue(float value, float min, float max)
        {
            if (value > max)
                return (value - max) + min;

            if (value < min)
                return max - (min - value);

            return value;
        }

        /*
         * 
         *  conversion          formula
            ------------------  --------------------------------------------------------------------------------
            absolute => screen  x - Game1.viewport.X, y - Game1.viewport.Y
            absolute => tile    x / Game1.tileSize,   y / Game1.tileSize

            screen => absolute  x + Game1.viewport.X, y + Game1.viewport.Y
            screen => tile      (x + Game1.viewport.X) / Game1.tileSize, (y + Game1.viewport.Y) / Game1.tileSize

            tile => absolute    x * Game1.tileSize, y * Game1.tileSize
            tile => screen      (x * Game1.tileSize) - Game1.viewport.X, (y * Game1.tileSize) - Game1.viewport.Y
        */

        //---------------------------------------
        // Absolute Position Conversions
        //---------------------------------------

        public static Vector2 AbsolutePosToScreenPos(int x, int y)
        {
            return new Vector2(x - Game1.viewport.X, y - Game1.viewport.Y);
        }

        public static Vector2 AbsolutePosToScreenPos(Vector2 pos)
        {
            return new Vector2(pos.X - Game1.viewport.X, pos.Y - Game1.viewport.Y);
        }

        public static Vector2 AbsolutePosToTilePos(int x ,int y)
        {
            return new Vector2(x / Game1.tileSize, y / Game1.tileSize);
        }

        public static Vector2 AbsolutePosToTilePos(Vector2 pos)
        {
            return new Vector2(pos.X / Game1.tileSize, pos.Y / Game1.tileSize);
        }

        //---------------------------------------
        // Screen Position Conversions
        //---------------------------------------

        public static Vector2 ScreenPosToAbsolutePos(int x, int y)
        {
            return new Vector2(x + Game1.viewport.X, y + Game1.viewport.Y);
        }

        public static Vector2 ScreenPosToAbsolutePos(Vector2 pos)
        {
            return new Vector2(pos.X + Game1.viewport.X, pos.Y + Game1.viewport.Y);
        }

        public static Vector2 ScreenPosToTilePos(int x, int y)
        {
            return new Vector2((x + Game1.viewport.X) / Game1.tileSize, (y + Game1.viewport.Y) / Game1.tileSize);
        }

        public static Vector2 ScreenPosToTilePos(Vector2 pos)
        {
            return new Vector2((pos.X + Game1.viewport.X) / Game1.tileSize, (pos.Y + Game1.viewport.Y) / Game1.tileSize);
        }

        //---------------------------------------
        // Tile Position Conversions
        //---------------------------------------

        public static Vector2 TilePosToAbsolutePos(int x, int y)
        {
            return new Vector2(x * Game1.tileSize, y * Game1.tileSize);
        }

        public static Vector2 TilePosToAbsolutePos(Vector2 pos)
        {
            return new Vector2(pos.X * Game1.tileSize, pos.Y * Game1.tileSize);
        }

        public static Vector2 TilePosToScreenPos(int x, int y)
        {
            return new Vector2((x * Game1.tileSize) - Game1.viewport.X, (y * Game1.tileSize) - Game1.viewport.Y);
        }

        public static Vector2 TilePosToScreenPos(Vector2 pos)
        {
            return new Vector2((pos.X * Game1.tileSize) - Game1.viewport.X, (pos.Y * Game1.tileSize) - Game1.viewport.Y);
        }
    }
}
