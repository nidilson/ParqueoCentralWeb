CREATE TABLE [dbo].[Vehiculo]
(
	[IdVehiculo] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Placa] NVARCHAR(8) NOT NULL, 
    [TipoVehiculo] NVARCHAR(50) NOT NULL, 
    [Propietario] NVARCHAR(50) NOT NULL, 
    [Contacto] NVARCHAR(50) NOT NULL
)
