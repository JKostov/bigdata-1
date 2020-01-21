# bigdata-1

## Restore dataset

- Clone project
```
git clone git@github.com:JKostov/bigdata-1.git
```
- Create folder for the dataset
```
mkdir data-to-process
```
- Change directory
```
cd data-to-process
```
- Download the dataset 
```
wget 'https://public.boxcloud.com/d/1/b1!O55iIdf9lRey52-qx2UJFHfOKZ3lPTXx9Xg-q6iIpNDeAzUU399Jrl5hf97zlUQJ7F1rmg7vtLqKV681WTmBl0M_93yEwIc8wZ0VgeOZHGj7nbmJzEMN6YuRc9_mVU2f9ErD7YJwrf2BmyrRJ-CPgsI2xPihttjQUNgqzcB5RXUFpYf321QLycPwI4E7qubwesaj0Z-bSae9oVxEpmhgLhdzUKrNsB1EZIxytncYxbNPCrqyDkiqBsE91Co6HzF5SGjdqpyi8M5IthhpX-HvsLLGajmCrMW8R8KVnw_3wDybM_ewb7YAS_UjKcy5AKNLgydzOv6e1icmxboUVmATftrRoAzQ3biuC4_P_AM4X-IT-WnaiSfx7uIK7VUVTFHU4A-QWol1AsZ7GgTBnHjqiFv7P7w7X3OvL0nBH4nFoRzClcsxmuvTBW6RWpVVjdFyV1B8ohIkqQNn6Rbr1a3WZasZ_5Kyr2I8FWd3pk4deU4YEbjAwg2OVt_OkSQdp65z0mjcbYlYUQFYhFGYuFbxahIQxbYTSQn1r_lMmHsEuiI9sslxv762D2FvUPAkNtvGyW06jGLM4JN0Wk3I1kVniSVSm95RU1UtJVIsQMYTKnpr5J7HGsmGpDYKjoCoDoYhnTKD6nxzzVnkEKedh27LmFvfZFBQPelFwAkbNSLvscbuUhwg2lLVMLlq4VAcnfjlhhpEdEbYeZawylwAzMH480UmVYhAG8Cv_xK4RKPKERyJnyVtadXot4xvtvBPlOzYyMGze5oUf59PJimiRKYlxHa86nGIM5QRkT1LXhH6b_237zJ30LsnH07VQoa1g5Y9-1ONU0WIFkMPmVpTG_1YTmDSGIFYgAYBkZeDyu9Io6UbjSJ-S2_EG87ofWzew_-o2GCnK9q5Q6JUG14Y4JE4-uoRI7rzJPYY9ruKUMJeDT1Phaurgka102sqG6q5rASXS8jz9btsjS2AlOyvJTXxz0O8kAbYKQkDaa5Mo8lgytbAcua-Q-HBlsDDJspPetS2m6TD0-1Q0qgRUtTmEld0XjlgY2pkzVckWqv-HPj8yY3oNyP-d9yJ5_WpE2dhsRCn8v0ENubX67XgBuy9xXGU5yhxtboZs0vayUW3r5G-JffsbdYvJ3uwVb00qUKWqipEhKRuo526W4R-Nz7XEjPHbGgO2pdm0TPQ3OmkjyerVy9YA8mptTuQmTtZ4nvaoqmrDsulTOwh1lIRpnyGF-urtKBri0eovjTRYOpwvi7aXiSQK4E4oWghYmjybOPswSyHfCXWtxidRGt3AfoTolQ./download'
```
- Extract the dataset
```
tar xf download
```
- Rename the extracted dataset
```
mv TrafficWeatherEvent_Aug16_June19_Publish.csv traffic-data.csv
```

## Pre-reqs

To build and run this app locally you will need a few things:
- Intall [.NET Core](https://dotnet.microsoft.com/learn/data/spark-tutorial/install-dotnet)
- Install [Java](https://dotnet.microsoft.com/learn/data/spark-tutorial/install-pre-reqs)
- Install [Spark(2.4.1) with hadoop(2.7)](https://dotnet.microsoft.com/learn/data/spark-tutorial/install-spark)
- Install [Microsoft Spark worker(0.7.0)](https://dotnet.microsoft.com/learn/data/spark-tutorial/install-worker)

## Running the project

```
export DOTNET_WORKER_DIR="/home/julije/bin/Microsoft.Spark.Worker-0.7.0/"

dotnet build

cp bin/SparkClient.dll /home/julije/bin/Microsoft.Spark.Worker-0.7.0/

~/bin/spark-2.4.1-bin-hadoop2.7/bin/spark-submit --class org.apache.spark.deploy.dotnet.DotnetRunner --master local bin/Debug/netcoreapp2.2/microsoft-spark-2.4.x-0.7.0.jar dotnet bin/Debug/netcoreapp2.2/SparkClient.dll
```
