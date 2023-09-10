using Tally_sheet.Exceptions;

namespace Tally_sheet
{
    // Object
    public partial class QuitOption : OptionBase
    {
        private bool _quitSavely;

        public QuitOption()
        {
            _quitSavely = true;
        }


        public override string Execute()
        {
            try
            {
                if (_quitSavely)
                {
                    new SaveOption().Execute();
                }

                Environment.Exit(0);

                return "Successfully saved quit the application";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    }


    // Statics
    /// <summary>
    /// Quit the application
    /// </summary>
    public partial class QuitOption : IOption
    {
        // Names have to have a different starting letter
        public IArgumentWrapper GenerateArgument(string name, dynamic value) => name.ToLower()[0] switch
        {
            's' => new SaveArgument(bool.Parse(value)),

            _ => throw new ArgumentInvalidException(),
        };


        public class SaveArgument : ArgumentBase<bool>
        {
            public SaveArgument(bool val) : base(val) { }

            public override void Apply(OptionBase option)
            {
                ((QuitOption)option)._quitSavely = Value;
            }
        }
    }
}
