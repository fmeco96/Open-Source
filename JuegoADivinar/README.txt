Hola a todos!

El siguiente código ha sido programado con C# y Framework visual WPF.
Es de código abierto, para quellos que están comenzando a programar.
El uso del mismo para ventas y/o negocios de capital no está permitido.

Sugerencias a tener en cuenta:
- Debes crear una base de datos con Sql.
- Conectar la base de datos (preferentemente con los parámetros del código y nombres del código).
- La bd tiene la siguiente estructura:

CREATE TABLE [dbo].[Puntaje] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [nombre]     NVARCHAR (12) NOT NULL,
    [puntuacion] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([nombre] ASC)
);

Cualquier consulta pueden ingresarla en mi instagram personal: @f.meco

Un saludo y que sigas programando!