using System;
using System.Collections.Generic;

namespace TransportTask.Logic.Models
{
    public struct Coord : IComparable<Coord>, IEqualityComparer<Coord>
    {
        public int I, J;
        public Coord(int i, int j)
        {
            I = i;
            J = j;
        }

        public int CompareTo(Coord other)
        {
            if (I == other.I && J == other.J) return 0;
            return -1;
        }

        public bool Equals(Coord x, Coord y) => x.CompareTo(y) == 0;
        public int GetHashCode(Coord obj) => 17 * obj.I.GetHashCode() + obj.J.GetHashCode();
    }
}