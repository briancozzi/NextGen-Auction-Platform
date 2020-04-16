using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace NextGen.BiddingPlatform.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
