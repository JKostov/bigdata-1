dotnet build
export DOTNET_WORKER_DIR="/home/julije/bin/Microsoft.Spark.Worker-0.7.0/"
cp bin/Debug/netcoreapp2.2/SparkClient.dll /home/julije/bin/Microsoft.Spark.Worker-0.7.0/
~/bin/spark-2.4.1-bin-hadoop2.7/bin/spark-submit --class org.apache.spark.deploy.dotnet.DotnetRunner --master local[*] bin/Debug/netcoreapp2.2/microsoft-spark-2.4.x-0.7.0.jar dotnet bin/Debug/netcoreapp2.2/SparkClient.dll