using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Redimensionamento.ViewModel;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
namespace Redimensionamento
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel(Navigation);
            MessagingCenter.Subscribe<MainPage, String>(this, "Erro", (sender, a) =>
            {
                this.DisplayToastAsync(a, 3000);
            });
            MessagingCenter.Subscribe<MainPage, String>(this, "Sucesso", (sender, a) =>
            {
                this.DisplayToastAsync(a, 3000);
            });
        }
    }
}
