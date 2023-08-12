create database terk;
go
use terk
go

-- init schema with tables

create table dbo.product
(
    id   int          not null identity,
    name nvarchar(30) not null,
    cost float        not null,

    constraint pk_product primary key (id),
    constraint uq_product_name unique (name),
    constraint ch_product_cost check (cost > 0),
);
go

create table dbo.[user]
(
    id    int          not null identity,
    name  nvarchar(12) not null,
    login varchar(20)  not null,

    constraint pk_user primary key (id),
    constraint uq_user_login unique (login),
);
go

create table dbo.[order]
(
    id           int identity,
    customer_id  int       not null,
    created_date datetime2 not null
        constraint df_order_created_date default getdate(),
    total_cost   money     not null,

    constraint pk_order primary key (id),
    constraint ch_order_total_cost check (total_cost > 0),
    constraint fk_order_customer foreign key (customer_id) references [user] (id),
);
go

create table dbo.order_position
(
    id            int identity,
    order_id      int     not null,
    product_id    int     not null,
    product_count tinyint not null,
    cost          money   not null,

    constraint pk_order_position primary key (id),
    constraint ch_order_position_cost check (cost > 0),
    constraint fk_order_position_order foreign key (order_id) references [order] (id),
    constraint fk_order_position_product foreign key (product_id) references product (id),
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