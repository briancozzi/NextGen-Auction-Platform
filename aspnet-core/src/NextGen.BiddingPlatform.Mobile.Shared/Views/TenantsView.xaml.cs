using NextGen.BiddingPlatform.Models.Tenants;
using NextGen.BiddingPlatform.ViewModels;
using Xamarin.Forms;

namespace NextGen.BiddingPlatform.Views
{
    public partial class TenantsView : ContentPage, IXamarinView
    {
        public TenantsView()
        {
            InitializeComponent();
        }

        private async void ListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            await ((TenantsViewModel)BindingContext).LoadMoreTenantsIfNeedsAsync(e.Item as TenantListModel);
        }
    }
}