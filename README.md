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
wget 'https://public.boxcloud.com/d/1/b1!hhQ1MT3fnKstfvZBWVvvkHFhTpV7mkLH2QahJsn_yai2r57fxzZ2l09QoqPwJFlrS87VeaL8tm828iB-eHdgUzK68BZ9D24iDMWzPMKvy1CSld6cRdViKriLwbujJQl0-yrgde6HVpjkpxc4-4xiOWF-XEVNyEac8nUpMhEp1surgk64smNRDMJ2_4rQV57QMIVbAVDJcuarYSZHt9HJ3ZKnxYcB2iOcf6a1yng_bN2XZt0XyaboAbSWzWifPgRTEpJ06J3HFPAOxKX8QJqmkaj-gMzLEEZsvIR52WDPoN7c9Nvy62S-_8eJqMN2fuIikxL-WBLIgV5V7a7iZIsoS57v8HhM2PgwAD_-Evs3TYGNlHrElzFg7Hz-dMdO9xnZfMA8fNlK3wD0IcK4ixGdvZVjSoOxZKccfHeOeTmBJjxPR-kLWJqbgKemHKIyzxxvk5NWysh4OkIH3SoLtp8bpdcyWidF1yibvOfkDL6OBrR-0Yrz7GYgs1oGtDBXu000s6r3y8TaJ6EaoNDTgH6EieKewzYwisRCP4RMfkLbBrQer9pBjBdB02ygw713w-MkpQgrh-3BJ-uDYECSxngkx3-IFqe8m-l46jXOtPjPCCL_6u5uaiD7HZlrftbY6elFVtiCJR6JnFHtyA3wVk8LUWwptfy9bF8Ip13qxmRnn1CHNVGYzrlGFOav-O9WiUFyTvg82gvDRhEM5de7ktmMthMuukE-HQJzHPHphFd_WdfRPYCmobReubx7h0vK0MuMStwiYBc679vODR5FkVOrZsoYhLX1sjdHVJF6wZhlSEayfTOWp17-6PXSm0ql0LbGB-Oo4xZVkE3cgnVV0y1Xadhr1jCOPwYuKeBX3w-We0nvdBqcH_kiP2gAnLt_nueWaLe9sMi52rPPEcxEj8Ma6ZknpJBhE0WnwSmcdc_ihfvhpQWaAFniAG8rOTeQkNWRaHuW8swb18m1sUbJVaGP6eHLG9uhwJmFUyesjN7CNlbw09wK-E874ceQoIkj5X6INl8zBvvoEX7Fbl6PX4I3pU1ag3pUdOLww4kFcNIvoyE6ztBQHW-FB1OiJSslpoVu_xqYbhC5PWN3Ve7eDlTs_RBozikoZmzw7vLo3JRlWpwX5K0wLwRQsqsdcAPBRfiq8pHoPMYHvRd-AtHAoJOpAjMGLgYi_e7RU7BZRxOtdTEb0txQ7Xi8IHJgMXkC96RkEp46xLs3ThNRpqOKy6qAV008BOdnXPTDTWl7ys12Ukqy-N6GZ1yuq4Hm3mXnBfvnmWO2hOiQVE9dDQlVGb-QdBmrAgJ5FpxB5k_cSETnQC6Wlow./download'
```
- If the command doesn't work download from this [link](https://osu.app.box.com/v/traffic-events-dec19)

- Extract the dataset
```
tar xf download
```
- Rename the extracted dataset
```
mv TrafficWeatherEvent_Aug16_June19_Publish.csv traffic-data.csv
```

## Setting up hadoop
- Download hadoop in `~/bin` from this [link](https://www.apache.org/dyn/closer.cgi/hadoop/common/hadoop-3.1.3/hadoop-3.1.3-src.tar.gz) and extract it
```
cd ~/bin
wget https://www-eu.apache.org/dist/hadoop/common/hadoop-3.1.3/hadoop-3.1.3.tar.gz
tar xf hadoop-3.1.3.tar.gz
```

- Install pre-reqs for hadoop as said on this link [link](https://hadoop.apache.org/docs/stable/hadoop-project-dist/hadoop-common/SingleCluster.html)
```
sudo apt-get install ssh
sudo apt-get install pdsh
```
- Do the configuration for pseudo-distributed operation from this [link](https://hadoop.apache.org/docs/stable/hadoop-project-dist/hadoop-common/SingleCluster.html#Pseudo-Distributed_Operation)

~/bin/hadoop-3.1.3/etc/hadoop/core-site.xml:
```
<configuration>
    <property>
        <name>fs.defaultFS</name>
        <value>hdfs://localhost:9000</value>
    </property>
</configuration>
```

~/bin/hadoop-3.1.3/etc/hadoop/hdfs-site.xml:
```
<configuration>
    <property>
        <name>dfs.replication</name>
        <value>1</value>
    </property>
</configuration>
```

- Setup passwordless shh from this [link](https://hadoop.apache.org/docs/stable/hadoop-project-dist/hadoop-common/SingleCluster.html#Pseudo-Distributed_Operation)

Check that you can ssh to the localhost without a passphrase:
```
ssh localhost
```

If you cannot ssh to localhost without a passphrase, execute the following commands:
```
ssh-keygen -t rsa -P '' -f ~/.ssh/id_rsa
cat ~/.ssh/id_rsa.pub >> ~/.ssh/authorized_keys
chmod 0600 ~/.ssh/authorized_keys
```
- Setup env variables in `~/bin/hadoop-3.1.3/etc/hadoop/hadoop-env.sh` at the end of the file:
```
export JAVA_HOME=<your_java_home; hint:/usr/lib/jvm/java-8-openjdk-amd64/>
export PDSH_RCMD_TYPE=ssh
export HDFS_NAMENODE_USER=<current_root_user>
export HDFS_DATANODE_USER=<current_root_user>
export HDFS_SECONDARYNAMENODE_USER=<current_root_user>
export YARN_RESOURCEMANAGER_USER=<current_root_user>
export YARN_NODEMANAGER_USER=<current_root_user>
```
- Pre-start commands
```
sudo ~/bin/hadoop-3.1.3/bin/hdfs namenode -format
sudo ~/bin/hadoop-3.1.3/sbin/start-dfs.sh
sudo ~/bin/hadoop-3.1.3/bin/hdfs dfs -mkdir /user
sudo ~/bin/hadoop-3.1.3/bin/hdfs dfs -mkdir /user/root
sudo ~/bin/hadoop-3.1.3/bin/hdfs dfs -mkdir /user/root/big-data
```

- Put file on hadoop
```
sudo ~/bin/hadoop-3.1.3/bin/hdfs dfs -put <csv_file_path>/traffic-data.csv /user/root/big-data/
```

- List hdfs
```
sudo ~/bin/hadoop-3.1.3/bin/hadoop fs -ls hdfs://localhost:9000
```


## Pre-reqs

Hadoop, Spark and spark worker will be installed in `~/bin/`
To build and run this app locally you will need a few things:
- Install [.NET Core](https://dotnet.microsoft.com/learn/data/spark-tutorial/install-dotnet)
- Install [Java](https://dotnet.microsoft.com/learn/data/spark-tutorial/install-pre-reqs)
- Install [Hadoop(3.1.3)](https://hadoop.apache.org/docs/stable/hadoop-project-dist/hadoop-common/SingleCluster.html)
- Install [Spark(2.4.1) with hadoop(2.7)](https://dotnet.microsoft.com/learn/data/spark-tutorial/install-spark)
- Install [Microsoft Spark worker(0.7.0)](https://dotnet.microsoft.com/learn/data/spark-tutorial/install-worker)

## Running the project

```
export DOTNET_WORKER_DIR="/home/julije/bin/Microsoft.Spark.Worker-0.7.0/"

dotnet build

cp bin/Debug/netcoreapp2.2/SparkClient.dll /home/julije/bin/Microsoft.Spark.Worker-0.7.0/

~/bin/spark-2.4.1-bin-hadoop2.7/bin/spark-submit --class org.apache.spark.deploy.dotnet.DotnetRunner --master local bin/Debug/netcoreapp2.2/microsoft-spark-2.4.x-0.7.0.jar dotnet bin/Debug/netcoreapp2.2/SparkClient.dll
```

- Running on all CPU cores
```
~/bin/spark-2.4.1-bin-hadoop2.7/bin/spark-submit --class org.apache.spark.deploy.dotnet.DotnetRunner --master local[*] bin/Debug/netcoreapp2.2/microsoft-spark-2.4.x-0.7.0.jar dotnet bin/Debug/netcoreapp2.2/SparkClient.dll
```
