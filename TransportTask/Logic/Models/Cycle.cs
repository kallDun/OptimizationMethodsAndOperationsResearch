using System.Collections.Generic;
using System.Windows;

namespace TransportTask.Logic.Models
{
    public struct Cycle
    {
        public bool IsVisible { get; set; }
        public Coord StartVertex { get; set; }
        public List<Vertex> Vertices { get; set; }
        public Vertex SelectedMinimumVertex { get; set; }
    }
}