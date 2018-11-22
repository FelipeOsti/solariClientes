using SolariClientes.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void btLogin_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(edUsuario.Text))
            {
                var dialog = new MessageDialog("Favor informar o usuário");
                await dialog.ShowAsync();
                return;
            }

            if (string.IsNullOrEmpty(edSenha.Password))
            {
                var dialog = new MessageDialog("Favor informar a senha");
                await dialog.ShowAsync();
                return;
            }

            progressRing.IsActive = true;
            ClienteInternoLogic cliente = new ClienteInternoLogic();
            App.nidClienteInterno = await cliente.VerificaLogin(edUsuario.Text, edSenha.Password);
            if(App.nidClienteInterno > 0)
            {
                App.bboLogado = true;
                MainPage.current.Navegar(typeof(MeusDadosPage));
            }
            else
            {

                var dialog = new MessageDialog("Usuário e/ou senha incorretos!");
                await dialog.ShowAsync();                
            }
            progressRing.IsActive = false;
        }
    }
}
