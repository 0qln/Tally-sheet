using Tally_sheet.Exceptions;

namespace Tally_sheet
{
    // Object
    public partial class SaveOption : OptionBase
    {
        private SaveOption()
        {
        }
        public static SaveOption Default => new SaveOption();


        public override string Execute()
        {
            try
            {
                var content = new List<string>();
                foreach (var keyValuePair in Program.Values)
                {
                    content.Add($"{keyValuePair.Key} {keyValuePair.Value}");
                }
                File.WriteAllLines(Program.File, content);

                return $"Succesfully saved the file to '{Program.File}'";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    }

    // Statics
    /// <summary>
    /// Save the document
    /// </summary>
    public partial class SaveOption : IOption
    {
        IArgumentWrapper IOption.GenerateArgument(string name, dynamic value) => name switch
        {
            _ => throw new ArgumentInvalidException(),
        };
    }
}
