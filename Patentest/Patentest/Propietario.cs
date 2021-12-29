using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patentest
{
    class Propietario
    {
        string id;
        string numeroRef;
        string nombre;
        string apellido;
        string dni;
        string sexo;
        string fecha;
        string pais;
        string provincia;
        string domicilio;
        string telefono;
        string patente;
        string marca;
        string modelo;        
        string anio;
        string estado;

        public string Id { get => id; set => id = value; }
        public string NumeroRef { get => numeroRef; set => numeroRef = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellido { get => apellido; set => apellido = value; }
        public string Dni { get => dni; set => dni = value; }
        public string Sexo { get => sexo; set => sexo = value; }
        public string Pais { get => pais; set => pais = value; }
        public string Provincia { get => provincia; set => provincia = value; }
        public string Domicilio { get => domicilio; set => domicilio = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string Patente { get => patente; set => patente = value; }
        public string Marca { get => marca; set => marca = value; }
        public string Modelo { get => modelo; set => modelo = value; }                
        public string Anio { get => anio; set => anio = value; }
        public string Fecha { get => fecha; set => fecha = value; }
        public string Estado { get => estado; set => estado = value; }
    }
}
