using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Transactions;
using testProject.ResponseModels;

namespace testProject.Nodes
{
    public static class Director
    {
        public static IList<Tile> GetPossibleTiles(Tile tile, IList<Tile> grid)
        {
            var board = ConvertToArray(grid);
            List<Tile> test = new List<Tile>();
            Vector2 direction = tile.direction switch
            {
                Direction.Down => new Vector2(0, 1),
                Direction.Up => new Vector2(0, -1),
                Direction.Left => new Vector2(-1, 0),
                Direction.Right => new Vector2(1, 0),
                Direction.DownLeft => new Vector2(-1, 1),
                Direction.DownRight => new Vector2(1, 1),
                Direction.UpLeft => new Vector2(-1, -1),
                Direction.UpRight => new Vector2(1, -1),
                Direction.End => new Vector2(0, 0),
                _ => throw new ArgumentOutOfRangeException(tile.direction.ToString())
            };
            var size = board.GetLength(0);
            for (int i = 1; i < size; i++)
            {
                int x = (tile.x - 1) + i * (int)direction.X;
                int y = (tile.y - 1) + i * (int)direction.Y;

                if (ExceedBoard(x, y, size)) break;
                test.Add(board[x, y]);
            }

            return test;
        }

        private static bool ExceedBoard(int x, int y, int boardSize)
        {
            return x < 0 || y < 0 || x >= boardSize || y >= boardSize;
        }

        private static Tile[,] ConvertToArray(IList<Tile> grid)
        {
            var size = grid.Last().x;
            var arr = new Tile[size, size];
            foreach (var tile in grid)
            {
                arr[tile.x - 1, tile.y - 1] = tile;
            }

            return arr;
        }

        public static Node<Tile> AddChildren(Node<Tile> root, IList<Tile> grid)
        {
            foreach (var node in GetPossibleTiles(root.Val, grid).Select(t => new Node<Tile>(t, root)))
            {
                if (!root.Contains(node) && !root.Equals(node))
                {
                    root.Children.Add(node);
                    if (node.GetLength() == grid.Count) return node;
                    var result = AddChildren(node, grid);
                    if (result != null) return result;
                }
            }

            return null;
        }

        public static IEnumerable<int> GetListOfParent(Node<Tile> lastNode)
        {
            for (var node = lastNode; node != null; node = node.Parent)
            {
                yield return node.Val.id;
            }
        }
    }
}