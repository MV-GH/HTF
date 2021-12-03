using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace testProject.Nodes
{
    public class Node<T>
    {
        public Node<T>? Parent;
        public T Val { get; set; }
        public IList<Node<T>> Children { get; set; }

        public Node(T val)
        {
            Val = val;
            Children = new List<Node<T>>();
        }

        public Node(T val, Node<T> node)
        {
            Parent = node;
            Val = val;
            Children = new List<Node<T>>();
        }

        public bool Equals(Node<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Val, other.Val);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Val, Children);
        }

        public override string ToString()
        {
            return $"({Val})";
        }

        public static void PrintTree(Node<T> tree, String indent = "", bool last = true)
        {
            Console.WriteLine(indent + "+- " + tree);
            indent += last ? "   " : "|  ";

            for (int i = 0; i < tree.Children.Count; i++)
            {
                PrintTree(tree.Children[i], indent, i == tree.Children.Count - 1);
            }
        }

        public bool Contains(Node<T> node)
        {
            var parent = Parent;
            while (parent != null)
            {
                if (parent.Equals(node)) return true;
                parent = parent.Parent;
            }

            return false;
        }

        public int GetLength()
        {
            var parent = Parent;
            int i = 1;
            while (parent != null)
            {
                i++;
                parent = parent.Parent;
            }

            return i;
        }

        public Node<T> GetRoot()
        {
            var parent = Parent ?? this;
            while (parent.Parent != null)
            {
                parent = parent.Parent;
            }
            return parent;
        }
    }
}