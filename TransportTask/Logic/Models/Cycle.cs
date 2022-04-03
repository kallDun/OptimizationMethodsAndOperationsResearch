using System.Collections.Generic;
using System.Windows;

namespace TransportTask.Logic.Models
{
    public struct Cycle
    {
        public Point StartVertex { get; set; }
        public List<Vertex> Vertices { get; set; }
        public Vertex SelectedMinimumVertex { get; set; }
    }
}