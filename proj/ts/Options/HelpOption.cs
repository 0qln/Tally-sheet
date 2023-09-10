using Tally_sheet.Exceptions;

namespace Tally_sheet
{
    // Object
    public partial class HelpOption : OptionBase
    {
        private HelpOption() { }
        public static HelpOption Default => new HelpOption();

        public override string Execute()
        {
            // Print a list of all options and their paramters
            return new NotImplementedException().ToString();
        }
    }


    // Statics
    /// <summary>
    /// Print all possible options and arguments
    /// </summary>
    public partial class HelpOption : IOption
    {
        public IArgumentWrapper GenerateArgument(string name, dynamic value) => name switch
        {
            _ => throw new ArgumentInvalidException(),
        };
    }
}
