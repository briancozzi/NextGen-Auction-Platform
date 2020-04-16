using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using NextGen.BiddingPlatform.MultiTenancy.Accounting.Dto;

namespace NextGen.BiddingPlatform.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
