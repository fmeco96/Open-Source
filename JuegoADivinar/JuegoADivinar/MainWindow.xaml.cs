using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Configuration;
using System.Data.SqlClient;

namespace JuegoADivinar
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Se debe crear una bbdd en su propio ordenador y crear la conexión.
            string miConexion = ConfigurationManager.ConnectionStrings["ventanaJuego.Properties.Settings.listaPuntajesConnectionString"].ConnectionString;

            miConexionSql = new SqlConnection(miConexion);
        }

        SqlConnection miConexionSql;

        //------FUNCIONES--------------------------------------
        int oport = 0;
        int eleccDific = 0;
        string numIngr = "0";
        int numRandom = 0;
        int puntos = 0;

        private void eleccionDificultad()
        {
            switch (eleccDific)
            {
                case 1:
                    eleccDific = 25;
                    oport = 5;
                    puntos = 25;
                    break;
                case 2:
                    eleccDific = 50;
                    oport = 4;
                    puntos = 50;
                    break;
                case 3:
                    eleccDific = 100;
                    oport = 3;
                    puntos = 75;
                    break;
            }
        }

        //-------MENU------------------------------------------
        private void mostrarMenu()
        {
            stackpanelMenu.Visibility = Visibility.Visible;
        }

        private void ocultarMenu()
        {
            stackpanelMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonJugar_Click(object sender, RoutedEventArgs e)
        {
            ocultarMenu();
            mostrarDificultad();
        }

        //-------DIFICULTAD-----------------------------------------
        private void mostrarDificultad()
        {
            dificultad.Visibility = Visibility.Visible;
            resetDificultad();
        }

        private void ocultarDificultad()
        {
            dificultad.Visibility = Visibility.Collapsed;
        }

        private void resetDificultad()
        {
            textBoxIntento.Text = "";
            labelDificultadFacil.Visibility = Visibility.Hidden;
            labelDificultadMedio.Visibility = Visibility.Hidden;
            labelDificultadDificil.Visibility = Visibility.Hidden;
            buttonEmpezar.IsEnabled = false;
            radioButtonFacil.IsChecked = false;
            radioButtonMedio.IsChecked = false;
            radioButtonDificil.IsChecked = false;
        }

        private void ButtonEmpezar_Click(object sender, RoutedEventArgs e)
        {
            ocultarDificultad();
            comenzarJuego();
            eleccionDificultad();
            numRandom = new Random().Next(0, eleccDific);
        }

        private void RadioButtonFacil_Checked(object sender, RoutedEventArgs e)
        {
            eleccDific = 1;
            buttonEmpezar.IsEnabled = true;
            labelDificultadFacil.Visibility = Visibility.Visible;
            labelDificultadMedio.Visibility = Visibility.Hidden;
            labelDificultadDificil.Visibility = Visibility.Hidden;
        }

        private void RadioButtonMedio_Checked(object sender, RoutedEventArgs e)
        {
            eleccDific = 2;
            buttonEmpezar.IsEnabled = true;
            labelDificultadFacil.Visibility = Visibility.Hidden;
            labelDificultadMedio.Visibility = Visibility.Visible;
            labelDificultadDificil.Visibility = Visibility.Hidden;
        }

        private void RadioButtonDificil_Checked(object sender, RoutedEventArgs e)
        {
            eleccDific = 3;
            buttonEmpezar.IsEnabled = true;
            labelDificultadFacil.Visibility = Visibility.Hidden;
            labelDificultadMedio.Visibility = Visibility.Hidden;
            labelDificultadDificil.Visibility = Visibility.Visible;
        }

        private void ButtonVolver_Click(object sender, RoutedEventArgs e)
        {
            ocultarDificultad();
            mostrarMenu();
        }

        //-----COMIENZO JUEGO-------------------------------------------
        private void comenzarJuego()
        {
            comienzoJuego.Visibility = Visibility.Visible;
        }

        private void ocultarComenzarJuego()
        {
            comienzoJuego.Visibility = Visibility.Collapsed;
        }

        private void ingresarIntento()
        {
            numIngr = textBoxIntento.Text;

            try
            {
                if (int.Parse(numIngr) > numRandom)
                {
                    oport -= 1;
                    puntos -= 5;
                    if (oport == 0)
                    {
                        MessageBox.Show($"¡Se te han acabado las oportunidades! :(\nEl número secreto era: {numRandom}", "¡Has perdido!",
                            MessageBoxButton.OK, MessageBoxImage.Asterisk);

                        ocultarComenzarJuego();
                        mostrarMenu();
                    }
                    else
                    {
                        if (oport == 1)
                        {
                            labelNumMayor.Content = $"El número {numIngr} es mayor\nTe queda {oport} oportunidad";
                        }
                        else
                        {
                            labelNumMayor.Content = $"El número {numIngr} es mayor\nTe quedan {oport} oportunidades";
                        }
                        labelNumMayor.Visibility = Visibility.Visible;
                        labelNumMenor.Visibility = Visibility.Hidden;
                    }
                }
                else if (int.Parse(numIngr) < numRandom)
                {
                    oport -= 1;
                    puntos -= 5;
                    if (oport == 0)
                    {
                        MessageBox.Show($"¡Se te han acabado las oportunidades! :(\nEl número secreto era: {numRandom}", "¡Has perdido!",
                            MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        textBoxIntento.Text = "";
                        labelNumMayor.Content = "";
                        labelNumMenor.Content = "";
                        ocultarComenzarJuego();
                        mostrarMenu();
                    }
                    else
                    {
                        if (oport == 1)
                        {
                            labelNumMenor.Content = $"El número {numIngr} es menor\nTe queda {oport} oportunidad";
                        }
                        else
                        {
                            labelNumMenor.Content = $"El número {numIngr} es menor\nTe quedan {oport} oportunidades";
                        }
                        labelNumMenor.Visibility = Visibility.Visible;
                        labelNumMayor.Visibility = Visibility.Hidden;
                    }
                }
                else
                {
                    juegoFinalizado();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Debes ingresar un número", "¡Atención!",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ButtonIngresar_Click(object sender, RoutedEventArgs e)
        {
            ingresarIntento();
        }

        private void TextBoxIntento_GotFocus(object sender, RoutedEventArgs e)
        {
            labelNumMayor.Visibility = Visibility.Hidden;
            labelNumMenor.Visibility = Visibility.Hidden;
        }

        //------VENTANA JUEGO FINALIZADO----------------------------------------------------

        private void juegoFinalizado()
        {
            VentanaRegistro ventanaAdivinado = new VentanaRegistro();
            ventanaAdivinado.labelNumSecret.Content = $"El número secreto era: {numRandom}";
            ventanaAdivinado.labelPuntos.Content = $"Has obtenido: {puntos} puntos";

            ventanaAdivinado.ShowDialog();

            if (ventanaAdivinado.buttonVolverJugar.IsDefault == false)
            {
                ocultarComenzarJuego();
                mostrarDificultad();
            }
            if (ventanaAdivinado.buttonRegistrarPunt.IsDefault == false)
            {
                string consulta = "INSERT INTO PUNTAJE (nombre,puntuacion) VALUES (@nombre,@puntos)";

                SqlCommand miSqlCommand = new SqlCommand(consulta, miConexionSql);

                miConexionSql.Open();

                miSqlCommand.Parameters.AddWithValue("@nombre", ventanaAdivinado.textBoxRegistro.Text);
                miSqlCommand.Parameters.AddWithValue("@puntos", puntos);

                miSqlCommand.ExecuteNonQuery();

                miConexionSql.Close();

                ventanaAdivinado.textBoxRegistro.Text = "";

                ocultarComenzarJuego();
                ocultarDificultad();
                mostrarPuntos();
            }
        }

        //------REGLAS----------------------------------------------------

        private void mostrarReglas()
        {
            ocultarMenu();
            reglas.Visibility = Visibility.Visible;
        }
        private void ocultarReglas()
        {
            reglas.Visibility = Visibility.Collapsed;
        }
        private void BotonReglas_Click(object sender, RoutedEventArgs e)
        {
            mostrarReglas();
        }

        private void ButtonAceptarReglas_Click(object sender, RoutedEventArgs e)
        {
            ocultarReglas();
            mostrarMenu();
        }

        private void BotonSalir_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Desea Salir?", "Salir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void BotonPuntajes_Click(object sender, RoutedEventArgs e)
        {
            mostrarPuntos();
            ocultarMenu();
        }
        //----MOSTRAR PUNTAJES---------------------------------------------------------------
        private void mostrarPuntos()
        {
            Puntajes.Visibility = Visibility.Visible;

            listPuntajes.Items.Clear();

            List<string> nombresLista = new List<string>();
            List<int> puntosLista = new List<int>();
            try
            {
                string consulta = "SELECT nombre,puntuacion FROM PUNTAJE ORDER BY puntuacion DESC";
                string consulta2 = "DELETE FROM PUNTAJE WHERE ID=@IDBORRAR SqlCommand miSqlCommand2 = new SqlCommand(consulta2, miConexionSql)";

                SqlCommand commandSelect = new SqlCommand(consulta, miConexionSql);

                miConexionSql.Open();

                SqlDataReader reader = commandSelect.ExecuteReader();

                while (reader.Read())
                {
                    nombresLista.Add(Convert.ToString(reader["nombre"]));
                    puntosLista.Add(Convert.ToInt32(reader["puntuacion"]));
                }

                miConexionSql.Close();

                for (var i = 1; i <= nombresLista.Count(); i++)
                {
                    listPuntajes.Items.Add(new
                    {
                        Puesto = 0 + i,
                        Nombre = nombresLista[i - 1],
                        Puntos = puntosLista[i - 1]
                    });
                    if (i == 5)
                    {
                        break;
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        private void ocultarMostrarPuntos()
        {
            Puntajes.Visibility = Visibility.Collapsed;
        }

        private void ButtonVolverMenu_Click(object sender, RoutedEventArgs e)
        {
            ocultarMostrarPuntos();
            mostrarMenu();
        }
    }
}


