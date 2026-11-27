// Предмет, который лежит на полу подземелья на клетке (X, Y).

public class FloorItem
{
    public Item Item { get; }
    public int X { get; }
    public int Y { get; }
    public bool IsTaken { get; private set; }   // true — предмет уже подобрали, на полу его больше нет

    public FloorItem(Item item, int x, int y)
    {
        Item = item;
        X = x;
        Y = y;
    }

    // Отмечает, что предмет подобрали.
    public void Take()
    {
        IsTaken = true;
    }
}
