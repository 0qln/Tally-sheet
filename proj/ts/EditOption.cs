namespace Tally_sheet
{
    // Object logic
    public partial class EditOption : OptionBase
    {
        private Action? _mode;
        private int _count;

        private EditOption()
        {
            _count = 1;
            _mode = null;
        }
        public static EditOption Default => new EditOption();


        public override string Execute()
        {
            try
            {
                if (_mode is null) throw new ArgumentNotSetException();

                for (int i = 0; i < _count; i++)
                {
                    _mode.Invoke();
                }

                return 
                    $"{TargetResult} was succesfully executed " +
                    $"{_count} times";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    }



    // Static declerations
    /// <summary>
    /// Edit the document
    /// </summary>
    public partial class EditOption : IOption
    {
        public IArgumentWrapper GenerateArgument(string name, dynamic value) => name switch
        {
            "a" => new AddArgument(value),
            "r" => new AddArgument(value),
            "d" => new AddArgument(value),
            "m" => new MultiArgument(int.Parse(value)),
            _ => throw new ArgumentException(),
        };

        /// <summary>
        /// Add to the given entry
        /// </summary>
        public class AddArgument : ArgumentBase<string>
        {
            public AddArgument(string val) : base(val) { }

            public override void Apply(OptionBase option)
            {
                ((EditOption)option)._mode = delegate {
                    if (Program.Values.ContainsKey(Value))
                    {
                        Program.Values[Value] += ((EditOption)option)._count;
                    }
                    else
                    {
                        Program.Values.Add(Value, 1);
                    }
                };
                option.TargetResult = "Addition";
            }
        }
        /// <summary>
        /// Remove from the given entry
        /// </summary>
        public class RemoveArgument : ArgumentBase<string>
        {
            public RemoveArgument(string val) : base(val) { }

            public override void Apply(OptionBase option)
            {
                ((EditOption)option)._mode = delegate {
                    Program.Values[Value] -= ((EditOption)option)._count;
                };
                option.TargetResult = "Removal";
            }
        }
        /// <summary>
        /// Delete the given entry
        /// </summary>
        public class DeleteArgument : ArgumentBase<string>
        {
            public DeleteArgument(string val) : base(val) { }

            public override void Apply(OptionBase option)
            {
                ((EditOption)option)._mode = delegate {
                    Program.Values.Remove(Value);
                };
                option.TargetResult = "Deletion";
            }
        }
        /// <summary>
        /// Multiply the action given times
        /// </summary>
        public class MultiArgument : ArgumentBase<int>
        {
            public MultiArgument(int val) : base(val) { }

            public override void Apply(OptionBase option)
            {
                ((EditOption)option)._count = Value;
            }
        }
    }
}
