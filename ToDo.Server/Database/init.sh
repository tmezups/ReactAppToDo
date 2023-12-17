#!/bin/bash
/opt/mssql/bin/sqlservr &
pid=$!

until /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Password1234! -Q "SELECT 1"; do
  >&2 echo "SQL Server is unavailable - sleeping"
  sleep 1
done

echo "Initializing database"
/opt/mssql-tools/bin/sqlcmd -U sa -P Password1234! -l 30 -i /scripts/ToDo.sql

wait $pid
  