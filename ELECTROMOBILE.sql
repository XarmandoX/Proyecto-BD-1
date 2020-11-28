-- Base de datos ELECTROMOBILE
-- Desarrolladores: Br. Yeh-Hsing Armando Yiin Anton
--					Br. Randall Agustín Hodgson

-- Creacion de la base de datos
CREATE DATABASE ELECTROMOBILEDB
GO

-- Usando la base de datos
USE ELECTROMOBILEDB
GO

-- Creacion de usuarios
-- Usuarios administradores

SP_ADDLOGIN 'Albania', '4dM1n', 'ELECTROMOBILEDB'
GO
SP_ADDSRVROLEMEMBER 'Albania', 'bulkadmin'
GO
SP_ADDUSER 'Albania', 'UsuarioAdmin'
GO
SP_ADDROLEMEMBER 'db_owner', 'UsuarioAdmin'
GO


SP_ADDLOGIN 'Frederick', '4dM1n2', 'ELECTROMOBILEDB'
GO
SP_ADDSRVROLEMEMBER 'Frederick', 'bulkadmin'
GO
SP_ADDUSER 'Frederick', 'UsuarioAdmin2'
GO
SP_ADDROLEMEMBER 'db_owner', 'UsuarioAdmin2'
GO

-- Usuario de solo lectura
SP_ADDLOGIN 'Lector', 'r34D', 'ELECTROMOBILEDB'
GO
SP_ADDUSER 'Lector', 'usuarioSoloLectura'
GO
SP_ADDROLEMEMBER 'db_datareader', 'usuarioSoloLectura' 
GO


-- Creacion de las tablas e insertando datos en las tablas que tienen registros por default

CREATE TABLE Tipo_piezas(
ID_TipoPiezas INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
Nombre NVARCHAR(35) NOT NULL 
)
GO

INSERT INTO Tipo_piezas (Nombre) VALUES ('Alternadores')
INSERT INTO Tipo_piezas (Nombre)VALUES ('Motor de arranque')
INSERT INTO Tipo_piezas (Nombre) VALUES ('Reguladores')
INSERT INTO Tipo_piezas (Nombre) VALUES ('Modelos')
INSERT INTO Tipo_piezas (Nombre) VALUES ('Portadiodos')
INSERT INTO Tipo_piezas (Nombre) VALUES ('Armadura')
INSERT INTO Tipo_piezas (Nombre) VALUES ('Bendix')
INSERT INTO Tipo_piezas (Nombre) VALUES ('Soleinoide')
GO


CREATE TABLE Tipo_proveedor (
ID_TipoProveedor TINYINT PRIMARY KEY IDENTITY(1,1) NOT NULL,
Tipo NVARCHAR(35) NOT NULL
)
GO

INSERT INTO Tipo_proveedor VALUES('Natural')
INSERT INTO Tipo_proveedor VALUES('Juridico')
GO




CREATE TABLE Proveedor (
ID_Proveedor NVARCHAR(75) PRIMARY KEY NOT NULL,
Nombre NVARCHAR(75) NOT NULL,
Apellido NVARCHAR(75) NOT NULL,
Empresa NVARCHAR(75),
Correo NVARCHAR(75),
Telefono CHAR(8) CHECK(Telefono LIKE '[2|5|7|8][0-9][0-9][0-9][0-9][0-9][0-9][0-9]') NOT NULL,
Direccion NVARCHAR(135) NOT NULL,
ID_TipoProveedor TINYINT FOREIGN KEY REFERENCES Tipo_proveedor(ID_TipoProveedor) NOT NULL
)
GO





CREATE TABLE Estado_uso (
	ID_Estado TINYINT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	Name NVARCHAR(55) NOT NULL
)
GO

INSERT INTO Estado_uso VALUES ('Nuevo')
INSERT INTO Estado_uso VALUES ('Semi-usado')
INSERT INTO Estado_uso VALUES ('Usado')
GO




CREATE TABLE Pieza (
ID_Pieza NVARCHAR(35) PRIMARY KEY NOT NULL,
ID_Proveedor NVARCHAR(75) FOREIGN KEY REFERENCES Proveedor(ID_Proveedor),
ID_Estado TINYINT FOREIGN KEY REFERENCES Estado_uso(ID_Estado),
ID_TipoPiezas INT FOREIGN KEY REFERENCES Tipo_piezas(ID_TipoPiezas) NOT NULL,
estante CHAR(5),
tension TINYINT,
precio DECIMAL (9,3) NOT NULL,
cantidad INT NOT NULL,
circuito NVARCHAR(MAX),
dientes TINYINT,
voltaje NVARCHAR(75),
Fabricante NVARCHAR(75)
)
GO





CREATE TABLE Cliente (
RUC NVARCHAR(14) PRIMARY KEY NOT NULL,
PNombre_rep NVARCHAR(30) NOT NULL,
SNombre_rep NVARCHAR(30),
PApell_rep NVARCHAR(30) NOT NULL,
SApell_rep NVARCHAR(30),
Correo NVARCHAR(45),
Telefono char(8) check(Telefono like '[2|5|7|8][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
Empresa NVARCHAR(35) NOT NULL,
Tipo_empresa NVARCHAR(75),
Tipo_Servicio NVARCHAR(55),
Direccion NVARCHAR(55),
)
GO





CREATE TABLE Compra (
ID_Compra NVARCHAR(75) PRIMARY KEY NOT NULL,
Id_Proveedor NVARCHAR(75) FOREIGN KEY REFERENCES Proveedor(ID_Proveedor) NOT NULL,
Fecha DATETIME NOT NULL,
Total_Compra DECIMAL(9,3) NOT NULL
)
GO





CREATE TABLE Detalle_compra (
ID_Compra NVARCHAR(75) FOREIGN KEY REFERENCES Compra(ID_Compra) NOT NULL,
ID_Pieza NVARCHAR(35) FOREIGN KEY REFERENCES Pieza(ID_Pieza) NOT NULL,
PRIMARY KEY(ID_Compra, ID_Pieza),
precio DECIMAL(9,3) NOT NULL,
cantidad INT NOT NULL,
subtotal DECIMAL(9,3) NOT NULL,
)
GO





CREATE TABLE Venta (
ID_Venta INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
ID_Cliente NVARCHAR(14) FOREIGN KEY REFERENCES Cliente(RUC) NULL,
Fecha_Inicial DATETIME,
FechaEntrega DATETIME NOT NULL,
Estado nvarchar(25) NOT NULL,
Total DECIMAL(9,3) NOT NULL
)
GO





CREATE TABLE Detalle_venta(
ID_Venta INT FOREIGN KEY REFERENCES Venta(ID_Venta),
ID_Pieza NVARCHAR(35) FOREIGN KEY REFERENCES Pieza(ID_Pieza) NOT NULL,
cantidad INT NOT NULL,
subtotal DECIMAL(9,3) NOT NULL,
PRIMARY KEY(ID_Venta, ID_Pieza)
)




CREATE TABLE Reparacion(
ID_Reparacion int PRIMARY KEY identity(1,1) not null,
ID_Cliente NVARCHAR(14) FOREIGN KEY REFERENCES Cliente(RUC),
FechaIni datetime not null,
FechaFinal datetime not null,
Total DECIMAL(9,3) not null
)
GO




CREATE TABLE DetalleReparacion(
ID_Reparacion INT FOREIGN KEY REFERENCES Reparacion(ID_Reparacion) not null,
ID_Pieza nvarchar(35) foreign key references Pieza(ID_Pieza)  not null,
PRIMARY KEY(ID_Reparacion, ID_Pieza),
Precio DECIMAL(9,3) not null,
Cantidad INT NOT NULL,
Subtotal DECIMAL(9,3) not null
)
GO





CREATE TABLE DevolucionCompra (
ID_DevolucionCompras INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
ID_Compra NVARCHAR(75) FOREIGN KEY REFERENCES Compra(ID_Compra) NOT NULL,
Descripcion NVARCHAR(MAX) NOT NULL,
Fecha DATETIME NOT NULL,
Total DECIMAL (9,3) NOT NULL,
)
GO





CREATE TABLE DetalleDevCompra (
ID_DevolucionCompra INT FOREIGN KEY REFERENCES DevolucionCompra(ID_DevolucionCompras) NOT NULL,
ID_Pieza NVARCHAR(35) FOREIGN KEY REFERENCES Pieza(ID_Pieza) NOT NULL,
PRIMARY KEY(ID_DevolucionCompra, ID_Pieza),
Cantidad INT NOT NULL
)
GO





CREATE TABLE DevolucionVenta(
ID_DevVenta INT PRIMARY KEY IDENTITY(1,1) not null,
ID_Venta INT FOREIGN KEY REFERENCES Venta(ID_Venta) not null,
Descripcion nvarchar(150) not null,
Fecha date not null,
Total float not null
)
GO



CREATE TABLE DetalleDevVenta(
ID_DevVenta INT FOREIGN KEY REFERENCES DevolucionVenta(ID_DevVenta) not null,
ID_Pieza nvarchar(35) FOREIGN KEY REFERENCES Pieza(ID_Pieza) not null,
CantPieza int not null
PRIMARY KEY(ID_DevVenta, ID_Pieza),
)
GO




-- CREACION Y ASIGNACION DE REGLAS
CREATE RULE entpos
AS
@v > 0
GO


CREATE RULE neg
AS
@x >= 0
GO

EXEC sp_bindrule 'neg', 'dbo.Compra.Total_Compra'
EXEC sp_bindrule 'entpos', 'dbo.Detalle_compra.precio'
EXEC sp_bindrule 'entpos', 'dbo.Detalle_compra.cantidad'
EXEC sp_bindrule 'entpos', 'dbo.Detalle_compra.subtotal'
EXEC sp_bindrule 'entpos', 'dbo.Detalle_venta.cantidad'
EXEC sp_bindrule 'entpos', 'dbo.Detalle_venta.subtotal'
EXEC sp_bindrule 'entpos', 'dbo.DetalleDevCompra.Cantidad'
EXEC sp_bindrule 'entpos', 'dbo.DetalleDevVenta.CantPieza'
EXEC sp_bindrule 'entpos', 'dbo.DetalleReparacion.Precio'
EXEC sp_bindrule 'entpos', 'dbo.DetalleReparacion.Cantidad'
EXEC sp_bindrule 'entpos', 'dbo.DetalleReparacion.Subtotal'
EXEC sp_bindrule 'neg', 'dbo.DevolucionCompra.Total'
EXEC sp_bindrule 'entpos', 'dbo.Pieza.cantidad'
EXEC sp_bindrule 'neg', 'dbo.Reparacion.Total'
EXEC sp_bindrule 'neg', 'dbo.Venta.Total'
GO





-- Creacion de procedimientos de almacenado de proveedores
-- Insercion
CREATE OR ALTER PROCEDURE InsertarProveedor
(
@IP NVARCHAR(75),
@Np NVARCHAR(75),
@Ap NVARCHAR(75),
@Emp NVARCHAR(75),
@Cor NVARCHAR(75),
@Tel CHAR(8),
@Dir NVARCHAR(135),
@IT TINYINT
)
AS
IF(@IP = '' AND @Np = '' AND @Ap = '' AND @Tel = '' AND @Dir = '' AND @IT = '' AND @EMP = '' AND @Cor = '')
BEGIN
	PRINT 'Inserte los valores requeridos'
END
ELSE
BEGIN
	IF EXISTS(SELECT * FROM Proveedor WHERE ID_Proveedor = @IP)
	BEGIN
		PRINT 'El proveedor ya esta resgistrado' 
	END
	ELSE
	BEGIN
		IF(@IP = '')
		BEGIN
			PRINT 'Insertar el id del proveedor'
		END
		ELSE
		BEGIN
			IF(@Np = '')
			BEGIN
				PRINT 'Insertar nombre del proveedor'
			END
			ELSE
			BEGIN
				IF(@Tel = '')
				BEGIN
					PRINT 'Insertar el telefono del proveedor'
				END
				ELSE
				BEGIN
					IF EXISTS(SELECT ID_Proveedor FROM Proveedor WHERE ID_Proveedor = @IP)
					BEGIN
						PRINT 'proveedor ya registrado'
					END
					ELSE
					BEGIN
						IF(@IT IN(1,2))
						BEGIN
							IF(@IT = 1)
							BEGIN
								BEGIN TRY
									INSERT INTO Proveedor(ID_Proveedor, Nombre, Apellido, Correo, Telefono, Direccion, ID_TipoProveedor)
									VALUES(@IP, @Np, @Ap, @Cor, @Tel, @Dir, 1)
								END TRY
								BEGIN CATCH
									SELECT
										ERROR_MESSAGE(),
										ERROR_LINE(),
										ERROR_NUMBER(),
										ERROR_SEVERITY(),
										ERROR_STATE(),
										ERROR_PROCEDURE()
								END CATCH
							END
							ELSE
							BEGIN
								BEGIN TRY
									INSERT INTO Proveedor
									VALUES(@IP, @Np, @Ap, @Emp, @Cor, @Tel, @Dir, 2)
								END TRY
								BEGIN CATCH
									SELECT
										ERROR_MESSAGE(),
										ERROR_LINE(),
										ERROR_NUMBER(),
										ERROR_SEVERITY(),
										ERROR_STATE(),
										ERROR_PROCEDURE()
								END CATCH
							END
						END
						ELSE
						BEGIN
							PRINT 'Tipo de cliente no registrado'
						END
					END
				END
			END
		END
	END
END
GO


-- Editar
CREATE OR ALTER PROCEDURE EditarProveedor
(
@IP NVARCHAR(75),
@Np NVARCHAR(75),
@Ap NVARCHAR(75),
@Emp NVARCHAR(75),
@Cor NVARCHAR(75),
@Tel CHAR(8),
@Dir NVARCHAR(135),
@ID_Tipo TINYINT
)
AS
IF(@IP = '' AND @Np = '' AND @Ap = '' AND @Tel = '' AND @Dir = '' AND  @EMP = '' AND @Cor = '' AND @ID_Tipo = '')
BEGIN
	PRINT 'Inserte los valores requeridos'
END
ELSE
BEGIN
	IF(@IP = '')
	BEGIN
		PRINT 'Insertar el id del proveedor'
	END
	ELSE
	BEGIN
		IF(@Np = '')
		BEGIN
			PRINT 'Insertar nombre del proveedor'
		END
		ELSE
		BEGIN
			IF(@Tel = '')
			BEGIN
				PRINT 'Insertar el telefono del proveedor'
			END
			ELSE
			BEGIN
				IF EXISTS(SELECT * FROM Proveedor WHERE ID_Proveedor = @IP)
				BEGIN
					IF(@ID_Tipo IN(1,2))
					BEGIN
						IF(@ID_Tipo= 1)
						BEGIN
							UPDATE Proveedor
							SET Nombre = @NP,
							Apellido = @Ap,
							Correo = @Cor,
							Telefono = @Tel,
							Direccion = @Dir,
							ID_TipoProveedor = @ID_Tipo
							WHERE ID_Proveedor = @IP
						END
						ELSE
						BEGIN
							UPDATE Proveedor
							SET
							Nombre = @Np,
							Apellido = @Ap,
							Empresa = @Emp,
							Correo = @Cor,
							Telefono = @Tel,
							Direccion = @Dir,
							ID_TipoProveedor = @ID_Tipo
							WHERE ID_Proveedor = @IP
						END
					END
					ELSE
					BEGIN
						PRINT 'Tipo de cliente no registrado'
					END
				END
				ELSE
				BEGIN
					PRINT 'Proveedor a editar no registrador'
				END
			END
		END
	END
END
GO


-- Borrar
CREATE PROCEDURE EliminarProveedor
(
@IP NVARCHAR(75)
)
AS
IF EXISTS(SELECT * FROM Proveedor WHERE ID_Proveedor = @IP)
BEGIN
	DELETE FROM Proveedor 
	WHERE ID_Proveedor = @IP
END
ELSE
BEGIN
	PRINT 'El proveedor no esta registrado'
END
GO






-- Procedimientos de almacenados de Pieza
CREATE OR ALTER PROCEDURE [dbo].[InsertarPieza] 
(
@IP NVARCHAR(35),
@IProv NVARCHAR(75),
@IE TINYINT,
@ITipo INT,
@Est CHAR(5),
@Ten TINYINT,
@Pre DECIMAL(9,3),
@Cir NVARCHAR(MAX),
@Di TINYINT,
@Volt NVARCHAR(75), 
@Fab NVARCHAR(75)

)
AS
DECLARE @IdProv AS NVARCHAR(75)
SET @IdProv = (SELECT ID_Proveedor FROM Proveedor WHERE ID_Proveedor = @IdProv)

IF(@IP = '' AND @IProv = '' AND @IE = '' AND @ITipo = '' AND @Est = '' AND @Ten = ''
AND @Pre = '' AND @Cir = '' AND @Di = '' AND @Volt = '')
BEGIN
    PRINT 'Porfavor inserte los datos'
END
ELSE 
BEGIN
    IF EXISTS (SELECT * FROM Pieza WHERE ID_Pieza = @IP)
    BEGIN
        PRINT 'Producto repetido'
    END
    ELSE 
    BEGIN
        IF(@IdProv = @IProv)
        BEGIN
            IF(@ITipo = 1 OR @ITipo = 2 OR @ITipo = 3 OR @ITipo = 4 OR @ITipo = 5 OR @ITipo = 6 OR @ITipo = 7 OR @ITipo = 8)
			BEGIN
				IF(@IE = 1 OR @IE = 2 OR @IE = 3)
				BEGIN
					BEGIN TRY
						INSERT INTO Pieza (ID_Pieza, ID_Proveedor,ID_Estado,ID_TipoPiezas, estante, tension, precio, cantidad, circuito, dientes, voltaje, Fabricante)
						VALUES (@IP, @IProv, @IE, @ITipo, @Est, @Ten, @Pre, 0, @Cir, @Di, @Volt, @Fab)
					END TRY
					BEGIN CATCH
						SELECT
							ERROR_MESSAGE(),
							ERROR_LINE(),
							ERROR_NUMBER(),
							ERROR_SEVERITY(),
							ERROR_STATE(),
							ERROR_PROCEDURE()
					END CATCH
				END
				ELSE
				BEGIN
					PRINT 'Verifique el estado de la pieza'
				END
					
			END
			ELSE
			BEGIN
				PRINT 'Por favor verifique el tipo de pieza que inserta'
			END
        END
        ELSE
        BEGIN
            PRINT 'Proveedor no registrado'
        END
    END
END
GO



--Edicion
CREATE OR ALTER PROCEDURE EditarPieza
(
@IP NVARCHAR(35),
@Est CHAR(5),
@Ten TINYINT,
@Pre DECIMAL(9,3),
@Cant INT,
@Cir NVARCHAR(MAX),
@Di TINYINT,
@Fab NVARCHAR(75),
@Volt NVARCHAR(75)
)
AS
IF(@IP = ''  AND @Est = '' AND @Ten = ''
AND @Pre = '' AND @Cant = '' AND @Cir = '' AND @Di = '' AND @Volt = '' AND @Fab = '')
BEGIN
	PRINT 'Inserte los valores requeridos'
END
ELSE
BEGIN 
	IF(@IP = '')
	BEGIN
		PRINT 'Inserte el codigo de la pieza a editar'
	END
	ELSE
	BEGIN
		IF(@EST = '')
		BEGIN
			PRINT 'Insertar el estante'
		END
		ELSE
		BEGIN
			IF(@Ten = '')
			BEGIN
				PRINT 'Insertar la tension de la pieza'
			END
			ELSE
			BEGIN
				IF(@Pre = '')
				BEGIN
					PRINT 'Insertar el precio de la pieza'
				END
				ELSE
				BEGIN
					IF(@Cant = '')
					BEGIN
						PRINT 'Insertar la cantidad'
					END
					ELSE
					BEGIN
						IF(@Cir = '')
						BEGIN
							PRINT 'Insertar el tipo de circuito de la pieza'
						END
						ELSE
						BEGIN
							IF(@Di = '')
							BEGIN
								PRINT 'Insertar los dientes de la pieza'
							END
							ELSE
							BEGIN
								IF(@Volt = '')
								BEGIN
									PRINT 'Insertar el voltaje de la pieza'
								END
								ELSE
								BEGIN
									IF(@Fab = '')
									BEGIN
										PRINT 'Insertar datos del fabricante'
									END
									ELSE
									BEGIN
										IF EXISTS(SELECT * FROM Pieza WHERE ID_Pieza = @IP)
										BEGIN
											BEGIN TRY
												Update Pieza
												SET 
												estante = @Est,
												tension = @Ten,
												precio = @pre,
												cantidad = @Cant,
												circuito = @Cir,
												dientes = @Di,
												voltaje = @Volt
												WHERE
												ID_Pieza = @IP
											END TRY
											BEGIN CATCH
												SELECT
													ERROR_MESSAGE(),
													ERROR_LINE(),
													ERROR_NUMBER(),
													ERROR_SEVERITY(),
													ERROR_STATE(),
													ERROR_PROCEDURE()
											END CATCH
										END
										ELSE
										BEGIN
											PRINT 'Pieza inexistente'
										END
									END
								END
							END
						END
					END
				END
			END
		END
	END
END
GO


-- Listar
CREATE PROCEDURE ListarPiezas
AS
SELECT *
FROM Pieza
GO


-- Eliminar
CREATE PROCEDURE EliminarPieza
@IdPi NVARCHAR(35)
AS
DELETE FROM Pieza 
WHERE ID_Pieza = @IdPi
GO



-- Creacion de procedimientos de almacenado de Cliente
-- Insercion
CREATE PROCEDURE InsertarCliente
(
@RUC NVARCHAR(14),
@Pn NVARCHAR(30),
@Sn NVARCHAR(30),
@Pa NVARCHAR(30),
@Sa NVARCHAR(30),
@CorC NVARCHAR(45),
@TelC CHAR(8),
@TipoEmp NVARCHAR(75),
@TipoSer NVARCHAR(55),
@DirC NVARCHAR(55),
@Emp NVARCHAR(30)
)
AS
IF(@RUC = '' AND @Pn = '' AND @Sn = '' AND @Pa = '' AND @Sa = '' AND @CorC = '' AND @Telc = '' AND @TipoEmp = '' AND @TipoSer = '' AND @DirC = '')
BEGIN 
	PRINT 'Datos incompletos'
END
ELSE
BEGIN
	IF EXISTS(SELECT * FROM Cliente WHERE RUC = @RUC)
	BEGIN
		PRINT'El Cliente ya ha sido registrado'
	END
	ELSE
	BEGIN
		INSERT INTO Cliente
		VALUES (@RUC, @Pn, @Sn, @Pa, @Sa, @CorC, @TelC, @TipoEmp, @TipoSer, @DirC, @Emp)
	END
END
GO


-- Edicion
CREATE PROCEDURE EditarCliente
(
@RUC NVARCHAR(14),
@Pn NVARCHAR(30),
@Sn NVARCHAR(30),
@Pa NVARCHAR(30),
@Sa NVARCHAR(30),
@CorC NVARCHAR(45),
@TelC CHAR(8),
@TipoEmp NVARCHAR(75),
@TipoSer NVARCHAR(55),
@DirC NVARCHAR(55),
@EMP NVARCHAR(30)
)
AS
IF EXISTS (SELECT RUC 
			FROM Cliente
			WHERE RUC = @RUC
			)
BEGIN
	IF(@Pn = '')
	BEGIN
		PRINT 'Primer nombre del representante requerido'
	END
	ELSE
	BEGIN
		IF(@Pa = '')
		BEGIN
			PRINT 'Primer apellido del representante requerido'
		END
		ELSE
		BEGIN
			IF(@TelC = '')
			BEGIN
				PRINT 'Telefono del cliente es requerido'
			END
			ELSE 
				IF(@Emp = '')
				BEGIN
					PRINT 'Empresa es requerida'
				END
				ELSE
				BEGIN
					UPDATE Cliente
					SET 
					PNombre_rep = @Pn,
					SNombre_rep = @Sn,
					PApell_rep = @Pa,
					SApell_Rep = @Sa,
					Correo = @CorC,
					Telefono = @TelC,
					Tipo_empresa = @TipoEmp,
					Tipo_Servicio = @TipoSer,
					Direccion = @DirC,
					Empresa = @Emp
					WHERE RUC = @RUC
				END
		END
	END
END
GO


-- Buscar
CREATE PROCEDURE BuscarCliente
(
@Valor VARCHAR
)
AS
SELECT *
FROM Cliente
WHERE 
RUC LIKE CONCAT('%',@Valor,'%') 
OR PNombre_rep LIKE CONCAT('%',@Valor,'%')
OR SNombre_rep LIKE CONCAT('%',@Valor,'%') 
OR PApell_rep LIKE CONCAT('%',@Valor,'%') 
OR SApell_rep LIKE CONCAT('%',@Valor,'%')
OR Correo LIKE CONCAT('%',@Valor,'%')
OR Telefono LIKE CONCAT('%',@Valor,'%')
OR Tipo_empresa LIKE CONCAT('%',@Valor,'%')
OR Tipo_Servicio LIKE CONCAT('%',@Valor,'%')
OR Direccion LIKE CONCAT('%',@Valor,'%')
OR Empresa LIKE CONCAT('%',@Valor,'%')
GO


-- Listar
CREATE PROCEDURE ListarCliente
AS
SELECT *
FROM Cliente
GO


-- Eliminar
CREATE PROCEDURE EliminarCliente
(
@RUC NVARCHAR(14)
)
AS
DELETE FROM Cliente
WHERE RUC = @RUC
GO





-- Procesos de almacenado de Compra
-- Insercion

CREATE OR ALTER PROCEDURE InsertarCompra
(
@IC NVARCHAR(75),
@IP NVARCHAR(75)
)
AS
DECLARE @IdProv AS NVARCHAR(75)
SET @IdProv = (SELECT ID_Proveedor FROM Proveedor WHERE ID_Proveedor = @IP)
IF (@IC = '' AND @IP = '')
BEGIN
	PRINT'Insertar informacion'
END
ELSE
BEGIN
	IF EXISTS(SELECT * FROM Compra WHERE ID_Compra = @IC)
	BEGIN
		PRINT 'Compra con codigo ya registrado'
	END
	ELSE
	BEGIN
		IF(@IdProv = @IP)
		BEGIN
			BEGIN TRY
				INSERT INTO Compra
				VALUES(@IC, @IP, FORMAT(GETDATE(), 'dd/mm/yy'), 0)
			END TRY
			BEGIN CATCH
			SELECT
				 ERROR_MESSAGE(),
				 ERROR_LINE(),
				 ERROR_NUMBER(),
				 ERROR_SEVERITY(),
				 ERROR_STATE(),
				 ERROR_PROCEDURE()
			 END CATCH
		END
		ELSE
		BEGIN
			PRINT 'Proveedor no registrado'
		END
	END
END
GO



-- Listar
CREATE PROCEDURE ListarCompras
AS
SELECT *
FROM Compra
GO


--Busqueda
CREATE PROCEDURE BuscarCompra
(
@IC NVARCHAR(75)
)
AS
BEGIN
	BEGIN TRY
		SELECT * FROM Compra WHERE ID_Compra = @IC
	END TRY
	BEGIN CATCH
		PRINT 'Compra no encontrada'
		SELECT
			ERROR_MESSAGE(),
			ERROR_LINE(),
			ERROR_NUMBER(),
			ERROR_SEVERITY(),
			ERROR_STATE(),
			ERROR_PROCEDURE()
	END CATCH
END
GO



-- Creacion de procediemiento de almacenado de Detalle_compra
-- Insercion
CREATE PROCEDURE InsertarDetalleCompra
(
@IC NVARCHAR(75),
@IP NVARCHAR(35),
@PC DECIMAL(9,3),
@Can INT
)
AS
DECLARE @IdComp NVARCHAR(75)
SET @IdComp = (SELECT ID_Compra FROM Compra WHERE ID_Compra = @IC)

DECLARE @IdPieza NVARCHAR(35)
SET @IdPieza = (SELECT ID_Pieza FROM Pieza WHERE ID_Pieza = @IP)

IF(@IC = '' AND @IP = '' AND @PC = '' AND @Can = '')
BEGIN
	PRINT 'Inserte la informacion requerida'
END
ELSE
BEGIN
	IF(@IdComp = @IC)
	BEGIN
		IF(@IdPieza = @IP)
		BEGIN
			BEGIN TRY
				INSERT INTO Detalle_compra
				VALUES (@IC, @IP, @PC, @Can, dbo.CalcularSubtotal(@Pc, @Can))
			END TRY
			BEGIN CATCH
			SELECT
				 ERROR_MESSAGE(),
				 ERROR_LINE(),
				 ERROR_NUMBER(),
				 ERROR_SEVERITY(),
				 ERROR_STATE(),
				 ERROR_PROCEDURE()
			END CATCH		
		END
		ELSE
		BEGIN
			PRINT 'Pieza no registrada'
		END
	END
	ELSE 
	BEGIN
		PRINT 'Compra no registrada'
	END
END
GO


--Edicion
CREATE PROCEDURE EditarDetalleCompra
(
--parametros de busqueda
@ICB NVARCHAR(75),
@IPB NVARCHAR(35),
--parametros de insercion
@PC DECIMAL(9,3),
@Can INT
)
AS
DECLARE @IdComp NVARCHAR(75)
SET @IdComp = (SELECT ID_Compra FROM Compra WHERE ID_Compra = @ICB)

DECLARE @IdPieza NVARCHAR(35)
SET @IdPieza = (SELECT ID_Pieza FROM Pieza WHERE ID_Pieza = @IPB)


DECLARE @cantTemp AS INT 
SET @cantTemp = (SELECT cantidad FROM Pieza WHERE ID_Pieza = @IPB)

DECLARE @cantTemp2 AS INT 
SET @cantTemp2 = (SELECT cantidad FROM Detalle_venta WHERE ID_Pieza = @IPB)

IF(@PC = '' AND @Can = '' AND @ICB = '' AND @IPB = '')
BEGIN
	PRINT 'Inserte los datos requeridos'
END
ELSE
BEGIN
	IF(@ICB = @IdComp)
	BEGIN
		IF(@IPB = @IdPieza)
		BEGIN
			IF(@PC = '')
			BEGIN
				PRINT 'Inserte el nuevo precio'
			END
			ELSE
			BEGIN
				IF(@Can = '')
				BEGIN
					PRINT 'Inserte la cantidad'
				END
				ELSE
				BEGIN
					IF (@Can <= @cantTemp)
					BEGIN
						IF (@cantTemp2 < @Can) --osea aumente la cantidad que habia insertado de primero
						BEGIN
							BEGIN TRY 
								UPDATE Detalle_compra SET cantidad = @Can, subtotal = dbo.CalcularSubtotal(@Can, @PC) WHERE ID_Compra = @ICB AND ID_Pieza = @IPB
								UPDATE Pieza SET cantidad = cantidad + (@Can - @cantTemp2) WHERE ID_Pieza = @IPB
							END TRY
							BEGIN CATCH 
								SELECT
									ERROR_MESSAGE(),
									ERROR_LINE(),
									ERROR_NUMBER(),
									ERROR_SEVERITY(),
									ERROR_STATE(),
									ERROR_PROCEDURE()
							END CATCH
						END
						ELSE
						BEGIN
							BEGIN TRY 
								UPDATE Detalle_compra SET cantidad = @Can, subtotal = dbo.CalcularSubtotal(@Can, @PC) WHERE ID_Compra = @ICB AND ID_Pieza = @IPB
								UPDATE Pieza SET cantidad = cantidad - (@cantTemp2 - @Can) WHERE ID_Pieza = @IPB
							END TRY
							BEGIN CATCH 

							END CATCH
						END
					END
					ELSE
					BEGIN
						PRINT 'No hay suficiente producto'
					END
				END
			END	
		END
		ELSE
		BEGIN
			PRINT 'No se ha encontrado la pieza referida'
		END
	END
	ELSE
	BEGIN
		PRINT 'No se ha encontrado la compra referida'
	END
END
GO


-- Eliminar
CREATE OR ALTER PROCEDURE EliminarDetalleDeCompra
(
@ICB NVARCHAR(75),
@IPB NVARCHAR(35)
)
AS
	IF EXISTS (SELECT ID_Compra FROM Detalle_compra WHERE ID_Compra = @ICB)
	BEGIN
		IF EXISTS (SELECT ID_Pieza FROM Detalle_venta WHERE ID_Pieza = @IPB)
		BEGIN
			DELETE FROM Detalle_compra 
			WHERE ID_Compra = @ICB AND ID_Pieza = @IPB
		END
		ELSE
		BEGIN
			PRINT 'Pieza no encontrada en los registros con esa compra'
		END
	END
	ELSE
	BEGIN
		PRINT 'Venta no detallada'
	END
GO


--Obtener los detalles de una compra
CREATE PROCEDURE ObtenerDetCompra
(
@IDC NVARCHAR(75)
)
AS
BEGIN
	IF EXISTS (SELECT * FROM Compra WHERE ID_Compra = @IDC)
	BEGIN
		BEGIN TRY
			SELECT * FROM Detalle_compra WHERE ID_Compra = @IDC
		END TRY
		BEGIN CATCH
			SELECT
				ERROR_MESSAGE(),
				ERROR_LINE(),
				ERROR_NUMBER(),
				ERROR_SEVERITY(),
				ERROR_STATE(),
				ERROR_PROCEDURE()
		END CATCH
	END
	ELSE
	BEGIN
		PRINT 'No se hallaron detalles de esta compra'
	END
END
GO


--TRIGGER para actualizar el total de compra cuando se inserte un detalle de compra
CREATE TRIGGER ActualizarCompraInventario_DetalleCompra
ON 
Detalle_compra
AFTER INSERT
AS
DECLARE @IDC AS NVARCHAR(75)
DECLARE @IP AS NVARCHAR(35)
DECLARE @canti AS INT
DECLARE @Total AS DECIMAL(9,3)



SELECT @canti= cantidad FROM inserted 
SELECT @IP = ID_Pieza FROM inserted


UPDATE Pieza SET cantidad = cantidad + @canti WHERE ID_Pieza = @IP


SELECT @IDC = ID_Compra FROM inserted 
SELECT @Total = SUM(subtotal) FROM Detalle_Compra WHERE ID_Compra = @IDC


UPDATE Compra SET Total_Compra = @Total WHERE ID_Compra = @IDC
GO



--TRIGGER para actualizar el total de compra cuando se edite un detalle de compra
CREATE TRIGGER ActualizarTotalCompra_EditarDetalleCompra
ON  
Detalle_compra
FOR UPDATE
AS

DECLARE @TotalNuevo AS DECIMAL(9,3) 
DECLARE @IDC AS NVARCHAR(75)

SELECT @IDC = ID_Compra FROM inserted
SELECT @TotalNuevo = SUM(subtotal) FROM Detalle_compra WHERE ID_Compra = @IDC

UPDATE Compra SET Total_Compra = @TotalNuevo WHERE ID_Compra = @IDC
GO




-- TRIGGER para actualizar total de compra cuando se elimine un detalle compra
CREATE TRIGGER ActualizarTotalCompraElimi
ON  
Detalle_compra
FOR DELETE
AS
DECLARE @IDC AS int
DECLARE @IDP AS NVARCHAR(35)
DECLARE @TotalNuevo AS float 
DECLARE @cant AS int

SELECT @IDC = ID_Compra FROM deleted
SELECT @TotalNuevo = SUM(subtotal) FROM Detalle_compra WHERE ID_Compra = @IDC 

UPDATE Compra SET Total_Compra = @TotalNuevo WHERE ID_Compra= @IDC

SELECT @cant =  cantidad  FROM deleted
SELECT @IDP = ID_Pieza FROM deleted

UPDATE Pieza SET cantidad = cantidad - @cant WHERE ID_Pieza = @IDP
GO






-- Creacion de Procedimientos Almacenados para Ventas
--Insercion
CREATE PROCEDURE InsertarVenta
(
@RUC NVARCHAR(14),
@FechaEntrega DATE
)
AS
DECLARE @RUCtemp AS NVARCHAR(14)
SET @RUCtemp = (SELECT RUC FROM Cliente WHERE RUC = @RUC)

IF(@RUCtemp = @RUC)
BEGIN
	IF(@RUC IS NULL AND @FechaEntrega IS NULL) 
	BEGIN
		INSERT INTO Venta(FechaEntrega, Estado,Total) VALUES (FORMAT(GETDATE(), 'dd/mm/yy'), 'Entregado',0)
	END
	ELSE
	BEGIN
		INSERT INTO Venta(Id_Cliente, Fecha_Inicial, FechaEntrega, Estado,Total) VALUES (@RUC, FORMAT(GETDATE(), 'dd/mm/yy'), FORMAT(@FechaEntrega, 'dd/mm/yy'), 'Solicitado',0)
	END
END
ELSE
BEGIN
	PRINT 'Cliente No registrado'
END
GO


--Edicion
CREATE PROCEDURE EditarVenta
@ID_Venta INT,
@RUC NVARCHAR(14),
@FechaEntrega DATE,
@Estado nvarchar(25)
AS
DECLARE @VentaTemp AS int
DECLARE @RUCTemp AS NVARCHAR(14)
SET @VentaTemp = (SELECT ID_Venta FROM Venta WHERE ID_Venta = @ID_Venta)
SET @RUCTemp=(SELECT RUC FROM Cliente WHERE RUC = @RUC)
IF(@VentaTemp = @ID_Venta)
BEGIN
	--Si el cliente es nulo
	IF(@RUC IS NULL)
	BEGIN
		IF(@Estado = 'Pedido' OR @Estado = 'Entregado')
		BEGIN
			UPDATE Venta SET FechaEntrega = @FechaEntrega, Estado = @Estado WHERE ID_Venta = @ID_Venta
		END
	END
	ELSE
	BEGIN
	--Si el cliente es juridico
		IF(@RUCTemp = @RUC)
		BEGIN
			IF(@Estado = 'Pedido' OR @Estado = 'Entregado')
			BEGIN
				UPDATE Venta SET ID_Cliente = @RUC, FechaEntrega = @FechaEntrega, Estado = @Estado WHERE ID_Venta = @ID_Venta
			END
		END
		ELSE
		BEGIN
			PRINT 'Cliente No Registrado'
		END
	END
END
ELSE
BEGIN
	PRINT 'Venta No registrada'
END
GO


--Busqueda
CREATE PROCEDURE BuscarVenta
@ID_Venta INT
AS
DECLARE @VentaTemp AS INT
SET @VentaTemp=(SELECT ID_Venta FROM Venta WHERE ID_Venta = @ID_Venta)
IF(@VentaTemp = @ID_Venta)
BEGIN
	SELECT * FROM Venta WHERE ID_Venta = @ID_Venta
END
ELSE
BEGIN
	PRINT 'Venta No Registrada'
END
GO


--Listado
CREATE PROCEDURE ListarVenta
AS
SELECT * FROM Venta
GO





-- Creacion de Procedimientos Almacenados para Detalle_venta
--Insercion
CREATE PROCEDURE AgregarDetalleVenta
@ID_Venta INT,
@ID_Pieza NVARCHAR(35),
@cantidad INT 
AS 
DECLARE @ID_VentaTemp AS INT
SET @ID_VentaTemp=(SELECT ID_Venta FROM Venta WHERE ID_Venta = @ID_Venta)

DECLARE @ID_PiezaTemp AS NVARCHAR(35) 
SET @ID_PiezaTemp = (SELECT ID_Pieza FROM Pieza WHERE ID_Pieza = @ID_Pieza)

DECLARE @cantidadTemp AS INT 
SET @cantidadTemp = (SELECT cantidad FROM Pieza WHERE ID_Pieza = @ID_Pieza)


IF(@ID_Venta = @ID_VentaTemp)
BEGIN
	IF(@ID_Pieza='')
	BEGIN
		PRINT 'El Codigo de la Pieza No Puede ser Nulo'
	END
	ELSE
	BEGIN
		IF(@ID_Pieza = @ID_PiezaTemp)
		BEGIN
		  IF(@cantidad>0)
		  BEGIN
				IF(@cantidad <= @cantidadTemp)
				BEGIN			
					INSERT INTO Detalle_venta VALUES(@ID_Venta,@ID_Pieza, @cantidad, dbo.CalcularSubtotal(@cantidad, (SELECT precio FROM Pieza WHERE ID_Pieza = @ID_Pieza))) 
				END
				ELSE
				BEGIN
					PRINT'No Hay Sufientes Existencia'
				END
		  END
		  ELSE
		  BEGIN
			PRINT 'La cantidad no debe ser Negativa Ni Cero'
		  END
		END
		ELSE
		BEGIN
			PRINT 'Producto No Registrado'
		END
	END
END
ELSE
BEGIN
	PRINT 'Venta No Registrada'
END
GO


--Edicion
CREATE PROCEDURE EditarDetalle_venta
(
@IVB INT,
@IPB NVARCHAR(35),
@cant INT
)
AS
DECLARE @cantTemp AS INT 
SET @cantTemp = (SELECT cantidad FROM Pieza WHERE ID_Pieza = @IPB)


DECLARE @cantTemp2 AS INT 
SET @cantTemp2 = (SELECT cantidad FROM Detalle_venta WHERE ID_Pieza = @IPB)

BEGIN
	IF(@IVB = '' OR @IPB = '' OR @cant = '')
	BEGIN
		PRINT 'No se admiten campos vacios'
	END
	ELSE
	BEGIN
		IF EXISTS (SELECT ID_Venta FROM Detalle_venta WHERE ID_Venta = @IVB) 
		BEGIN
			IF EXISTS (SELECT ID_Pieza FROM Detalle_venta WHERE ID_Pieza = @IPB)
			BEGIN
				IF (@cant <= @cantTemp)
				BEGIN
					IF (@cantTemp2 < @cant) --osea aumente la cantidad que habia insertado de primero
					BEGIN
						BEGIN TRY 
							UPDATE Detalle_venta SET cantidad = @cant, subtotal = dbo.CalcularSubtotal(@cant, (SELECT precio FROM Pieza WHERE ID_Pieza = @IPB)) WHERE ID_Venta = @IVB AND ID_Pieza = @IPB
							UPDATE Pieza SET cantidad = cantidad - (@cant - @cantTemp2) WHERE ID_Pieza = @IPB
						END TRY
						BEGIN CATCH 
							SELECT
								ERROR_MESSAGE(),
								ERROR_LINE(),
								ERROR_NUMBER(),
								ERROR_SEVERITY(),
								ERROR_STATE(),
								ERROR_PROCEDURE()
						END CATCH
					END
					ELSE
					BEGIN
						BEGIN TRY 
							UPDATE Detalle_venta SET cantidad = @cant, subtotal = dbo.CalcularSubtotal(@cant, (SELECT precio FROM Pieza WHERE ID_Pieza = @IPB)) WHERE ID_Venta = @IVB AND ID_Pieza = @IPB
							UPDATE Pieza SET cantidad = cantidad + (@cantTemp2 - @cant) WHERE ID_Pieza = @IPB
						END TRY
						BEGIN CATCH 

						END CATCH
					END
				END
				ELSE
				BEGIN
					PRINT 'No hay suficiente producto'
				END
			END
			ELSE 
			BEGIN
				PRINT 'Pieza no existente'
			END
			
		END
		ELSE
		BEGIN
			PRINT 'Venta no existente'
		END
	END
END
GO


 --Busqueda
CREATE PROCEDURE BuscarDetalle_venta
(
@IVB INT,
@IPB NVARCHAR(35)
)
AS
BEGIN
	IF EXISTS (SELECT ID_Venta FROM Detalle_venta WHERE ID_Venta = @IVB)
	BEGIN
		IF EXISTS (SELECT ID_Pieza FROM Detalle_venta WHERE ID_Pieza = @IPB)
		BEGIN
			BEGIN TRY
				SELECT * FROM Detalle_venta WHERE ID_Venta = @IVB AND ID_Pieza = @IPB
			END TRY
			BEGIN CATCH
				SELECT
					ERROR_MESSAGE(),
					ERROR_LINE(),
					ERROR_NUMBER(),
					ERROR_SEVERITY(),
					ERROR_STATE(),
					ERROR_PROCEDURE()
			END CATCH
		END
		ELSE
		BEGIN
			PRINT 'Pieza no existente'
		END
	END
	ELSE
	BEGIN
		PRINT 'Venta no existente'
	END
END
GO


--Listado
CREATE PROCEDURE ListarDetalle_venta
AS
SELECT * FROM Detalle_venta
GO

--Eliminar
CREATE PROCEDURE EliminarDetalle_venta
@IV INT,
@IP NVARCHAR(35)
AS
BEGIN
	IF EXISTS (SELECT ID_Venta FROM Detalle_venta WHERE ID_Venta = @IV)
	BEGIN
		IF EXISTS (SELECT ID_Pieza FROM Detalle_venta WHERE ID_Pieza = @IP)
		BEGIN
			DELETE FROM Detalle_venta 
			WHERE ID_Venta = @IV AND ID_Pieza = @IP
		END
		ELSE
		BEGIN
			PRINT 'Pieza no encontrada en los registros con esa venta'
		END
	END
	ELSE
	BEGIN
		PRINT 'Venta no detallada'
	END
END
GO


--Buscar Detalle de venta de las ventas
CREATE PROCEDURE ObtenerDetVentas
(
@IDV NVARCHAR(75)
)
AS
BEGIN
	IF EXISTS (SELECT * FROM Venta WHERE ID_Venta = @IDV)
	BEGIN
		BEGIN TRY
			SELECT * FROM Detalle_venta WHERE ID_Venta = @IDV
		END TRY
		BEGIN CATCH
			SELECT
				ERROR_MESSAGE(),
				ERROR_LINE(),
				ERROR_NUMBER(),
				ERROR_SEVERITY(),
				ERROR_STATE(),
				ERROR_PROCEDURE()
		END CATCH
	END
	ELSE
	BEGIN
		PRINT 'No se hallaron detalles de esta compra'
	END
END
GO


--Trigger para actualizar inventario en piezas asi como el total de la venta
CREATE TRIGGER ActualizarTotalVenta_NuevoDetalleVenta
ON 
Detalle_venta
FOR INSERT
AS
DECLARE @IV AS INT
DECLARE @total AS DECIMAL (9,3)
DECLARE @cant AS INT
DECLARE @IP AS NVARCHAR(35)

SELECT @cant = cantidad FROM inserted 
SELECT @IP = ID_Pieza FROM inserted

UPDATE Pieza SET cantidad = cantidad - @cant WHERE ID_Pieza = @IP

SELECT @IV = ID_Venta FROM inserted 

SELECT @total = SUM(subtotal) FROM Detalle_venta WHERE ID_Venta = @IV

UPDATE Venta set Total = @total WHERE ID_Venta = @IV
GO


--TRIGGER para actualizar el total de la Venta luego de una Edicion en Detalle_venta
CREATE TRIGGER ActualizarTotalVenta_EditarDetalleVenta
ON  
Detalle_venta
FOR UPDATE
AS
DECLARE @IV AS INT
DECLARE @Nuevo_total AS DECIMAL(9,3) 

SELECT @IV = ID_Venta FROM INSERTED 

SELECT @Nuevo_total = SUM(subtotal) FROM Detalle_venta WHERE ID_Venta = @IV

UPDATE Venta SET Total = @Nuevo_total WHERE ID_Venta = @IV
GO


--TRIGGER para actualizar el total de una Venta una vez de elimine uno de sus detalles
CREATE TRIGGER ActualizarTotalVenta_EliminarDetalleVenta
ON  
Detalle_venta
FOR DELETE
AS
DECLARE @IV AS INT
DECLARE @NuevoTotal AS DECIMAL(9,3) 

SELECT @IV = ID_Venta FROM deleted 

SELECT @NuevoTotal = SUM(subtotal) FROM Detalle_venta WHERE ID_Venta = @IV

UPDATE Venta SET Total = @NuevoTotal WHERE ID_Venta = @IV
GO





--Procedimientos Almacenados para Reparacion
--Insercion
CREATE PROCEDURE InsertarReparacion
(
@IDC NVARCHAR(14),
@FECHAFIN DATE
)
AS
BEGIN
    IF(@FECHAFIN = '' AND @IDC = '')
    BEGIN
        PRINT 'Campos vacios'
    END
    ELSE
    BEGIN
            BEGIN TRY
                INSERT INTO Reparacion VALUES (@IDC, FORMAT(GETDATE(), 'dd/mm/yy'), @FECHAFIN, 0)
            END TRY
            BEGIN CATCH
                SELECT
                    ERROR_MESSAGE(),
                    ERROR_LINE(),
                    ERROR_NUMBER(),
                    ERROR_SEVERITY(),
                    ERROR_STATE(),
                    ERROR_PROCEDURE()
            END CATCH
    END
END
GO



--Editar
CREATE PROCEDURE EditarReparacion
(
--Parametro de busqueda
@IDR INT,
--Parametros para cambiar
@IDC NVARCHAR(14),
@FECHAFIN DATE
)
AS
BEGIN
    IF(@IDR = '' OR @IDC = '' OR @FECHAFIN = '')
    BEGIN
        PRINT 'Campos Vacios'
    END
    ELSE
    BEGIN
		IF EXISTS (SELECT ID_Reparacion FROM Reparacion WHERE ID_Reparacion = @IDR)
        BEGIN
			IF(@IDC IS NULL)
			BEGIN
				UPDATE Reparacion SET FechaFinal = @FECHAFIN WHERE ID_Reparacion = @IDR
			END
			ELSE
			BEGIN
                BEGIN TRY
                    UPDATE Reparacion SET ID_Cliente = @IDC, FechaFinal = @FECHAFIN WHERE ID_Reparacion = @IDR
                END TRY
                BEGIN CATCH
                    SELECT
                        ERROR_MESSAGE(),
                        ERROR_LINE(),
                        ERROR_NUMBER(),
                        ERROR_SEVERITY(),
                        ERROR_STATE(),
                        ERROR_PROCEDURE()
                END CATCH
            END
		END	
		ELSE
        BEGIN
			PRINT 'Reparacion no registrada'
        END
    END
END
GO


--Listado
CREATE PROCEDURE ListarReparacion
AS
BEGIN
	SELECT * FROM Reparacion
END
GO


--Busqueda
CREATE PROCEDURE BuscarReparacion
(
@IDR INT
)
AS
BEGIN
	SELECT * FROM Reparacion WHERE ID_Reparacion = @IDR
END
GO





--Procedimiento de alamacenado de los detalles de reparacion
--Insercion
CREATE PROCEDURE InsertarDetalleReparacion
(
@IR INT,
@IP NVARCHAR(35),
@PR DECIMAL(9,3),
@CanR INT
)
AS
DECLARE @IdP NVARCHAR(35)
SET @IdP = (SELECT ID_Pieza FROM Pieza WHERE ID_Pieza = @IP)

DECLARE @IdR INT
SET @IdR = (SELECT ID_Reparacion FROM Reparacion WHERE ID_Reparacion = @IR)

IF(@IR = '' AND @IP = '' AND @PR = '' AND @CanR = '')
BEGIN
	PRINT 'Insertar los datos requeridos'
END
ELSE
BEGIN
	IF(@IR = '')
	BEGIN
		PRINT 'Insertar el id de la reparacion'
	END
	ELSE
	BEGIN
		IF(@IP = '')
		BEGIN
			PRINT 'Insertar el codigo de la pieza'
		END
		ELSE
		BEGIN
			IF(@PR = '')
			BEGIN
				PRINT 'Insertar el precio de la reparacion'
			END
			ELSE
			BEGIN
				IF(@CanR = '')
				BEGIN
					PRINT 'Insertar la cantidad de piezas reparadas'
				END
				ELSE
				BEGIN
					IF(@IdP = @IP)
					BEGIN
						IF(@IdR = @IR)
						BEGIN
							BEGIN TRY
								INSERT INTO DetalleReparacion
								VALUES(@IR, @IP, @PR, @CanR, dbo.CalcularSubtotal(@PR, @CanR))
							END TRY
							BEGIN CATCH
								SELECT
								 ERROR_MESSAGE(),
								 ERROR_LINE(),
								 ERROR_NUMBER(),
								 ERROR_SEVERITY(),
								 ERROR_STATE(),
								 ERROR_PROCEDURE()
							END CATCH
						END
						ELSE
						BEGIN
							PRINT 'La reparacion no ha sido registrada'
						END
					END
					ELSE
					BEGIN
						PRINT 'La pieza no esta registrada'
					END
				END
			END
		END
	END
END
GO

--Edicion
CREATE OR ALTER PROCEDURE EditarDetalleReparacion
(
@IR INT,
@IP NVARCHAR(35),
@PR DECIMAL(9,3),
@CanR INT
)
AS
DECLARE @IdP NVARCHAR(35)
SET @IdP = (SELECT ID_Pieza FROM Pieza WHERE ID_Pieza = @IP)

DECLARE @IdR INT
SET @IdR = (SELECT ID_Reparacion FROM Reparacion WHERE ID_Reparacion = @IR)

IF(@IR = '' AND @IP = '' AND @PR = '' AND @CanR = '')
BEGIN
	PRINT 'Insertar los datos requeridos'
END
ELSE
BEGIN
	IF EXISTS(SELECT * FROM DetalleReparacion WHERE ID_Reparacion = @IR AND ID_Pieza = @IP)
	BEGIN
		IF(@IdP = @IP)
		BEGIN
			IF(@IdR = @IR)
			BEGIN
				UPDATE DetalleReparacion
				SET 
				PRECIO = @PR,
				Cantidad = @CanR,
				Subtotal = dbo.CalcularSubtotal(@PR, @CanR)
				WHERE ID_Reparacion = @IR 
				AND ID_Pieza = @IP
			END
			ELSE
			BEGIN
				PRINT 'La reparacion no ha sido registrada'
			END
		END
		ELSE
		BEGIN
			PRINT 'Pieza no registrada'
		END
	END
	ELSE
	BEGIN
		PRINT 'El registro no existe'
	END
END
GO

CREATE OR ALTER PROCEDURE EditarDetalleReparacion
(
@IR INT,
@IP NVARCHAR(35)
)
AS
BEGIN
	IF EXISTS (SELECT * FROM DetalleReparacion WHERE ID_Reparacion = @IR AND ID_Pieza = @IP)
	BEGIN
		DELETE DetalleReparacion WHERE ID_Reparacion = @IR AND ID_Pieza = @IP
	END
	ELSE
	BEGIN
		PRINT 'Detalle de Reparacion no encontrador'
	END
END





-- Creando procedimientos de la devolucion de compra
-- Insercion
CREATE PROCEDURE InsertarDevolucionCompra
( 
@IC NVARCHAR(75),
@Desc NVARCHAR(MAX)
)
AS
DECLARE @IdCom NVARCHAR(75)
SET @IdCom = (SELECT ID_Compra FROM Compra WHERE ID_Compra = @IC)

IF(@IC = '' AND @Desc = '')
BEGIN
	PRINT 'Insertar los datos requeridos'
END
ELSE
BEGIN
	IF(@IC = '')
	BEGIN
		PRINT 'Insertar el id de la compra'
	END
	ELSE
	BEGIN
		IF (@IC = @IdCom)
		BEGIN
			INSERT INTO DevolucionCompra VALUES (@IC, @Desc, FORMAT(GETDATE(), 'dd/mm/yy'), 0)
		END
		ELSE
		BEGIN
			PRINT 'Compra no existente'
		END
	END
END
GO


-- Edicion
CREATE PROCEDURE EditarDevolucionDeCompra
(
@IDV INT,
@Desc NVARCHAR(MAX)
)
AS
IF(@IDV = '' AND @Desc = '')
BEGIN
	PRINT 'Inserte los valores requeridos'
END
ELSE
BEGIN
	IF(@IDV = '')
	BEGIN
		PRINT 'Inserte el id de la devolucion de la compra'
	END
	ELSE
	BEGIN
		IF(@Desc = '')
		BEGIN
			PRINT 'Inserte la descripcion de la devolucion'
		END
		ELSE
		BEGIN
			UPDATE DevolucionCompra
			SET 
			Descripcion = @Desc
			WHERE ID_DevolucionCompras = @IDV
		END
	END
END
GO


--Listar
CREATE PROCEDURE ListarDevolucionCompras
AS
BEGIN
	SELECT * FROM DevolucionCompra
END
GO



--Creacion de procedimientos de almacenado de Detalle de devolucion de compra
CREATE PROCEDURE InsertarDetalleDevCompra
(
@IDC INT,
@IP NVARCHAR(35),
@Can INT
)
AS
DECLARE @IdPi NVARCHAR(35)
SET @IdPi = (SELECT ID_Pieza FROM Pieza WHERE ID_Pieza = @IP)

DECLARE @ID_Compra INT

IF(@IP = '' AND @Can = '')
BEGIN
	PRINT 'Inserte los datos requeridos'
END
ELSE
BEGIN
	IF(@IP = '')
	BEGIN
		IF(@Can = '')
		BEGIN
			PRINT 'Insertar la cantidad'
		END
		ELSE
		BEGIN
			IF(@IdPi = @IP)
			BEGIN
				IF(@IDC = '')
				BEGIN
					PRINT 'Insertar el codigo de la devolucion compra'
				END
				ELSE 
				BEGIN
					IF EXISTS (SELECT ID_DevolucionCompras FROM DevolucionCompra WHERE ID_DevolucionCompras = @IDC)
					BEGIN
						
						BEGIN TRY
							INSERT INTO DetalleDevCompra
							VALUES (@IDC, @IP, @Can)
						END TRY
						BEGIN CATCH
							SELECT
								ERROR_MESSAGE(),
								ERROR_LINE(),
								ERROR_NUMBER(),
								ERROR_SEVERITY(),
								ERROR_STATE(),
								ERROR_PROCEDURE()
						END CATCH
					END
					ELSE
					BEGIN
						PRINT 'Devolucion no ha sido registrada'
					END
				END
			END
			ELSE 
			BEGIN 
				PRINT 'La pieza no ha sido registrada'
			END
		END
	END
	ELSE
	BEGIN
		PRINT 'Inserte el codigo de la pieza'
	END
END
GO


--Editar
CREATE PROCEDURE EditarDetalleDevCompra
(
@ID_DevCompra INT,
@IDP NVARCHAR(35),
@cant_Edit int 
)
AS
DECLARE @cant_Pieza AS INT 
SET @cant_Pieza = (SELECT cantidad FROM Pieza WHERE ID_Pieza = @IDP)

DECLARE @cant_Dev AS INT
SET @cant_Dev = (SELECT Cantidad FROM DetalleDevCompra WHERE ID_DevolucionCompra = @ID_DevCompra AND ID_Pieza = @IDP)

DECLARE @IDCOMPRA AS NVARCHAR(75)
DECLARE @PRECIO AS DECIMAL(9,3)

BEGIN
	IF (@ID_DevCompra = '' OR @IDP = '' OR @cant_Edit = '')
	BEGIN
		PRINT 'Campos Vacios'
	END
	ELSE 
	BEGIN
		IF EXISTS (SELECT * FROM DetalleDevCompra WHERE ID_DevolucionCompra = @ID_DevCompra AND ID_Pieza = @IDP)
		BEGIN
			IF(@cant_Edit = @cant_Dev)
			BEGIN
				PRINT 'por favor inserte una cantidad diferente'
			END
			ELSE
			BEGIN
				IF(@cant_Edit >= 1)
				BEGIN
					IF(@cant_Dev > @cant_Edit)
					BEGIN
						BEGIN TRY
							SET @IDCOMPRA = (SELECT ID_Compra FROM DevolucionCompra WHERE ID_DevolucionCompras = @ID_DevCompra)
					
							SET @PRECIO = (SELECT precio FROM Detalle_compra WHERE ID_Compra = @IDCOMPRA AND ID_Pieza = @IDP)
							UPDATE DetalleDevCompra SET Cantidad = @cant_Edit WHERE ID_DevolucionCompra = @ID_DevCompra
							UPDATE Pieza SET cantidad = cantidad + (@cant_Dev - @cant_Edit) WHERE ID_Pieza = @IDP
							UPDATE Detalle_compra SET cantidad = cantidad + (@cant_Dev - @cant_Edit) WHERE ID_Compra = @IDCOMPRA AND ID_Pieza = @IDP
							UPDATE DevolucionCompra SET Total = Total - dbo.CalcularSubtotal(@cant_Edit, @PRECIO)
						END TRY
						BEGIN CATCH
							SELECT
								ERROR_MESSAGE(),
								ERROR_LINE(),
								ERROR_NUMBER(),
								ERROR_SEVERITY(),
								ERROR_STATE(),
								ERROR_PROCEDURE()
						END CATCH
					END
					ELSE
					BEGIN
						SET @IDCOMPRA = (SELECT ID_Compra FROM DevolucionCompra WHERE ID_DevolucionCompras = @ID_DevCompra)
					
						SET @PRECIO = (SELECT precio FROM Detalle_compra WHERE ID_Compra = @IDCOMPRA AND ID_Pieza = @IDP)

						UPDATE DetalleDevCompra SET Cantidad = @cant_Edit WHERE ID_DevolucionCompra = @ID_DevCompra
						UPDATE Pieza SET cantidad = cantidad - (@cant_Edit - @cant_Dev) WHERE ID_Pieza = @IDP
						UPDATE Detalle_compra SET cantidad = cantidad - (@cant_Dev - @cant_Edit) WHERE ID_Compra = @IDCOMPRA AND ID_Pieza = @IDP
						UPDATE DevolucionCompra SET Total = Total + dbo.CalcularSubtotal(@cant_Edit, @PRECIO)
					END
				END
				ELSE
				BEGIN
					PRINT 'la cantidad no debe ser menor que 1'
				END
			END
		END
		ELSE
		BEGIN
			PRINT 'Detalle de DevCompra no existente'
		END
	END
END
GO


--Eliminar
CREATE PROCEDURE EliminarDetDevCompra
(
@ID_DevCompra INT,
@IDP NVARCHAR(35)
)
AS
BEGIN
	IF EXISTS (SELECT * FROM DetalleDevCompra WHERE ID_DevolucionCompra = @ID_DevCompra AND ID_Pieza = @IDP)
	BEGIN
		DELETE DetalleDevCompra WHERE ID_DevolucionCompra = @ID_DevCompra AND ID_Pieza = @IDP
	END
	ELSE
	BEGIN
		PRINT 'Detalle de Dev no encontrador'
	END
END
GO


-- TRIGGER para actualizar inventario luego de insertar un DetalleDevCompra
CREATE TRIGGER ActualizarInventario_InsertarDetDevCompra
ON DetalleDevCompra
AFTER INSERT
AS
BEGIN
	DECLARE @IP AS NVARCHAR(35)
	DECLARE @cant AS INT

	DECLARE @ID_DevCompra AS INT
	DECLARE @IDC AS NVARCHAR(75)
	DECLARE @PRECIO AS DECIMAL(9,3)

	SELECT @cant = cantidad FROM inserted 
	SELECT @IP = ID_Pieza FROM inserted

	UPDATE Pieza SET cantidad = cantidad - @cant WHERE ID_Pieza = @IP

	SELECT @ID_DevCompra = ID_DevolucionCompra FROM inserted 
	SELECT @IDC = (SELECT ID_Compra FROM DevolucionCompra WHERE ID_DevolucionCompras = @ID_DevCompra)
	SELECT @PRECIO = (SELECT precio FROM Detalle_compra WHERE ID_Compra = @IDC AND ID_Pieza = @IP)


	UPDATE DevolucionCompra SET Total = Total + dbo.CalcularSubtotal(@cant, @PRECIO) WHERE ID_DevolucionCompras = @ID_DevCompra;

	UPDATE Detalle_compra SET cantidad = cantidad - @cant WHERE ID_Compra = @IDC AND ID_Pieza = @IP
END
GO


-- TRIGGER para actualizar inventario luego de Eliminiar un DetalleDevCompra
CREATE TRIGGER ActualizarInventario_EliminarDetDevCompra
ON DetalleDevCompra
AFTER DELETE
AS
BEGIN
	DECLARE @IP AS NVARCHAR(35)
	DECLARE @cant AS INT

	DECLARE @ID_DevCompra AS INT
	DECLARE @IDC AS NVARCHAR(75)
	DECLARE @PRECIO AS DECIMAL(9,3)

	SELECT @cant = cantidad FROM deleted 
	SELECT @IP = ID_Pieza FROM deleted

	UPDATE Pieza SET cantidad = cantidad + @cant WHERE ID_Pieza = @IP

	SELECT @ID_DevCompra = ID_DevolucionCompra FROM deleted 
	SELECT @IDC = (SELECT ID_Compra FROM DevolucionCompra WHERE ID_DevolucionCompras = @ID_DevCompra)
	SELECT @PRECIO = (SELECT precio FROM Detalle_compra WHERE ID_Compra = @IDC AND ID_Pieza = @IP)


	UPDATE DevolucionCompra set Total = Total - dbo.CalcularSubtotal(@cant, @PRECIO) WHERE ID_DevolucionCompras = @ID_DevCompra
END
GO





--Creacion de procedimientos Almacenados para DevolucionVenta
--Insercion
CREATE PROCEDURE InsertarDevVenta
(
@IDV INT,
@DESC NVARCHAR(150),
@FECHADEV DATE,
@total DECIMAL(9,3)
)
AS
BEGIN
	IF (@IDV = '' OR @DESC = '' OR @FECHADEV = '')
	BEGIN
		PRINT 'Campos vacios'
	END
	ELSE
	BEGIN
		IF EXISTS (SELECT ID_Venta FROM Venta WHERE ID_Venta =  @IDV)
		BEGIN
			INSERT INTO DevolucionVenta VALUES (@IDV, @DESC, @FECHADEV, @total)
		END
		ELSE
		BEGIN
			PRINT 'Venta no ingresada'
		END
	END
END
GO


--Editar
CREATE PROCEDURE EditarDevVenta
(
@IDV INT,
@Desc NVARCHAR(MAX)
)
AS
IF(@IDV = '' AND @Desc = '')
BEGIN
	PRINT 'Inserte los valores requeridos'
END
ELSE
BEGIN
	IF(@IDV = '')
	BEGIN
		PRINT 'Inserte el id de la devolucion de la compra'
	END
	ELSE
	BEGIN
		IF(@Desc = '')
		BEGIN
			PRINT 'Inserte la descripcion de la devolucion'
		END
		ELSE
		BEGIN
			UPDATE DevolucionVenta
			SET 
			Descripcion = @Desc
			WHERE ID_DevVenta = @IDV
		END
	END
END
GO


--Listar
CREATE PROCEDURE ListarDevVenta
AS
BEGIN
	SELECT * FROM DevolucionVenta
END
GO




--Procedimientos de almacenado de detalle de devolucion de venta
CREATE PROCEDURE InsertarDetalleDevVenta
(
@IDV INT,
@IP NVARCHAR(35),
@Can INT
)
AS
DECLARE @IdDev INT
SET @IdDev = (SELECT ID_DevVenta FROM DevolucionVenta WHERE ID_DevVenta = @IDV)

DECLARE @IdP NVARCHAR(35)
SET @IdP = (SELECT ID_Pieza FROM Pieza WHERE ID_Pieza = @IdP)

IF(@IDV = '' AND @IP = '' AND @Can = '')
BEGIN
	PRINT 'Inserte los valores requeridos'
END
ELSE 
BEGIN
	IF EXISTS(SELECT * FROM DetalleDevVenta WHERE ID_DevVenta = @IDV AND ID_Pieza = @IP)
	BEGIN
		PRINT 'Este detalle devolucion ya esta registrado'
	END
	ELSE
	BEGIN
		IF(@IDV = '')
		BEGIN
			PRINT 'Inserte el id de la devolucion de la venta'
		END
		ELSE
		BEGIN
			IF(@IP = '')
			BEGIN
				PRINT 'Inserte el codigo del producto'
			END
			ELSE
			BEGIN
				IF(@CAN = '')
				BEGIN
					PRINT 'Inserte la cantidad de productos a insertar'
				END
				ELSE
				BEGIN
					BEGIN TRY
						INSERT INTO DetalleDevVenta
						VALUES(@IDV, @IP, @Can)
					END TRY
					BEGIN CATCH
						SELECT
						 ERROR_MESSAGE(),
						 ERROR_LINE(),
						 ERROR_NUMBER(),
						 ERROR_SEVERITY(),
						 ERROR_STATE(),
						 ERROR_PROCEDURE()
					END CATCH
				END
			END
		END
	END
END
GO


--Editar
CREATE PROCEDURE EditarDetalleDevVenta
(
@ID_DevVenta int,
@IDP NVARCHAR(35),
@cant_Edit int
)
AS

DECLARE @cant_Pieza AS INT 
SET @cant_Pieza = (SELECT cantidad FROM Pieza WHERE ID_Pieza = @IDP)

DECLARE @cant_Dev AS INT
SET @cant_Dev = (SELECT CantPieza FROM DetalleDevVenta WHERE ID_DevVenta = @ID_DevVenta AND ID_Pieza = @IDP)

DECLARE @IDVENTA AS NVARCHAR(75)
DECLARE @PRECIO AS DECIMAL(9,3)

BEGIN
	IF(@ID_DevVenta = '' OR @IDP = '' OR @cant_Edit = '')
	BEGIN
		PRINT 'Campos Vacios'
	END
	ELSE
	BEGIN 
		IF EXISTS (SELECT * FROM DetalleDevVenta WHERE ID_DevVenta = @ID_DevVenta AND ID_Pieza = @IDP)
		BEGIN
			IF(@cant_Edit >= 1)
			BEGIN
				IF(@cant_Dev > @cant_Edit)
					BEGIN
						BEGIN TRY
							SET @IDVENTA = (SELECT ID_Venta FROM DevolucionVenta WHERE ID_DevVenta = @ID_DevVenta)
					
							SET @PRECIO = (SELECT precio FROM Pieza WHERE ID_Pieza = @IDP)

							UPDATE DetalleDevVenta SET CantPieza = @cant_Edit WHERE ID_DevVenta = @ID_DevVenta
							UPDATE Pieza SET cantidad = cantidad - (@cant_Dev - @cant_Edit) WHERE ID_Pieza = @IDP
							UPDATE Detalle_venta SET cantidad = cantidad + (@cant_Dev - @cant_Edit) WHERE ID_Venta = @IDVENTA AND ID_Pieza = @IDP
							UPDATE DevolucionVenta SET Total = Total - dbo.CalcularSubtotal(@cant_Edit, @PRECIO)
						END TRY
						BEGIN CATCH
							SELECT
								ERROR_MESSAGE(),
								ERROR_LINE(),
								ERROR_NUMBER(),
								ERROR_SEVERITY(),
								ERROR_STATE(),
								ERROR_PROCEDURE()
						END CATCH
					END
					ELSE
					BEGIN
						SET @IDVENTA = (SELECT ID_Venta FROM DevolucionVenta WHERE ID_DevVenta = @ID_DevVenta)
					
						SET @PRECIO = (SELECT precio FROM Pieza WHERE ID_Pieza = @IDP)

						UPDATE DetalleDevVenta SET CantPieza = @cant_Edit WHERE ID_DevVenta = @ID_DevVenta
						UPDATE Pieza SET cantidad = cantidad + (@cant_Edit - @cant_Dev) WHERE ID_Pieza = @IDP
						UPDATE Detalle_venta SET cantidad = cantidad - (@cant_Dev - @cant_Edit) WHERE ID_Venta = @IDVENTA AND ID_Pieza = @IDP
						UPDATE DevolucionVenta SET Total = Total + dbo.CalcularSubtotal(@cant_Edit, @PRECIO)
					END
			END
			ELSE
			BEGIN
				PRINT 'La cantidad no debe ser menor que 1, si desea puede eliminar dicho detalle'
			END
		END
		ELSE
		BEGIN
			PRINT 'Detalle de Dev Venta no existente, verifique datos'
		END
	END
END
GO


CREATE OR ALTER PROCEDURE EliminarDetalleDevVenta
(
@ID_DevVenta int,
@IDP NVARCHAR(35)
)
AS
BEGIN
	IF EXISTS (SELECT * FROM DetalleDevVenta WHERE ID_DevVenta = @ID_DevVenta AND ID_Pieza = @IDP)
	BEGIN
		DELETE DetalleDevVenta WHERE ID_DevVenta = @ID_DevVenta AND ID_Pieza = @IDP
	END
	ELSE
	BEGIN
		PRINT 'Detalle de devolucion no registrado'
	END
END
GO



-- TRIGGER para actualizar inventario luego de insertar un DetalleDevCompra
CREATE TRIGGER ActualizarInventario_InsertarDetDevVenta
ON DetalleDevVenta
AFTER INSERT
AS
BEGIN
	DECLARE @IP AS NVARCHAR(35)
	DECLARE @cant AS INT

	DECLARE @ID_DevVenta AS INT
	DECLARE @IDV AS NVARCHAR(75)
	DECLARE @PRECIO AS DECIMAL(9,3)

	SELECT @cant = CantPieza FROM inserted 
	SELECT @IP = ID_Pieza FROM inserted

	UPDATE Pieza SET cantidad = cantidad + @cant WHERE ID_Pieza = @IP

	SELECT @ID_DevVenta = ID_DevVenta FROM inserted 
	SELECT @IDV = (SELECT ID_Venta FROM DevolucionVenta WHERE ID_DevVenta = @ID_DevVenta)
	SELECT @PRECIO = (SELECT precio FROM Pieza WHERE ID_Pieza = @IP)

	UPDATE Detalle_venta SET cantidad = cantidad - @cant WHERE ID_Venta = @IDV AND ID_Pieza = @IP
END
GO


-- TRIGGER para actualizar inventario luego de Eliminiar un DetalleDevCompra
CREATE TRIGGER ActualizarInventario_EliminarDetDevVenta
ON DetalleDevVenta
AFTER INSERT
AS
BEGIN
	DECLARE @IP AS NVARCHAR(35)
	DECLARE @cant AS INT

	DECLARE @ID_DevVenta AS INT
	DECLARE @IDV AS NVARCHAR(75)
	DECLARE @PRECIO AS DECIMAL(9,3)

	SELECT @cant = CantPieza FROM inserted 
	SELECT @IP = ID_Pieza FROM inserted

	UPDATE Pieza SET cantidad = cantidad + @cant WHERE ID_Pieza = @IP

	SELECT @ID_DevVenta = ID_DevVenta FROM inserted 
	SELECT @IDV = (SELECT ID_Venta FROM DevolucionVenta WHERE ID_DevVenta = @ID_DevVenta)
	SELECT @PRECIO = (SELECT precio FROM Pieza WHERE ID_Pieza = @IP)


	UPDATE DevolucionVenta SET Total = Total - dbo.CalcularSubtotal(@cant, @PRECIO) WHERE ID_DevVenta = @ID_DevVenta;

	UPDATE Detalle_venta SET cantidad = cantidad - @cant WHERE ID_Venta = @IDV AND ID_Pieza = @IP
END
GO





-- CREACION DE FUNCIONES
CREATE FUNCTION CalcularSubtotal
(
@Cant INT,
@Precio DECIMAL(9,3)
)
RETURNS DECIMAL(9,3)
AS
BEGIN
DECLARE @Subtotal DECIMAL(9,3)
SET @Subtotal = @Cant * @Precio
	RETURN(@Subtotal)
END
GO





--CREACION DE VIEWS
CREATE or alter VIEW ReportesVentasPorPiezas
AS
SELECT v.ID_Venta AS ID_Venta,
	p.ID_Pieza AS Codigo_pieza,
	tp.Nombre AS Nombre_pieza,
	SUM(dv.cantidad) AS Cantidad,
	SUM(dv.subtotal) AS Subtotal
FROM Venta v
INNER JOIN Detalle_venta dv
ON v.ID_Venta = dv.ID_Venta
INNER JOIN Pieza p
ON dv.ID_Pieza = p.ID_Pieza
INNER JOIN Tipo_piezas tp
ON tp.ID_TipoPiezas = p.ID_TipoPiezas
WHERE v.FechaEntrega = GETDATE()
GROUP BY v.ID_Venta, p.ID_Pieza, tp.Nombre
GO





CREATE or alter VIEW ReportesVentasPorCliente
AS
SELECT v.ID_Venta,
 c.RUC AS Codigo_cliente,
 c.Empresa AS Empresa,
 CONCAT(c.SNombre_rep, c.PApell_rep) AS Nombre_representante,
 v.FechaEntrega AS fecha,
 SUM(dv.cantidad) AS Cantidad,
 SUM(p.precio) AS precio,
 SUM(dv.cantidad) * SUM(p.precio) AS Subtotal
FROM Venta v
INNER JOIN Detalle_venta dv
ON v.ID_Venta = dv.ID_Venta
INNER JOIN Cliente c
ON c.RUC = v.ID_Cliente
INNER JOIN Pieza p
ON p.ID_Pieza = dv.ID_Pieza
GROUP BY v.ID_Venta, C.RUC, C.Empresa, CONCAT(c.SNombre_rep, c.PApell_rep), v.FechaEntrega
GO






CREATE or alter VIEW ReportesVentasPorPiezas
AS
SELECT v.ID_Venta AS ID_Venta,
	p.ID_Pieza AS Codigo_pieza,
	tp.Nombre AS Nombre_pieza,
	SUM(dv.cantidad) AS Cantidad,
	SUM(dv.subtotal) AS Subtotal
FROM Venta v
INNER JOIN Detalle_venta dv
ON v.ID_Venta = dv.ID_Venta
INNER JOIN Pieza p
ON dv.ID_Pieza = p.ID_Pieza
INNER JOIN Tipo_piezas tp
ON tp.ID_TipoPiezas = p.ID_TipoPiezas
GROUP BY v.ID_Venta, p.ID_Pieza, tp.Nombre
GO







CREATE or alter VIEW ReportesVentasPorCliente
AS
SELECT v.ID_Venta,
 c.RUC AS Codigo_cliente,
 c.Empresa AS Empresa,
 CONCAT(c.SNombre_rep, c.PApell_rep) AS Nombre_representante,
 v.FechaEntrega AS fecha,
 SUM(dv.cantidad) AS Cantidad,
 SUM(p.precio) AS precio,
 SUM(dv.cantidad) * SUM(p.precio) AS Subtotal,
 tp.Nombre AS Tipo,
 e.Name AS Estado_Pieza
FROM Venta v
INNER JOIN Detalle_venta dv
ON v.ID_Venta = dv.ID_Venta
INNER JOIN Cliente c
ON c.RUC = v.ID_Cliente
INNER JOIN Pieza p
ON p.ID_Pieza = dv.ID_Pieza
INNER JOIN Tipo_piezas tp
ON tp.ID_TipoPiezas = p.ID_TipoPiezas
INNER JOIN Estado_uso e
ON e.ID_Estado = p.ID_Estado
GROUP BY v.ID_Venta, C.RUC, C.Empresa, CONCAT(c.SNombre_rep, c.PApell_rep), v.FechaEntrega
GO






CREATE or alter VIEW InformacionDeVentas
AS
SELECT v.ID_Venta AS ID_Venta,
		v.FechaEntrega AS Fecha_venta,
		P.ID_Pieza AS Codigo_producto,
		tp.Nombre AS Tipo_pieza,
		e.Name AS estado,
		p.precio AS precio_venta,
		dv.cantidad AS cantidad,
		dbo.CalcularSubtotal(p.precio, dv.cantidad) AS subtotal
FROM Venta v
INNER JOIN Detalle_venta dv
ON v.ID_Venta = dv.ID_Venta
INNER JOIN Pieza p
ON dv.ID_Pieza = p.ID_Pieza
INNER JOIN Tipo_piezas tp
ON p.ID_TipoPiezas = tp.ID_TipoPiezas
INNER JOIN Estado_uso e
ON p.ID_Estado = e.ID_Estado
WHERE Fecha_Inicial IS NULL
GO






CREATE or alter VIEW InformacionProveedores
AS
SELECT P.ID_Proveedor,
		Nombre,
		Apellido,
		Empresa,
		Correo,
		Telefono,
		Direccion,
		Tipo
FROM Proveedor p
INNER JOIN Tipo_proveedor tp
ON p.ID_Proveedor = tp.ID_TipoProveedor
GO





CREATE or alter  VIEW InformacionDePedidos
AS
SELECT v.ID_Venta AS ID_Venta,
		v.ID_Cliente AS ID_Cliente,
		v.Fecha_Inicial AS Fecha_inicial,
		v.FechaEntrega AS Fecha_venta,
		P.ID_Pieza AS Codigo_producto,
		tp.Nombre AS Tipo_pieza,
		e.Name AS estado,
		p.precio AS precio_venta,
		dv.cantidad AS cantidad,
		dbo.CalcularSubtotal(p.precio, dv.cantidad) AS subtotal
FROM Venta v
INNER JOIN Detalle_venta dv
ON v.ID_Venta = dv.ID_Venta
INNER JOIN Pieza p
ON dv.ID_Pieza = p.ID_Pieza
INNER JOIN Tipo_piezas tp
ON p.ID_TipoPiezas = tp.ID_TipoPiezas
INNER JOIN Estado_uso e
ON p.ID_Estado = e.ID_Estado
JOIN Cliente c
ON c.RUC = v.ID_Cliente
WHERE Fecha_Inicial IS NOT NULL AND FechaEntrega IS NOT NULL
GO






CREATE VIEW InformacionProveedores
AS
SELECT P.ID_Proveedor,
        Nombre,
        Apellido,
        Empresa,
        Correo,
        Telefono,
        Direccion,
        Tipo
FROM Proveedor p
INNER JOIN Tipo_proveedor tp
ON p.ID_Proveedor = tp.ID_TipoProveedor
GO






CREATE VIEW VistaDePiezas
AS
SELECT p.ID_Pieza AS codigo_pieza,
        prov.Empresa AS empresa,
        e.Name AS estado,
        tp.Nombre As tipo_pieza,
        estante,
        tension,
        precio,
        cantidad,
        circuito,
        dientes,
        Fabricante
FROM Pieza p
INNER JOIN Proveedor prov
ON p.ID_Proveedor = prov.ID_Proveedor
INNER JOIN Estado_uso e
ON e.ID_Estado = p.ID_Estado
INNER JOIN Tipo_piezas tp
ON p.ID_TipoPiezas = tp.ID_TipoPiezas
GO






CREATE VIEW VentasAnuales
AS
SELECT v.ID_Cliente as ID,
		c.PNombre_rep as Nombre,
		c.Empresa as Empresa_Cliente,
		p.ID_Pieza as ID_Pieza,
		dv.cantidad as CantVendida,
		p.precio as PrecioVenta,
		v.Fecha_Inicial as Fecha_Inicial,
		v.FechaEntrega as Fecha_Entrega,
		v.Estado as Estado
FROM Venta v
INNER JOIN Detalle_venta dv
ON dv.ID_Venta = v.ID_Venta
INNER JOIN Cliente c
ON V.ID_Cliente = c.RUC
INNER JOIN Pieza p
on p.ID_Pieza = dv.ID_Pieza WHERE v.FechaEntrega BETWEEN (SELECT DATEADD(year, -1 , GETDATE())) AND (GETDATE())
GO





CREATE VIEW VentasDia
AS
SELECT v.ID_Cliente as ID,
		c.PNombre_rep as Nombre,
		c.Empresa as Empresa_Cliente,
		p.ID_Pieza as ID_Pieza,
		dv.cantidad as CantVendida,
		p.precio as PrecioVenta,
		v.Fecha_Inicial as Fecha_Inicial,
		v.FechaEntrega as Fecha_Entrega,
		v.Estado as Estado
FROM Venta v
INNER JOIN Detalle_venta dv
ON dv.ID_Venta = v.ID_Venta
INNER JOIN Cliente c
ON V.ID_Cliente = c.RUC
INNER JOIN Pieza p
on p.ID_Pieza = dv.ID_Pieza WHERE v.FechaEntrega = GETDATE()
GO