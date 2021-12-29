using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Patentest
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection miConexionSql;
        string sex = "";
        string estado = "";
        bool comprobCampos;
        bool numRef;
        string patente;

        public MainWindow()
        {
            InitializeComponent();

            string miConexion = ConfigurationManager.ConnectionStrings["Patentest.Properties.Settings.DatosPatentestConnectionString"].ConnectionString;

            miConexionSql = new SqlConnection(miConexion);

            ComboBoxBuscar();
            txtBusqueda.Focus();
        }


        //----CRUD-----------------------------------------------------------------------------------

        private void IngresarDatos() //Inserta los datos en la BD
        {
            int num = 0;
            try
            {
                miConexionSql.Open();

                string insertPropConsulta = "INSERT INTO PROPIETARIO (nombre, apellido, dni, fecha, sexo, pais, provincia, domicilio, telefono, numReferencia)" +
                "VALUES (@nombre, @apellido, @dni, @fecha, @sexo, @pais, @provincia, @domicilio, @telefono, @numRef);" +
                                            "INSERT INTO AUTOMOTOR (marca, modelo, año, patente, estado, id_numReferencia)" +
                "VALUES (@marca, @modelo, @año, @patente, @estado, @id_numReferencia)";

                string insertNumRef = "SELECT TOP 1 numReferencia FROM PROPIETARIO ORDER BY numReferencia DESC";
                SqlCommand commandSelectNumRef = new SqlCommand(insertNumRef, miConexionSql);

                SqlDataReader reader = commandSelectNumRef.ExecuteReader();

                reader.Read();

                //Variableque toma el numero de referencia de la tabla propietario e incrementar su valor en 1
                //para generar un id de numeros consecutivos
                try
                {
                    num = Convert.ToInt32(reader["numReferencia"]);
                    num += 1;
                }
                catch
                {
                    num = 0;
                }

                reader.Close();

                SqlCommand miSqlCommand = new SqlCommand(insertPropConsulta, miConexionSql);

                miSqlCommand.Parameters.AddWithValue("@nombre", tBoxNombre.Text);
                miSqlCommand.Parameters.AddWithValue("@apellido", tBoxApellido.Text);
                miSqlCommand.Parameters.AddWithValue("@dni", tBoxDni.Text);
                miSqlCommand.Parameters.AddWithValue("@fecha", datePickerFecha.SelectedDate);
                miSqlCommand.Parameters.AddWithValue("@sexo", sex);
                miSqlCommand.Parameters.AddWithValue("@pais", tBoxPais.Text);
                miSqlCommand.Parameters.AddWithValue("@provincia", tBoxProvincia.Text);
                miSqlCommand.Parameters.AddWithValue("@domicilio", tBoxDomicilio.Text);
                miSqlCommand.Parameters.AddWithValue("@telefono", tBoxTel.Text);
                miSqlCommand.Parameters.AddWithValue("@numRef", num.ToString());

                miSqlCommand.Parameters.AddWithValue("@marca", tBoxMarca.Text);
                miSqlCommand.Parameters.AddWithValue("@modelo", tBoxModelo.Text);
                miSqlCommand.Parameters.AddWithValue("@año", ComboBoxAnio.Text);
                miSqlCommand.Parameters.AddWithValue("@patente", tBoxPatente.Text);
                miSqlCommand.Parameters.AddWithValue("@estado", estado);
                miSqlCommand.Parameters.AddWithValue("@id_numReferencia", num.ToString());

                miSqlCommand.ExecuteNonQuery();

                MessageBox.Show("Los datos han sido cargado con éxito");
                VaciarCampos();
            }

            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                CamposNull();
            }
            finally
            {
                miConexionSql.Close();
            }


            RestablecerCampos();
        }

        private void IngresarDatosRef() //Ingresa registros a la BD de automotor con el numero de referencia
        {
            ComprobarCampos();

            if (comprobCampos == false && numRef == false)
            {
                try
                {
                    miConexionSql.Open();

                    string insertPropConsulta = "INSERT INTO AUTOMOTOR (marca, modelo, año, patente, estado, id_numReferencia)" +
                    "VALUES (@marca, @modelo, @año, @patente, @estado, @id_numReferencia)";

                    SqlCommand miSqlCommand = new SqlCommand(insertPropConsulta, miConexionSql);
                    int num = Int16.Parse(tBoxRef.Text);
                    miSqlCommand.Parameters.AddWithValue("@marca", tBoxMarca.Text);
                    miSqlCommand.Parameters.AddWithValue("@modelo", tBoxModelo.Text);
                    miSqlCommand.Parameters.AddWithValue("@año", ComboBoxAnio.Text);
                    miSqlCommand.Parameters.AddWithValue("@patente", tBoxPatente.Text.ToUpper());
                    miSqlCommand.Parameters.AddWithValue("@estado", estado);
                    miSqlCommand.Parameters.AddWithValue("@id_numReferencia", num);

                    miSqlCommand.ExecuteNonQuery();

                    MessageBox.Show("Los datos han sido cargado con éxito");
                    VaciarCampos();
                }

                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    CamposNull();
                }
                finally
                {
                    miConexionSql.Close();
                }

                RestablecerCampos();
            }
        }

        //
        public void BusquedaRapida()
        {
            List<Propietario> listBusqRap = new List<Propietario>();
            txtBusqNom.Text = "";
            txtBusqApell.Text = "";
            txtBusqNumRef.Text = "";

            try
            {
                string selectConsulta = "SELECT nombre, apellido, numReferencia FROM propietario " +
                $"WHERE dni = {txtBusqRap.Text}";

                miConexionSql.Open();

                SqlCommand commandSelect = new SqlCommand(selectConsulta, miConexionSql);

                SqlDataReader reader = commandSelect.ExecuteReader();

                while (reader.Read())
                {
                    txtBusqNom.Text = Convert.ToString(reader["nombre"]);
                    txtBusqApell.Text = Convert.ToString(reader["apellido"]);
                    txtBusqNumRef.Text = Convert.ToString(reader["numReferencia"]);
                }
                if (txtBusqNumRef.Text == "")
                {
                    MessageBox.Show("Usuario no encontrado");
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
        }

        public void ModificarDatos()
        {
            string sexo;
            try
            {
                if (RButtonMascBusq.IsChecked == true)
                {
                    sexo = "M";
                }
                else if (RButtonFemBusq.IsChecked == true)
                {
                    sexo = "F";
                }
                else
                {
                    sexo= "O";
                }
                if (RButtonMascBusq.IsChecked == true || RButtonFemBusq.IsChecked == true || RButtonOtroBusq.IsChecked == true)
                {
                    if (RButtonRobDet.IsChecked == true || RButtonNoRobDet.IsChecked == true)
                    {
                        if (RButtonRobDet.IsChecked == true)
                        {
                            estado = "Robado";
                        }
                        else if (RButtonNoRobDet.IsChecked == true)
                        {
                           estado = "No Robado";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Por favor seleccione el estado del automotor");
                    }
                }

                string modificarConsulta = $"UPDATE p SET" +
                    $" nombre = @nombre, apellido = @apellido," +
                    $" dni = @dni, fecha = @fecha, sexo = @sexo, pais = @pais, provincia = @provincia," +
                    $" domicilio = @domicilio, telefono = @telefono" +
                    $" FROM propietario AS p" +
                    $" INNER JOIN automotor AS a ON p.numReferencia = a.id_numReferencia" +
                    $" WHERE patente = '{patente.ToLower()}';" +
                    $" UPDATE automotor SET" +
                    $" marca = @marca, modelo = @modelo, año = @año, estado = @estado, patente = @patente" +
                    $" WHERE patente = '{patente.ToLower()}'";

                miConexionSql.Open();

                SqlCommand commandModificar = new SqlCommand(modificarConsulta, miConexionSql);

                commandModificar.Parameters.AddWithValue("@nombre", txtBoxNombre.Text);
                commandModificar.Parameters.AddWithValue("@apellido", txtBoxApellido.Text);
                commandModificar.Parameters.AddWithValue("@dni", txtBoxDni.Text);
                commandModificar.Parameters.AddWithValue("@fecha", txtBoxNac.Text);
                commandModificar.Parameters.AddWithValue("@sexo", sexo);
                commandModificar.Parameters.AddWithValue("@pais", txtBoxPais.Text);
                commandModificar.Parameters.AddWithValue("@provincia", txtBoxProv.Text);
                commandModificar.Parameters.AddWithValue("@domicilio", txtBoxDom.Text);
                commandModificar.Parameters.AddWithValue("@telefono", txtBoxTel.Text);

                commandModificar.Parameters.AddWithValue("@patente", txtBoxPatente.Text);
                commandModificar.Parameters.AddWithValue("@marca", txtBoxMarca.Text);
                commandModificar.Parameters.AddWithValue("@modelo", txtBoxModelo.Text);
                commandModificar.Parameters.AddWithValue("@año", ComboBoxAnioDet.Text);
                commandModificar.Parameters.AddWithValue("@estado", estado);

                commandModificar.ExecuteNonQuery();                

                MessageBox.Show("Datos actualizados con exito");                         
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                miConexionSql.Close();
                BusquedaDatos();
            }
        }


        private void EliminarDatos() //Elimina registros de la BD automotor
        {
            try
            {
                string deleteConsulta = $"DELETE FROM automotor WHERE patente = '{patente.ToLower()}'";

                SqlCommand commandDelete = new SqlCommand(deleteConsulta, miConexionSql);

                miConexionSql.Open();

                SqlDataReader reader = commandDelete.ExecuteReader();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                miConexionSql.Close();
            }
        }

        private void BusquedaDatos() //Consulta registros en la BD
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            List<Propietario> listPropiet = new List<Propietario>();
            listaPropietarios.Items.Clear();

            try //Diferentes consultas con base en el tipo de filtro de datos que se aplique
            {
                string selectConsulta;
                if (txtBusqueda.Text != "")
                {
                    if (comboBoxBusqueda.SelectedIndex == 2)
                    {
                        comboBoxBusqueda.Text = "numReferencia";
                    }
                    if (comboBoxFltrOrden.SelectedIndex != 0 && comboBoxFltrOrden2.SelectedIndex == 0 && comboBoxFltrEstado.SelectedIndex == 0)
                    {
                        selectConsulta = "SELECT numReferencia, apellido, nombre, dni, patente, marca, modelo, estado FROM propietario" +
                        $" p JOIN automotor a ON p.numReferencia = a.id_numReferencia WHERE {comboBoxBusqueda.Text} LIKE '{txtBusqueda.Text}%' " +
                        $"ORDER BY {comboBoxFltrOrden.Text} ASC";
                    }
                    else if (comboBoxFltrOrden.SelectedIndex != 0 && comboBoxFltrOrden2.SelectedIndex != 0 && comboBoxFltrEstado.SelectedIndex == 0)
                    {
                        selectConsulta = "SELECT numReferencia, apellido, nombre, dni, patente, marca, modelo, estado FROM propietario" +
                        $" p JOIN automotor a ON p.numReferencia = a.id_numReferencia WHERE {comboBoxBusqueda.Text} LIKE '{txtBusqueda.Text}%' " +
                        $"ORDER BY {comboBoxFltrOrden.Text} ASC, {comboBoxFltrOrden2.Text} ASC";
                    }
                    else if (comboBoxFltrOrden.SelectedIndex != 0 && comboBoxFltrOrden2.SelectedIndex != 0 && comboBoxFltrEstado.SelectedIndex != 0)
                    {
                        selectConsulta = "SELECT numReferencia, apellido, nombre, dni, patente, marca, modelo, estado FROM propietario" +
                        $" p JOIN automotor a ON p.numReferencia = a.id_numReferencia WHERE {comboBoxBusqueda.Text} LIKE '{txtBusqueda.Text}%' " +
                        $"and estado = '{comboBoxFltrEstado.Text}' ORDER BY {comboBoxFltrOrden.Text} ASC,{comboBoxFltrOrden2.Text} ASC";
                    }
                    else if (comboBoxFltrOrden.SelectedIndex != 0 && comboBoxFltrOrden2.SelectedIndex == 0 && comboBoxFltrEstado.SelectedIndex != 0)
                    {
                        selectConsulta = "SELECT numReferencia, apellido, nombre, dni, patente, marca, modelo, estado FROM propietario" +
                        $" p JOIN automotor a ON p.numReferencia = a.id_numReferencia WHERE {comboBoxBusqueda.Text} LIKE '{txtBusqueda.Text}%' " +
                        $"and estado = '{comboBoxFltrEstado.Text}' ORDER BY {comboBoxFltrOrden.Text} ASC";
                    }
                    else if (comboBoxFltrEstado.SelectedIndex != 0)
                    {
                        selectConsulta = "SELECT numReferencia, apellido, nombre, dni, patente, marca, modelo, estado FROM propietario" +
                        $" p JOIN automotor a ON p.numReferencia = a.id_numReferencia WHERE {comboBoxBusqueda.Text} LIKE '{txtBusqueda.Text}%' " +
                        $"and estado = '{comboBoxFltrEstado.Text}'";
                    }
                    else
                    {
                        selectConsulta = "SELECT numReferencia, apellido, nombre, dni, patente, marca, modelo FROM propietario" +
                        $" p JOIN automotor a ON p.numReferencia = a.id_numReferencia WHERE {comboBoxBusqueda.Text} LIKE '{txtBusqueda.Text}%'";
                    }

                    SqlCommand commandSelect = new SqlCommand(selectConsulta, miConexionSql);

                    miConexionSql.Open();

                    SqlDataReader reader = commandSelect.ExecuteReader();

                    while (reader.Read())
                    {
                        listPropiet.Add(new Propietario
                        {
                            NumeroRef = Convert.ToString(reader["numReferencia"]),
                            Apellido = Convert.ToString(reader["apellido"]),
                            Nombre = Convert.ToString(reader["nombre"]),
                            Dni = Convert.ToString(reader["dni"]),
                            Patente = Convert.ToString(reader["patente"]),
                            Marca = Convert.ToString(reader["marca"]),
                            Modelo = Convert.ToString(reader["modelo"])
                        });
                    }

                    for (var i = 0; i < listPropiet.Count; i++)
                    {
                        listaPropietarios.Items.Add(new
                        {
                            labelid = listPropiet[i].Id,
                            labelRef = listPropiet[i].NumeroRef,
                            labelApellido = listPropiet[i].Apellido,
                            labelNombre = listPropiet[i].Nombre,
                            labelDni = listPropiet[i].Dni,
                            labelPatente = listPropiet[i].Patente.ToUpper(),
                            labelMarca = listPropiet[i].Marca,
                            labelModelo = listPropiet[i].Modelo,
                        });
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                miConexionSql.Close();
                if (comboBoxBusqueda.Text == "numReferencia")
                {
                    comboBoxBusqueda.SelectedIndex = 2;
                }
            }
        }


        //----BOTONES CRUD---------------------------------------------------------------------------

        //Ingresa el registro a la BD
        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (tBoxRef.Text == "")
            {
                IngresarDatos();
            }
            else
            {
                IngresarDatosRef();
            }
        }

        //Boton que activa la funcion de ver los detalles
        private void BtnVerDet_Click(object sender, RoutedEventArgs e)
        {
            VerDetalles();
        }

        //Activa la funcion de buscar registros en la BD
        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (txtBusqueda.Text != "")
            {
                BusquedaDatos();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BusquedaRapida();
        }

        //Boton que activa el delete de un registro de la BD
        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (patente != "")
            {
                if (MessageBox.Show($"¿Está seguro que desea eliminar el registro con patente {patente}?", "Advertencia",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    EliminarDatos();
                    MessageBox.Show($"El registro con patente {patente} ha sido eliminado");
                    patente = "";
                    BusquedaDatos();
                }
            }
        }


        //----FUNCIONES APARTE-----------------------------------------------------------------------

        //Comprueba si el textbox de N° Referencia esta vacio o lleno y con base en eso
        //corroborar los demas campos de Datos Personales
        private bool ComprobarCampos()
        {
            numRef = false;
            comprobCampos = false;

            List<string> listComprob = new List<string>();
            listComprob.Clear();

            if (tBoxRef.Text != "")
            {
                listComprob.Add(tBoxNombre.Text);
                listComprob.Add(tBoxApellido.Text);
                listComprob.Add(tBoxDni.Text);
                listComprob.Add(datePickerFecha.Text);
                listComprob.Add(sex);
                listComprob.Add(tBoxPais.Text);
                listComprob.Add(tBoxProvincia.Text);
                listComprob.Add(tBoxDomicilio.Text);
                listComprob.Add(tBoxTel.Text);

                foreach (var a in listComprob)
                {
                    if (a != "")
                    {
                        comprobCampos = true;
                        MessageBox.Show("Vacíe el resto de los campos en Datos Personales primero");
                        Console.WriteLine(a);
                        break;
                    }
                }
            }

            try
            {
                miConexionSql.Open();

                string selectConsulta = $"SELECT numReferencia FROM propietario WHERE numReferencia = {tBoxRef.Text}";

                SqlCommand commandSelect = new SqlCommand(selectConsulta, miConexionSql);

                SqlDataReader reader = commandSelect.ExecuteReader();

                reader.Read();
                string num = (reader["numReferencia"]).ToString();
                reader.Close();

                if (num == "" || num == null)
                {
                    numRef = true;
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

            return comprobCampos;
        }

        //↓ REVISAR - Comprueba que los textbox obligatorios no queden vacios
        private void CamposNull()
        {
            bool campoNull = false;

            if (tBoxDni.Text is null || tBoxDni.Text == "")
            {
                labelDni.Foreground = new SolidColorBrush(Colors.Red);
                campoNull = true;
            }
            if (tBoxPatente.Text is null || tBoxPatente.Text == "")
            {
                labelPatente.Foreground = new SolidColorBrush(Colors.Red);
                campoNull = true;
            }
            if (campoNull is true)
            {
                MessageBox.Show("Por favor ingrese datos en los campos seleccionados");
            }
        }

        //Colorea el color de los campos obligatorios por defecto (negro)
        private void RestablecerCampos()
        {
            labelDni.Foreground = new SolidColorBrush(Colors.Black);
            labelPatente.Foreground = new SolidColorBrush(Colors.Black);
        }

        //Vacia todos los textbox y reestablece los radiobuttons de "Añadir"
        private void VaciarCampos()
        {
            tBoxNombre.Text = "";
            tBoxApellido.Text = "";
            tBoxDni.Text = "";
            datePickerFecha.Text = "";
            sex = "";
            tBoxPais.Text = "";
            tBoxProvincia.Text = "";
            tBoxDomicilio.Text = "";
            tBoxTel.Text = "";
            tBoxRef.Text = "";
            tBoxMarca.Text = "";
            tBoxModelo.Text = "";
            ComboBoxAnio.Text = "Año";
            tBoxPatente.Text = "";
            estado = "";
            RButtonMasc.IsChecked = false;
            RButtonFem.IsChecked = false;
            RButtonOtro.IsChecked = false;
            RButtonRob.IsChecked = false;
            RButtonNoRob.IsChecked = false;

            RestablecerCampos();
        }

        //Vacia todos los textbox de "Buscar"/Ver detalles
        private void VaciarCamposBusqueda()
        {
            txtBoxNombre.Text = "";
            txtBoxApellido.Text = "";
            txtBoxDni.Text = "";
            txtBoxNac.Text = "";
            txtBoxPais.Text = "";
            txtBoxProv.Text = "";
            txtBoxDom.Text = "";
            txtBoxTel.Text = "";
            txtBoxMarca.Text = "";
            txtBoxModelo.Text = "";
            ComboBoxAnioDet.Text = "Año";
            txtBoxPatente.Text = "";

            RButtonMascBusq.IsChecked = false;
            RButtonFemBusq.IsChecked = false;
            RButtonOtroBusq.IsChecked = false;

            ComboBoxAnioDet.IsEnabled = false;

            RButtonRobDet.IsChecked = false;
            RButtonNoRobDet.IsChecked = false;
        }


        //Muestra en detalle los registros que estan y no estan en el listview
        private void VerDetalles()
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            List<Propietario> listDetalles = new List<Propietario>();
            listDetalles.Clear();
            try
            {
                miConexionSql.Open();

                string selectConsulta = "SELECT numReferencia, apellido, nombre, dni, sexo, fecha, pais, provincia, domicilio, telefono, patente, marca, modelo, año, estado " +
                      $"FROM propietario p JOIN automotor a ON p.numReferencia = a.id_numReferencia WHERE patente = '{patente}'";

                SqlCommand commandSelect = new SqlCommand(selectConsulta, miConexionSql);

                SqlDataReader reader = commandSelect.ExecuteReader();

                while (reader.Read())
                {
                    listDetalles.Add(new Propietario
                    {
                        NumeroRef = Convert.ToString(reader["numReferencia"]),
                        Apellido = Convert.ToString(reader["apellido"]),
                        Nombre = Convert.ToString(reader["nombre"]),
                        Dni = Convert.ToString(reader["dni"]),
                        Sexo = Convert.ToString(reader["sexo"]),
                        Fecha = Convert.ToString(reader["fecha"]),
                        Pais = Convert.ToString(reader["pais"]),
                        Provincia = Convert.ToString(reader["provincia"]),
                        Domicilio = Convert.ToString(reader["domicilio"]),
                        Telefono = Convert.ToString(reader["telefono"]),
                        Patente = Convert.ToString(reader["patente"]),
                        Marca = Convert.ToString(reader["marca"]),
                        Modelo = Convert.ToString(reader["modelo"]),
                        Anio = Convert.ToString(reader["año"]),
                        Estado = Convert.ToString(reader["Estado"])
                    });
                }
                reader.Close();

                txtBoxApellido.Text = listDetalles[0].Apellido;
                txtBoxNombre.Text = listDetalles[0].Nombre;
                txtBoxDni.Text = listDetalles[0].Dni;
                txtBoxNac.Text = listDetalles[0].Fecha;

                txtBoxPais.Text = listDetalles[0].Pais;
                txtBoxProv.Text = listDetalles[0].Provincia;
                txtBoxDom.Text = listDetalles[0].Domicilio;
                txtBoxTel.Text = listDetalles[0].Telefono;
                txtBoxMarca.Text = listDetalles[0].Marca;
                txtBoxModelo.Text = listDetalles[0].Modelo;
                txtBoxPatente.Text = listDetalles[0].Patente.ToUpper();
                ComboBoxAnioDet.Text = listDetalles[0].Anio;                

                if (listDetalles[0].Fecha.Equals("01/01/1900 0:00:00"))
                {
                    txtBoxNac.Text = "";
                }

                if (txtBoxTel.Text.Equals("0"))
                {
                    txtBoxTel.Text = "";
                }

                if (listDetalles[0].Sexo.ToLower() == "m")
                {
                    RButtonMascBusq.IsChecked = true;
                }
                else if (listDetalles[0].Sexo.ToLower() == "f")
                {
                    RButtonFemBusq.IsChecked = true;
                }
                else
                {
                    RButtonOtroBusq.IsChecked = true;
                }

                if (listDetalles[0].Estado.ToLower() == "robado")
                {
                    RButtonRobDet.IsChecked = true;
                }
                else
                {
                    RButtonNoRobDet.IsChecked = true;
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
        }

        //Genera los items del ComboBox de busqueda y de los filtros
        private void ComboBoxBuscar()
        {
            if (comboBoxBusqueda.Items.Count == 0)
            {
                comboBoxBusqueda.Items.Insert(0, "Patente");
                comboBoxBusqueda.Items.Insert(1, "DNI");
                comboBoxBusqueda.Items.Insert(2, "N° Referencia");
                comboBoxBusqueda.Items.Insert(3, "Apellido");
                comboBoxBusqueda.Items.Insert(4, "Nombre");
            }
            comboBoxBusqueda.SelectedIndex = 0;

            if (comboBoxFltrOrden.Items.Count == 0)
            {
                comboBoxFltrOrden.Items.Insert(0, "--");
                comboBoxFltrOrden.Items.Insert(1, "Apellido");
                comboBoxFltrOrden.Items.Insert(2, "Nombre");
                comboBoxFltrOrden.Items.Insert(3, "Patente");
                comboBoxFltrOrden.Items.Insert(4, "Modelo");
            }
            comboBoxFltrOrden.SelectedIndex = 0;

            if (comboBoxFltrEstado.Items.Count == 0)
            {
                comboBoxFltrEstado.Items.Add("--");
                comboBoxFltrEstado.Items.Add("Robado");
                comboBoxFltrEstado.Items.Add("No Robado");
            }
            comboBoxFltrEstado.SelectedIndex = 0;
        }

        //Habilita la edicion de los campos
        private void HabilitarEdicion()
        {
            txtBoxNombre.IsReadOnly = false;
            txtBoxApellido.IsReadOnly = false;
            txtBoxDni.IsReadOnly = false;
            txtBoxNac.IsReadOnly = false;
            txtBoxPais.IsReadOnly = false;
            txtBoxProv.IsReadOnly = false;
            txtBoxDom.IsReadOnly = false;
            txtBoxTel.IsReadOnly = false;
            txtBoxMarca.IsReadOnly = false;
            txtBoxModelo.IsReadOnly = false;
            ComboBoxAnioDet.IsEnabled = true;
            txtBoxPatente.IsReadOnly = false;

            txtBoxNombre.BorderThickness = new Thickness(1.0);
            txtBoxApellido.BorderThickness = new Thickness(1.0);
            txtBoxDni.BorderThickness = new Thickness(1.0);
            txtBoxNac.BorderThickness = new Thickness(1.0);
            txtBoxPais.BorderThickness = new Thickness(1.0);
            txtBoxProv.BorderThickness = new Thickness(1.0);
            txtBoxDom.BorderThickness = new Thickness(1.0);
            txtBoxTel.BorderThickness = new Thickness(1.0);
            txtBoxMarca.BorderThickness = new Thickness(1.0);
            txtBoxModelo.BorderThickness = new Thickness(1.0);
            txtBoxPatente.BorderThickness = new Thickness(1.0);

            RButtonMascBusq.IsEnabled = true;
            RButtonFemBusq.IsEnabled = true;
            RButtonOtroBusq.IsEnabled = true;

            RButtonRobDet.IsEnabled = true;
            RButtonNoRobDet.IsEnabled = true;

            checkBoxVerDet.IsChecked = true;

            btnModificar.IsEnabled = true;
        }

        private void DeshabilitarEdicion()
        {
            txtBoxNombre.IsReadOnly = true;
            txtBoxApellido.IsReadOnly = true;
            txtBoxDni.IsReadOnly = true;
            txtBoxNac.IsReadOnly = true;
            txtBoxPais.IsReadOnly = true;
            txtBoxProv.IsReadOnly = true;
            txtBoxDom.IsReadOnly = true;
            txtBoxTel.IsReadOnly = true;
            txtBoxMarca.IsReadOnly = true;
            txtBoxModelo.IsReadOnly = true;
            ComboBoxAnioDet.IsEnabled = true;
            txtBoxPatente.IsReadOnly = true;

            txtBoxNombre.BorderThickness = new Thickness(0.4);
            txtBoxApellido.BorderThickness = new Thickness(0.4);
            txtBoxDni.BorderThickness = new Thickness(0.4);
            txtBoxNac.BorderThickness = new Thickness(0.4);
            txtBoxPais.BorderThickness = new Thickness(0.4);
            txtBoxProv.BorderThickness = new Thickness(0.4);
            txtBoxDom.BorderThickness = new Thickness(0.4);
            txtBoxTel.BorderThickness = new Thickness(0.4);
            txtBoxMarca.BorderThickness = new Thickness(0.4);
            txtBoxModelo.BorderThickness = new Thickness(0.4);
            txtBoxPatente.BorderThickness = new Thickness(0.4);

            RButtonMascBusq.IsEnabled = false;
            RButtonFemBusq.IsEnabled = false;
            RButtonOtroBusq.IsEnabled = false;

            ComboBoxAnioDet.IsEnabled = false;

            RButtonRobDet.IsEnabled = false;
            RButtonNoRobDet.IsEnabled = false;

            btnModificar.IsEnabled = false;
        }

        public void FormatoFecha()
        {
            datePickerFecha.DisplayDateStart = new DateTime(1900, 1, 1);
            datePickerFecha.DisplayDateEnd = DateTime.Today;
        }


        //----FUNCIONES DE CONTROL WPF---------------------------------------------------------------


        //Genera los items de los años del ComboBox en "Añadir"
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxAnio.Items.Count == 0)
            {
                for (int i = DateTime.Today.Year; i >= 1920; i--)
                {
                    ComboBoxAnio.Items.Add(i);
                }
            }

            if (ComboBoxAnioDet.Items.Count == 0)
            {
                for (int i = DateTime.Today.Year; i >= 1920; i--)
                {
                    ComboBoxAnioDet.Items.Add(i);
                }
            }

            FormatoFecha();
        }

        //↓↓ ELIMINAR O MODIFICAR -- Prueba de seleccion de items del listview
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                //MessageBox.Show(item.ToString() + "SE ha seleccionado un item");

            }
        }

        //Caracter para el ingreso de registro a la BD Propietario
        private void RButtonMasc_Checked(object sender, RoutedEventArgs e)
        {
            sex = "M";
        }

        //Caracter para el ingreso de registro a la BD Propietario
        private void RButtonFem_Checked(object sender, RoutedEventArgs e)
        {
            sex = "F";
        }

        //Caracter para el ingreso de registro a la BD Propietario
        private void RButtonOtro_Checked(object sender, RoutedEventArgs e)
        {
            sex = "O";
        }

        //String para el ingreso de registro a la BD Automotor
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            estado = "Robado";
        }

        //String para el ingreso de registro a la BD Automotor
        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            estado = "No Robado";
        }

        //Boton que activa la funcion de vaciar los textboxs
        private void VaciarCampos_Click(object sender, RoutedEventArgs e)
        {
            VaciarCampos();
        }

        private void ListaPropietarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic var = listaPropietarios.SelectedItem;
            if (var != null)
            {
                patente = var.labelPatente;
                if (checkBoxVerDet.IsChecked == true && txtBusqueda.Text != "")
                {
                    VerDetalles();
                }
            }

        }

        //Evita la clonacion de items en los filtros de orden y ayuda al refresco del ListView
        private void ComboBoxFltrOrden_DropDownClosed(object sender, EventArgs e)
        {
            comboBoxFltrOrden2.Items.Clear();

            switch (comboBoxFltrOrden.SelectedIndex)
            {
                case 0:
                    //Si no hay filtro aplicado en el ComboBoxOrden1 el 2 desaparece
                    comboBoxFltrOrden2.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    comboBoxFltrOrden2.Items.Add("--");
                    comboBoxFltrOrden2.Items.Add("Nombre");
                    comboBoxFltrOrden2.Items.Add("Patente");
                    comboBoxFltrOrden2.Items.Add("Modelo");
                    comboBoxFltrOrden2.Visibility = Visibility.Visible;
                    break;
                case 2:
                    comboBoxFltrOrden2.Items.Add("--");
                    comboBoxFltrOrden2.Items.Add("Apellido");
                    comboBoxFltrOrden2.Items.Add("Patente");
                    comboBoxFltrOrden2.Items.Add("Modelo");
                    comboBoxFltrOrden2.Visibility = Visibility.Visible;
                    break;
                case 3:
                    comboBoxFltrOrden2.Items.Add("--");
                    comboBoxFltrOrden2.Items.Add("Apellido");
                    comboBoxFltrOrden2.Items.Add("Nombre");
                    comboBoxFltrOrden2.Items.Add("Modelo");
                    comboBoxFltrOrden2.Visibility = Visibility.Visible;
                    break;
                case 4:
                    comboBoxFltrOrden2.Items.Add("--");
                    comboBoxFltrOrden2.Items.Add("Apellido");
                    comboBoxFltrOrden2.Items.Add("Nombre");
                    comboBoxFltrOrden2.Items.Add("Patente");
                    comboBoxFltrOrden2.Visibility = Visibility.Visible;
                    break;
            }
            comboBoxFltrOrden2.SelectedIndex = 0;

            if (txtBusqueda.Text != "")
            {
                BusquedaDatos();
            }
        }

        //Ayuda al refresco del ListView
        private void ComboBoxFltrEstado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtBusqueda.Text != "")
            {
                BusquedaDatos();
            }
        }

        //Evita la carga de todos los registros de la BD y da el foco al textbox de busqueda
        private void TxtBusqueda_SelectionChanged(object sender, RoutedEventArgs e)
        {
            txtBusqueda.Focus();

            if (txtBusqueda.Text != "")
            {
                BusquedaDatos();
            }
            else
            {
                listaPropietarios.Items.Clear();
                VaciarCamposBusqueda();
            }
        }

        //Ayuda al refresco del ListView
        private void ComboBoxBusqueda_DropDownClosed(object sender, EventArgs e)
        {
            if (txtBusqueda.Text != "")
            {
                if (comboBoxBusqueda.SelectedIndex == 1 || comboBoxBusqueda.SelectedIndex == 2)
                {
                    if (!txtBusqueda.Text.All(char.IsDigit))
                    {
                        txtBusqueda.Text = "";
                    }
                }
                BusquedaDatos();
            }
            txtBusqueda.Focus();
        }

        //Ayuda al refresco del Listview
        private void ComboBoxFltrEstado_DropDownClosed(object sender, EventArgs e)
        {
            if (txtBusqueda.Text != "")
            {
                BusquedaDatos();
            }
        }

        private void TxtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (comboBoxBusqueda.SelectedIndex == 2)
            {
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        private void CheckBoxVerDet_Checked(object sender, RoutedEventArgs e)
        {
            expanderDatos.IsExpanded = true;
            //expanderDatos.IsEnabled = true;

            expanderDatos.Visibility = Visibility.Visible;

            if (txtBusqueda.Text != "")
            {
                VerDetalles();
            }
        }

        private void CheckBoxVerDet_Unchecked(object sender, RoutedEventArgs e)
        {
            expanderDatos.IsExpanded = false;
            expanderDatos.IsEnabled = false;

            checkBoxEditar.IsChecked = false;

            VaciarCamposBusqueda();
        }

        private void CheckBoxEditar_Checked(object sender, RoutedEventArgs e)
        {
            expanderDatos.IsExpanded = true;
            expanderDatos.IsEnabled = true;

            HabilitarEdicion();
        }

        private void CheckBoxEditar_Unchecked(object sender, RoutedEventArgs e)
        {
            DeshabilitarEdicion();
        }

        private void TxtBoxDni_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void TxtBoxTel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void DatePickerFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerFecha.DisplayDate < new DateTime(1900, 1, 1) || datePickerFecha.DisplayDate > DateTime.Today)
            {
                datePickerFecha.SelectedDate = null;
            }
        }

        private void TxtBusqRap_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void BtnModificar_Click(object sender, RoutedEventArgs e)
        {
            ModificarDatos();
        }
        
    }
}