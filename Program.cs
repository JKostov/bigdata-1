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

            ShowTrafficEventTypesCountByCityAndTimeSpan(dataFrame, city, startTime, endTime, greaterThan);
            //GetCountriesSortedByMostSevereWinter(dataFrame, year);
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
    }
}
