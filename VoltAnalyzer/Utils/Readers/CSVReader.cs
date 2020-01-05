using CsvHelper;
using LanguageManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoltAnalyzer.ValueObjects.Types.Common;

namespace TorqueAnalyzer.Utils
{
    public static class CSVReader
    {
        public static List<DataTable> allCSV;
        public static int amountFile = 1;

        public static string ReadFilesInDirectory(String directory)
        {
            allCSV = new List<DataTable>();

            if (Directory.Exists(directory))
            {
                Parallel.ForEach(Directory.EnumerateFiles(directory, "*.csv").ToList(), (source) =>
                {
                    allCSV.Add(ReadCSVFile(source));
                });

                allCSV = allCSV.OrderBy(x => x.ExtendedProperties["Datetime"]).ToList();
            }
            else
            {
                return ManageLanguage.GetLanguageString("Home$FolderDoesntExist", ManageLanguage.LanguageSelected);
            }

            return "";
        }

        private static DataTable ReadCSVFile(String filePath)
        {
            DataTable dataTable = new DataTable();

            // Check if file exists.
            if (File.Exists(filePath))
            {
                // Read column headers from file
                CsvReader csv = new CsvReader(File.OpenText(filePath));
                csv.Read();
                csv.ReadHeader();
                csv.Configuration.MissingFieldFound = null;

                dataTable.TableName = filePath.Split('\\')[filePath.Split('\\').Length-1];

                String tryparse = dataTable.TableName.Replace("trackLog-", "").Replace(".csv", "");
                dataTable.ExtendedProperties.Add("Datetime", DateTime.ParseExact(tryparse, "yyyy-MMM-dd_HH-mm-ss", CultureInfo.InvariantCulture));

                List<string> headers = csv.Context.HeaderRecord.ToList();

                // Read csv into datatable
                foreach (string header in headers)
                {
                    dataTable.Columns.Add(new System.Data.DataColumn(header));
                }

                // Import csv
                while (csv.Read())
                {
                    System.Data.DataRow row = dataTable.NewRow();

                    foreach (System.Data.DataColumn column in dataTable.Columns)
                    {
                        row[column.ColumnName] = csv.GetField(column.DataType, column.ColumnName);
                    }

                    dataTable.Rows.Add(row);
                }

                Trace.Write(amountFile.ToString() + " File " + dataTable.TableName + " loaded into datatable" + Environment.NewLine);
                amountFile += 1;

                VoltAnalyzerMessage.Send<string>(MessageConstants.SetBusyMessage, ManageLanguage.GetLanguageString("Home$BusyMessage", ManageLanguage.LanguageSelected).Replace("XX", amountFile.ToString()));
            }

            return dataTable;
        }
    }
}
