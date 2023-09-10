namespace Tally_sheet
{
    // Object
    public partial class ViewOption : OptionBase
    {
        private ViewOption() { }
        public static ViewOption Default => new ViewOption();

        public override string Execute()
        {
            return new NotImplementedException().ToString();
        }
    }

    // Statics
    /// <summary>
    /// Change the appearance, e.g. sort the sheet
    /// </summary>
    public partial class ViewOption : IOption
    {
        public IArgumentWrapper GenerateArgument(string name, dynamic value) => name switch
        {
            _ => throw new ArgumentException(),
        };
    }
}
