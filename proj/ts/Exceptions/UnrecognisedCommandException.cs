using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tally_sheet.Exceptions
{
    internal class UnrecognisedCommandException : Exception
    {
        public UnrecognisedCommandException()
            : base("Unrecognised command.")
        {
        }

        public UnrecognisedCommandException(string message)
            : base(message)
        {
        }

        public UnrecognisedCommandException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
