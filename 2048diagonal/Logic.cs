using System;
using System.Collections.Generic;

public class Logic {
    public int Width { get; private set; }
    public int Height { get; private set; }

    private Block[,] map;

    public List<Block> BlockList;

    public void Init(int width, int height)
    {
        Width = width;
        Height = height;

        map = new Block[Width, Height];
        BlockList = new List<Block>();
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

    private Dictionary<int, List<MergeBlock>> DoMerge(int x, int y)
    {
        Dictionary<int, List<MergeBlock>> mergeLineList = new Dictionary<int, List<MergeBlock>>();

        int deltaX = -x;
        int deltaY = -y;

        int startX = 0;
        int startY = 0;
        int endX = 0;
        int endY = 0;

        if (x == 1) {
            startX = Width - 1;
            endX = 0;
        }
        else if (x == -1) {
            startX = 0;
            endX = Width - 1;
        }

        if (y == 1) {
            startY = Height - 1;
            endY = 0;
        }
        else if (y == -1) {
            startY = 0;
            endY = Height - 1;
        }

        Action<int, int, bool, bool> findMergeList = (lineX, lineY, checkX, checkY) => {
            MergeBlock lastMerge = null;

            int i = lineX;
            int j = lineY;

            while ((checkX && (i != endX)) || (checkY && (j != endY))) {
                var block = map[i, j];

                if (i != endX) {
                    i += deltaX;
                }

                if (j != endY) {
                    j += deltaY;
                }

                if (block == null) {
                    continue;
                }

                if (mergeLineList.ContainsKey(j) == false) {
                    mergeLineList.Add(j, new List<MergeBlock>());
                }

                if (lastMerge == null) {
                    lastMerge = new MergeBlock();
                    lastMerge.Add(block);
                }
                else if (lastMerge.IsFull() == false) {
                    lastMerge.Add(block);
                    mergeLineList[j].Add(lastMerge);

                    lastMerge = null;
                }
            }
        };

        if (x != 0 && y == 0) {
            for (int i = 0; i < Height; ++i) {
                findMergeList(startX, i, false, true);
            }
        }
        else if (x == 0 && y != 0) {
            for (int i = 0; i < Width; ++i) {
                findMergeList(i, startY, true, false);
            }
        }
        else {

        }

        foreach (var pair in mergeLineList) {
            var mergeList = pair.Value;

            foreach (var merge in mergeList) {
                merge.Merge();
            }
        }

        return mergeLineList;
    }

    private void AlignAfterMerge(int x, int y, Dictionary<int, List<MergeBlock>> mergeLineList)
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
            var mergeList = pair.Value;

            int newX = startX;
            int newY = startY;

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
}
