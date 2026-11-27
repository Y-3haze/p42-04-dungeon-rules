// Точка входа.
//   dotnet run        — запускает проверки (Checks.cs)
//   dotnet run play   — запускает саму игру

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        if (args.Length > 0 && args[0] == "play")
            Play();
        else
            Checks.RunAll();
    }

    // Игра в консоли: рисуем карту, читаем команду, выполняем её — и так до конца игры.
    static void Play()
    {
        Game game = new Game();

        Console.WriteLine("ПОДЗЕМЕЛЬЕ. Найдите ключ и дойдите до выхода (>).");
        Console.WriteLine(Commands.Help);

        while (!game.IsOver)
        {
            Console.WriteLine();
            foreach (string line in Renderer.MapLines(game))
                Console.WriteLine(line);
            Console.WriteLine(Renderer.Status(game));
            Console.Write("> ");

            string input = Console.ReadLine();
            if (input == null || input.Trim() == "q")
                break;

            Console.WriteLine(game.Execute(input));
        }

        Console.WriteLine();
        if (game.IsWon)
            Console.WriteLine("Победа! Ходов: " + game.Turn + ".");
        else
            Console.WriteLine("Игра окончена.");
    }
}
