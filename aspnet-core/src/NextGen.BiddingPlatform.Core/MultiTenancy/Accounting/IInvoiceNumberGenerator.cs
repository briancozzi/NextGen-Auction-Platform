using System.Threading.Tasks;
using Abp.Dependency;

namespace NextGen.BiddingPlatform.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}