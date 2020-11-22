using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2019.Day12.P01
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
            Vector position = Position;

            for (int i = 0; i < _neighbors.Length; i++)
            {
                Moon neighbor = _neighbors[i];
                Vector neighborPosition = neighbor.Position;

                x += Math.Sign(neighborPosition.X - position.X);
                y += Math.Sign(neighborPosition.Y - position.Y);
                z += Math.Sign(neighborPosition.Z - position.Z);
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