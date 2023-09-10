using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tally_sheet.Exceptions
{
    internal class OptionInvalidException : Exception
    {
        public OptionInvalidException()
            : base("Option was invalid.")
        {
        }

        public OptionInvalidException(string message)
            : base(message)
        {
        }

        public OptionInvalidException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
