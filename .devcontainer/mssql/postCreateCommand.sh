#!/bin/bash
dacpac="false"
sqlfiles="false"
dacpath=$1
sqlpath=$2

echo "SELECT * FROM SYS.DATABASES" | dd of=testsqlconnection.sql
for i in {1..60};
do
    sqlcmd -S localhost -U sa -d master -i testsqlconnection.sql > /dev/null
    if [ $? -eq 0 ]
    then
        echo "SQL server ready"
        break
    else
        echo "Not ready yet..."
        sleep 1
    fi
done
rm testsqlconnection.sql

for f in $dacpath/*
do
    if [ $f == $dacpath/*".dacpac" ]
    then
        dacpac="true"
        echo "Found dacpac $f"
    fi
done

for f in $sqlpath/*
do
    if [ $f == $sqlpath/*".sql" ]
    then
        sqlfiles="true"
        echo "Found SQL file $f"
    fi
done

if [ $sqlfiles == "true" ]
then
    for f in $sqlpath/*
    do
        if [ $f == $sqlpath/*".sql" ]
        then
            echo "Executing $f"
            sqlcmd -S localhost -U sa -d master -i $f
        fi
    done
fi

if [ $dacpac == "true" ] 
then
    for f in $dacpath/*
    do
        if [ $f == $dacpath/*".dacpac" ]
        then
            dbname=$(basename $f ".dacpac")
            echo "Deploying dacpac $f"
            sqlpackage /Action:Publish /SourceFile:$f /TargetServerName:localhost /TargetDatabaseName:$dbname /TargetUser:sa /TargetPassword:P@ssw0rd
        fi
    done
fi

# Generate a dev cert
dotnet dev-certs https