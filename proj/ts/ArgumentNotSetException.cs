using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tally_sheet
{
    internal class ArgumentNotSetException : Exception
    {
        public ArgumentNotSetException()
            : base("Some argument of an option had to be set in order" +
                  "for this command to execute.")
        {
        }

        public ArgumentNotSetException(string message)
            : base(message)
        {
        }

        public ArgumentNotSetException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
