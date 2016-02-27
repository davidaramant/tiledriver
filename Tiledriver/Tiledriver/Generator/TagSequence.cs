using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiledriver.Generator
{
    public sealed class TagSequence
    {
        private int _currentTagId = 0;

        public int GetNext()
        {
            return _currentTagId++;
        }
    }
}
