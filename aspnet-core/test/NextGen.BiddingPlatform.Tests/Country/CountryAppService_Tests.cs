using NextGen.BiddingPlatform.Country;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using NextGen.BiddingPlatform.Country.Dto;

namespace NextGen.BiddingPlatform.Tests.Country
{
    public class CountryAppService_Tests : AppTestBase
    {
        private readonly ICountryAppService _countryAppService;
        public CountryAppService_Tests()
        {
            _countryAppService = Resolve<ICountryAppService>();
        }

        [Fact]
        public async Task Should_Get_Country()
        {
            var countries = await _countryAppService.GetAllCountry();
            countries.Count.ShouldBe(0);
        }

        [Fact]
        public async Task Should_Create_Country_And_Update_Country()
        {
            await _countryAppService.Create(new CreateCountryDto
            {
                CountryCode = "USA",
                CountryName = "United States of America"
            });
            await UsingDbContextAsync(async context =>
            {
                await context.SaveChangesAsync();
            });

            var countries = await _countryAppService.GetAllCountry();
            var country = countries.FirstOrDefault();
            //country.ShouldNotBe(null);

            country.CountryCode.ShouldBe("USA");

            await _countryAppService.Update(new CountryDto
            {
                UniqueId = country.UniqueId,
                CountryCode = "US",
                CountryName = "United States of America"
            });

            await UsingDbContextAsync(async context =>
            {
                await context.SaveChangesAsync();
            });

            var updatedCountries = await _countryAppService.GetAllCountry();
            var updatedCountry = updatedCountries.FirstOrDefault();

            updatedCountry.CountryCode.ShouldBe("US");
        }
    }
}
