using Microsoft.Spark.Sql;

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
            var dataFrame = Spark.Read().Csv(dataPath);
            dataFrame.PrintSchema();
            dataFrame.Show();
        }
    }
}
