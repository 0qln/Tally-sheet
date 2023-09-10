using System.Reflection;
using System.Text;
using Tally_sheet.Exceptions;

namespace Tally_sheet
{
    // Object
    public partial class HelpOption : OptionBase
    {
        private const string tab = "    ";


        public HelpOption() { }


        public override string Execute()
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(@" --General Command structure--  ");
            sb.AppendLine(@" -[Option]   \[Argument]   '[Value]'    ");
            sb.AppendLine(@"                            ");
            sb.AppendLine(@"e.g: -e \a 'myValue' \m '4' ");

            sb.AppendLine(@"                            ");
            sb.AppendLine(@"                            ");
            sb.AppendLine(@" --Options--  ");
            foreach (var option in OptionHelper.OptionTypes)
            {
                var optName = option.Name.Replace("Option", "");
                sb.AppendLine(@"Option:");
                sb.AppendLine($"{tab}{optName} / {optName.ToLower()} / {optName.ToLower()[0]}");
                sb.AppendLine(@"Arguments:");
                foreach (var arg in OptionHelper.OptionArguments(option))
                {
                    var argName = arg.Name.Replace("Argument", "");
                    sb.AppendLine($"{tab}{argName} / {argName.ToLower()} / {argName.ToLower()[0]}");

                }
                sb.AppendLine($"Defaults: ");
                object instance = Activator.CreateInstance(option);
                foreach (var field in 
                    option.GetFields().Concat(  //public fields
                    option.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)))  // private fields
                {
                    if (field.Name == "TargetResult") continue;
                    object defaultValue = field.GetValue(instance);
                    var val = defaultValue is null ? "null" : defaultValue.ToString();
                    sb.AppendLine($"{tab}{field.Name}: {val}");
                }

                sb.AppendLine(@"                            ");
                sb.AppendLine(@"                            ");
            }
            
            sb.AppendLine(@"                            ");
            sb.AppendLine(@" --  Value types  --  ");
            sb.AppendLine(@"boolean:    [true / false]");
            sb.AppendLine(@"string:     [value]");
            sb.AppendLine(@"number:     [numeric value]");


            return sb.ToString();
        }
    }


    // Statics
    /// <summary>
    /// Print all possible options and arguments
    /// </summary>
    public partial class HelpOption : IOption
    {
        // Names have to have a different starting letter
        public IArgumentWrapper GenerateArgument(string name, dynamic value) => name.ToLower()[0] switch
        {


            _ => throw new ArgumentInvalidException(),
        };
    }
}
 