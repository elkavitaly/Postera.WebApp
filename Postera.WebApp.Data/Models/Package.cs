using System;

namespace Postera.WebApp.Data.Models
{
    public class Package
    {
        public Guid Id { get; set; }

        public float Weight { get; set; }

        public float Height { get; set; }

        public float Width { get; set; }

        public float Depth { get; set; }

        public string Type { get; set; }

        public string Notes { get; set; }
    }
}