CREATE TABLE [dbo].[EspacioEstacionamiento]
(
	[IdEspacio] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [CodigoEspacio] NVARCHAR(20) NOT NULL, 
    [TipoEspacio] NVARCHAR(50) NOT NULL, 
    [Estado] NVARCHAR(50) NOT NULL, 
    [Activo] BIT NOT NULL
)
