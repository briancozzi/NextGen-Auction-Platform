using System.Threading.Tasks;
using NextGen.BiddingPlatform.Views;
using Xamarin.Forms;

namespace NextGen.BiddingPlatform.Services.Modal
{
    public interface IModalService
    {
        Task ShowModalAsync(Page page);

        Task ShowModalAsync<TView>(object navigationParameter) where TView : IXamarinView;

        Task<Page> CloseModalAsync();
    }
}
