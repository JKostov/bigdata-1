using System;
using Microsoft.Spark.Sql;
using static Microsoft.Spark.Sql.Functions;
using Column = Microsoft.Spark.Sql.Column;

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

            ShowTrafficData(dataFrame);
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
    }
}
