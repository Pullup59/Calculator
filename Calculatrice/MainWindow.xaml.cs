using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Calculatrice
{
    /// <summary>
    /// Interaction logic for basic compute using compute c# method
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool valid, noComa = true;
        public int parCount;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((Window)sender).Width = 0.75 * ((Window)sender).ActualHeight;
        }


        //-----------------------Event-----------------------------------------
        private void Btn_Num_Click(object sender, RoutedEventArgs e)
        {
            lblAfficheur.Content += (string)((Button)sender).Content;

            if (parCount < 1)
            {
                Res_Activation();
            }
            else 
            {
                btnParFermante.IsEnabled = true;
            }

            Btn_Activation();
            ParOuvrante_Desactivation();
        }

        private void Btn_Op_Click(object sender, RoutedEventArgs e)
        {
            lblAfficheur.Content += (string)((Button)sender).Content;

            if (parCount != 0) btnParFermante.IsEnabled = false;
            noComa = true;
            valid = true;

            Btn_Desactivation();
            Coma_Desactivation();
            ParOuvrante_Activation();
            Res_Desactivation();
        }

        private void Btn_Coma_Click(object sender, RoutedEventArgs e)
        {
            lblAfficheur.Content += (string)((Button)sender).Content;

            if (parCount != 0) btnParFermante.IsEnabled = false;
            noComa = false;

            Btn_Desactivation();
            Coma_Desactivation();
            ParOuvrante_Desactivation();
            Res_Desactivation();
        }

        private void Btn_ParOuvrante_Click(object sender, RoutedEventArgs e)
        {
            lblAfficheur.Content += (string)((Button)sender).Content;

            parCount++;

            Moins_activation();
            Res_Desactivation();
        }

        private void Btn_ParFermante_Click(object sender, RoutedEventArgs e)
        {
            lblAfficheur.Content += (string)((Button)sender).Content;

            if (--parCount == 0)
            {
                btnParFermante.IsEnabled = false;
                Res_Activation();
            }

            Btn_Activation();
        }

        //--------------------Button hidder----------------------------

        private void Btn_Activation()
        {
            if (noComa) btnVirgule.IsEnabled = true;

            btnPlus.IsEnabled = true;
            btnMoins.IsEnabled = true;
            btnDivise.IsEnabled = true;
            btnMultiplie.IsEnabled = true;
            btnPow.IsEnabled = true;
        }

        private void Btn_Desactivation()
        {
            btnPlus.IsEnabled = false;
            btnMoins.IsEnabled = false;
            btnDivise.IsEnabled = false;
            btnMultiplie.IsEnabled = false;
            btnPow.IsEnabled = false;
        }

        private void Coma_Desactivation()
        {
            btnVirgule.IsEnabled = false;
        }

        private void ParOuvrante_Activation()
        {
            btnParOuvrante.IsEnabled = true;
        }

        private void ParOuvrante_Desactivation()
        {
            btnParOuvrante.IsEnabled = false;
        }

        private void Moins_activation()
        {
            btnMoins.IsEnabled = true;
        }

        private void Res_Activation()
        {
            if (valid) btnEgal.IsEnabled = true;
        }

        private void Res_Desactivation()
        {
            btnEgal.IsEnabled = false;
        }

        //--------------------Special button----------------------------

        private void ButtonEQUALS_Click(object sender, RoutedEventArgs e)
        {
            valid = false;

            var res = Convert.ToString(lblAfficheur.Content);
            res = res?.Replace(",", ".");
            var content = res?.Substring(res.Length - 2, 2);

            lblAfficheur.Content = content == "/0" ? "0" : new DataTable().Compute(res, string.Empty); // if content != 0
            Res_Desactivation();
        }

        private void ButtonCE_Click(object sender, RoutedEventArgs e)
        {
            lblAfficheur.Content = "";
            valid = false;

            Btn_Desactivation();
            Coma_Desactivation();
            ParOuvrante_Activation();
            Res_Desactivation();
            Moins_activation();
        }

        private void ButtonScientific_Click(object sender, RoutedEventArgs e) 
        {
            var scientificWindow = new ScientificWindow();
            Visibility = Visibility.Hidden;
            scientificWindow.ShowDialog();

            Visibility = Visibility.Visible;
        }
    }
}