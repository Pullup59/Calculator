using System;
using System.Collections.Generic;

namespace Calculatrice.Core
{
    public class Calculator
    {
        private static Parser parser;

        public Calculator(Parser parser)
        {
            Calculator.parser = parser;
        }

        public static (double? Result, string? Error) Evaluate(string expression)
        {
            ASTree? tree = parser.Parse(expression);
            return tree == null ? (null, "Expression invalide") : EvaluateNode(tree);
        }

        public static (double? Result, string? Error) EvaluateNode(ASTree node)
        {
            switch (node.Type)
            {
                case ASType.NUMERIC:
                    return (double.Parse(node.Root), null);
                case ASType.BINARYOP:
                    double left = (double)EvaluateNode(node.Children[0]).Result;
                    if (node.Children.Count == 1 && node.Root == "-") // Opérateur unaire -
                        return (-left, null);
                    double right = (double)EvaluateNode(node.Children[1]).Result;
                    switch (node.Root)
                    {
                        case "+": return (left + right, null);
                        case "-": return (left - right, null);
                        case "*": return (left * right, null);
                        case "%": return (left % right, null);
                        case "/":
                            if (right == 0) return (null, "Infini");
                            return (left / right, null);
                        default:
                            return (null, $"Erreur binaryop evaluate node: {node.Root}");
                    }
                case ASType.UNARYOP:
                    double operand = (double)EvaluateNode(node.Children[0]).Result;
                    if (node.Root == "-") return (-operand, null);
                    return (null, $"Erreur unaryop evaluate node: {node.Root}");
                default:
                    return (null, "Charactère non traité");
            }
        }
    }
}