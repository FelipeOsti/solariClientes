using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolariClientes.Models
{
    class CR_PARCELAModel
    {
        public long ID_PARCELA { get; set; }
        public long ID_DOCUM { get; set; }
        public string DS_DOCUM { get; set; }
        public double VL_PARCELA { get; set; }
        public string DS_PARCELA {
            get {
                return "R$ " + VL_PARCELA.ToString(CultureInfo.CreateSpecificCulture("pt-BR"));
            }
        }
        public double VL_PAGO { get; set; }        
        public string DT_VENCIMENTO { get; set; }
        public bool bboProcessando { get; set; }
    }
}
