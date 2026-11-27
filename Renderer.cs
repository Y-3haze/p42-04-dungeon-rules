// Всё, что игра показывает на экране: карта, строка состояния, сумка, товары торговца.
// Методы ничего не печатают сами — они возвращают текст.

public static class Renderer
{
    public const char PlayerSymbol = '@';

    // Карта построчно: вместе с игроком, живыми монстрами и предметами на полу.
    public static string[] MapLines(Game game)
    {
        string[] lines = new string[game.Map.Height];

        for (int y = 0; y < game.Map.Height; y++)
        {
            string line = "";
            for (int x = 0; x < game.Map.Width; x++)
                line += SymbolAt(game, x, y);
            lines[y] = line;
        }

        return lines;
    }

    // Что нарисовать на клетке (x, y): игрока, живого монстра, предмет на полу или саму клетку карты.
    static char SymbolAt(Game game, int x, int y)
    {
        if (game.Player.X == x && game.Player.Y == y)
            return PlayerSymbol;

        Monster monster = game.FindMonsterAt(x, y);
        if (monster != null)
            return monster.Symbol;

        FloorItem loot = game.FindLootAt(x, y);
        if (loot != null)
            return LootSymbol(loot.Item);

        return game.Map.TileAt(x, y);
    }

    // Символ предмета на полу: зелье — '!', ключ — 'k', оружие — '/'.
    static char LootSymbol(Item item)
    {
        if (item.Kind == Item.Potion)
            return '!';
        if (item.Kind == Item.Key)
            return 'k';
        return '/';
    }

    // Строка состояния под картой.
    public static string Status(Game game)
    {
        Player player = game.Player;
        return "HP " + player.Health + "/" + Player.MaxHealth +
               " | Атака " + player.Attack +
               " | Золото " + player.Gold +
               " | Ход " + game.Turn;
    }

    // Содержимое сумки (команда i).
    public static string BagText(Player player)
    {
        string text = "Сумка (" + player.Bag.Count + " из " + Inventory.Capacity + "):";
        if (player.Bag.Count == 0)
            text += " пусто";

        for (int i = 0; i < player.Bag.Count; i++)
            text += "\n  " + (i + 1) + ". " + player.Bag.Get(i).Name;

        if (player.Weapon != null)
            text += "\nВ руках: " + player.Weapon.Name;

        return text;
    }

    // Список товаров торговца (команда shop).
    public static string ShopText(Shop shop)
    {
        string text = "Товары торговца (покупать можно, стоя на клетке $):";

        for (int i = 0; i < shop.Count; i++)
        {
            Item item = shop.Get(i);
            text += "\n  " + (i + 1) + ". " + item.Name + " — " + item.Price + " зол.";
        }

        return text;
    }
}
