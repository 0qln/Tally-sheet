using Microsoft.VisualBasic;
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
            'v' => new ValueSortArgument(bool.Parse(value)),
            'k' => new KeySortArgument(bool.Parse(value)),

            _ => throw new ArgumentInvalidException(),
        };

        public class ClearArgument : ArgumentBase<object>
        {
            public ClearArgument(object val) : base(val) { }
            public override void Apply(OptionBase option) { /* just skip */  }
        }

        public class ValueSortArgument : ArgumentBase<bool>
        {
            public ValueSortArgument(bool ascending) : base(ascending) { }

            public override void Apply(OptionBase option)
            {
                ((ViewOption)option)._mode = delegate
                {
                    var keys = Program.Keys.ToArray();
                    var vals = Program.Values.ToArray();
                    Array.Sort(vals, keys);
                    if (Value)
                    {
                        Array.Reverse(vals);
                        Array.Reverse(keys);
                    }
                    Program.Keys = keys.ToList();
                    Program.Values = vals.ToList();
                };
            }
        }
        public class KeySortArgument : ArgumentBase<bool>
        {
            public KeySortArgument(bool ascending) : base(ascending) { }

            public override void Apply(OptionBase option)
            {
                ((ViewOption)option)._mode = delegate
                {
                    var keys = Program.Keys.ToArray();
                    var vals = Program.Values.ToArray();
                    Array.Sort(keys, vals);
                    if (Value)
                    {
                        Array.Reverse(vals);
                        Array.Reverse(keys);
                    }
                    Program.Keys = keys.ToList();
                    Program.Values = vals.ToList();
                };
            }
        }
    }
}
