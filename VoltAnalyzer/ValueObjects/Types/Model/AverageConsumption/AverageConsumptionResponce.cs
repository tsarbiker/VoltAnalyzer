using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoltAnalyzer.ValueObjects.Types.Model
{
    public class AverageConsumptionResponce
    {
        public AverageConsumptionResponce()
        {

        }

        public decimal FuelConsumption { get; set; }
        public decimal EVConsumption { get; set; }
        public decimal WholeConsumption { get; set; }
        public decimal WorstFuelConsumption { get; set; }
        public decimal BestFuelConsumption { get; set; }
        public decimal WorstEVConsumption { get; set; }
        public decimal BestEVConsumption { get; set; }
        public decimal AverageEVKM { get; set; }

        public decimal TotalRegen { get; set; }

        public decimal TotalKM { get; set; }
        public decimal TotalCharging { get; set; }
        public decimal TotalFuelUsed { get; set; }

        public decimal AverageSpeed { get; set; }

        public List<decimal> FuelConsumptionList { get; set; }
        public List<decimal> EVConsumptionList { get; set; }
        public List<string> DateFuelConsumption { get; set; }
        public List<string> DateEVConsumption { get; set; }
    }
}
