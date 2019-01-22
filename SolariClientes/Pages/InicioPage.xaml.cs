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
using Windows.UI;
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
    public sealed partial class InicioPage : Page
    {
        public InicioPage()
        {
            this.InitializeComponent();
            IniciarPage();
        }

        public async void IniciarPage()
        {
            try
            {
                progressRing.IsActive = true;
                var planos = await GetPlanos();
                int nnrColum = 0;
                string sdsPlano = planos[0].DS_PLANO;
                string obPlano = "";

                var itens = new List<CM_PLANOModel>();

                foreach (var plano in planos)
                {
                    if (!plano.DS_PLANO.ToUpper().Contains("GRATIS") && !plano.DS_PLANO.ToUpper().Contains("GRÁTIS"))
                    {
                        if (plano.DS_PLANO != sdsPlano)
                        {
                            AddPlanoTela(ref sdsPlano,ref obPlano, ref itens, ref nnrColum);
                        }
                        itens.Add(plano);
                    }
                    sdsPlano = plano.DS_PLANO;
                    obPlano = plano.OB_PLANO;
                }

                AddPlanoTela(ref sdsPlano, ref obPlano, ref itens, ref nnrColum);

                progressRing.IsActive = false;
            }
            catch
            {
                var msg = new MessageDialog("Falha ao montar planos");
                await msg.ShowAsync();
            }
        }

        public void AddPlanoTela(ref string sdsPlano, ref string obPlano, ref List<CM_PLANOModel> itens, ref int nnrColum)
        {
            var stk = new StackPanel();

            var txtTitulo = new TextBlock();
            txtTitulo.Foreground = new SolidColorBrush(Colors.DarkOrange);
            txtTitulo.FontSize = 28;
            txtTitulo.HorizontalAlignment = HorizontalAlignment.Center;
            txtTitulo.Text = sdsPlano;

            var txtConteudo = new TextBlock();
            txtConteudo.Foreground = new SolidColorBrush(Colors.Gray);
            txtConteudo.FontSize = 16;
            txtConteudo.HorizontalAlignment = HorizontalAlignment.Left;
            txtConteudo.Text = obPlano.Replace("-", Environment.NewLine + "-");
            txtConteudo.VerticalAlignment = VerticalAlignment.Stretch;

            var cBox = new ComboBox();
            foreach (var plano in itens)
            {
                string sflPlano = "Mensal";
                if (plano.FL_PLANO.Equals("T")) sflPlano = "Trimestral";
                if (plano.FL_PLANO.Equals("S")) sflPlano = "Semestral";
                if (plano.FL_PLANO.Equals("A")) sflPlano = "Anual";
                cBox.Items.Add(plano.ID_PLANO.ToString()+" - "+sflPlano + " - R$ " + plano.VL_PLANO.ToString());
            }
            cBox.SelectedIndex = 0;
 
            cBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            cBox.Margin = new Thickness(10, 10, 10, 0);
            itens.Clear();

            var btContratar = new Button();
            btContratar.Content = "Contratar";
            btContratar.HorizontalAlignment = HorizontalAlignment.Left;
            btContratar.Margin = new Thickness(0, 20, 0, 0);

            btContratar.Click += BtContratar_Click;

            stk.Children.Add(txtTitulo);
            stk.Children.Add(txtConteudo);
            stk.Children.Add(cBox);
            stk.Children.Add(btContratar);

            Grid.SetColumn(stk, nnrColum);
            Grid.SetRow(stk, 0);
            gridDados.Children.Add(stk);
            nnrColum++;
        }

        private void BtContratar_Click(object sender, RoutedEventArgs e)
        {
            if(!App.bboLogado) App.novoCliente = true;

            var stack = (StackPanel)((Button)sender).Parent;
            var cBox = (ComboBox)stack.Children[2];

            var item = (string)cBox.SelectedValue;
            var posId = item.IndexOf("-");
            var id = Convert.ToInt64(item.Substring(0, posId).Trim());

            App.planoSelecionado = id;
            MainPage.current.Navegar(typeof(MeusDadosPage));
        }

        public async Task<List<CM_PLANOModel>> GetPlanos()
        {
            try
            {
                var planosLogic = new PlanosLogic();
                return await planosLogic.getPlanosAsync();                
            }
            catch
            {
                throw;
            }
        }

        private void BtTemPlano_Click(object sender, RoutedEventArgs e)
        {
            MainPage.current.Navegar(typeof(Pages.LoginPage));
        }
    }
}
