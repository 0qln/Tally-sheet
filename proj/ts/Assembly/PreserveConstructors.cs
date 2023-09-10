namespace Tally_sheet
{
    public partial class Assembly
    {
        public void EnsureValuePreserved()
        {
            new ViewOption();
            new ViewOption.ValueSortArgument(true);
            new ViewOption.KeySortArgument(true);
            new ViewOption.ClearArgument(true);

            new EditOption();
            new EditOption.AddArgument("");
            new EditOption.RemoveArgument("");
            new EditOption.DeleteArgument("");
            new EditOption.CountArgument(0);

            new AnalyzeOption();

            new HelpOption();

            new QuitOption();
            new QuitOption.SaveArgument(true);

            new SaveOption();
        }
    }
}