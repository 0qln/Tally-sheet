

public static class Programm
{
    private static Dictionary<string, int> _values = new();
    private static readonly string AppData = Path.Combine(Environment.GetFolderPath(
    Environment.SpecialFolder.ApplicationData), "Tally sheet");
    private static string _file = string.Empty;

    public static void Main(string[] args)
    {
        Init();

        do
        {
            Console.Clear();
            Console.WriteLine(_file);
            PrintValues();
            Console.WriteLine("\nquit | save | +{value} | -{value}");

            var userInput = Console.ReadLine();
            bool quit = false;
            switch (userInput)
            {
                case "quit": quit = true; break;
                case "save": Save(); break;
                case null: case "": continue;
                default: HandleUserInput(userInput); break;
            }
            if (quit) break;
        }
        while (true);
    }

    private static void HandleUserInput(string userInput)
    {
        var value = userInput.Substring(1, userInput.Length - 1);
        var mode = userInput[0];
        if (mode == '+')
        {
            if (_values.ContainsKey(value))
                _values[value]++;
            else
                _values.Add(value, 1);
            
        }
        if (mode == '-')
        {
            if (_values.ContainsKey(value))
            {
                _values[value]--;
                if (_values[value] == 0) 
                    _values.Remove(value);               
            }
        }
    }

    private static void Save()
    {
        var content = new List<string>();
        foreach (var keyValuePair  in _values)
        {
            content.Add($"{keyValuePair.Key} {keyValuePair.Value}");
        }
        File.WriteAllLines(_file, content);
    }

    private static void Init()
    {
        if (!Directory.Exists(AppData))
            Directory.CreateDirectory(AppData);

        SelectSheet();
        InitDict();
    }

    private static void InitDict()
    {
        if (File.Exists(_file))
        {
            var lines = File.ReadAllLines(_file);

            _values = new Dictionary<string, int>(lines.Length);
            foreach (var line in lines)
            {
                var key = Word(line);
                var value = Count(line);
                if (_values.ContainsKey(key)) _values[key] += value;
                else _values.Add(key, value);
            }
        }
        else
        {

            using var _ = File.Create(_file);
            _values = new Dictionary<string, int>();
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
            _file = s;
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
                    chars.RemoveAt(Console.CursorLeft-1);
                    Console.CursorLeft--;
                    Reprint();
                    break;

                case ConsoleKey.LeftArrow:
                    Console.CursorLeft--;
                    break;

                case ConsoleKey.RightArrow:
                    Console.CursorLeft++;
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
        if (_values.Count == 0) return;

        int maxValueWidith = _values.Max(x => x.ToString().Length);
        int i = -1;
        foreach (var item in _values)
        {
            if (++i == selected) Console.Write(" > ");
            Console.WriteLine($"{item.Key.ToString().PadRight(maxValueWidith)} {item.Value}");
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