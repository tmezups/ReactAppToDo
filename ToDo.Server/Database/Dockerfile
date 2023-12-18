FROM mcr.microsoft.com/mssql/server:2022-latest

COPY . /scripts/

#ENV MSSQL_SA_PASSWORD=Password1234!
#ENV SQLCMDPASSWORD=Password1234!
#RUN /opt/mssql-tools/bin/sqlcmd -Q Select 1;

ENTRYPOINT [ "sh", "./scripts/init.sh" ]


