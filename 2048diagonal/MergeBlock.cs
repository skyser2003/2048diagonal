public class MergeBlock {
    private Block[] list = new Block[2];

    public Block First { get { return list[0]; } }
    public Block Second { get { return list[1]; } }

    public void Add(Block block)
    {
        if (list[0] == null) {
            list[0] = block;
        }
        else if (list[1] == null) {
            list[1] = block;
        }
    }

    public bool IsFull()
    {
        return list[0] != null && list[1] != null;
    }

    public void Merge()
    {
        if (Second != null) {
            First.Add(Second);
        }
    }
}