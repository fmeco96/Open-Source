using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patentest
{
    class Administrador
    {
        string usuario;
        string pass;
        string estado;

        public string Usuario { get => usuario; set => usuario = value; }
        public string Pass { get => pass; set => pass = value; }
        public string Estado { get => estado; set => estado = value; }
    }
}
