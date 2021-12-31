Hola a todos/as!!!

Este es un proyecto personal creado por mi, Facundo Mecodangelo.
Contacto
telefono: +549(0351)6193732
instagram: f.meco
correo: facundo.mecodangelo@gmail.com



Recomendaciones:

- Crear una base de datos con el siguiente script:

	USE [DatosPatentest]
GO
/****** Object:  Table [dbo].[Administrador]    Script Date: 31/12/2021 13:30:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Administrador](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[usuario] [varchar](30) NOT NULL,
	[pass] [varchar](30) NOT NULL,
	[estado] [bit] NOT NULL,
UNIQUE NONCLUSTERED 
(
	[usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[automotor]    Script Date: 31/12/2021 13:30:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[automotor](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[marca] [varchar](15) NOT NULL,
	[modelo] [varchar](20) NULL,
	[año] [int] NULL,
	[patente] [char](8) NOT NULL,
	[estado] [varchar](15) NULL,
	[id_numReferencia] [int] NULL,
 CONSTRAINT [PK__automoto__3213E83FEE3BF9DB] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ__automoto__40228D0836951F5D] UNIQUE NONCLUSTERED 
(
	[patente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[propietario]    Script Date: 31/12/2021 13:30:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[propietario](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[numReferencia] [int] NOT NULL,
	[nombre] [varchar](15) NULL,
	[apellido] [varchar](20) NULL,
	[dni] [char](8) NOT NULL,
	[fecha] [date] NULL,
	[sexo] [char](1) NULL,
	[pais] [varchar](15) NULL,
	[provincia] [varchar](15) NULL,
	[domicilio] [varchar](15) NULL,
	[telefono] [int] NULL,
 CONSTRAINT [PK__propieta__3213E83FFFD64D91] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ__propieta__C78DFD3E94EE375E] UNIQUE NONCLUSTERED 
(
	[numReferencia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ__propieta__D87608A737D9D78F] UNIQUE NONCLUSTERED 
(
	[dni] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO




ACLARACION IMPORTANTE:

Terminantemente PROHIBIDO el uso de dicho codigo para uso de comercializacion.
