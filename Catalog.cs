// Справочник: какие в игре бывают предметы и монстры и какие у них числа.

public static class Catalog
{
    // ----- Предметы: название, вид, сила, цена -----

    public static Item SmallPotion()
    {
        return new Item("Малое зелье", Item.Potion, 8, 5);
    }

    public static Item Sword()
    {
        return new Item("Меч", Item.Weapon, 3, 12);
    }

    public static Item Key()
    {
        return new Item("Ключ", Item.Key, 0, 0);
    }

    // ----- Монстры: название, символ на карте, здоровье, атака, броня, награда, клетка (x, y) -----

    public static Monster Rat(int x, int y)
    {
        return new Monster("Крыса", 'r', 6, 2, 0, 4, x, y);
    }

    public static Monster Goblin(int x, int y)
    {
        return new Monster("Гоблин", 'g', 9, 3, 0, 8, x, y);
    }

    public static Monster Skeleton(int x, int y)
    {
        return new Monster("Скелет", 's', 12, 4, 2, 10, x, y);
    }
}
