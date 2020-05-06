using Microsoft.EntityFrameworkCore;
using NextGen.BiddingPlatform.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextGen.BiddingPlatform.Migrations.Seed.Host
{
    public class DefaultCountryAndStateBuilder
    {
        private readonly BiddingPlatformDbContext _context;
        public DefaultCountryAndStateBuilder(BiddingPlatformDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateDefaultCountryAndState();
        }

        private void CreateDefaultCountryAndState()
        {
            var hasCountryExist = _context.Countries.AsNoTracking().Any(x => x.CountryName == "United States Of America");
            if (!hasCountryExist)
            {
                var country = new Country.Country
                {
                    UniqueId = Guid.NewGuid(),
                    CountryCode = "USA",
                    CountryName = "United States Of America"
                };
                _context.Countries.Add(country);
                _context.SaveChanges();

                _context.States.AddRange(new List<Core.State.State>
                {
                    new Core.State.State{ CountryId = country.Id,StateCode="AL",StateName="Alaska",UniqueId=Guid.NewGuid()},
                    new Core.State.State{ CountryId = country.Id,StateCode="CA",StateName="California",UniqueId=Guid.NewGuid()},
                    new Core.State.State{ CountryId = country.Id,StateCode="CO",StateName="Colorado",UniqueId=Guid.NewGuid()},
                    new Core.State.State{ CountryId = country.Id,StateCode="TX",StateName="Texas",UniqueId=Guid.NewGuid()},
                });
                _context.SaveChanges();
            }
        }
    }
}
