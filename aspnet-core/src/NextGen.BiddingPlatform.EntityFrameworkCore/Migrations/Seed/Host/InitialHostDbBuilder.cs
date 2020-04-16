using NextGen.BiddingPlatform.EntityFrameworkCore;

namespace NextGen.BiddingPlatform.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly BiddingPlatformDbContext _context;

        public InitialHostDbBuilder(BiddingPlatformDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
