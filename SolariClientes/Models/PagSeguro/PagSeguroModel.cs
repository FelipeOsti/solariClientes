using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolariClientes.Models.PagSeguro
{
    public class PagSeguroModel
    {
        public ClienteModel cliente { get; set; }
        public List<ProdutoModel> produtos { get; set; }
        public EnderecoModel enderecoEntrega { get; set; }

        public int shippingType { get; set; }
        public decimal shippingCost { get; set; }
        public string paymontReference { get; set; }
    }
}
