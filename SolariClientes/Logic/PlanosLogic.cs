using Newtonsoft.Json;
using SolariClientes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolariClientes.Logic
{
    class PlanosLogic
    {
		public async Task<List<CM_PLANOModel>> getPlanosAsync()
        {
            try
            {
                var request = await RequestWS.RequestGET("planos/getPlanos");
                var json = await request.Content.ReadAsStringAsync();
                return (List<CM_PLANOModel>)JsonConvert.DeserializeObject(json,typeof(List<CM_PLANOModel>));
            }
            catch
            {
                throw;
            }
        }

        public async Task<CM_PLANOModel> getPlano(long nidPlano)
        {
            try
            {
                var request = await RequestWS.RequestGET("planos/getPlano/"+nidPlano);
                var json = await request.Content.ReadAsStringAsync();
                return (CM_PLANOModel)JsonConvert.DeserializeObject(json, typeof(CM_PLANOModel));
            }
            catch
            {
                throw;
            }
        }
    }
}
