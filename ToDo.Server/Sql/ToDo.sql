create database Todo;
go


use Todo;
create table Todo(
     Id uniqueidentifier primary key,
     Title varchar(200) not null,
     IsDone bit not null default 0,
     CreatedOn datetime2 not null default getdate(),
     UpdatedOn datetime2 not null default getdate()
)
Go

insert into Todo (Id, Title) values (newid(), 'Learn F#')
insert into Todo (Id, Title) values (newid(), 'Learn C#')
insert into Todo (Id, Title) values (newid(), 'Learn typescript')

GO