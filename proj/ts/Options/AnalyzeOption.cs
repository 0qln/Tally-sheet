using Tally_sheet.Exceptions;

namespace Tally_sheet
{
    public partial class AnalyzeOption : OptionBase
    {
        private decimal _relativeProb;

        public AnalyzeOption()
        {
            _relativeProb = -1;
        }


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


        // calculate the relative prob of a given entry
        public class RelativeProbabilityArgument : ArgumentBase<string>
        {
            public RelativeProbabilityArgument(string name) : base(name) { }

            public override void Apply(OptionBase option)
            {
                //((AnalyzeOption)option)._relativeProb = 
            }
        }
    }
}
