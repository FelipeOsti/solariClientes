using Newtonsoft.Json;
using SolariClientes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolariClientes.Logic
{
    class ClienteInternoLogic
    {
        public async Task<long> VerificaLogin(string usuario, string senha)
        {
            try
            {
                var request = await RequestWS.RequestGET("clienteinterno/VerificaUsuario/" + usuario + "/" + RequestWS.CriptografaSHA256(senha));
                var json = await request.Content.ReadAsStringAsync();
                return Convert.ToInt64(json);
            }
            catch
            {
                throw;
            }
        }

        internal async Task<IN_CLIENTEINTERNOModel> GetClienteInterno(long nidClienteInterno)
        {
            try
            {
                var request = await RequestWS.RequestGET("clienteinterno/GetClienteInterno/" + nidClienteInterno);
                var json = await request.Content.ReadAsStringAsync();
                return (IN_CLIENTEINTERNOModel)JsonConvert.DeserializeObject(json,typeof(IN_CLIENTEINTERNOModel));
            }
            catch
            {
                throw;
            }
        }

        internal async void SalvarCliente(IN_CLIENTEINTERNOModel cli)
        {
            try
            {
                var json = JsonConvert.SerializeObject(cli);
                var request = await RequestWS.RequestPOST("clienteinterno/SalvarCliente",json);
                await request.Content.ReadAsStringAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
