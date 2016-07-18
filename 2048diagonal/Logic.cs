using System;
using System.Collections.Generic;

public class Logic {
    public int Width { get; private set; }
    public int Height { get; private set; }

    private Block[,] map;

    public List<Block> BlockList;

    public Block[,] Map { get { return map; } }

    public void Init(int width, int height)
    {
        Width = width;
        Height = height;

        map = new Block[Width, Height];
        BlockList = new List<Block>();

        for (int i = 0; i < Width; ++i) {
            for (int j = 0; j < Height; ++j) {
                var block = new Block();
                block.X = i;
                block.Y = j;

                map[i, j] = block;
                BlockList.Add(block);
            }
        }
    }

    public void Add(int x, int y)
    {
        map[x, y] = new Block();
    }

    public void Move(int x, int y)
    {
        x = Math.Sign(x);
        y = Math.Sign(y);

        var mergeLineList = DoMerge(x, y);
        AlignAfterMerge(x, y, mergeLineList);
    }

    private Dictionary<Vec2, List<MergeBlock>> DoMerge(int x, int y)
    {
        Dictionary<Vec2, List<MergeBlock>> mergeLineList = new Dictionary<Vec2, List<MergeBlock>>();

        int deltaX = -x;
        int deltaY = -y;

        int startX = 0;
        int startY = 0;
        int endX = 0;
        int endY = 0;

        if (x == 1) {
            startX = Width - 1;
            endX = -1;
        }
        else if (x == -1) {
            startX = 0;
            endX = Width;
        }

        if (y == 1) {
            startY = Height - 1;
            endY = -1;
        }
        else if (y == -1) {
            startY = 0;
            endY = Height;
        }

        Action<int, int, bool, bool> findMergeList = (lineX, lineY, checkX, checkY) => {
            MergeBlock lastMerge = null;

            int i = lineX;
            int j = lineY;

            var mergeList = new List<MergeBlock>();
            mergeLineList.Add(new Vec2(i, j), mergeList);

            while ((checkX && (i != endX)) || (checkY && (j != endY))) {
                if (IsInRange(i, j) == false) {
                    break;
                }

                var block = map[i, j];

                if (block != null) {
                    if (lastMerge == null) {
                        lastMerge = new MergeBlock();
                        lastMerge.Add(block);
                    }
                    else if (lastMerge.IsFull() == false) {
                        lastMerge.Add(block);
                        mergeList.Add(lastMerge);

                        lastMerge = null;
                    }
                }

                if (i != endX) {
                    i += deltaX;
                }

                if (j != endY) {
                    j += deltaY;
                }
            }

            if (lastMerge != null) {
                mergeList.Add(lastMerge);
            }
        };

        // Horizontal
        if (x != 0 && y == 0) {
            for (int i = 0; i < Height; ++i) {
                findMergeList(startX, i, true, false);
            }
        }
        // Vertical
        else if (x == 0 && y != 0) {
            for (int i = 0; i < Width; ++i) {
                findMergeList(i, startY, false, true);
            }
        }
        // Diagonal
        else {
            int localDeltaX = -deltaX;

            int localStartX = endX - deltaX;
            int localEndX = startX - deltaX;

            int i = localStartX;
            int j = startY;

            while (true) {
                findMergeList(
                    i == localEndX ? localEndX - localDeltaX : i,
                    j == endY ? endY - deltaX : j,
                    true, true);

                if (i != localEndX - localDeltaX) {
                    i += localDeltaX;
                }
                else if (j != endY) {
                    j += deltaY;
                }
                else {
                    break;
                }
            }
        }

        foreach (var pair in mergeLineList) {
            foreach (var merge in pair.Value) {
                merge.Merge();
            }
        }

        return mergeLineList;
    }

    private void AlignAfterMerge(int x, int y, Dictionary<Vec2, List<MergeBlock>> mergeLineList)
    {
        int deltaX = -x;
        int deltaY = -y;

        int startX = 0;
        int startY = 0;

        if (x == 1) {
            startX = Width - 1;
        }

        if (y == 1) {
            startY = Height - 1;
        }

        // Clear
        for (int i = 0; i < Width; ++i) {
            for (int j = 0; j < Height; ++j) {
                map[i, j] = null;
            }
        }

        BlockList.Clear();

        // Reset map
        foreach (var pair in mergeLineList) {
            var endPoint = pair.Key;
            var mergeList = pair.Value;

            int newX = endPoint.X;
            int newY = endPoint.Y;

            foreach (var merge in mergeList) {
                int mergeX = newX;
                int mergeY = newY;

                if (x == 0) {
                    mergeX = merge.First.X;
                }
                if (y == 0) {
                    mergeY = merge.First.Y;
                }

                Reposition(merge.First, mergeX, mergeY);
                BlockList.Add(merge.First);

                newX += deltaX;
                newY += deltaY;
            }
        }
    }

    public void Reposition(Block block, int newX, int newY)
    {
        block.X = newX;
        block.Y = newY;
        map[block.X, block.Y] = block;
    }

    public bool IsInRange(int x, int y)
    {
        return 0 <= x && x < Width && 0 <= y && y < Height;
    }
}
