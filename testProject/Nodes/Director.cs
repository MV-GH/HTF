using System.Collections.Generic;
using System.Linq;
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
            switch (tile.direction)
            {
                case Direction.Down:
                    for (int i = tile.x; i < board.Length; i++)
                    {
                        test.Add(board[i, tile.y]);
                    }
                    break;
                case Direction.Up:
                    for (int i = tile.x - 1; i > 0; i--)
                    {
                        test.Add(board[i, tile.y]);
                    }
                    break;
                case Direction.Left:
                    for (int i = tile.y - 1; i > 0; i--)
                    {
                        test.Add(board[tile.x, i]);
                    }
                    break;
                case Direction.Right:
                    for (int i = tile.y; i < board.Length; i++)
                    {
                        test.Add(board[tile.x, i]);
                    }
                    break;
                case Direction.DownRight:
                    for (int i = 0; i < board.Length; i++)
                    {
                        var x = tile.x + i;
                        var y = tile.y + i;
                        if (x >= board.Length || y >= board.Length) break;
                        test.Add(board[x, y]);
                    }
                    break;
                case Direction.UpLeft:
                    for (int i = 1; i < board.Length; i++)
                    {
                        var x = tile.x - i;
                        var y = tile.y - i;
                        if (x < 0 || y < 0) break;
                        test.Add(board[x, y]);
                    }
                    break;
                case Direction.DownLeft:
                    for (int i = 1; i < board.Length; i++)
                    {
                        var x = tile.x - i;
                        var y = tile.y + i;
                        if (ExceedBoard(x, y, board)) break;
                        test.Add(board[x, y]);
                    }
                    break;
                case Direction.UpRight:
                    for (int i = 1; i < board.Length; i++)
                    {
                        var x = tile.x + i;
                        var y = tile.y - i;
                        if (ExceedBoard(x, y, board)) break;
                        test.Add(board[x, y]);
                    }
                    break;
                case Direction.End:
                    break;
            }

            return test;
        }

        private static bool ExceedBoard(int x, int y, Tile[,] board)
        {
            return !(x >= 0 && y >= 0 && x < board.Length && y < board.Length);
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
        public static void AddChildren(Node<Tile> root, IList<Tile> grid)
        {
            foreach (var node in Director.GetPossibleTiles(root.Val, grid).Select(t => new Node<Tile>(t, root)))
            {
                if (!root.Contains(node))
                {
                    root.Children.Add(node);
                    AddChildren(node, grid);
                }

            }
        }
    }
}