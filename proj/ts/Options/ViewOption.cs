using Tally_sheet.Exceptions;

namespace Tally_sheet
{
    // Object
    public partial class ViewOption : OptionBase
    {
        private Action? _mode;


        public ViewOption()
        {
            _mode = null;
        }


        public override string Execute()
        {
            try
            {
                _mode?.Invoke();

                return TargetResult == "" || TargetResult is null ? "" :
                    $"{TargetResult} was succesfully executed ";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    }

    // Statics
    /// <summary>
    /// Change the appearance, e.g. sort the sheet, clear terminal
    /// </summary>
    public partial class ViewOption : IOption
    {
        // Names have to have a different starting letter
        public IArgumentWrapper GenerateArgument(string name, dynamic value) => name.ToLower()[0] switch
        {
            'c' => new ClearArgument(value),

            _ => throw new ArgumentInvalidException(),
        };

        public class ClearArgument : ArgumentBase<object>
        {
            public ClearArgument(object val) : base(val)
            {
            }

            public override void Apply(OptionBase option)
            {
                // just skip 
            }
        }
    }
}
