# bigdata-1

## To run the project first do this [restore dataset](https://github.com/JKostov/bigdata-1/blob/master/data-to-process/data.md)

## Spark
Spark installed in `~/bin`
Spark version 2.4.1 with hadoop 2.7

## Microsoft spark worker 
Worker installed in `~/bin`
Worker version 0.7.0

## Run project command
```
~/bin/spark-2.4.1-bin-hadoop2.7/bin/spark-submit --class org.apache.spark.deploy.dotnet.DotnetRunner --master local bin/Debug/netcoreapp2.2/microsoft-spark-2.4.x-0.7.0.jar dotnet bin/Debug/netcoreapp2.2/SparkClient.dll
```
