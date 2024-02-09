using Calculatrice.Core;
using System;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;

namespace Calculatrice
{
    /// <summary>
    /// Interaction logic for ScientificWindow.xaml
    /// </summary>
    public partial class ScientificWindow : Window
    {
        public MainWindow Mw { get; set; }

        private string? exprBefore;
        private bool clearDisplayer = false;

        public ScientificWindow(MainWindow w)
        {
            Mw = w;
            InitializeComponent();
        }

        //-----------------------Event-----------------------------------------
        private void Btn_Elem_Click(object sender, RoutedEventArgs e)
        {
            if (clearDisplayer)
            {
                lblAfficheur.Content = "";
                clearDisplayer = false;
            }
            lblAfficheur.Content += (string)((Button)sender).Content;
        }

        //--------------------Special button----------------------------
        private void ButtonEQUALS_Click(object sender, RoutedEventArgs e)
        {
            var expr = Convert.ToString(lblAfficheur.Content);

            if (expr != "")
            {
                expr = expr?.Replace(",", ".");
                Parser parser = new();
                Calculator calculator = new(parser);

                var result = Calculator.Evaluate(expr);

                if (result.Error != null)
                {
                    exprBefore = "";
                    lblExpr.Content = "";
                    lblResultat.Content = result.Error;
                }
                else
                {
                    lblExpr.Content = expr;
                    lblResultat.Content = $" = {Convert.ToString(result.Result)}";
                    exprBefore = expr;
                    clearDisplayer = true;
                }
                
            }
        }

        private void ButtonCE_Click(object? sender, RoutedEventArgs? e)
        {
            lblAfficheur.Content = "";
        }


        private void ButtonPolonaise_Click(object? sender, RoutedEventArgs? e)
        {
            if (exprBefore != "" && exprBefore != null)
            {
                Parser parser = new();
                ReversePolishNotation rpn = new(parser);

                var result = ReversePolishNotation.LocalParser(exprBefore);
                lblExpr.Content = result;
            }
        }

        //--------------------Change Window----------------------------
        private void ButtonScientific_Click(object sender, RoutedEventArgs e)
        {
            var mw = new MainWindow();
            Visibility = Visibility.Hidden;
            mw.Show();
        }
    }
}
