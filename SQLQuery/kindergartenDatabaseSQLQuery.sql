create database kindergarten;

go

use kindergarten;

go


create table grade(
	id int identity(1, 1) not null primary key,
	name nvarchar(100) not null
);


create table condition(
	id int identity(1, 1) primary key not null,
	name nvarchar(1000) not null
);

go

create table parent(
	id int identity(1, 1) primary key not null,
	FatherName nvarchar(1000) not null,
	Mothername nvarchar(1000) not null,
	address nvarchar(1000) not null,
	phonenumber nvarchar(30) not null
	);    
go

create table class(
	id int identity(1, 1) not null primary key,
	id_grade int foreign key references grade(id) not null,
	name nvarchar(1000) not null

);

go

create table children(
	id int identity(1, 1) primary key not null,
	name nvarchar(1000) not null,
	nickname nvarchar(1000),
	birthdate datetime not null,
	enrolldate datetime not null,
	sex bit not null,
	id_condition int foreign key references condition(id),
	id_parent int foreign key references parent(id) not null,
	id_class int foreign key references class(id)
);

go


create table report(
	id int identity(1, 1) not null primary key,
	generateDate datetime not null,
	id_class int not null foreign key references class(id)
)

go 

create table regulation(
	id int identity(1, 1) not null primary key,
	content nvarchar not null,
	ValueReg int not null
)