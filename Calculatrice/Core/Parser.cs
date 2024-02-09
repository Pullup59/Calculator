using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Calculatrice.Core
{
    public class Parser
    {
        protected string[]? tokens;
        protected int pos;
        protected Stack<int> memory;
        protected Stack<int> stateStack;
        protected Stack<ASTree> forest;

        public Parser()
        {
            memory = new Stack<int>();
            stateStack = new Stack<int>();
            forest = new Stack<ASTree>();
        }

        public ASTree? Parse(string input)
        {
            return ParseInput(input) ? forest.Pop() : null;
        }

        // 30 + 2 * 5
        // ou
        // (3 + 2) * 5

        protected bool HasToken()
        {
            return tokens != null && pos < tokens.Length;
        }

        protected string Token()
        {
            return HasToken() ? tokens[pos] : string.Empty;
        }

        protected void NextToken()
        {
            if (HasToken()) pos++;
        }

        public bool ParseInput(string input)
        {
            string regexp = @"\s*(\+|\*|-|/|%|\(|\)|\d+(\.\d+)?)\s*";
            //string regexp = @"\s*(\+|\*|-|/|%|\(|\))\s*";
            tokens = Regex.Split(input.Trim(), regexp)
                            .Where(c => c != String.Empty).ToArray();
            pos = 0;

            Memorize();

            return Arith_Expr();
        }


        // a * x * x + b * x + c
        // <ARITH_EXPR>      := <TERM> <END_ARITH_EXPR>
        protected bool Arith_Expr()
        {
            return Term() ? End_Arith_Expr() : false;
        }

        // + b * x + c
        // <END_ARITH_EXPR>  := '+' <ARITH_EXPR>
        //                   := '-' <ARITH_EXPR>
        //                   :=
        protected bool End_Arith_Expr()//bool hasTerm)
        {
            //string token = Token();

            //if (token == "+" || token == "-")
            //{
            //    NextToken();
            //    Memorize(); // Memorize before trying to parse another term

            //    if (Term()) // Parse the next term
            //    {
            //        ASTree rightTree = forest.Pop(); // Get the right operand from the forest
            //        ASTree leftTree = forest.Pop(); // Get the left operand from the forest
            //        ASTree newTree = new ASTree(ASType.BINARYOP, token, new List<ASTree> { leftTree, rightTree }); // Create a new ASTree for the operation
            //        forest.Push(newTree); // Push the new ASTree onto the forest
            //        return true;
            //    }

            //    Recall(); // Restore the state if parsing the next term fails
            //    return false;
            //}

            //return true; // No operator, end of arithmetic expression

            while (Token() == "+" || Token() == "-")
            {
                string operatorToken = Token();
                NextToken();
                Memorize(); // Memorize before trying to parse another term

                if (Term()) // Parse the next term
                {
                    ASTree rightTree = forest.Pop();
                    ASTree leftTree = forest.Pop();
                    ASTree newTree = new ASTree(ASType.BINARYOP, operatorToken, new List<ASTree> { leftTree, rightTree });
                    forest.Push(newTree);
                }
                else
                {
                    Recall();
                    return false; // Unable to parse the next term
                }
            }
            return true;
        }

        // b * x
        // <TERM>            := <UNARY_MINUS> <END_TERM>
        protected bool Term()
        {
            return Unary_Minus() ? End_Term() : false;
        }

        // <END_TERM>        := '*' <TERM>
        //                   := '/' <TERM>
        //                   := '%' <TERM>
        //                   :=
        protected bool End_Term()//bool hasMinus)
        {
            //if (!hasMinus) return false;

            string token = Token();

            if (token == "*" || token == "/" || token == "%")
            {
                NextToken();
                Memorize();  // Memorize before trying to parse another term

                if (Term()) // Parse the next term
                {
                    ASTree rightTree = forest.Pop();
                    ASTree leftTree = forest.Pop();
                    ASTree newTree = new ASTree(ASType.BINARYOP, token, new List<ASTree> { leftTree, rightTree });
                    forest.Push(newTree); // Push the new ASTree onto the forest
                    return End_Term(); // Recursively check for further operators
                }

                Recall(); // Restore the state if parsing the next term fails
                return false;
            }
            return true;
        }

        // -4 ou 4
        // <UNARY_MINUS>     := '-' <UNARY_MINUS>
        //                   := <FACTOR>
        protected bool Unary_Minus()
        {
            if (Token() == "-")
            {
                NextToken();
                return Unary_Minus();
            }

            return Factor();
        }

        // b ou x
        // <FACTOR>          := <NUMERIC>
        //                   := '(' <ARITH_EXPR> ')'
        protected bool Factor()
        {
            if (Token() == "(")
            {
                NextToken();
                //bool retour = Arith_Expr();
                Memorize();  // Memorize before trying to parse arithmetic expression within parentheses

                if (Arith_Expr() && Token() == ")")
                {
                    NextToken();
                    return true;
                }
                Recall();
                return false;
            }

            return Numeric();
        }

        // 1 ou 12 ou 1.2 ou 0.1
        // <NUMERIC>        := <INT_PART> <END_NUMERIC>
        // <END_NUMERIC>    := '.' <INT_PART>
        //                  :=
        // <INT_PART>       := <DIGIT> <END_INT_PART>
        // <END_INT_PART>   := <INT_PART>
        //                  :=
        // <DIGIT>          := [0-9]
        protected bool Numeric()
        {
            if (Regex.IsMatch(Token(), @"^[0-9]+(\.[0-9]*)?|\.[0-9]+$"))
            {
                forest.Push(new ASTree(ASType.NUMERIC, Token()));
                NextToken();
                return true;
            }

            return false;
        }

        protected void Memorize()
        {
            memory.Push(pos);
            stateStack.Push(forest.Count);
        }

        protected void Recall()
        {
            pos = memory.Pop();
            int state = stateStack.Pop();
            while (forest.Count > state)
            {
                forest.Pop();
            }
        }
    }
}