using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolariClientes.Models.PagSeguro
{
    public class ClienteModel
    {
        public string documento { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string ddd { get; set; }
        public string telefone { get; set; }
    }
}
