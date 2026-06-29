CREATE TABLE [dbo].[MovimientoEstacionamiento]
(
	[IdMovimiento] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [IdVehiculo] INT NOT NULL, 
    [IdEspacio] INT NOT NULL, 
    [FechaHoraEntrada] DATETIME NOT NULL, 
    [FechaHoraSalida] DATETIME NULL, 
    [EstadoMovimiento] NVARCHAR(50) NOT NULL, 
    [MontoCobrado] NUMERIC(18, 2) NULL, 
    [UsuarioRegistro] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [FK_MovimientoEstacionamiento_Vehiculo] FOREIGN KEY (IdVehiculo) REFERENCES [Vehiculo]([IdVehiculo]), 
    CONSTRAINT [FK_MovimientoEstacionamiento_EspacioEstacionamiento] FOREIGN KEY (IdEspacio) REFERENCES [EspacioEstacionamiento]([IdEspacio])
)
