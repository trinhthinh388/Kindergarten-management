create database kindergarten;

go

use kindergarten;

go

create table teacher(
	id int identity(1, 1) primary key,
	name nvarchar(1000) not null,
)

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
	id_teacher int foreign key references teacher(id), 
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
	imageUrl nvarchar(1000),
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
	content nvarchar(1000) not null,
	ValueInt int not null,
	ValueStr nvarchar(1000)
)

go



create table users(
	id int identity(1,1) not null primary key,
	position int not null,
	id_teacher int not null foreign key references teacher(id),
	username nvarchar(1000) not null,
	password nvarchar(1000) not null
)
