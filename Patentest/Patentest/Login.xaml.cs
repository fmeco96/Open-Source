using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Patentest
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window    
    {
        SqlConnection miConexionSql;
        bool usuario = false;
        bool pass = false;
        bool validacion = false;        

        public Login()
        {
            InitializeComponent();

            string miConexion = ConfigurationManager.ConnectionStrings["Patentest.Properties.Settings.DatosPatentestConnectionString"].ConnectionString;

            miConexionSql = new SqlConnection(miConexion);

            txtBoxUsuario.Focus();
        }

        private void Ingreso()
        {
            List<Administrador> listaAdmin = new List<Administrador>();
            listaAdmin.Clear();

            try
            {
                string selectConsulta = $"SELECT * FROM administrador WHERE usuario='{txtBoxUsuario.Text}' and pass='{passBox.Password}'";

                SqlCommand commandSelect = new SqlCommand(selectConsulta, miConexionSql);

                miConexionSql.Open();

                SqlDataReader reader = commandSelect.ExecuteReader();

                while (reader.Read())
                {
                    listaAdmin.Add(new Administrador
                    {
                        Usuario = Convert.ToString(reader["usuario"]),
                        Pass = Convert.ToString(reader["pass"]),
                        Estado = Convert.ToString(reader["estado"]),
                    });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                miConexionSql.Close();
            }
           
            if (listaAdmin.Count != 0)
            {
                if (txtBoxUsuario.Text == listaAdmin[0].Usuario && passBox.Password == listaAdmin[0].Pass)
                {
                    if (listaAdmin[0].Estado == listaAdmin[0].Estado.ToString())
                    {
                        validacion = true;
                    }
                    else
                    {
                        validacion = false;
                    }
                }
                else
                {
                    validacion = false;
                }
            }
        }

        private void Validacion()
        {
            if (validacion)
            {
                MainWindow main = new MainWindow();
                App.Current.MainWindow = main;
                main.Show();
                this.Close();
            }
            else if (txtBoxUsuario.Text == "" || passBox.Password == "")
            {
                MessageBox.Show("Por favor complete los campos.", "Advertencia",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                if (txtBoxUsuario.Text == "")
                {
                    txtBoxUsuario.Focus();
                }
                else
                {
                    passBox.Focus();
                }
            }
            else
            {
                MessageBox.Show("Usuario o clave incorrecta", "Advertencia",
                    MessageBoxButton.OK, MessageBoxImage.Hand);
                txtBoxUsuario.Focus();
                txtBoxUsuario.SelectAll();
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            Ingreso();
            Validacion();           
        }
     
       
    }
}
