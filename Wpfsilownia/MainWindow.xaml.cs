using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassLibrarySilownia;
using ClassLibrarySilownia.Model;

namespace Wpfsilownia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Silownia silownia = new Silownia();
        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            DietyDataGrid.ItemsSource = silownia.diety;
            TreningiDataGrid.ItemsSource = silownia.treningi;
        }

        private void DodajDieteButton_Click(object sender, RoutedEventArgs e)
        {
            silownia.DodajDiete(DietaTextBox.Text);
        }

        private void DodajTreningButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(IloscPowtorzen.Text, out int result))
            {
                silownia.DodajTrening(TreningTextBox.Text, result);
            }
            else
            {
                // Jeśli konwersja na int nie powiodła się
                MessageBox.Show("Wprowadź poprawną liczbę całkowitą.");
            }
            
        }

        private void ObliczBMIButton_Click(Object sender, RoutedEventArgs e)
        {
            try
            {
                if (double.TryParse(WagaTextBox.Text, out double waga) && double.TryParse(WzrostTextBox.Text, out double wzrost))
                {
                    double bmiResult = silownia.ObliczBMI(waga, wzrost);
                    WynikBMITextBox.Text = $"BMI: {bmiResult:F2}";
                }
            }
            catch (InvalidOperationException ex) {  MessageBox.Show(ex.Message); }
        }

        private void TreningiDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void WagaTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TablicaBMIButton_Click(Object sender, RoutedEventArgs e)
        {
            InterpretacjaTextBox.Text = "Interpretacja wyników BMI:" +
                Environment.NewLine +
                "Poniżej 16: Wygłodzenie" +
                Environment.NewLine +
                "16 - 16.9: Wychudzenie" +
                Environment.NewLine +
                "17 - 18.4: Niedowaga" +
                Environment.NewLine +
                "18.5 - 24.9: Waga prawidłowa" +
                Environment.NewLine +
                "25 - 29.9: Nadwaga" +
                Environment.NewLine +
                "30 - 34.9: I stopień otyłości" +
                Environment.NewLine +
                "35 - 39.9: II stopień otyłości (otyłość kliniczna)" +
                Environment.NewLine +
                "Powyżej 40: III stopień otyłości (otyłość skrajna)";
        }
    }
}