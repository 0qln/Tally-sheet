﻿using Tally_sheet.Exceptions;

namespace Tally_sheet
{
    // Object logic
    public partial class EditOption : OptionBase
    {
        private Action? _mode;
        private int _count;


        public EditOption()
        {
            _count = 1;
            _mode = null;
        }


        public override string Execute()
        {
            try
            {
                if (_mode is null) throw new ArgumentNotSetException();

                _mode.Invoke();                

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
        // Names have to have a different starting letter
        public IArgumentWrapper GenerateArgument(string name, dynamic value) => name.ToLower()[0] switch
        {
            'a' => new AddArgument(value),
            'r' => new RemoveArgument(value),
            'd' => new DeleteArgument(value),
            'c' => new CountArgument(int.Parse(value)),

            _ => throw new ArgumentInvalidException(),
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
                        Program.Values[Value] += ((EditOption)option)._count;
                    
                    else                    
                        Program.Values.Add(Value, ((EditOption)option)._count);
                    
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
                    if (Program.Values.ContainsKey(Value) &&
                        Program.Values[Value] >= ((EditOption)option)._count)                    
                        Program.Values[Value] -= ((EditOption)option)._count;
                    
                    if (Program.Values[Value] == 0)                    
                        Program.Values.Remove(Value);
                    
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
        public class CountArgument : ArgumentBase<int>
        {
            public CountArgument(int val) : base(val) { }

            public override void Apply(OptionBase option)
            {
                ((EditOption)option)._count = Value;
            }
        }
    }
}
