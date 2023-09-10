using Tally_sheet;
using Tally_sheet.Exceptions;


public static class Program
{
    public static List<string> Keys { get; set; } = new();
    public static List<int> Values { get; set; } = new();
    private static readonly string AppData = Path.Combine(Environment.GetFolderPath(
        Environment.SpecialFolder.ApplicationData), "Tally sheet");
    public static string File { get; private set; } = string.Empty;

    public static void Main(string[] args)
    {
        Startup();

        // Main application
        string lastOutput = string.Empty;
        do
        {
            Console.Clear();
            Console.WriteLine(File);
            
            PrintValues();
            Console.WriteLine();
            Console.WriteLine("type '-help' for more information");
            Console.WriteLine("\n");
            
            Console.WriteLine("\n"+lastOutput);
            Console.WriteLine();

            lastOutput = string.Empty;
            var userInput = Console.ReadLine();
            try
            {
                var commands = TokenAssembler.GetCommands(userInput).ToList();

                if (commands is null || commands.Count() == 0) 
                    throw new UnrecognisedCommandException();

                foreach (var command in commands)
                    lastOutput += command.Execute() + "\n";
            }
            catch (Exception e)
            {
                lastOutput = $"Error: {e}";
            }
        }
        while (true);
    }

    private static void Startup()
    {
        if (!Directory.Exists(AppData))
            Directory.CreateDirectory(AppData);

        SelectSheet();
        InitDict();
    }

    private static void InitDict()
    {
        if (System.IO.File.Exists(File))
        {
            var lines = System.IO.File.ReadAllLines(File);

            Keys = new List<string>(lines.Length);
            Values = new List<int>(lines.Length);

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var key = Word(line);
                var value = Count(line);

                if (Keys.Contains(key)) Values[Keys.IndexOf(key)] += value;
                else
                {
                    Keys.Add(key);
                    Values.Add(value);
                }
            }
        }
        else
        {
            using var _ = 
                System.IO.File.Create(File);
        }
    }

    private static void SelectSheet()
    {
        var selected = 0;
        var quit = false;
        do
        {
            Console.Clear();
            Console.WriteLine("Select file or create a new one: [arrow keys]");
            int count = PrintSheets(out var selection, selected);
            Console.Write("\n");

            var s = ReadLine(selection, out var key);

            switch (key)
            {
                case ConsoleKey.Enter: quit = true ; break; // confirm
                case ConsoleKey.UpArrow: selected--; break; // select one up
                case ConsoleKey.DownArrow: selected++; break; // select one up
            }

            selected = Math.Clamp(selected, 0, count - 1);
            File = s;
        }
        while (!quit);
    }

    private static string ReadLine(string Default, out ConsoleKey result)
    {
        result = (ConsoleKey)(-1);
        Console.Write(Default);
        List<char> chars = new List<char>();
        if (!string.IsNullOrEmpty(Default))
            chars.AddRange(Default.ToCharArray());

        bool quit = false;
        while (!quit)
        {
            var info = Console.ReadKey(true);

            switch (info.Key)
            {
                case ConsoleKey.Backspace:
                    if (Console.CursorLeft >= 1)
                    {
                        chars.RemoveAt(Console.CursorLeft-1);
                        Console.CursorLeft--;
                        Reprint();
                    }
                    break;

                case ConsoleKey.LeftArrow:
                    if (Console.CursorLeft >= 1) Console.CursorLeft--;
                    break;

                case ConsoleKey.RightArrow:
                    if (Console.CursorLeft < chars.Count) Console.CursorLeft++;
                    break;

                case ConsoleKey.Enter:
                case ConsoleKey.DownArrow:
                case ConsoleKey.UpArrow:
                    result = info.Key;
                    quit = true;
                    break;

                default:
                    Console.CursorLeft++;
                    chars.Insert(Console.CursorLeft-1, info.KeyChar);
                    Reprint();
                    break;
            }            
        }

        void Reprint()
        {
            int x = Console.CursorLeft, y = Console.CursorTop;
            ClearCurrentConsoleLine();
            Console.WriteLine(new string(chars.ToArray()));
            Console.SetCursorPosition(x, y);
        }

        if ((int)result == -1) throw new Exception("How the fuck did you get here??");
        
        return new string(chars.ToArray());
    }
    private static void ClearCurrentConsoleLine()
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLineCursor);
    }
    private static int PrintSheets(out string selection, int selected = -1)
    {
        selection = "";
        int i = -1;
        var files = Directory.GetFiles(AppData);
        foreach (var item in files)
        {
            if (++i == selected)
            {
                selection = item.ToString();
                Console.Write(" > ");
            }
            Console.WriteLine($"{item}");
        };
        return files.Length;
    }

    private static void PrintValues(int selected = -1)
    {
        if (Values.Count == 0) return;

        int maxValueWidith = Values.Max(x => x.ToString().Length);
        for (int i = 0; i < Values.Count; i++)
        {
            if (i == selected) Console.Write(" > ");
            Console.WriteLine($"{Keys[i].PadRight(maxValueWidith)} {Values[i]}");
        }
    }

    private static string Word(string s)
    {
        return new string(s.Split(" ")[0]) ?? "";
    }

    private static int Count(string s)
    {
        try
        {
            return int.Parse(s.Split(" ")[^1]);
        }
        catch (FormatException)
        {
            return 1;
        }
    }

}