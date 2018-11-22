using SolariClientes.Logic;
using SolariClientes.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace SolariClientes.Pages
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class FinanceiroPage : Page
    {

        public FinanceiroPage()
        {
            this.InitializeComponent();
            if (!App.bboLogado) return;

            CarregarFinanceiro();
        }

        private async void CarregarFinanceiro()
        {
            FinanceiroLogic finL = new FinanceiroLogic();
            lstView.ItemsSource = await finL.GetDadosFinanceiro(false);
        }

        private async void btPagamento_Click(object sender, RoutedEventArgs e)
        {
            var item = (CR_PARCELAModel)lstView.SelectedItem;
            if(item == null)
            {
                var dialog = new MessageDialog("Nenhuma parcela selecionada!");
                await dialog.ShowAsync();
                return;
            }

            PagSeguroLogic psL = new PagSeguroLogic();

            var modelPS = new Models.PagSeguro.PagSeguroModel();
            modelPS.cliente = new Models.PagSeguro.ClienteModel()
            {
                ddd = App.ClienteLogado.NR_DDD,
                documento = string.IsNullOrEmpty(App.ClienteLogado.NR_CPF) ? App.ClienteLogado.NR_CNPJ : App.ClienteLogado.NR_CPF,
                email = App.ClienteLogado.DS_EMAIL,
                nome = App.ClienteLogado.DS_NOME,
                telefone = App.ClienteLogado.DS_TELEFONE
            };
            modelPS.enderecoEntrega = new Models.PagSeguro.EnderecoModel()
            {
                bairro = App.ClienteLogado.DS_BAIRRO,
                cep = App.ClienteLogado.NR_CEP,
                cidade = App.ClienteLogado.DS_CIDADE,
                complemento = "",
                endereco = App.ClienteLogado.DS_ENDERECO,
                estado = App.ClienteLogado.CD_UF,
                numero = App.ClienteLogado.NR_NUMERO,
                pais = "BRA"
            };

            modelPS.produtos = new List<Models.PagSeguro.ProdutoModel>()
            {
                new Models.PagSeguro.ProdutoModel()
                {
                    descricao = App.PlanoCliente.DS_PLANO,
                    id = App.PlanoCliente.ID_PLANO.ToString(),
                    qtde = 1,
                    unitario = (decimal)App.PlanoCliente.VL_PLANO
                }
            };
            
            modelPS.paymontReference = item.ID_PARCELA.ToString();

            modelPS.shippingCost = 0;
            modelPS.shippingType = 3;
           
            string sdsUrl = await psL.GetUrlPagamento(modelPS);
            sdsUrl = sdsUrl.Replace("\"", "");

            await Launcher.LaunchUriAsync(new Uri(sdsUrl));

            
        }
        
    }
}
