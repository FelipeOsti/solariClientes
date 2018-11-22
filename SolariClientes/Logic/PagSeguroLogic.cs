using Newtonsoft.Json;
using SolariClientes.Models.PagSeguro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolariClientes.Logic
{
    class PagSeguroLogic
    {
        public async Task<string> GetUrlPagamento(PagSeguroModel pgModel)
        {
            try
            {
                var json = JsonConvert.SerializeObject(pgModel);
                var request = await RequestWS.RequestPOST("pagseguro/CriarPagamento", json);
                var retorno = await request.Content.ReadAsStringAsync();
                return retorno;
            }
            catch
            {
                throw;
            }
        }
    }
}
