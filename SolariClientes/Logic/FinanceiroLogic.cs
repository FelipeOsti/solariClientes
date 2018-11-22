using Newtonsoft.Json;
using SolariClientes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolariClientes.Logic
{
    class FinanceiroLogic
    {
        public async Task<List<CR_PARCELAModel>> GetDadosFinanceiro(bool bboPendente)
        {
            try
            {
                var sflPendente = bboPendente == true ? "T" : "F";
                var request = await RequestWS.RequestGET("financeiro/GetDocumentos/" + App.nidClienteInterno + "/" + sflPendente);

                var json = await request.Content.ReadAsStringAsync();

                var docum =  (List<CR_DOCUMModel>)JsonConvert.DeserializeObject(json, typeof(List<CR_DOCUMModel>));

                var parcelas = new List<CR_PARCELAModel>();

                foreach (var doc in docum)
                {
                    foreach (var parc in doc.parcelas)
                    {
                        parcelas.Add(new CR_PARCELAModel()
                        {
                            ID_DOCUM = doc.ID_DOCUM,
                            ID_PARCELA = parc.ID_PARCELA,
                            DS_DOCUM = doc.DS_DOCUM,
                            DT_VENCIMENTO = parc.DT_VENCIMENTO,
                            VL_PARCELA = parc.VL_PARCELA,
                            VL_PAGO = parc.VL_PAGO
                        });
                    }
                }

                return parcelas;
            }
            catch
            {
                throw;
            }
        }
    }
}
