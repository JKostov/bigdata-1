using System;
using System.Collections.Generic;
using Microsoft.Spark.Sql;
using static Microsoft.Spark.Sql.Functions;

namespace SparkClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataPath = "data-to-process/traffic-data.csv";
            var Spark = SparkSession
                           .Builder()
                           .GetOrCreate();

            var dataFrame = Spark
                .Read()
                .Option("header", true)
                .Csv(dataPath);

            var city = "Salt Lake City";
            var startTime = "2017-00-00 00:00:00";
            var endTime = "2018-00-00 00:00:00";
            var greaterThan = 500;

            var year = 2017;

            // ShowTrafficEventTypesCountByCityAndTimeSpan(dataFrame, city, startTime, endTime, greaterThan);
            //GetCountriesSortedByMostSevereWinter(dataFrame, year);

            // ShowTrafficData(dataFrame);
            ShowWeatherData(dataFrame);
        }

        static void GetCountriesSortedByMostSevereWinter(DataFrame dataFrame, int year)
        {
            var winterStart = $"{year}-12-22 00:00:00";
            var winterEnd = $"{year + 1}-3-20 00:00:00";

            dataFrame
               .Filter(
                   dataFrame["Source"].EqualTo("W")
                       .And(
                   dataFrame["Severity"].EqualTo("Severe")
                        .And(
                   dataFrame["Type"].EqualTo("Cold"))
                        .And(
                   dataFrame["StartTime(UTC)"].Gt(Lit(winterStart))
                        .And(
                        dataFrame["EndTime(UTC)"].Lt(Lit(winterEnd))
               ))))
               .GroupBy("State")
               .Agg(Count(dataFrame["State"]))
               .Sort(-Count(dataFrame["State"]))
               .Show(50);
        }

        static void ShowTrafficEventTypesCountByCityAndTimeSpan(
            DataFrame dataFrame, string city, string startTime, string endTime, int greaterThan)
        {
            dataFrame
                .Filter(
                    dataFrame["Source"].EqualTo("T")
                        .And(
                    dataFrame["City"].EqualTo(city)
                        .And(
                    dataFrame["StartTime(UTC)"].Gt(Lit(startTime))
                        .And(
                    dataFrame["EndTime(UTC)"].Lt(Lit(endTime)))
                )))
                .GroupBy("Type")
                .Agg(Count(dataFrame["Type"]))
                .Filter(Count(dataFrame["Type"]).Gt(greaterThan))
                .Show();
        }

        static void ShowTrafficData(DataFrame dataFrame)
        {
            string city = "San Francisco";
            string startDate = "2010-01-01 00:00:00";
            string endDate = "2020-01-01 00:00:00";

            dataFrame.Filter(
                    Col("Source").EqualTo("T")
                    .And(Col("City") == city)
                    .And(Col("StartTime(UTC)").Between(startDate, endDate))
                    .And(Col("EndTime(UTC)").Between(startDate, endDate))
                )
                .GroupBy("Type")
                .Agg(Min("Distance(mi)"), Max("Distance(mi)"), Avg("Distance(mi)"), Stddev("Distance(mi)"))
                .Show();
        }

        static void ShowWeatherData(DataFrame dataFrame)
        {
            string city = "San Francisco";
            string startDate = "2010-01-01 00:00:00";
            string endDate = "2020-01-01 00:00:00";
            
            Func<Column, Column> convertSeverity = Udf<string, int>(severityString => {
                switch (severityString) {
                    case "Light":
                        return 1;
                    case "Moderate":
                        return 2;
                    case "Severe":
                        return 3;
                    case "Heavy":
                        return 4;
                    default:
                        return 0;
                }
            });

            dataFrame.Filter(
                    Col("Source").EqualTo("W")
                    .And(Col("Severity").IsIn("Light", "Moderate", "Severe", "Heavy"))
                    .And(Col("City") == city)
                    .And(Col("StartTime(UTC)").Between(startDate, endDate))
                    .And(Col("EndTime(UTC)").Between(startDate, endDate))
                )
                .GroupBy("Type")
                .Agg(Min(convertSeverity(Col("Severity"))), Max(convertSeverity(Col("Severity"))), Avg(convertSeverity(Col("Severity"))), Stddev(convertSeverity(Col("Severity"))))
                // .Agg(Min(Col("Severity")), Max(Col("Severity")), Avg(Col("Severity")), Stddev(Col("Severity")))
                .Show();
        }
    }
}
