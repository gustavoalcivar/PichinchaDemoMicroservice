use master;
go

DROP DATABASE IF EXISTS DemoPichincha;
go

CREATE DATABASE DemoPichincha;
go

use DemoPichincha;
go

CREATE TABLE Clientes(
ClienteId int not null identity(1,1) primary key,
Contrasena nvarchar(100),
Estado bit,
Nombre nvarchar(100),
Genero nvarchar(20),
Edad int,
Identificacion nvarchar(30),
Direccion nvarchar(100),
Telefono nvarchar(30)--,
--Discriminator nvarchar(100)
)

CREATE TABLE Cuentas(
CuentaId int not null identity(1,1) primary key,
NumeroCuenta nvarchar(100),
TipoCuenta nvarchar(100),
SaldoInicial money,
Estado bit,
IdentificacionCliente nvarchar(30)
)

CREATE TABLE Movimientos(
MovimientoId int not null identity(1,1) primary key,
Fecha datetime,
TipoMovimiento nvarchar(100),
Valor money,
Saldo money,
CuentaOrigen nvarchar(100)
)

declare
@cliente1 int = '1007589620',
@cliente2 int = '1785202589',
@cliente3 int = '1397592802'

insert into Clientes(Nombre,Direccion,Telefono,Contrasena,Estado,Genero,Edad,Identificacion)
values('José Lema','Otavalo sn y principal','098254785','1234',1,'Masculino',40,@cliente1),
('Marianela Montalvo ','Amazonas y NNUU','097548965','5678',1,'Femenino',30,@cliente2),
('Juan Osorio','13 junio y Equinoccial','098874587','1245',1,'Masculino',20,@cliente3)

insert into Cuentas(NumeroCuenta,TipoCuenta,SaldoInicial,Estado,IdentificacionCliente)
values('478758','Ahorro',2000,1,@cliente1),
('225487','Corriente',100,1,@cliente2),
('495878','Ahorro',0,1,@cliente3),
('496825','Ahorro',540,1,@cliente2)

go