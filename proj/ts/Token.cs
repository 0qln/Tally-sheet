using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Tally_sheet
{
    //      COMMAND
    // -[Option]
    //    \[Argument]
    //        "[Value]"
    // -e \a "Artemis" \m "4"


    public static class TokenAssembler
    {
        public const char OPTION_INDICATOR = '-';
        public const char ARGUMENT_INDICATOR = '\\';
        public const char VALUE_INDICATOR = '\'';

        public static string NormalizeWhiteSpace(string text)
        {
            while     (text.Contains("  "))            
                text = text.Replace ("  ", " ");
            
            if (text [0] == ' ') text = text.Substring(1);
            if (text[^1] != ' ') text += " ";

            return text;
        }
        public static string GetInBetween(string text, char start, char end)
        {
            int startIdx = text.IndexOf(start) + 1;
            int endIdx = text.IndexOf(end);
            return text.Substring(startIdx, endIdx - startIdx);
        }

        public static Command[] GetCommands(string? userInput)
        {
            List<Command> result = new List<Command>();

            if (string.IsNullOrEmpty(userInput) ||
                string.IsNullOrWhiteSpace(userInput))
                return result.ToArray();

            int token;
            userInput = NormalizeWhiteSpace (userInput);
            var tokens = new List<string>();
            var commands = new List<int>();

            int index = -1;
            while (++index < userInput.Length)
            {
                string window = userInput.Substring(index, userInput.Length - index);
                char indicator = window[0];

                var isArg = indicator == ARGUMENT_INDICATOR;
                var isOpt = indicator == OPTION_INDICATOR;
                var isVal = indicator == VALUE_INDICATOR;
                if (!isArg && !isVal && !isOpt) continue; 
                
                string tokenS = GetInBetween(window, indicator, ' ');              
                if (tokenS == "") continue;
                
                if (isOpt) commands.Add(tokens.Count);
                tokens.Add(tokenS);
            }

            for (int i  = 0; i < commands.Count; i++)
            {
                token = commands[i];
                var opt = tokens[token];
                var option = OptionHelper.GenerateOption(opt);
                List<IArgumentWrapper> args = new();

                string currArg = string.Empty;
                while (token < (i+1 >= commands.Count ? tokens.Count : commands[i+1]))
                {
                    bool isVal = tokens[token][^1] == VALUE_INDICATOR;
                    if (!isVal) currArg = tokens[token];
                    else
                    {
                        var value = tokens[token].Replace("\"", "");
                        args.Add(option.GenerateArgument(currArg, value));
                    }
                    ++token;
                }

                result.Add(new Command(option, args.ToArray()));
            }

            return result.ToArray();
        }
    }
}
