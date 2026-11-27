// ПРОВЕРКИ. Этот файл НЕ МЕНЯТЬ.
// В самих проверках ошибок нет: ошибки — в остальных файлах проекта.
// Читать этот файл можно и нужно: по тексту проверки видно, что именно делала программа.

public static class Checks
{
    static int _ok;
    static int _failed;

    public static void RunAll()
    {
        Section("1. Игрок");
        Run("Игрок: урон", PlayerDamage);
        Run("Игрок: лечение", PlayerHeal);
        Run("Игрок: золото", PlayerGold);

        Section("2. Монстр");
        Run("Монстр: урон", MonsterDamage);

        Section("3. Сумка");
        Run("Сумка: заполнение", BagFill);
        Run("Сумка: удаление предмета", BagRemove);

        Section("4. Бой");
        Run("Бой: формула урона", CombatDamage);
        Run("Бой: игрок бьёт гоблина", CombatPlayerAttacks);
        Run("Бой: гоблин бьёт игрока", CombatMonsterAttacks);

        Section("5. Карта");
        Run("Карта: клетки", MapTiles);

        Section("6. Движение");
        Run("Движение: четыре стороны", MoveDirections);
        Run("Движение: стены", MoveWalls);
        Run("Движение: счётчик ходов", TurnCounter);

        Section("7. Предметы");
        Run("Предметы: подобрать с пола", PickUp);
        Run("Предметы: зелье", UsePotion);
        Run("Предметы: меч", UseWeapon);

        Section("8. Торговец");
        Run("Торговец: покупка", ShopBuy);
        Run("Торговец: отказ", ShopRefusals);
        Run("Торговец: полная сумка", ShopFullBag);

        Section("9. Ход монстров");
        Run("Ход монстров: кто бьёт игрока", MonstersNear);
        Run("Ход монстров: погибший монстр", MonstersDead);
        Run("Ход монстров: бой с крысой", MonstersKill);

        Section("10. Уровень");
        Run("Уровень: начало игры", LevelStart);

        Section("11. Прохождение");
        Run("Прохождение", Walkthrough);

        Console.WriteLine();
        Console.WriteLine("Итог: OK — " + _ok + ", ОШИБКА — " + _failed + ".");
    }

    // ---------- 1. Игрок ----------

    static void PlayerDamage()
    {
        Player player = new Player(1, 1);

        player.TakeDamage(5);
        Check("Игрок (20 HP) получил 5 урона, здоровье", player.Health, 15);
        Check("Игрок с 15 HP жив", YesNo(player.IsAlive), "да");

        player.TakeDamage(50);
        Check("Игрок (15 HP) получил 50 урона, здоровье", player.Health, 0);
        Check("Игрок после 50 урона жив", YesNo(player.IsAlive), "нет");
    }

    static void PlayerHeal()
    {
        Player player = new Player(1, 1);
        player.TakeDamage(12);

        player.Heal(5);
        Check("Игрок (8 HP) вылечил 5, здоровье", player.Health, 13);

        player.Heal(100);
        Check("Игрок (13 HP) вылечил 100, здоровье", player.Health, 20);
    }

    static void PlayerGold()
    {
        Player player = new Player(1, 1);
        player.AddGold(10);

        Check("Игрок (10 зол.) платит 4, оплата прошла", YesNo(player.SpendGold(4)), "да");
        Check("После оплаты 4 из 10 осталось золота", player.Gold, 6);
        Check("Игрок (6 зол.) платит 7, оплата прошла", YesNo(player.SpendGold(7)), "нет");
        Check("После отказа в оплате осталось золота", player.Gold, 6);
        Check("Игрок (6 зол.) платит 6, оплата прошла", YesNo(player.SpendGold(6)), "да");
    }

    // ---------- 2. Монстр ----------

    static void MonsterDamage()
    {
        Monster rat = Catalog.Rat(0, 0);

        rat.TakeDamage(4);
        Check("Крыса (6 HP) получила 4 урона, здоровье", rat.Health, 2);
        Check("Крыса с 2 HP жива", YesNo(rat.IsAlive), "да");

        rat.TakeDamage(2);
        Check("Крыса (2 HP) получила 2 урона, здоровье", rat.Health, 0);
        Check("Крыса с 0 HP жива", YesNo(rat.IsAlive), "нет");

        rat.TakeDamage(3);
        Check("Крыса (0 HP) получила ещё 3 урона, здоровье", rat.Health, 0);
    }

    // ---------- 3. Сумка ----------

    static void BagFill()
    {
        Inventory bag = new Inventory();
        for (int i = 0; i < 5; i++)
            bag.Add(Catalog.SmallPotion());

        Check("В сумку положили 5 зелий, предметов в сумке", bag.Count, 5);
        Check("Сумка с 5 предметами полная", YesNo(bag.IsFull), "да");

        bool added = bag.Add(Catalog.Key());
        Check("Шестой предмет поместился в сумку", YesNo(added), "нет");
        Check("После попытки положить шестой предмет, предметов в сумке", bag.Count, 5);
    }

    static void BagRemove()
    {
        Inventory bag = new Inventory();
        bag.Add(Catalog.SmallPotion());
        bag.Add(Catalog.Key());
        bag.Add(Catalog.Sword());

        bag.RemoveAt(0);
        Check("Из сумки [зелье, ключ, меч] убрали первый предмет, предметов в сумке", bag.Count, 2);
        Check("Теперь первым в сумке лежит", bag.Get(0).Name, "Ключ");
        Check("Теперь вторым в сумке лежит", bag.Get(1).Name, "Меч");
        Check("В сумке [ключ, меч] есть ключ", YesNo(bag.Has(Item.Key)), "да");
        Check("В сумке [ключ, меч] есть зелье", YesNo(bag.Has(Item.Potion)), "нет");
    }

    // ---------- 4. Бой ----------

    static void CombatDamage()
    {
        Check("Урон при атаке 5 и броне 2", Combat.Damage(5, 2), 3);
        Check("Урон при атаке 4 и броне 0", Combat.Damage(4, 0), 4);
        Check("Урон при атаке 3 и броне 5 (самый слабый удар)", Combat.Damage(3, 5), 1);
    }

    static void CombatPlayerAttacks()
    {
        Player player = new Player(1, 1);
        Monster goblin = Catalog.Goblin(2, 1);

        Combat.PlayerAttacks(player, goblin);
        Check("Игрок (атака 3) ударил гоблина (9 HP), здоровье гоблина", goblin.Health, 6);
        Check("Гоблин ещё жив, золото игрока", player.Gold, 0);

        Combat.PlayerAttacks(player, goblin);
        Combat.PlayerAttacks(player, goblin);
        Check("Гоблин после трёх ударов жив", YesNo(goblin.IsAlive), "нет");
        Check("Гоблин (награда 8) убит, золото игрока", player.Gold, 8);
    }

    static void CombatMonsterAttacks()
    {
        Player player = new Player(1, 1);
        Monster goblin = Catalog.Goblin(2, 1);

        Combat.MonsterAttacks(goblin, player);
        Check("Гоблин (атака 3) ударил игрока (20 HP), здоровье игрока", player.Health, 17);
    }

    // ---------- 5. Карта ----------

    static void MapTiles()
    {
        GameMap map = new GameMap(Levels.MapRows);

        Check("Ширина карты", map.Width, 10);
        Check("Высота карты", map.Height, 10);
        Check("Клетка (3, 1) — стена", YesNo(map.IsWall(3, 1)), "да");
        Check("Клетка (1, 3) — стена", YesNo(map.IsWall(1, 3)), "нет");
        Check("Клетка (6, 8) — торговец", YesNo(map.IsShop(6, 8)), "да");
        Check("Клетка (8, 6) — торговец", YesNo(map.IsShop(8, 6)), "нет");
        Check("Клетка (8, 8) — выход", YesNo(map.IsExit(8, 8)), "да");
        Check("Клетка (-1, 5) за краем карты — стена", YesNo(map.IsWall(-1, 5)), "да");
        Check("Клетка (4, 10) за краем карты — стена", YesNo(map.IsWall(4, 10)), "да");
    }

    // ---------- 6. Движение ----------

    static void MoveDirections()
    {
        Game game = new Game();

        game.Execute("s");
        Check("Игрок на (1, 1), команда s (вниз), теперь игрок на клетке", Where(game), "(1, 2)");
        game.Execute("d");
        Check("Затем команда d (вправо), теперь игрок на клетке", Where(game), "(2, 2)");
        game.Execute("w");
        Check("Затем команда w (вверх), теперь игрок на клетке", Where(game), "(2, 1)");
        game.Execute("a");
        Check("Затем команда a (влево), теперь игрок на клетке", Where(game), "(1, 1)");
    }

    static void MoveWalls()
    {
        Game game = new Game();

        game.Execute("a");
        Check("Игрок на (1, 1), слева стена, команда a, теперь игрок на клетке", Where(game), "(1, 1)");

        game.Execute("d");
        game.Execute("d");
        Check("Две команды d, но клетка (3, 1) — стена, теперь игрок на клетке", Where(game), "(2, 1)");
    }

    static void TurnCounter()
    {
        Game game = new Game();

        game.Execute("d");
        game.Execute("a");
        game.Execute("a");
        Check("После команд d, a, a номер хода", game.Turn, 3);

        game.Execute("help");
        game.Execute("i");
        game.Execute("abracadabra");
        Check("Команды help, i и неизвестная команда ход не тратят, номер хода", game.Turn, 3);
    }

    // ---------- 7. Предметы ----------

    static void PickUp()
    {
        Game game = new Game();
        game.Player.MoveTo(1, 2);

        game.Execute("d");
        Check("Игрок встал на клетку (2, 2) с зельем, предметов в сумке", game.Player.Bag.Count, 1);
        Check("Зелье всё ещё лежит на полу на клетке (2, 2)", YesNo(game.FindLootAt(2, 2) != null), "нет");
    }

    static void UsePotion()
    {
        Game game = new Game();
        Player player = game.Player;
        player.Bag.Add(Catalog.SmallPotion());
        player.Bag.Add(Catalog.Key());
        player.TakeDamage(10);

        game.Execute("use 1");
        Check("В сумке [зелье, ключ], у игрока 10 HP, команда use 1, здоровье", player.Health, 18);
        Check("После команды use 1 предметов в сумке", player.Bag.Count, 1);

        game.Execute("use 1");
        Check("В сумке [ключ], команда use 1 (ключ использовать нельзя), предметов в сумке", player.Bag.Count, 1);

        game.Execute("use 7");
        Check("Команда use 7 (такого предмета нет), здоровье", player.Health, 18);
    }

    static void UseWeapon()
    {
        Game game = new Game();
        Player player = game.Player;
        player.Bag.Add(Catalog.SmallPotion());
        player.Bag.Add(Catalog.Sword());

        Check("Игрок без оружия, атака", player.Attack, 3);

        game.Execute("use 2");
        Check("В сумке [зелье, меч], команда use 2, атака", player.Attack, 6);
        Check("После команды use 2 предметов в сумке", player.Bag.Count, 1);
    }

    // ---------- 8. Торговец ----------

    static void ShopBuy()
    {
        Game game = new Game();
        Player player = game.Player;
        player.MoveTo(6, 8);
        player.AddGold(12);

        game.Execute("buy 2");
        Check("Игрок у торговца, 12 зол., команда buy 2 (меч за 12), золото", player.Gold, 0);
        Check("После покупки меча предметов в сумке", player.Bag.Count, 1);
    }

    static void ShopRefusals()
    {
        Game game = new Game();
        Player player = game.Player;
        player.AddGold(12);

        game.Execute("buy 2");
        Check("Игрок на (1, 1), торговца рядом нет, команда buy 2, золото", player.Gold, 12);

        player.MoveTo(6, 8);
        game.Execute("buy 9");
        Check("Игрок у торговца, команда buy 9 (такого товара нет), золото", player.Gold, 12);

        player.SpendGold(8);
        game.Execute("buy 1");
        Check("Игрок у торговца, 4 зол., команда buy 1 (зелье за 5), золото", player.Gold, 4);
        Check("После отказа в покупке предметов в сумке", player.Bag.Count, 0);
    }

    static void ShopFullBag()
    {
        Game game = new Game();
        Player player = game.Player;
        player.AddGold(10);
        for (int i = 0; i < 5; i++)
            player.Bag.Add(Catalog.SmallPotion());

        game.Shop.Buy(player, 1);
        Check("Сумка полная, 10 зол., игрок покупает товар 1 (зелье за 5), золото", player.Gold, 10);
        Check("После покупки с полной сумкой предметов в сумке", player.Bag.Count, 5);
    }

    // ---------- 9. Ход монстров ----------
    // Команда "use 9" здесь — способ пропустить ход: предмета номер 9 нет, но ход тратится.

    static void MonstersNear()
    {
        Game game = new Game();
        Player player = game.Player;

        player.MoveTo(1, 5);
        game.Execute("use 9");
        Check("Игрок на (1, 5), монстров рядом нет, прошёл ход, здоровье", player.Health, 20);

        player.MoveTo(2, 2);
        game.Execute("use 9");
        Check("Игрок на (2, 2), крыса на (3, 3) по диагонали, прошёл ход, здоровье", player.Health, 20);

        player.MoveTo(2, 3);
        game.Execute("use 9");
        Check("Игрок на (2, 3) вплотную к крысе (атака 2), прошёл ход, здоровье", player.Health, 18);
    }

    static void MonstersDead()
    {
        Game game = new Game();
        Player player = game.Player;
        player.MoveTo(2, 3);

        game.Monsters[0].TakeDamage(100);
        game.Execute("use 9");
        Check("Крыса получила 100 урона и погибла, игрок стоит рядом, прошёл ход, здоровье игрока", player.Health, 20);
    }

    static void MonstersKill()
    {
        Game game = new Game();
        Player player = game.Player;
        player.MoveTo(2, 3);

        game.Execute("d");
        Check("Игрок ударил крысу (6 HP), крыса ударила в ответ, здоровье игрока", player.Health, 18);

        game.Execute("d");
        Check("Второй удар убил крысу, здоровье игрока", player.Health, 18);
        Check("Крыса (награда 4) убита, золото игрока", player.Gold, 4);

        game.Execute("use 9");
        Check("Прошёл ход рядом с убитой крысой, здоровье игрока", player.Health, 18);

        game.Execute("d");
        Check("Команда d, игрок идёт на клетку убитой крысы, теперь игрок на клетке", Where(game), "(3, 3)");
    }

    // ---------- 10. Уровень ----------

    static void LevelStart()
    {
        Game game = new Game();
        string[] lines = Renderer.MapLines(game);

        Check("Начало игры, строка состояния", Renderer.Status(game), "HP 20/20 | Атака 3 | Золото 0 | Ход 0");
        Check("Начало игры, строка карты y = 1", lines[1], "#@.#..g.k#");
        Check("Начало игры, строка карты y = 2", lines[2], "#.!#.##.##");
        Check("Начало игры, строка карты y = 3", lines[3], "#..r..#..#");
        Check("Начало игры, строка карты y = 7", lines[7], "#...#!.#s#");
        Check("Начало игры, строка карты y = 8", lines[8], "#.#...$#>#");

        bool lootInWall = false;
        for (int i = 0; i < game.Loot.Length; i++)
        {
            if (game.Map.IsWall(game.Loot[i].X, game.Loot[i].Y))
                lootInWall = true;
        }
        Check("Какой-то предмет лежит внутри стены", YesNo(lootInWall), "нет");
    }

    // ---------- 11. Прохождение ----------
    // Вся игра от начала до победы. Пройдёт, только когда исправлены все ошибки.

    static void Walkthrough()
    {
        string[] route =
        {
            "d", "s",                       // зелье на (2, 2)
            "a", "s", "d",                  // к крысе
            "d", "d",                       // бой с крысой
            "d", "d", "w", "w", "d",        // к гоблину
            "d", "d", "d",                  // бой с гоблином
            "use 1",                        // выпить зелье
            "d", "d", "d",                  // ключ на (8, 1)
            "a", "s", "s", "s", "s",        // вниз по правому коридору
            "a", "s", "s", "a",             // зелье на (5, 7)
            "d", "s",                       // к торговцу на (6, 8)
            "buy 2", "use 3", "use 2",      // купить меч, взять в руки, выпить зелье
            "w", "w", "w", "d", "d", "s",   // к скелету
            "s", "s", "s",                  // бой со скелетом
            "s", "s",                       // выход на (8, 8)
        };

        Game game = new Game();
        for (int i = 0; i < route.Length; i++)
            game.Execute(route[i]);

        Check("Прохождение: игрок на клетке", Where(game), "(8, 8)");
        Check("Прохождение: победа", YesNo(game.IsWon), "да");
        Check("Прохождение: число ходов", game.Turn, 44);
        Check("Прохождение: здоровье в конце", game.Player.Health, 8);
        Check("Прохождение: золото в конце", game.Player.Gold, 10);
    }

    // ---------- служебное ----------

    static void Section(string title)
    {
        Console.WriteLine();
        Console.WriteLine("=== " + title + " ===");
    }

    // Запускает группу проверок. Если программа в ней падает с исключением, это тоже ОШИБКА.
    static void Run(string name, Action checks)
    {
        try
        {
            checks();
        }
        catch (Exception e)
        {
            _failed++;
            Console.WriteLine(name + ": программа упала (" + e.GetType().Name + ": " + e.Message + ") - ОШИБКА");
        }
    }

    static void Check(string what, int result, int expected)
    {
        Report(what, result.ToString(), expected.ToString(), result == expected);
    }

    static void Check(string what, string result, string expected)
    {
        Report(what, result, expected, result == expected);
    }

    static void Report(string what, string result, string expected, bool ok)
    {
        if (ok) _ok++; else _failed++;
        string status = ok ? "OK" : "ОШИБКА";
        Console.WriteLine($"{what}: получено {result}, ожидалось {expected} - {status}");
    }

    static string YesNo(bool value)
    {
        return value ? "да" : "нет";
    }

    static string Where(Game game)
    {
        return "(" + game.Player.X + ", " + game.Player.Y + ")";
    }
}
