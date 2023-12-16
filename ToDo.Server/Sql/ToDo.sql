create database Todo;
go


use Todo;
go;
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
    
    
insert into Todo (Id, Title) values (newid(), 'Learn F#')
insert into Todo (Id, Title) values (newid(), 'Learn C#')
insert into Todo (Id, Title) values (newid(), 'Learn typescript')

GO
    
    
    insert into UserAccount (UserAccountId, UserName, Password) 
    values (newid(), 'admin', 'admin')