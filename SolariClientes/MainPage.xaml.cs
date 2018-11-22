using SolariClientes.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x416

namespace SolariClientes
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string PageNamespace = nameof(SolariClientes.Pages);
        internal static string apiURI = @"http://solariweb.azurewebsites.net/API/";

        public static MainPage current;

        public MainPage()
        {
            this.InitializeComponent();
            current = this;

            AppFrame.Navigated += AppFrame_Navigated;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;

            Navegar(typeof(InicioPage));
        }

        public void Navegar(Type objeto)
        {
            AppFrame.Navigate(objeto);
        }

        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (AppFrame.CanGoBack)
            {
                AppFrame.GoBack();
            }
        }

        private void AppFrame_Navigated(object sender, NavigationEventArgs e)
        {
            var pageName = AppFrame.Content.GetType().Name;
            AppNavView.SelectedItem = AppNavView.MenuItems.OfType<NavigationViewItem>().Where(item => item.Tag.ToString() == pageName).First();
        }

        private void AppNavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var invokedMenuItem = sender.MenuItems.OfType<NavigationViewItem>().Where(item => item.Content.ToString() == args.InvokedItem.ToString()).First();
            var pageTypeName = invokedMenuItem.Tag.ToString();
            var pageType = Assembly.GetExecutingAssembly().GetType($"SolariClientes.{PageNamespace}.{pageTypeName}");
            AppFrame.Navigate(pageType);
        }
    }
}
