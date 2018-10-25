using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VindLocatieWeekend
{
    struct Vector
    {
        public double x { get; }
        public double y { get; }

        public static Vector Zero { get { return new Vector(0, 0); } }
        public static Vector NaN { get { return new Vector(double.NaN, double.NaN); } }


        public double SquaredLength { get { return x * x + y * y; } }
        public double Length { get { return Math.Sqrt(SquaredLength); } }

        public Vector(double x, double y)
        {
            this.x = x; this.y = y;
        }

        public Vector(Point point)
        {
            x = point.X; y = point.Y;
        }

        public System.Windows.Point AsPoint()
        {
            return new System.Windows.Point(x, y);
        }

        internal static double Distance(Vector v1, Vector v2)
        {
            return Math.Sqrt(Math.Pow(v1.x - v2.x, 2) + Math.Pow(v1.y - v2.y, 2));
        }

        internal static double DistanceSq(Vector v1, Vector v2)
        {
            double diffx = v1.x - v2.x;
            double diffy = v1.y - v2.y;
            return diffx * diffx + diffy * diffy;
        }

        internal static bool IsClose(Vector v1, Vector v2)
        {
            return Math.Abs(v1.x - v2.x) <= double.Epsilon && Math.Abs(v1.y - v2.y) <= double.Epsilon;
        }

        internal static Vector Average(Vector v1, Vector v2)
        {
            return new Vector((v1.x + v2.x) / 2, (v1.y + v2.y) / 2);
        }

        /// <summary>
        /// Is the circle through these vectors going counterclockwise
        /// </summary>
        /// <param name="P0"></param>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <param name="PlusOneOnZeroDegrees"></param>
        /// <returns></returns>
        internal static int ccw(Vector P0, Vector P1, Vector P2, bool PlusOneOnZeroDegrees)
        {
            double dx1, dx2, dy1, dy2;
            dx1 = P1.x - P0.x; dy1 = P1.y - P0.y;
            dx2 = P2.x - P0.x; dy2 = P2.y - P0.y;
            if (dx1 * dy2 > dy1 * dx2) return +1;
            if (dx1 * dy2 < dy1 * dx2) return -1;
            if ((dx1 * dx2 < 0) || (dy1 * dy2 < 0)) return -1;
            if ((dx1 * dx1 + dy1 * dy1) < (dx2 * dx2 + dy2 * dy2) && PlusOneOnZeroDegrees)
                return +1;
            return 0;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.x - v2.x, v1.y - v2.y);
        }

        public static Vector operator *(double d, Vector v)
        {
            return new Vector(d * v.x, d * v.y);
        }

        public static Vector operator *(Vector v, double d)
        {
            return new Vector(d * v.x, d * v.y);
        }

        public static double Dot(Vector v1, Vector v2)
        {
            return v1.x * v2.x + v1.y * v2.y;
        }

        //public static bool operator ==(Vector v1, Vector v2)
        //{
        //    return v1.x == v2.x && v1.y == v2.y;
        //}

        //public static bool operator !=(Vector v1, Vector v2)
        //{
        //    return v1.x != v2.x || v1.y != v2.y;
        //}

        /// <summary>
        /// In radian
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double Angle(Vector v1, Vector v2)
        {
            //double ang1 = (Math.Atan2(v2.y, v2.x) + Math.PI) % (2 * Math.PI);
            //double ang2 = (Math.Atan2(v1.y, v1.x) + Math.PI) % (2 * Math.PI);
            //return ang1 - ang2;
            return Math.Atan2(v2.y - v1.y, v2.x - v1.x);
        }

        public static List<Vector> CleanAndOrderVectorsAroundPoint(Vector point, List<Vector> list)
        {
            List<Tuple<Vector, double>> angles = list.ConvertAll(c => new Tuple<Vector, double>(c, Vector.Angle(point, c)));
            var ret = new List<Vector>();
            while (angles.Count > 0)
            {
                double smallestValue = angles.Min(tp => tp.Item2);
                Vector smallest = angles.Find(tpl => tpl.Item2 - smallestValue <= double.Epsilon).Item1;
                angles.RemoveAll(tpl => Vector.DistanceSq(tpl.Item1, smallest) <= double.Epsilon);
                ret.Add(smallest);
            }
            return ret;
        }

        public override string ToString()
        {
            return "{" + x.ToString("000") + "::" + y.ToString("000") + "}";
        }

        public static implicit operator System.Windows.Point(Vector v)
        {
            return new Point(v.x, v.y);
        }

        public static Comparer<Vector> Comparer_x { get { return Comparer<Vector>.Create((a, b) => a.x.CompareTo(b.x)); } }
        public static Comparer<Vector> Comparer_y { get { return Comparer<Vector>.Create((a, b) => a.y.CompareTo(b.y)); } }
    }
}
