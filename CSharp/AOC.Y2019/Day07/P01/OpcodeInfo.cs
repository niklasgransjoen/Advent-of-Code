using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AOC.Y2019.Day07.P01
{
    public readonly struct OpcodeInfo
    {
        public OpcodeInfo(IEnumerable<bool> isWriteDestination)
        {
            IsWriteDestination = new ReadOnlyCollection<bool>(isWriteDestination.ToArray());
        }

        public int ParameterCount => IsWriteDestination.Count;

        public IReadOnlyList<bool> IsWriteDestination { get; }
    }
}