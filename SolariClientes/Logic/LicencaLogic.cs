using Newtonsoft.Json;
using SolariClientes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolariClientes.Logic
{
    class LicencaLogic
    {
        public async Task<List<IN_CHAVELICENCAModel>> GetLicencas()
        {
            try
            {
                var request = await RequestWS.RequestGET("licenca/GetLicenca/" + App.nidClienteInterno);

                var json = await request.Content.ReadAsStringAsync();

                var licencas = (List<IN_CHAVELICENCAModel>)JsonConvert.DeserializeObject(json, typeof(List<IN_CHAVELICENCAModel>));

                return licencas;
            }
            catch
            {
                throw;
            }
        }
    }
}
