using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2019.Day12.P02
{
    public sealed class Moon
    {
        private Moon[] _neighbors = Array.Empty<Moon>();

        public Moon(Vector position)
        {
            Position = position;
        }

        public Vector Position { get; private set; }
        public Vector Velocity { get; private set; }

        public void Initialize(IEnumerable<Moon> neighbors)
        {
            _neighbors = neighbors.ToArray();
        }

        /// <summary>
        /// Changes the two moons' velocity by interacting with each other.
        /// </summary>
        public void Interact()
        {
            int x, y, z;
            x = y = z = 0;

            for (int i = 0; i < _neighbors.Length; i++)
            {
                Moon neighbor = _neighbors[i];

                x += Math.Sign(neighbor.Position.X - Position.X);
                y += Math.Sign(neighbor.Position.Y - Position.Y);
                z += Math.Sign(neighbor.Position.Z - Position.Z);
            }

            Velocity += new Vector(x, y, z);
        }

        /// <summary>
        /// Executes a single step forward for this moon (time-domain).
        /// </summary>
        public void Step()
        {
            Position += Velocity;
        }
    }
}