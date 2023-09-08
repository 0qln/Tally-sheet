public static class Programm
{
    private static Dictionary<string, int> _values;
    private static readonly string AppData = Path.Combine(Environment.GetFolderPath(
    Environment.SpecialFolder.ApplicationData), "Tally sheet");
    private static string _file;

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
        

        Console.WriteLine("Select file or create a new one:");
        foreach (var item in Directory.GetFiles(AppData))
        {
            Console.WriteLine($"{item}");
        };
        Console.Write("\n");

        _file = Console.ReadLine() ?? "";
        if (File.Exists(_file))
        {
            var lines = File.ReadAllLines(_file);

            _values = new Dictionary<string, int>(lines.Length);
            foreach (var line in lines)
            {
                var key = Word(line);
                var value = Count(line);
                if  (_values.ContainsKey(key)) _values[key] += value; 
                else _values.Add(key, value);
            }
        }
        else
        {

            using var _ = File.Create(_file);
            _values = new Dictionary<string, int>();
        }
    }

    private static void PrintValues()
    {
        if (_values.Count == 0) return;

        int maxValueWidith = _values.Max(x => x.ToString().Length);
        foreach (var item in _values)
        {
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