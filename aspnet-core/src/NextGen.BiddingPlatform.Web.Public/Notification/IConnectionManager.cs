using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Web.Public.Notification
{
    public interface IConnectionManager
    {
        void Add(string key, string connectionId);
        IEnumerable<string> GetConnections(string key);
        void Remove(string key, string connectionId);

        IEnumerable<string> GetCloseBiddingConnections(List<Guid> keys);
    }
}
