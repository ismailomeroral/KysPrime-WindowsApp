	go
create database DbKYSprime

go 

use DbKYSprime

create table tbl_kullanici(

id int primary key identity(1,1), 
ppurl varchar(100),
isim varchar(50),
sifre varchar(50),
cevrim bit,
sil_id bit
)

create table tbl_post(

id int primary key identity(1,1), 
gonderen varchar(50),
posturl varchar(100),
cisiyet bit,
kalp int,
sil_id bit
)
create table tbl_geribidirim(

id int primary key identity(1,1), 
gonderen varchar(50),
metin varchar(100),
sil_id bit
)
