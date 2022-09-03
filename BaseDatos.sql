use master;

DROP DATABASE IF EXISTS DemoPichincha;

CREATE DATABASE DemoPichincha;

use DemoPichincha;

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
);

CREATE TABLE Cuentas(
CuentaId int not null identity(1,1) primary key,
NumeroCuenta nvarchar(100),
TipoCuenta nvarchar(100),
SaldoInicial money,
Estado bit
);

CREATE TABLE Movimientos(
MovimientoId int not null identity(1,1) primary key,
Fecha datetime,
TipoMovimiento nvarchar(100),
Valor money,
Saldo money
);
