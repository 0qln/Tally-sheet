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
        // Names have to have a different starting letter
        public IArgumentWrapper GenerateArgument(string name, dynamic value) => name.ToLower()[0] switch
        {


            _ => throw new ArgumentInvalidException(),
        };
    }
}
