using System;
using System.Collections.Generic;

namespace Architecture.Core 
{
    public class Board
    {
        public Guid Id { get; private set; }
        public IEnumerable<Section> Sections { get; set; }
        public DateTime LastUpdated { get; private set; }
    }
}
