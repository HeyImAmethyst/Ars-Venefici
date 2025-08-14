using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Util
{
    public class VectorLine
    {
        public Vector2 p1, p2;

        public VectorLine()
        {
            this.p1 = Vector2.Zero;
            this.p2 = Vector2.Zero;
        }

        public VectorLine(Vector2 p1, Vector2 p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        public void SetP1(Vector2 p1)
        {
            this.p1 = p1;
        }

        public void SetP2(Vector2 p2)
        {
            this.p2 = p2;
        }

        public Vector2 CalculateP2FromAngleAndDistance(float angle, int distance)
        {
            float x = (float)(p1.X + distance * Math.Cos(MathHelper.ToRadians(angle)));
            float y = (float)(p1.Y + distance * Math.Sin(MathHelper.ToRadians(angle)));

            return new Vector2(x, y);
        }

        public Vector2[] GetPoints(int quantity)
        {
            var points = new Vector2[quantity];
            int ydiff = (int)(p2.Y - p1.Y), xdiff = (int)(p2.X - p1.X);
            double slope = (double)(p2.Y - p1.Y) / (p2.X - p1.X);
            double x, y;

            --quantity;

            for (double i = 0; i < quantity; i++)
            {
                y = slope == 0 ? 0 : ydiff * (i / quantity);
                x = slope == 0 ? xdiff * (i / quantity) : y / slope;
                points[(int)i] = new Vector2((int)Math.Round(x) + p1.X, (int)Math.Round(y) + p1.Y);
            }

            points[quantity] = p2;
            return points;
        }
    }
}
