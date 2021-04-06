using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.Authorization.Users;
using NextGen.BiddingPlatform.CardDetail.Dto;
using NPOI.HSSF.Record.Aggregates;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.CardDetail
{
    public class CardDetailAppService : BiddingPlatformAppServiceBase, ICardDetailAppService
    {
        private readonly IRepository<Core.CardDetails.CardDetail> _carddetailRepository;
        private readonly IRepository<Country.Country> _countryRepository;
        private readonly IRepository<Core.State.State> _stateRepository;
        private readonly IDataProtector protector;
        private readonly IAbpSession AbpSession;
        public CardDetailAppService(IRepository<Core.CardDetails.CardDetail> carddetailRepository,
                                    IRepository<Country.Country> countryRepository,
                                    IRepository<Core.State.State> stateRepository,
                                    IDataProtectionProvider provider,
                                    IAbpSession abpSession)
        {
            _carddetailRepository = carddetailRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            AbpSession = abpSession;
            // Encryption key
            protector = provider.CreateProtector("BiddingPlatform");
        }

        public async Task<CreateCardDetailDto> Create(CreateCardDetailDto input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            if (user == null)
                throw new Exception("User not found on this Id");

            var country = await _countryRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.CountryUniqueId);
            var state = await _stateRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.StateUniqueId);
            if (country == null || state == null)
                throw new Exception("Country or State not found");

            if (!AbpSession.TenantId.HasValue)
                throw new Exception("You are not Authorized user.");


            var cardDetail = ObjectMapper.Map<Core.CardDetails.CardDetail>(input);
            cardDetail.UniqueId = Guid.NewGuid();
            cardDetail.Address.UniqueId = Guid.NewGuid();
            cardDetail.UserId = input.UserId;
            cardDetail.Address.StateId = state.Id;
            cardDetail.Address.CountryId = country.Id;
            cardDetail.TenantId = AbpSession.TenantId.Value;
            cardDetail.CVV = protector.Protect(cardDetail.CVV);
            cardDetail.CreditCardNo = protector.Protect(cardDetail.CreditCardNo);
            await _carddetailRepository.InsertAsync(cardDetail);
            return input;
        }


        public async Task<UpdateCardDetailDto> Update(UpdateCardDetailDto input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            if (user == null)
                throw new Exception("User not found on this Id");

            if (!user.FullName.Equals(input.FullName))
                throw new Exception("User Name is not found");

            var country = await _countryRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.CountryUniqueId);
            var state = await _stateRepository.FirstOrDefaultAsync(x => x.UniqueId == input.Address.StateUniqueId);
            if (country == null || state == null)
                throw new Exception("Country or State not found");

            var cardDetail = await _carddetailRepository.GetAllIncluding(x => x.Address,x=>x.User).FirstOrDefaultAsync(x => x.UniqueId == input.UniqueId);
            if (cardDetail == null)
                throw new Exception("AppAccount not found for given Id");

            //User Card Properties

            cardDetail.CreditCardNo = protector.Protect(input.CreditCardNo);
            cardDetail.CVV = protector.Protect(input.CVV);
            cardDetail.ExpiryMonth = input.ExpiryMonth;
            cardDetail.ExpiryYear = input.ExpiryYear;

            //User Address Properties
            cardDetail.Address.Address1 = input.Address.Address1;
            cardDetail.Address.Address2 = input.Address.Address2;
            cardDetail.Address.City = input.Address.City;
            cardDetail.Address.ZipCode = input.Address.ZipCode;
            cardDetail.Address.StateId = state.Id;
            cardDetail.Address.CountryId = country.Id;
            await _carddetailRepository.UpdateAsync(cardDetail);
            return input;
        }


        public async Task Delete(Guid Id)
        {
            var cardDetail = await _carddetailRepository.FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (cardDetail == null)
                throw new Exception("User Card Detail not found for given Id");

            await _carddetailRepository.DeleteAsync(cardDetail);
        }

        public async Task<List<CardDetailDto>> GetAllcardDetails()
        {
            var cardDetails = await _carddetailRepository.GetAll()
                                             .Select(y => new CardDetailDto
                                             {
                                                 UniqueId = y.UniqueId,
                                                 UserId = y.UserId,
                                                 CreditCardNo = protector.Unprotect( y.CreditCardNo),
                                                 CVV = protector.Unprotect(y.CVV),
                                                 FullName = y.User.FullName,
                                                 ExpiryMonth = y.ExpiryMonth,
                                                 ExpiryYear = y.ExpiryYear
                                             }).ToListAsync();
            return cardDetails;
        }

        public async Task<CardDetailDto> GetcardDetailById(Guid Id)
        {
            var cardDetail = await _carddetailRepository.GetAllIncluding(x => x.Address, x => x.Address.State, x => x.Address.Country,x=>x.User)
                                                         .Select(y => new CardDetailDto
                                                         {
                                                             UniqueId = y.UniqueId,
                                                             UserId = y.UserId,
                                                             FullName = y.User.FullName,
                                                             CreditCardNo = protector.Unprotect(y.CreditCardNo),
                                                             CVV = protector.Unprotect(y.CVV),
                                                             ExpiryMonth = y.ExpiryMonth,
                                                             ExpiryYear = y.ExpiryYear,
                                                             Address = new Address.Dto.AddressDto {
                                                                                                     Address1 = y.Address.Address1,
                                                                                                     CountryUniqueId = y.Address.Country.UniqueId,
                                                                                                     StateUniqueId = y.Address.State.UniqueId,
                                                                                                     ZipCode = y.Address.ZipCode
                                                                                                   }
                                                         })
                                                        .FirstOrDefaultAsync(x => x.UniqueId == Id);
            if (cardDetail == null)
                throw new Exception("User Card Detail not found for given Id");

            return ObjectMapper.Map<CardDetailDto>(cardDetail);
        }
    }
}
