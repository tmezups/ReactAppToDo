#!/bin/bash
/opt/mssql/bin/sqlservr &
pid=$!

until /opt/mssql-tools18/bin/sqlcmd -U sa -P 'Password1234!' -C -Q "SELECT 1;"; do
  >&2 echo "SQL Server is unavailable - sleeping"
  sleep 1
done

echo "Initializing database"
/opt/mssql-tools18/bin/sqlcmd -U sa -P 'Password1234!' -C -l 30 -i /scripts/ToDo.sql

wait $pid
