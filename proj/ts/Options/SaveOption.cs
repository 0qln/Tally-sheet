using System.Collections.Generic;
using Tally_sheet.Exceptions;

namespace Tally_sheet
{
    // Object
    public partial class SaveOption : OptionBase
    {
        public SaveOption()
        {
        }


        public override string Execute()
        {
            try
            {
                var content = new List<string>();
                for (int i = 0; i < Program.Keys.Count; i++)
                {
                    content.Add($"{Program.Keys[i]} {Program.Values[i]}");
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
        // Names have to have a different starting letter
        public IArgumentWrapper GenerateArgument(string name, dynamic value) => name.ToLower()[0] switch
        {


            _ => throw new ArgumentInvalidException(),
        };
    }
}
