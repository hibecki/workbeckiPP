/*==============================================================*/
/* Table: product_group                                         */
/*==============================================================*/
create table product_group (
   product_group_code   char(3)              not null,
   product_group_desc   nvarchar(100)        not null,
   x_status             char(1)              null,
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   rowversion           timestamp            null,
   constraint pk_product_group primary key (product_group_code)
)
go

/*==============================================================*/
/* Table: product_type                                          */
/*==============================================================*/
create table product_type (
   product_group_code   char(3)              not null,
   product_type_code    char(3)              not null,
   product_type_desc    nvarchar(100)        not null,
   x_status             char(1)              null,
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   rowversion           timestamp            null,
   constraint pk_product_type primary key (product_type_code, product_group_code)
)
go

alter table product_type
   add constraint fk_product_group foreign key (product_group_code)
      references product_group (product_group_code)
go

/*==============================================================*/
/* Table: product                                               */
/*==============================================================*/
create table product (
   product_code         char(3)              not null,
   product_type_code    char(3)              null,
   product_group_code   char(3)              null,
   product_desc         nvarchar(100)        not null,
   x_status             char(1)              null,
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   rowversion           timestamp            null,
   constraint pk_product primary key (product_code)
)
go

alter table product
   add constraint fk_prodt_type foreign key (product_type_code, product_group_code)
      references product_type (product_type_code, product_group_code)
go

/*==============================================================*/
/* Table: mem_product                                           */
/*==============================================================*/
create table mem_product (
   member_code          varchar(30)          not null,
   product_code         char(3)              not null,
   grow_area            decimal(7,2)         null,
   x_status             char(1)              null,
   x_note               nvarchar(50)         null,
   x_log                nvarchar(500)        null,
   id                   binary(99)           not null,
   rowversion           timestamp            null,
   constraint pk_mem_product primary key (product_code, member_code)
)
go

alter table mem_product
   add constraint fk_prod_mem foreign key (product_code)
      references product (product_code)
go

alter table mem_product
   add constraint fk_mem_prod foreign key (member_code)
      references member (member_code)
go
