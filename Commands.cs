// Разбор строки, которую ввёл игрок: слово-команда и номер после него.

public static class Commands
{
    public const string Help =
        "Команды: w — вверх, s — вниз, a — влево, d — вправо; " +
        "i — сумка; use N — использовать предмет номер N; " +
        "shop — товары торговца; buy N — купить товар номер N; " +
        "help — эта подсказка; q — выйти.";

    // Первое слово строки маленькими буквами: "  USE 2 " → "use". Пустая строка → "".
    public static string Word(string line)
    {
        string[] parts = Split(line);
        if (parts.Length == 0)
            return "";
        return parts[0].ToLower();
    }

    // Номер после команды: "use 2" → 2. Если номера нет или это не число — 0.
    public static int Number(string line)
    {
        string[] parts = Split(line);
        if (parts.Length < 2)
            return 0;

        int number;
        if (int.TryParse(parts[1], out number))
            return number;
        return 0;
    }

    // Делит строку на слова по пробелам.
    static string[] Split(string line)
    {
        if (line == null)
            return new string[0];
        return line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    }
}
