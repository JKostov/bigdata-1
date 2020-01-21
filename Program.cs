using System;
using Microsoft.Spark.Sql;
using static Microsoft.Spark.Sql.Functions;
using Column = Microsoft.Spark.Sql.Column;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace SparkClient
{
    class Program
    {
        static Dictionary<string, int> dictionary = InitDictionary();
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

            ShowTrafficEventTypesCountByCityAndTimeSpan(dataFrame);

            
            ShowDistanceOfRoadAffectedPerTypeData(dataFrame);
            
            ShowCityWithTheMaxAndMinNumberOfAccidentsInPeriod(dataFrame);

            ShowAverageCongestionDurationPerCityInPeriod(dataFrame);
        }

        static void ShowTrafficEventTypesCountByCityAndTimeSpan(DataFrame dataFrame)
        {
            string city = "Salt Lake City";
            string startTime = "2017-00-00 00:00:00";
            string endTime = "2018-00-00 00:00:00";
            int greaterThan = 500;

            dataFrame.Filter(
                    Col("Source").EqualTo("T")
                    .And(Col("City").EqualTo(city))
                    .And(Col("StartTime(UTC)").Gt(Lit(startTime))
                    .And(Col("EndTime(UTC)").Lt(Lit(endTime))
                )))
                .GroupBy("Type")
                .Agg(Count(dataFrame["Type"]))
                .Filter(Count(dataFrame["Type"]).Gt(greaterThan))
                .Show()
            ;
        }

        static void ShowDistanceOfRoadAffectedPerTypeData(DataFrame dataFrame)
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
                .Show()
            ;
        }

        static void ShowCityWithTheMaxAndMinNumberOfAccidentsInPeriod(DataFrame dataFrame)
        {
            string startDate = "2010-01-01 00:00:00";
            string endDate = "2020-01-01 00:00:00";
            string eventType = "Accident";

            dataFrame.Filter(
                    Col("Source").EqualTo("T")
                    .And(Col("Type").EqualTo(eventType))
                    .And(Col("StartTime(UTC)").Between(startDate, endDate))
                    .And(Col("EndTime(UTC)").Between(startDate, endDate))
                )
                .GroupBy("City")
                .Agg(Count("City"))
                .Agg(Min("City"), Max("City"))
                .Show()
            ;
        }

        static void ShowAverageCongestionDurationPerCityInPeriod(DataFrame dataFrame)
        {
            string startDate = "2010-01-01 00:00:00";
            string endDate = "2020-01-01 00:00:00";
            string eventType = "Congestion";

            Func<Column, Column> extractMinutesFromDescription = Udf<string, int>(descriptionString => {
                var asd = Regex.Match(descriptionString, "(\\w*) (minutes|minute)");
                var numberString = asd.Groups[1].Value;
                return ConvertToNumber(numberString);
            });

            dataFrame.Filter(
                    Col("Source").EqualTo("T")
                    .And(Col("Type").EqualTo(eventType))
                    .And(Col("StartTime(UTC)").Between(startDate, endDate))
                    .And(Col("EndTime(UTC)").Between(startDate, endDate))
                )
                .GroupBy("City")
                .Agg(Avg(extractMinutesFromDescription(Col("Description"))))
                .Show()
            ;
        }

        static int ConvertToNumber(string numberString)
        {
            if (Int32.TryParse(numberString, out int num))
            {
                return num;
            }

            var split = numberString.Replace('-', ' ').Split(" ");
            if (split.Length > 2)
            {
                return 0;
            }

            if (split.Length == 2)
            {
                if (!dictionary.TryGetValue(split[0], out int num1))
                {
                    return 0;
                }

                if (!dictionary.TryGetValue(split[1], out int num2))
                {
                    return 0;
                }

                return num1 + num2;
            }

            if (!dictionary.TryGetValue(split[0], out int number))
            {
                return 0;
            }

            return number;
        }

        static Dictionary<string, int> InitDictionary()
        {
            var dic = new Dictionary<string, int>();
            dic.Add("one", 1);
            dic.Add("two", 2);
            dic.Add("three", 3);
            dic.Add("four", 4);
            dic.Add("five", 5);
            dic.Add("six", 6);
            dic.Add("seven", 7);
            dic.Add("eight", 8);
            dic.Add("nine", 9);
            dic.Add("ten", 10);
            dic.Add("eleven", 11);
            dic.Add("twelve", 12);
            dic.Add("thirteen", 13);
            dic.Add("fourteen", 14);
            dic.Add("fifteen", 15);
            dic.Add("sixteen", 16);
            dic.Add("seventeen", 17);
            dic.Add("eighteen", 18);
            dic.Add("nineteen", 19);
            dic.Add("twenty", 20);
            dic.Add("thirty", 30);
            dic.Add("forty", 40);
            dic.Add("fifty", 50);
            dic.Add("sixty", 60);
            dic.Add("seventy", 70);
            dic.Add("eighty", 80);
            dic.Add("ninety", 90);

            return dic;
        }
    }
}
