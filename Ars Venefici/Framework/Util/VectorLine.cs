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
        public Vector2 point1, point2;

        public VectorLine()
        {
            this.point1 = Vector2.Zero;
            this.point2 = Vector2.Zero;
        }

        public VectorLine(Vector2 point1, Vector2 point2)
        {
            this.point1 = point1;
            this.point2 = point2;
        }

        public void SetPoint1(Vector2 point1)
        {
            this.point1 = point1;
        }

        public void SetPoint2(Vector2 point2)
        {
            this.point2 = point2;
        }

        public Vector2 CalculateP2FromAngleAndDistance(float angle, int distance)
        {
            float x = (float)(point1.X + distance * Math.Cos(MathHelper.ToRadians(angle)));
            float y = (float)(point1.Y + distance * Math.Sin(MathHelper.ToRadians(angle)));

            return new Vector2(x, y);
        }

        public Vector2[] GetPoints(int quantity)
        {
            var points = new Vector2[quantity];
            int yDifference = (int)(point2.Y - point1.Y), xDifference = (int)(point2.X - point1.X);
            double slope = (double)(point2.Y - point1.Y) / (point2.X - point1.X);
            double x, y;

            --quantity;

            for (double i = 0; i < quantity; i++)
            {
                y = slope == 0 ? 0 : yDifference * (i / quantity);
                x = slope == 0 ? xDifference * (i / quantity) : y / slope;
                points[(int)i] = new Vector2((int)Math.Round(x) + point1.X, (int)Math.Round(y) + point1.Y);
            }

            points[quantity] = point2;
            return points;
        }
    }
}
