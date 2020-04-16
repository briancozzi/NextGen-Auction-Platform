using Xamarin.Forms.Internals;

namespace NextGen.BiddingPlatform.Behaviors
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        bool Execute(object sender, object parameter);
    }
}