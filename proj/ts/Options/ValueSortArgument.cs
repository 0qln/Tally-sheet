namespace Tally_sheet
{
    public partial class ViewOption
    {
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
    }
}
