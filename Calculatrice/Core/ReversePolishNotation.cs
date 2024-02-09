using Calculatrice.Core;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculatrice.Core
{
    public class ReversePolishNotation
    {
        private static Parser parser;

        public ReversePolishNotation(Parser parser)
        {
            ReversePolishNotation.parser = parser;
        }
        public static string LocalParser(string expression)
        {
            ASTree? tree = parser.Parse(expression);
            return ToReversePolishNotation(tree);
        }

        public static string ToReversePolishNotation(ASTree tree)
        {
            string retour = string.Empty;

            switch (tree.Type)
            {
                case ASType.NUMERIC:
                    retour += tree.Root + " ";
                    break;
                case ASType.UNARYOP:
                    retour += ToReversePolishNotation(tree.Children[0]) + tree.Root + " ";
                    break;
                case ASType.BINARYOP:
                    retour += ToReversePolishNotation(tree.Children[0]) +
                              ToReversePolishNotation(tree.Children[1]) + tree.Root + " ";
                    break;
            }

            return retour;
        }

        //public string ToPrefixNotation()
        //{
        //    string retour = string.Empty;

        //    switch (Type)
        //    {
        //        case ASType.NUMERIC:
        //            retour += Root;
        //            break;
        //        case ASType.UNARYOP:
        //            retour += Root + Children[0].ToPrefixNotation();
        //            break;
        //        case ASType.BINARYOP:
        //            retour += "(" + Root + " " + Children[0].ToPrefixNotation() + " " +
        //              Children[1].ToPrefixNotation() + ")";
        //            break;
        //    }

        //    return retour;
        //}

        //public string ToString()
        //{
        //    string retour = string.Empty;

        //    switch (Type)
        //    {
        //        case ASType.NUMERIC:
        //            retour += Root;
        //            break;
        //        case ASType.UNARYOP:
        //            retour += Root + Children[0].ToPrefixNotation();
        //            break;
        //        case ASType.BINARYOP:
        //            // YK : TO FIX : Parenthesis for priority change
        //            retour += Children[0].ToString() + " " + Root + " " +
        //                Children[1].ToString();
        //            break;
        //    }

        //    return retour;
        //}
    }
}
