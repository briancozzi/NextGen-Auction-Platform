using Abp.Application.Services.Dto;
using NextGen.BiddingPlatform.CardDetail.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.CardDetail
{
    public interface ICardDetailAppService
    {
        Task<List<CardDetailDto>> GetAllcardDetails();
        Task<CreateCardDetailDto> Create(CreateCardDetailDto input);
        Task<UpdateCardDetailDto> Update(UpdateCardDetailDto input);
        Task Delete(Guid Id);
        Task<CardDetailDto> GetcardDetailById(Guid input);
    }
}
