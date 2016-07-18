public class Block {
    public int X, Y;

    public int Value { get; private set; } = 1;

    public void Add(Block other)
    {
        Value += other.Value;
    }
}