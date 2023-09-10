using Tally_sheet.Exceptions;

namespace Tally_sheet
{
    public partial class AnalyzeOption : OptionBase
    {

        public AnalyzeOption() { }


        public override string Execute()
        {
            throw new NotImplementedException();
        }
    }

    // Statics
    /// <summary>
    /// Print analysis, e.g. Avarage, Standart deviation
    /// </summary>
    public partial class AnalyzeOption : IOption
    {
        public IArgumentWrapper GenerateArgument(string name, dynamic value) => name switch
        {
            _ => throw new ArgumentInvalidException(),
        };
    }
}
