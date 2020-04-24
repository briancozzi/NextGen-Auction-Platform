using NextGen.BiddingPlatform.Country;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using NextGen.BiddingPlatform.Country.Dto;
using Microsoft.EntityFrameworkCore;

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
        public async Task Should_Get_All_Country()
        {
            var countries = await _countryAppService.GetAllCountry();
            countries.Items.Count.ShouldBe(0);
        }

        [Fact]
        public async Task Should_Create_Country_And_Update_Country()
        {
            await _countryAppService.Create(new CreateCountryDto
            {
                CountryCode = "USA",
                CountryName = "United States of America"
            });
            await _countryAppService.Create(new CreateCountryDto
            {
                CountryCode = "IN",
                CountryName = "India"
            });
            await UsingDbContextAsync(async context =>
            {
                await context.SaveChangesAsync();
            });

            var countries = await _countryAppService.GetAllCountry();
            var country = countries.Items.FirstOrDefault();

            country.ShouldNotBe(null);

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
            var updatedCountry = updatedCountries.Items.FirstOrDefault();

            updatedCountry.CountryCode.ShouldBe("US");
        }
    }
}
