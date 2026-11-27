// Предмет: зелье, оружие или ключ.

public class Item
{
    // Виды предметов
    public const string Potion = "зелье";
    public const string Weapon = "оружие";
    public const string Key = "ключ";

    public string Name { get; }
    public string Kind { get; }     // один из видов выше
    public int Power { get; }       // зелье: сколько здоровья лечит; оружие: сколько добавляет к атаке
    public int Price { get; }       // цена у торговца

    public Item(string name, string kind, int power, int price)
    {
        Name = name;
        Kind = kind;
        Power = power;
        Price = price;
    }

    public override string ToString() => $"{Name} ({Kind})";
}
