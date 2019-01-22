using SolariClientes.Logic;
using SolariClientes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class MeusDadosPage : Page
    {
        public MeusDadosPage()
        {
            this.InitializeComponent();

            if (App.novoCliente)
                NovoCliente();
            else
            {
                if (!App.bboLogado) return;
                PrepararCampos();
            }
        }

        private async void NovoCliente()
        {
            await GetPlanosAsync(App.planoSelecionado);
            edLogin.IsReadOnly = false;

            ValorImplantacao.IsReadOnly = true;
            ValorImplantacao.Text = "R$ 1.000,00";
            NumeroParcelas.SelectedIndex = 0;

            var dialog = new MessageDialog("Informe seus dados e confirme seu cadastro para começar a utilizar o sistema SOLARI!");
            await dialog.ShowAsync();
        }

        private async void PrepararCampos()
        {
            ClienteInternoLogic cliLogic = new ClienteInternoLogic();
            var cliente = await cliLogic.GetClienteInterno(App.nidClienteInterno);
            edLogin.IsReadOnly = true;

            edLogin.Text = cliente.CD_LOGIN;
            edNome.Text = cliente.DS_NOME;
            cidade.Text = cliente.DS_CIDADE;
            endereco.Text = cliente.DS_ENDERECO;
            cep.Text = cliente.NR_CEP;
            cpf.Text = cliente.NR_CPF;
            cnpj.Text = cliente.NR_CNPJ;
            numero.Text = cliente.NR_NUMERO;
            uf.Text = cliente.CD_UF;
            bairro.Text = cliente.DS_BAIRRO;
            telefone.Text = cliente.DS_TELEFONE;
            celular.Text = cliente.DS_CELULAR;
            ddd.Text = cliente.NR_DDD;
            email.Text = cliente.DS_EMAIL;

            ValorImplantacao.Text = cliente.VL_IMPLANTACAO.ToString();
            NumeroParcelas.SelectedIndex = cliente.NR_PARCELASIMPLANT-1;

            await GetPlanosAsync(cliente.ID_PLANO);

            App.ClienteLogado = cliente;
        }

        public async Task GetPlanosAsync(long nidPLano)
        {
            var planosLogic = new PlanosLogic();
            var planos = await planosLogic.getPlano(nidPLano);
            plano.Text = planos.DS_PLANO + " - R$ " + planos.VL_PLANO;
            observacaoPlano.Text = planos.OB_PLANO.Replace("-", Environment.NewLine + "-");

            App.PlanoCliente = planos;
        }

        private async void btConfirma_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClienteInternoLogic cliLogic = new ClienteInternoLogic();
                cliLogic.SalvarCliente(new IN_CLIENTEINTERNOModel()
                {
                    ID_CLIENTEINTERNO = App.nidClienteInterno,
                    CD_LOGIN = edLogin.Text,
                    DS_SENHA = RequestWS.CriptografaSHA256(edSenha.Password),
                    ID_PLANO = App.planoSelecionado > 0 ? App.planoSelecionado : App.PlanoCliente.ID_PLANO,
                    NR_DIAVENCIMENTO = Convert.ToInt32(diaPagamento.Text),
                    NR_PARCELASIMPLANT = Convert.ToInt32(NumeroParcelas.SelectedIndex+1),
                    VL_IMPLANTACAO = Convert.ToDouble(ValorImplantacao.Text.Replace("R$", "").Trim()),
                    BO_ADMIN = "F",
                    DT_CADASTRO = DateTime.Now.Date.ToString(),
                    DS_BAIRRO = bairro.Text,
                    CD_UF = uf.Text,
                    DS_CIDADE = cidade.Text,
                    DS_NOME = edNome.Text,
                    DS_ENDERECO = endereco.Text,
                    NR_CEP = cep.Text,
                    NR_CPF = cpf.Text,
                    NR_CNPJ = cnpj.Text,
                    NR_NUMERO = numero.Text,
                    NR_DDD = ddd.Text,
                    DS_TELEFONE = telefone.Text,
                    DS_CELULAR = celular.Text,
                    DS_EMAIL = email.Text                    
                });

                var dialog = new MessageDialog("Dados salvos com sucesso!");
                await dialog.ShowAsync();
            }
            catch
            {
                var dialog = new MessageDialog("Erro ao confirmar informações! Tente novamente em instantes!");
                await dialog.ShowAsync();
            }
        }
    }
}
