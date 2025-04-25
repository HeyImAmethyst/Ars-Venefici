using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Util
{
    public class VectorPolygon
    {
        private List<Vector2> points;

        public VectorPolygon(List<Vector2> points)
        {
            this.points = points;
        }

        public int Count()
        {
            return points.Count;
        }

        public bool IsInPolygon(Vector2 p)
        {
            //Vector2 p1, p2;
            //bool inside = false;

            //if (points.ToArray().Length < 3)
            //{
            //    return inside;
            //}

            //var oldPoint = new Vector2(
            //    points.ToArray()[points.ToArray().Length - 1].X, points.ToArray()[points.ToArray().Length - 1].Y);

            //for (int i = 0; i < points.ToArray().Length; i++)
            //{
            //    var newPoint = new Vector2(points.ToArray()[i].X, points.ToArray()[i].Y);

            //    if (newPoint.X > oldPoint.X)
            //    {
            //        p1 = oldPoint;
            //        p2 = newPoint;
            //    }
            //    else
            //    {
            //        p1 = newPoint;
            //        p2 = oldPoint;
            //    }

            //    if ((newPoint.X < p.X) == (p.X <= oldPoint.X)
            //        && (p.Y - (long)p1.Y) * (p2.X - p1.X)
            //        < (p2.Y - (long)p1.Y) * (p.X - p1.X))
            //    {
            //        inside = !inside;
            //    }

            //    oldPoint = newPoint;
            //}

            //return inside;

            //bool result = false;
            //int j = points.Count - 1;
            //for (int i = 0; i < points.Count; i++)
            //{
            //    if (points[i].Y < p.Y && points[j].Y >= p.Y ||
            //        points[j].Y < p.Y && points[i].Y >= p.Y)
            //    {
            //        if (points[i].X + (p.Y - points[i].Y) /
            //           (points[j].Y - points[i].Y) *
            //           (points[j].X - points[i].X) < p.X)
            //        {
            //            result = !result;
            //        }
            //    }

            //    j = i;
            //}

            //return result;

            //List<double> px = new List<double>();
            //List<double> py = new List<double>();

            //foreach (var vector in points)
            //{
            //    px.Add(vector.X);
            //    py.Add(vector.Y);
            //}

            //var point = px.Zip(py, (x_, y_) => new { x = x_, y = y_ });

            //int flag = point.Zip(point.Skip(1).Concat(point), (a, b) =>
            //{
            //    if (a.y == p.Y && b.y == p.Y)
            //        return (a.x <= p.X && p.X <= b.x) || (b.x <= p.X && p.X <= a.x) ? 0 : 1;
            //    return a.y <= b.y ?
            //        p.Y <= a.y || b.y < p.Y ? 1 : Math.Sign((a.x - p.X) * (b.y - p.Y) - (a.y - p.Y) * (b.x - p.X)) :
            //        p.Y <= b.y || a.y < p.Y ? 1 : Math.Sign((b.x - p.X) * (a.y - p.Y) - (b.y - p.Y) * (a.x - p.X));
            //}).Aggregate(-1, (r, v) => r * v);

            //return flag == 0 ? true : false;

            //int sides = this.Count();

            //int j = sides - 1;
            //bool pointStatus = false;

            //for (int i = 0; i < sides; i++)
            //{
            //    if (points[i].Y < p.Y && points[j].Y >= p.Y || points[j].Y < p.Y && points[i].Y >= p.Y)
            //    {
            //        if (points[i].X + (p.Y - points[i].Y) / (points[j].Y - points[i].Y) * (points[j].X - points[i].X) < p.X)
            //        {
            //            pointStatus = !pointStatus;
            //        }
            //    }

            //    j = i;
            //}

            //return pointStatus;

            bool result = false;
            var a = points.Last();
            foreach (var b in points)
            {
                if ((b.X == p.X) && (b.Y == p.Y))
                    return true;

                if ((b.Y == a.Y) && (p.Y == a.Y))
                {
                    if ((a.X <= p.X) && (p.X <= b.X))
                        return true;

                    if ((b.X <= p.X) && (p.X <= a.X))
                        return true;
                }

                if ((b.Y < p.Y) && (a.Y >= p.Y) || (a.Y < p.Y) && (b.Y >= p.Y))
                {
                    if (b.X + (p.Y - b.Y) / (a.Y - b.Y) * (a.X - b.X) <= p.X)
                        result = !result;
                }
                a = b;
            }
            return result;
        }
    }
}
