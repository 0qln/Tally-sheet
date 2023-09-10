using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tally_sheet.Exceptions;

namespace Tally_sheet
{
    public static class OptionHelper
    {
        public static IEnumerable<Type> OptionTypes => AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(IOption).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        public static IEnumerable<Type> OptionArguments(Type option)
        {
            return option.GetNestedTypes();
        }
     

        public static IOption GenerateOption(string name) => name.ToLower()[0] switch
        {
            'q' => new QuitOption(),
            's' => new SaveOption(),
            'e' => new EditOption(), 
            'v' => new ViewOption(),
            'h' => new HelpOption(),
            'a' => new AnalyzeOption(),

            _ => throw new OptionInvalidException()
        };
    }


    public partial record class Command(IOption Option, params IArgumentWrapper[] Arguments);

    public partial record class Command
    {
        public string Execute()
        {
            return Option.ExecuteWith(Arguments);
        }
    }



    public interface IArgumentWrapper
    {
        public void Apply(OptionBase option);
    }
    public interface IArgument<T> : IArgumentWrapper
    {
        public static IArgument<T>? Default { get; }
        public T Value { get; }
    }
    public abstract class ArgumentBase<T> : IArgument<T>
    {
        public T Value { get; }
        public ArgumentBase(T val) { Value = val; }
        public abstract void Apply(OptionBase option);
    }
    public class DummyArg : IArgumentWrapper
    {
        public void Apply(OptionBase opt) { }
    }


    public interface IOptionWrapper
    {
    }
    public interface IOption : IOptionWrapper
    {
        public IArgumentWrapper GenerateArgument(string name, dynamic value);
        public string ExecuteWith(IArgumentWrapper[] args);
    }
    public abstract class OptionBase : IOptionWrapper
    {
        public string? TargetResult;
        public abstract string Execute();

        public string ExecuteWith(IArgumentWrapper[] args)
        {
            SetArgs(args);
            return Execute();
        }

        public void SetArgs(IArgumentWrapper[] args)
        {
            foreach (var arg in args)
            {
                arg.Apply(this);
            }
        }
    }
}
