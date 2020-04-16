using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NextGen.BiddingPlatform.MultiTenancy.HostDashboard.Dto;

namespace NextGen.BiddingPlatform.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}