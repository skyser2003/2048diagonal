public class Block {
    public int X, Y;

    public int Value { get; private set; }

    public void Add(Block other)
    {
        Value += other.Value;
    }
}