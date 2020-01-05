using System;
using System.Linq;
using VoltAnalyzer.Model.DataServices.Torque;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using TorqueAnalyzer.Utils;
using System.Globalization;
using LanguageManager;
using VoltAnalyzer.ValueObjects.Types.Common;
using VoltAnalyzer.Helpers.MVVMHelpers;
using VoltAnalyzer.ValueObjects.Types.Model;
using System.Collections.Generic;
using System.Configuration;

namespace VoltAnalyzer.Model.DataServices.SmoothDrive
{
    internal class TorqueData : AbstractTorqueData
    {
        #region "Properties"

        private Boolean DataLoaded;

        private decimal TotalkwUsed = 0;
        private decimal TotalEVKM = 0;

        private decimal PartialEVKM = 0;
        private decimal PartialkwUsed = 0;
        private decimal PartialSOCAtBeginningDrive = 0;

        private decimal TotalFuel = 0;
        private decimal TotalFuelKM = 0;
        private decimal PartialFuelKM = 0;
        private decimal PartialFuelUsed = 0;
        private string PartialFuelDate = "";
        private string PartialEVDate = "";

        private decimal WorstFuelConsumption = 0;
        private decimal BestFuelConsumption = 100;

        private decimal WorstEVConsumption = 0;
        private decimal BestEVConsumption = 100;

        private decimal TotalCharging = 0;
        private decimal PreviousCharging = 0;
        private decimal PreviousChargingOriginal = 0;
        private DateTime PreviousFileDate;

        private decimal AverageEVKM = 0;
        private decimal ValidAmountEVKM = 0;

        private decimal PartialAverageSpeed = 0;
        private int PartialAmountValues = 0;
        private decimal AverageSpeed = 0;
        private int AmountValuesSpeed = 0;

        private decimal TotalRegen = 0;

        public List<decimal> FuelConsumptionList = new List<decimal>();
        public List<decimal> EVConsumptionList = new List<decimal>();
        public List<string> DateEVConsumption = new List<string>();
        public List<string> DateFuelConsumption = new List<string>();

        private decimal m_batterySize = decimal.Parse((16 + (1 / 2)).ToString());
        private decimal m_FullCharge = 13000;

        private Boolean GroupByDay = false;

        #endregion

        #region "public functions"

        public override void LoadHomeData(LoadHomeDataCallBack a_processResult, string a_directory)
        {
            string s_message = "";

            GetSettingGroupAverageConsumption();

            if (DataLoaded == false)
            {
                s_message = LoadData(a_directory);

                if (string.IsNullOrEmpty(s_message))
                {
                    DataLoaded = true;
                }
            }
            else
            {
                ResetAllProperties();
            }

            if (string.IsNullOrEmpty(s_message))
            {
                VoltAnalyzerMessage.Send<string>(MessageConstants.SetBusyMessage, ManageLanguage.GetLanguageString("Home$ProcessingBusyMessage", ManageLanguage.LanguageSelected));

                AverageConsumptionResponce avr_result = AverrageConsumption();

                a_processResult(true, "", avr_result.ModelToViewModel());
            }
            else
            {
                a_processResult(false, s_message, null);
            }
        }

        #endregion

        #region "private functions"

        private void GetSettingGroupAverageConsumption()
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains("FuelConsumptionAverageByDay"))
            {
                String s_value = ConfigurationManager.AppSettings.GetValues("FuelConsumptionAverageByDay")[0].ToString();

                if (s_value.ToLower() == "true")
                {
                    GroupByDay = true;
                }
            }

            if (ConfigurationManager.AppSettings.AllKeys.Contains("BatterySize"))
            {
                m_batterySize = decimal.Parse(ConfigurationManager.AppSettings.GetValues("BatterySize")[0].ToString(), CultureInfo.InvariantCulture);
            }

            if (ConfigurationManager.AppSettings.AllKeys.Contains("FullCharge"))
            {
                m_FullCharge = decimal.Parse(ConfigurationManager.AppSettings.GetValues("FullCharge")[0].ToString(), CultureInfo.InvariantCulture);
            }
        }

        private void ResetAllProperties()
        {
            TotalkwUsed = 0;
            TotalEVKM = 0;

            PartialEVKM = 0;
            PartialkwUsed = 0;
            PartialSOCAtBeginningDrive = 0;
            PartialEVDate = "";

            TotalFuel = 0;
            TotalFuelKM = 0;
            PartialFuelKM = 0;
            PartialFuelUsed = 0;
            PartialFuelDate = "";

            WorstFuelConsumption = 0;
            BestFuelConsumption = 100;

            WorstEVConsumption = 0;
            BestEVConsumption = 100;

            TotalCharging = 0;
            PreviousCharging = 0;
            PreviousChargingOriginal = 0;

            AverageEVKM = 0;
            ValidAmountEVKM = 0;

            PartialAverageSpeed = 0;
            PartialAmountValues = 0;
            AverageSpeed = 0;

            TotalRegen = 0;

            FuelConsumptionList = new List<decimal>();
            EVConsumptionList = new List<decimal>();
            DateFuelConsumption = new List<string>();
            DateEVConsumption = new List<string>();
        }

        private string LoadData(string a_directory)
        {
            return CSVReader.ReadFilesInDirectory(a_directory);
        }

        private AverageConsumptionResponce AverrageConsumption()
        {
            BothConsumption();

            decimal d_EVAverrage = 0;

            if (TotalEVKM != 0)
            {
                d_EVAverrage = (TotalkwUsed / TotalEVKM) * 100;
            }

            decimal d_FuelAverrage = 0;

            if (TotalFuelKM != 0)
            {
                d_FuelAverrage = (TotalFuel / TotalFuelKM) * 100;
            }

            decimal d_TotalAverrage = 0;

            if ((TotalEVKM + TotalFuelKM) != 0)
            {
                d_TotalAverrage = (TotalFuel / (TotalEVKM + TotalFuelKM)) * 100;
            }

            if (TotalCharging / 1000 > 0)
            {
                TotalCharging = TotalCharging / 1000;
            }

            decimal TotalAverageEVKM = 0;

            if (ValidAmountEVKM != 0)
            {
                TotalAverageEVKM = AverageEVKM / ValidAmountEVKM;
            }

            decimal AverageS = 0;

            if (AmountValuesSpeed != 0)
            {
                AverageS = AverageSpeed / AmountValuesSpeed;
            }

            AverageConsumptionResponce acr_responce = new AverageConsumptionResponce();

            acr_responce.EVConsumptionList = EVConsumptionList;
            acr_responce.DateEVConsumption = DateEVConsumption;
            acr_responce.FuelConsumptionList = FuelConsumptionList;
            acr_responce.DateFuelConsumption= DateFuelConsumption;

            acr_responce.AverageSpeed = AverageS;
            acr_responce.EVConsumption = d_EVAverrage;
            acr_responce.FuelConsumption = d_FuelAverrage;
            acr_responce.WholeConsumption = d_TotalAverrage;
            acr_responce.WorstEVConsumption = WorstEVConsumption;
            acr_responce.BestEVConsumption = BestEVConsumption;
            acr_responce.WorstFuelConsumption = WorstFuelConsumption;
            acr_responce.BestFuelConsumption = BestFuelConsumption;
            acr_responce.TotalKM = TotalEVKM + TotalFuelKM;
            acr_responce.TotalCharging = TotalCharging;
            acr_responce.TotalFuelUsed = TotalFuel;
            acr_responce.AverageEVKM = TotalAverageEVKM;
            acr_responce.TotalRegen = TotalRegen;

            return acr_responce;
        }

        private void BothConsumption()
        {
            foreach (DataTable a_datatable in CSVReader.allCSV)
            {
                Trace.Write(Environment.NewLine + DateTime.Parse(a_datatable.ExtendedProperties["Datetime"].ToString()) + Environment.NewLine);
                EVConsumptionCalculation(a_datatable);
                FuelConsumptionCalculation(a_datatable);
                AverageSpeedTrip(a_datatable);
                Regeneration(a_datatable);
            }

            if (PartialFuelKM != 0)
            {
                decimal d_fuelConsumption = (PartialFuelUsed / PartialFuelKM) * 100;

                FuelConsumptionList.Add(d_fuelConsumption);
                DateFuelConsumption.Add(PartialFuelDate);

                Trace.Write("GroupByDay - Fuel:" + PartialFuelUsed.ToString() + " KM:" + PartialFuelKM.ToString() + " Consumption:" + d_fuelConsumption.ToString() + Environment.NewLine);

                PartialFuelKM = 0;
                PartialFuelUsed = 0;
                PartialFuelDate = "";
            }

            if (PartialEVKM != 0)
            {
                TotalkwUsed = TotalkwUsed + PartialkwUsed;
                TotalEVKM = TotalEVKM + PartialEVKM;

                decimal d_evConsumption = (PartialkwUsed / PartialEVKM) * 100;

                EVConsumptionList.Add(d_evConsumption);
                DateEVConsumption.Add(PartialEVDate);

                Trace.Write("EV:" + PartialkwUsed.ToString() + " KM:" + PartialEVKM.ToString() + " Consumption:" + d_evConsumption.ToString() + Environment.NewLine);

                if (d_evConsumption < BestEVConsumption)
                {
                    BestEVConsumption = d_evConsumption;
                }

                if (d_evConsumption > WorstEVConsumption)
                {
                    WorstEVConsumption = d_evConsumption;
                }

                AverageEVKM = AverageEVKM + PartialEVKM;
                ValidAmountEVKM += 1;

                PartialkwUsed = 0;
                PartialEVKM = 0;
                PartialSOCAtBeginningDrive = 0;
            }
        }

        #region "EV"

        private void EVConsumptionCalculation(DataTable a_datatable)
        {
            int indexLastEVRow = FindLastRowEVKM(a_datatable);
            int indexFirstEVRow = FindFirstRowEVKM(a_datatable);

            if (indexFirstEVRow != 0 && indexLastEVRow != 0)
            {
                if (indexLastEVRow > indexFirstEVRow)
                {
                    decimal d_startKW = GetValueFromColumnAtIndex(indexFirstEVRow, a_datatable, "!Hybrid Pack Remaining (SOC)(%)");

                    if (d_startKW == 0)
                    {
                        Boolean b_notFound = true;
                        while (b_notFound)
                        {
                            indexFirstEVRow += 1;
                            d_startKW = GetValueFromColumnAtIndex(indexFirstEVRow, a_datatable, "!Hybrid Pack Remaining (SOC)(%)");

                            if (d_startKW != 0)
                            {
                                b_notFound = false;
                            }
                            if (indexLastEVRow <= indexFirstEVRow)
                            {
                                b_notFound = false;
                            }
                        }
                    }

                    decimal SOCAtBeginningDrive = GetValueFromColumnAtIndex(indexFirstEVRow, a_datatable, "!Hybrid Pack Remaining (SOC)(%)");
                    decimal SOCAtEndDrive = GetValueFromColumnAtIndex(indexLastEVRow, a_datatable, "!Hybrid Pack Remaining (SOC)(%)");

                    decimal SOCUsed = SOCAtBeginningDrive - SOCAtEndDrive;

                    decimal kwUsed = SOCUsed * (m_batterySize / 100);
                    decimal evKM = GetValueFromColumnAtIndex(indexLastEVRow, a_datatable, "!EV km this Cycle(km)");
                    decimal lastCharge = FindCharge(a_datatable);

                    if (SOCUsed > 0)
                    {
                        if (evKM != 0)
                        {
                            //In some files the charge is 0 in all rows, if the SOC is over 70% consider thatthe charge should have a value of full charge
                            if (lastCharge == 0 && SOCAtBeginningDrive > 70)
                            {
                                lastCharge = m_FullCharge;
                                Trace.Write("FIX Charge - New: " + lastCharge.ToString() + " - The value from the file was 0." + Environment.NewLine);
                            }

                            if (PreviousChargingOriginal == lastCharge && DateTime.Parse(a_datatable.ExtendedProperties["Datetime"].ToString()).ToShortDateString() == PreviousFileDate.ToShortDateString())
                            {
                                Trace.Write("DUPLICATE Charge:" + lastCharge.ToString() + Environment.NewLine);

                                PartialEVKM = PartialEVKM + evKM;
                                PartialkwUsed = PartialkwUsed + kwUsed;
                            }
                            else
                            {
                                //Fix incorrect values over 18000wh
                                if (lastCharge > 18000)
                                {
                                    PreviousChargingOriginal = lastCharge;
                                    decimal _lastCharge = lastCharge;

                                    if (PreviousCharging > 9000)
                                    {
                                        lastCharge = lastCharge - m_FullCharge-1000;
                                    }
                                    else
                                    {
                                        lastCharge = lastCharge - PreviousCharging;
                                    }

                                    Trace.Write("FIX Charge - New:" + lastCharge.ToString() + " Last charge: " + _lastCharge.ToString() + " Previous charge:" + PreviousCharging.ToString() + Environment.NewLine);
                                }
                                else
                                {
                                    PreviousChargingOriginal = lastCharge;
                                }

                                if (PartialEVKM != 0)
                                {
                                    TotalkwUsed = TotalkwUsed + PartialkwUsed;
                                    TotalEVKM = TotalEVKM + PartialEVKM;

                                    decimal d_evConsumption = (PartialkwUsed / PartialEVKM) * 100;

                                    EVConsumptionList.Add(d_evConsumption);
                                    DateEVConsumption.Add(a_datatable.ExtendedProperties["Datetime"].ToString().Split(' ')[0]);

                                    Trace.Write("EV:" + PartialkwUsed.ToString() + " KM:" + PartialEVKM.ToString() + " Consumption:" + d_evConsumption.ToString() + Environment.NewLine);

                                    if (d_evConsumption < BestEVConsumption)
                                    {
                                        BestEVConsumption = d_evConsumption;
                                    }

                                    if (d_evConsumption > WorstEVConsumption)
                                    {
                                        WorstEVConsumption = d_evConsumption;
                                    }

                                    if (PartialSOCAtBeginningDrive > 80 && SOCAtEndDrive < 35)
                                    {
                                        AverageEVKM = AverageEVKM + PartialEVKM;
                                        ValidAmountEVKM += 1;
                                        Trace.Write("EV KM :" + PartialEVKM.ToString() + " has been used in calculation." + Environment.NewLine);
                                    }

                                    PartialkwUsed = 0;
                                    PartialEVKM = 0;
                                    PartialSOCAtBeginningDrive = 0;
                                }

                                TotalCharging = TotalCharging + lastCharge;
                                PreviousCharging = lastCharge;
                                PreviousFileDate = DateTime.Parse(a_datatable.ExtendedProperties["Datetime"].ToString());
                                Trace.Write("Charge:" + lastCharge.ToString() + Environment.NewLine);

                                PartialEVKM = PartialEVKM + evKM;
                                PartialkwUsed = PartialkwUsed + kwUsed;
                                PartialSOCAtBeginningDrive = SOCAtBeginningDrive;
                            }
                        }
                    }
                }
            }
        }

        private decimal FindCharge(DataTable a_record)
        {
            if (a_record.Columns.Contains("!Last Charge AC Wh(Wh)"))
            {
                for (int i = 0; i < a_record.Rows.Count - 1; i++)
                {
                    decimal value = ConvertValueToDecimal(a_record.Rows[i]["!Last Charge AC Wh(Wh)"].ToString());

                    if (value != 0)
                    {
                        return value;
                    }
                }

                return 0;
            }

            return 0;
        }

        private int FindFirstRowEVKM(DataTable a_record)
        {
            if (a_record.Columns.Contains("!EV km this Cycle(km)"))
            {
                for (int i = 0; i < a_record.Rows.Count - 1; i++)
                {
                    decimal value = ConvertValueToDecimal(a_record.Rows[i]["!EV km this Cycle(km)"].ToString());

                    if (value != 0)
                    {
                        return i;
                    }
                }

                return a_record.Rows.Count - 1;
            }

            return 0;
        }

        private int FindLastRowEVKM(DataTable a_record)
        {
            if (a_record.Columns.Contains("!EV km this Cycle(km)"))
            {
                int lastRowEVKM = 0;

                decimal previousValue = 0;
                for (int i = 0; i < a_record.Rows.Count - 1; i++)
                {
                    decimal value = ConvertValueToDecimal(a_record.Rows[i]["!EV km this Cycle(km)"].ToString());

                    lastRowEVKM = i;

                    if (value >= previousValue)
                    {
                        previousValue = value;
                    }
                    else
                    {
                        lastRowEVKM -= 1;
                        break;
                    }
                }

                if (previousValue == 0)
                {
                    return 0;
                }
                else
                {
                    return lastRowEVKM;
                }
            }

            return 0;
        }

        #endregion

        #region "fuel"

        private void FuelConsumptionCalculation(DataTable a_datatable)
        {
            int indexFirstFuelRow = FindFirstRowtripKM(a_datatable);
            int indexLastFuelRow = FindLastRowtripKM(a_datatable);

            if (a_datatable.Columns.Contains("Fuel used (trip)(l)") && a_datatable.Columns.Contains("Trip Distance(km)") && a_datatable.Columns.Contains("!EV km this Cycle(km)"))
            {
                if (indexLastFuelRow != 0)
                {
                    decimal fuelKM = GetValueFromColumnAtIndex(indexLastFuelRow, a_datatable, "Trip Distance(km)") - GetValueFromColumnAtIndex(indexFirstFuelRow, a_datatable, "Trip Distance(km)");
                    decimal fuelUsed = ConvertValueToDecimal(a_datatable.Rows[indexLastFuelRow]["Fuel used (trip)(l)"].ToString());

                    if (fuelUsed != 0)
                    {
                        TotalFuel = TotalFuel + fuelUsed;
                        TotalFuelKM = TotalFuelKM + fuelKM;

                        decimal d_fuelConsumption = 0;

                        if (GroupByDay)
                        {
                            if (PartialFuelDate == a_datatable.ExtendedProperties["Datetime"].ToString().Split(' ')[0])
                            {
                                PartialFuelKM = PartialFuelKM + fuelKM;
                                PartialFuelUsed = PartialFuelUsed + fuelUsed;
                                PartialFuelDate = a_datatable.ExtendedProperties["Datetime"].ToString().Split(' ')[0];
                            }
                            else
                            {
                                if(PartialFuelKM != 0)
                                {
                                    d_fuelConsumption = (PartialFuelUsed / PartialFuelKM) * 100;

                                    FuelConsumptionList.Add(d_fuelConsumption);
                                    DateFuelConsumption.Add(PartialFuelDate);

                                    Trace.Write("GroupByDay - Fuel:" + PartialFuelUsed.ToString() + " KM:" + PartialFuelKM.ToString() + " Consumption:" + d_fuelConsumption.ToString() + Environment.NewLine);

                                    PartialFuelKM = 0;
                                    PartialFuelUsed = 0;
                                    PartialFuelDate = "";
                                }

                                PartialFuelKM = PartialFuelKM + fuelKM;
                                PartialFuelUsed = PartialFuelUsed + fuelUsed;
                                PartialFuelDate = a_datatable.ExtendedProperties["Datetime"].ToString().Split(' ')[0];
                            }
                        }
                        else
                        {
                            d_fuelConsumption = (fuelUsed / fuelKM) * 100;

                            FuelConsumptionList.Add(d_fuelConsumption);
                            DateFuelConsumption.Add(a_datatable.ExtendedProperties["Datetime"].ToString().Split(' ')[0]);

                            Trace.Write("Fuel:" + fuelUsed.ToString() + " KM:" + fuelKM.ToString() + " Consumption:" + d_fuelConsumption.ToString() + Environment.NewLine);
                        }

                        if (d_fuelConsumption < BestFuelConsumption && d_fuelConsumption != 0)
                        {
                            BestFuelConsumption = d_fuelConsumption;
                        }

                        if (d_fuelConsumption > WorstFuelConsumption)
                        {
                            WorstFuelConsumption = d_fuelConsumption;
                        }
                    }
                }
            }
        }

        private int FindFirstRowtripKM(DataTable a_record)
        {
            if (a_record.Columns.Contains("Fuel used (trip)(l)"))
            {
                for (int i = 1; i < a_record.Rows.Count - 1; i++)
                {
                    decimal value = ConvertValueToDecimal(a_record.Rows[i]["Fuel used (trip)(l)"].ToString());

                    if (value != 0)
                    {
                        return i;
                    }
                }

                return a_record.Rows.Count - 1;
            }

            return 0;
        }

        private int FindLastRowtripKM(DataTable a_record)
        {
            if (a_record.Columns.Contains("Fuel used (trip)(l)"))
            {
                int lastRowFuelUsed = 0;

                for (int i = 1; i < a_record.Rows.Count - 1; i++)
                {
                    decimal value = ConvertValueToDecimal(a_record.Rows[i]["Fuel used (trip)(l)"].ToString());

                    if (value != 0)
                    {
                        lastRowFuelUsed = i;
                    }
                }

                return lastRowFuelUsed;
            }

            return 0;
        }

        #endregion

        #region "Average speed"

        private void AverageSpeedTrip(DataTable a_datatable)
        {
            PartialAverageSpeed = 0;
            PartialAmountValues = 0;

            foreach (DataRow a_datarow in a_datatable.Rows)
            {
                decimal d_speed = GetValueFromColumn(a_datarow, "GPS Speed (Meters/second)");

                if (d_speed != 0)
                {
                    PartialAverageSpeed = PartialAverageSpeed + d_speed;
                    PartialAmountValues += 1;
                }
            }

            if (PartialAmountValues != 0)
            {
                AverageSpeed = AverageSpeed + (PartialAverageSpeed / PartialAmountValues);
                AmountValuesSpeed += 1;
            }
        }

        #endregion

        #region "Regen"

        private void Regeneration(DataTable a_datatable)
        {
            if (a_datatable.Columns.Contains("!! Inst. kPower(kW)"))
            {
                foreach (object a_value in a_datatable.AsEnumerable().Select(x => x.Field<object>("!! Inst. kPower(kW)")).ToList())
                {
                    if (a_value.ToString().Contains("-") && a_value.ToString() != "-")
                    {
                        decimal d_power = decimal.Parse(a_value.ToString().Replace("-",""), System.Globalization.NumberStyles.AllowDecimalPoint, new CultureInfo("en-GB"));
                        TotalRegen = TotalRegen + (d_power/3600);
                    }
                }
            }
        }

        #endregion

        #region "helpers"

        private decimal GetValueFromColumnAtIndex(int a_index, DataTable a_record, string a_column)
        {
            if (a_record.Columns.Contains(a_column))
            {
                return ConvertValueToDecimal(a_record.Rows[a_index][a_column].ToString());
            }

            return 0;
        }

        private decimal GetValueFromColumn(DataRow a_record, string a_column)
        {
            if (a_record.Table.Columns.Contains(a_column))
            {
                return ConvertValueToDecimal(a_record[a_column].ToString());
            }

            return 0;
        }

        private decimal ConvertValueToDecimal(string a_input)
        {
            if (string.IsNullOrEmpty(a_input) == false && a_input != "-" && Regex.Matches(a_input, @"[a-zA-Z]").Count == 0)
            {
                try
                {
                    return decimal.Parse(a_input, System.Globalization.NumberStyles.AllowDecimalPoint, new CultureInfo("en-GB"));
                }
                catch (Exception ex)
                {
                    Trace.Write("Could not convert following value:" + a_input + Environment.NewLine);
                }
            }

            return 0;
        }

        #endregion

        #endregion
    }
}
