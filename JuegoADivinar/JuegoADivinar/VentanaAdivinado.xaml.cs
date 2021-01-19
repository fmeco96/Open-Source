using System.Windows;

namespace JuegoADivinar
{
    /// <summary>
    /// Lógica de interacción para VentanaRegistro.xaml
    /// </summary>
    public partial class VentanaRegistro : Window
    {
        public VentanaRegistro()
        {
            InitializeComponent();
        }

        private void ButtonVolverJugar_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        private void ButtonRegistrarPunt_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}