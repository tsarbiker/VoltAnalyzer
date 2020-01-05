using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoltAnalyzer.ValueObjects.Types.Model;
using VoltAnalyzer.ViewModel.BusinessViewModels.Home;

namespace VoltAnalyzer.Helpers.MVVMHelpers
{
    public static class TorqueHelper
    {
        public static CarInfoVM ModelToViewModel(this AverageConsumptionResponce a_averageConsumption)
        {
            return new CarInfoVM
            {
                AverageFuelConsumption = Decimal.Round(a_averageConsumption.FuelConsumption, 3),
                AverageCombinedConsumption = Decimal.Round(a_averageConsumption.WholeConsumption, 3),
                AverageEVKM = Decimal.Round(a_averageConsumption.AverageEVKM, 3),
                AverageEVConsumption = Decimal.Round(a_averageConsumption.EVConsumption, 3),
                BestFuelConsumption = Decimal.Round(a_averageConsumption.BestFuelConsumption, 3),
                WorstFuelConsumption = Decimal.Round(a_averageConsumption.WorstFuelConsumption, 3),
                BestEVConsumption = Decimal.Round(a_averageConsumption.BestEVConsumption, 3),
                WorstEVConsumption = Decimal.Round(a_averageConsumption.WorstEVConsumption, 3),
                TotalCharging = Decimal.Round(a_averageConsumption.TotalCharging, 2),
                TotalFuelUsed = Decimal.Round(a_averageConsumption.TotalFuelUsed, 2),
                TotalKM = Decimal.Round(a_averageConsumption.TotalKM, 2),
                AverageSpeed = decimal.Round(((a_averageConsumption.AverageSpeed * 36) / 10), 2),
                FuelConsumptionList = a_averageConsumption.FuelConsumptionList,
                DateFuelConsumption = a_averageConsumption.DateFuelConsumption,
                TotalRegen = decimal.Round(a_averageConsumption.TotalRegen,2),
                EVConsumptionList = a_averageConsumption.EVConsumptionList,
                DateEVConsumption = a_averageConsumption.DateEVConsumption,
            };
        }
    }
}
