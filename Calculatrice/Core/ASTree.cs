using System;
using System.Collections.Generic;

namespace Calculatrice.Core
{
    public enum ASType { NUMERIC, BINARYOP, UNARYOP };

    public class ASTree : IComparable
    {
        public string Root { get; set; }
        public ASType Type { get; set; }
        public List<ASTree>? Children { get; set; }

        public ASTree(ASType type, string root = "", List<ASTree>? children = null)
        {
            Type = type;
            Root = root;
            Children = children;
        }

        public void AddChild(ASTree son)
        {
            Children ??= new List<ASTree>();
            Children.Add(son);
        }

        public bool HasAtLeastChildren(int n = 0)
        {
            return n == 0 || Children != null && Children.Count >= n;
        }

        public int CompareTo(object? b)
        {
            ASTree tree = (ASTree)b;

            // Checker le null ?
            //ArgumentNullException.ThrowIfNull(Root);
            //ArgumentNullException.ThrowIfNull(tree?.Root);

            if (String.Compare(Root, tree.Root) < 0)
            {
                return -1;
            }
            else
            {
                if (String.Compare(Root, tree.Root) > 0)
                {
                    return 1;
                }
            }

            // No son for the two tree
            if (!HasAtLeastChildren(1) && !tree.HasAtLeastChildren(1)) return 0;

            // Each has at least 1 child
            if (HasAtLeastChildren(1) && tree.HasAtLeastChildren(1))
            {
                int left_children_compare = Children[0].CompareTo(tree.Children[0]);

                if (left_children_compare != 0) return left_children_compare;

                if (HasAtLeastChildren(2) && tree.HasAtLeastChildren(2))
                    return Children[1].CompareTo(tree.Children[1]);

                return HasAtLeastChildren(2) ? 1 : -1;
            }

            return HasAtLeastChildren(1) ? 1 : -1;
        }
    }
}
