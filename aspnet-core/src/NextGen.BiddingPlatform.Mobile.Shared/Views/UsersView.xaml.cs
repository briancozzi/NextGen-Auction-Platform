using NextGen.BiddingPlatform.Models.Users;
using NextGen.BiddingPlatform.ViewModels;
using Xamarin.Forms;

namespace NextGen.BiddingPlatform.Views
{
    public partial class UsersView : ContentPage, IXamarinView
    {
        public UsersView()
        {
            InitializeComponent();
        }

        public async void ListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            await ((UsersViewModel) BindingContext).LoadMoreUserIfNeedsAsync(e.Item as UserListModel);
        }
    }
}