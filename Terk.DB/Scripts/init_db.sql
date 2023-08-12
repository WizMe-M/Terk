create database terk;
go
use terk
go

-- init schema with tables

create table dbo.product
(
    id   int          not null identity primary key,
    name nvarchar(30) not null unique,
    cost float        not null check (cost > 0),
);
go

create table dbo.[user]
(
    id    int          not null identity primary key,
    name  nvarchar(12) not null,
    login varchar(20)  not null unique,
);
go

create table dbo.[order]
(
    id           int identity primary key,
    customer_id  int       not null references [user] (id),
    created_date datetime2 not null default getdate(),
    total_cost   money     not null check (total_cost > 0),
);
go

create table dbo.order_position
(
    id            int identity primary key,
    order_id      int     not null references [order] (id),
    product_id    int     not null references product (id),
    product_count tinyint not null,
    position_cost money   not null check (position_cost > 0),
);
go

-- fill tables with data

insert into [user] (name, login)
values (N'Олег', 'oleg123'),
       (N'Иван', 'iva_ivan');
go

insert into product (name, cost)
values (N'Гематоген', 40),
       (N'Бадяга', 125),
       (N'Лейкопластырь', 100);
go