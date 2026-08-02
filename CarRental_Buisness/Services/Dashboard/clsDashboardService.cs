using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.Dashboard;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using CarRental_Entities.Dashboard;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.Dashboard
{
    public class clsDashboardService
    {
        public async Task<clsServiceResult<clsDashboardDto>> GetDashboardDataAsync()
        {
            try
            {
                var statisticsTask = clsDashboardData.GetStatisticsAsync();
                var latestBookingsTask = clsDashboardData.GetLatestBookingsAsync();
                var alertsTask = clsDashboardData.GetAlertsAsync();

                await Task.WhenAll(statisticsTask, latestBookingsTask, alertsTask);

                var statistics = await statisticsTask;
                var latestBookings = await latestBookingsTask;
                var alerts = await alertsTask;

                return clsServiceResult<clsDashboardDto>.OK(BuildDashboardDto(statistics, latestBookings, alerts));
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsDashboardService.GetDashboardDataAsync", ex);
                return clsServiceResult<clsDashboardDto>.Fail("فشل تحميل البيانات");
            }
        }
        private  clsDashboardDto BuildDashboardDto(clsStatisticsEntity statistics,List<clsLatestBookingsEntity> latestBookings,
             List<clsAlertsEntity> alerts)
        {
            return new clsDashboardDto
            {
                Statistics = clsDashboardMapper.ToStatisticsDto(statistics),
                LatestBookings = latestBookings.Select(clsDashboardMapper.ToLatestBookingsDto).ToList(),
                Alerts = alerts.Select(clsDashboardMapper.ToAlertsDto).ToList()
            };
        }
    }
}
