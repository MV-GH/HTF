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

        protected bool Equals(Node<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Val, other.Val);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Val, Children);
        }

        public override string ToString()
        {
            return $"({Val})\n {string.Join(" | ", Children)} !!!!";
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
            int i = 0;
            while (parent != null)
            {
                i++;
                parent = parent.Parent;
            }

            return i;
        }
       
    }
}