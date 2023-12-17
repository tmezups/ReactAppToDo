-- create database Todo;
-- go

create table Todo(
     TodoId uniqueidentifier primary key,
     Title varchar(200) not null,
     IsDone bit not null default 0,
     CreatedOn datetime2 not null default getdate(),
     UpdatedOn datetime2 not null default getdate()
)
Go

create table UserAccount(
 UserAccountId uniqueidentifier primary key,
 UserName varchar(100) not null,
 Password varchar(100) not null,
 CreatedOn datetime2 not null default getdate()
)
Go
    
