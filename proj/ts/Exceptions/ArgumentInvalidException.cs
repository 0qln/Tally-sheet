using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tally_sheet.Exceptions
{
    internal class ArgumentInvalidException : Exception
    {
        public ArgumentInvalidException()
            : base("Argument was invalid.")
        {
        }

        public ArgumentInvalidException(string message)
            : base(message)
        {
        }

        public ArgumentInvalidException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
