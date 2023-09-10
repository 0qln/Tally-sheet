namespace Tally_sheet
{
    // Object
    public partial class QuitOption : OptionBase
    {
        private bool _quitSavely;

        private QuitOption()
        {
            _quitSavely = true;
        }
        public static QuitOption Default => new QuitOption();


        public override string Execute()
        {
            try
            {
                if (_quitSavely)
                {
                    SaveOption.Default.Execute();
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
        public IArgumentWrapper GenerateArgument(string name, dynamic value) => name switch
        {
            "s" => new SaveArgument(bool.Parse(value)),
            _ => throw new ArgumentException(),
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
