
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/04/2024 16:14:42
-- Generated from EDMX file: C:\Users\Moises\source\repos\GestionColegios\GestionColegios\Models\BDColegio.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [BdColegios];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_RolPermiso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Permisos] DROP CONSTRAINT [FK_RolPermiso];
GO
IF OBJECT_ID(N'[dbo].[FK_RolUsuario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Usuarios] DROP CONSTRAINT [FK_RolUsuario];
GO
IF OBJECT_ID(N'[dbo].[FK_MaestroCursoAcademico]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CursosAcademicos] DROP CONSTRAINT [FK_MaestroCursoAcademico];
GO
IF OBJECT_ID(N'[dbo].[FK_SeccionCursoAcademico]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CursosAcademicos] DROP CONSTRAINT [FK_SeccionCursoAcademico];
GO
IF OBJECT_ID(N'[dbo].[FK_CursoAcademicoEficienciaFisica]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EficienciasFisicas] DROP CONSTRAINT [FK_CursoAcademicoEficienciaFisica];
GO
IF OBJECT_ID(N'[dbo].[FK_EficienciaFisicaEficienciaFisicaEstudiante]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EficienciasFisicasEstudiantes] DROP CONSTRAINT [FK_EficienciaFisicaEficienciaFisicaEstudiante];
GO
IF OBJECT_ID(N'[dbo].[FK_ConsolidadoEficienciaFisicaEficienciaFisica]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EficienciasFisicas] DROP CONSTRAINT [FK_ConsolidadoEficienciaFisicaEficienciaFisica];
GO
IF OBJECT_ID(N'[dbo].[FK_CursoAcademicoCursoAcademicoEstudiante]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CursosAcademicosEstudiantes] DROP CONSTRAINT [FK_CursoAcademicoCursoAcademicoEstudiante];
GO
IF OBJECT_ID(N'[dbo].[FK_EstudianteCursoAcademicoEstudiante]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CursosAcademicosEstudiantes] DROP CONSTRAINT [FK_EstudianteCursoAcademicoEstudiante];
GO
IF OBJECT_ID(N'[dbo].[FK_EstudianteCalificacion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Calificaciones] DROP CONSTRAINT [FK_EstudianteCalificacion];
GO
IF OBJECT_ID(N'[dbo].[FK_MateriaCalificacion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Calificaciones] DROP CONSTRAINT [FK_MateriaCalificacion];
GO
IF OBJECT_ID(N'[dbo].[FK_AñoAcademicoMateria]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Materias] DROP CONSTRAINT [FK_AñoAcademicoMateria];
GO
IF OBJECT_ID(N'[dbo].[FK_AñoAcademicoCursoAcademico]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CursosAcademicos] DROP CONSTRAINT [FK_AñoAcademicoCursoAcademico];
GO
IF OBJECT_ID(N'[dbo].[FK_EstudianteEficienciaFisicaEstudiante]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EficienciasFisicasEstudiantes] DROP CONSTRAINT [FK_EstudianteEficienciaFisicaEstudiante];
GO
IF OBJECT_ID(N'[dbo].[FK_CursoAcademicoControlMerienda]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ControlesMeriendas] DROP CONSTRAINT [FK_CursoAcademicoControlMerienda];
GO
IF OBJECT_ID(N'[dbo].[FK_InventarioAlimentoControlMerienda]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ControlesMeriendas] DROP CONSTRAINT [FK_InventarioAlimentoControlMerienda];
GO
IF OBJECT_ID(N'[dbo].[FK_InventarioAlimentoControlMerienda1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ControlesMeriendas] DROP CONSTRAINT [FK_InventarioAlimentoControlMerienda1];
GO
IF OBJECT_ID(N'[dbo].[FK_InventarioAlimentoControlMerienda2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ControlesMeriendas] DROP CONSTRAINT [FK_InventarioAlimentoControlMerienda2];
GO
IF OBJECT_ID(N'[dbo].[FK_InventarioAlimentoControlMerienda3]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ControlesMeriendas] DROP CONSTRAINT [FK_InventarioAlimentoControlMerienda3];
GO
IF OBJECT_ID(N'[dbo].[FK_InventarioAlimentoControlMerienda4]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ControlesMeriendas] DROP CONSTRAINT [FK_InventarioAlimentoControlMerienda4];
GO
IF OBJECT_ID(N'[dbo].[FK_EstudianteMatricula]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Matriculas] DROP CONSTRAINT [FK_EstudianteMatricula];
GO
IF OBJECT_ID(N'[dbo].[FK_TutorMatricula]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Matriculas] DROP CONSTRAINT [FK_TutorMatricula];
GO
IF OBJECT_ID(N'[dbo].[FK_PeriodosMatricula]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Matriculas] DROP CONSTRAINT [FK_PeriodosMatricula];
GO
IF OBJECT_ID(N'[dbo].[FK_ParcialCalificacion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Calificaciones] DROP CONSTRAINT [FK_ParcialCalificacion];
GO
IF OBJECT_ID(N'[dbo].[FK_SemestreParcial]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Parciales] DROP CONSTRAINT [FK_SemestreParcial];
GO
IF OBJECT_ID(N'[dbo].[FK_SemestreConsolidadoEficienciaFisica]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ConsolidadosEficienciasFisicas] DROP CONSTRAINT [FK_SemestreConsolidadoEficienciaFisica];
GO
IF OBJECT_ID(N'[dbo].[FK_SemestreEficienciaFisica]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EficienciasFisicas] DROP CONSTRAINT [FK_SemestreEficienciaFisica];
GO
IF OBJECT_ID(N'[dbo].[FK_AñoCursoAcademico]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CursosAcademicos] DROP CONSTRAINT [FK_AñoCursoAcademico];
GO
IF OBJECT_ID(N'[dbo].[FK_AñoEficienciaFisica]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EficienciasFisicas] DROP CONSTRAINT [FK_AñoEficienciaFisica];
GO
IF OBJECT_ID(N'[dbo].[FK_AñoConsolidadoEficienciaFisica]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ConsolidadosEficienciasFisicas] DROP CONSTRAINT [FK_AñoConsolidadoEficienciaFisica];
GO
IF OBJECT_ID(N'[dbo].[FK_AñoPeriodos]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Periodos] DROP CONSTRAINT [FK_AñoPeriodos];
GO
IF OBJECT_ID(N'[dbo].[FK_EstudianteControlMerienda]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ControlesMeriendas] DROP CONSTRAINT [FK_EstudianteControlMerienda];
GO
IF OBJECT_ID(N'[dbo].[FK_AñoAcademicoMatricula]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Matriculas] DROP CONSTRAINT [FK_AñoAcademicoMatricula];
GO
IF OBJECT_ID(N'[dbo].[FK_TutorEstudiante]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Estudiantes] DROP CONSTRAINT [FK_TutorEstudiante];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[Permisos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Permisos];
GO
IF OBJECT_ID(N'[dbo].[Usuarios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Usuarios];
GO
IF OBJECT_ID(N'[dbo].[Maestros]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Maestros];
GO
IF OBJECT_ID(N'[dbo].[CursosAcademicos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CursosAcademicos];
GO
IF OBJECT_ID(N'[dbo].[EficienciasFisicas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EficienciasFisicas];
GO
IF OBJECT_ID(N'[dbo].[EficienciasFisicasEstudiantes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EficienciasFisicasEstudiantes];
GO
IF OBJECT_ID(N'[dbo].[CursosAcademicosEstudiantes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CursosAcademicosEstudiantes];
GO
IF OBJECT_ID(N'[dbo].[AñosAcademicos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AñosAcademicos];
GO
IF OBJECT_ID(N'[dbo].[Calificaciones]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Calificaciones];
GO
IF OBJECT_ID(N'[dbo].[Estudiantes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Estudiantes];
GO
IF OBJECT_ID(N'[dbo].[Materias]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Materias];
GO
IF OBJECT_ID(N'[dbo].[Secciones]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Secciones];
GO
IF OBJECT_ID(N'[dbo].[ConsolidadosEficienciasFisicas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ConsolidadosEficienciasFisicas];
GO
IF OBJECT_ID(N'[dbo].[InventariosAlimentos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InventariosAlimentos];
GO
IF OBJECT_ID(N'[dbo].[ControlesMeriendas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ControlesMeriendas];
GO
IF OBJECT_ID(N'[dbo].[Matriculas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Matriculas];
GO
IF OBJECT_ID(N'[dbo].[Tutores]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tutores];
GO
IF OBJECT_ID(N'[dbo].[Periodos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Periodos];
GO
IF OBJECT_ID(N'[dbo].[Parciales]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Parciales];
GO
IF OBJECT_ID(N'[dbo].[Semestres]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Semestres];
GO
IF OBJECT_ID(N'[dbo].[Años]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Años];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Codigo] nvarchar(max)  NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [FechaModificacion] datetime  NOT NULL,
    [Activo] bit  NOT NULL
);
GO

-- Creating table 'Permisos'
CREATE TABLE [dbo].[Permisos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [FechaModificacion] datetime  NOT NULL,
    [Activo] bit  NOT NULL,
    [RolId] int  NOT NULL
);
GO

-- Creating table 'Usuarios'
CREATE TABLE [dbo].[Usuarios] (
    [CodigoUsuario] nvarchar(max)  NOT NULL,
    [Id] int IDENTITY(1,1) NOT NULL,
    [NombreUsuario] nvarchar(max)  NOT NULL,
    [ClaveHash] nvarchar(max)  NOT NULL,
    [CorreoRecuperacion] nvarchar(max)  NOT NULL,
    [FechaModificacion] datetime  NOT NULL,
    [Activo] bit  NOT NULL,
    [RolId] int  NOT NULL
);
GO

-- Creating table 'Maestros'
CREATE TABLE [dbo].[Maestros] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Codigo] nvarchar(max)  NOT NULL,
    [Cedula] nvarchar(max)  NOT NULL,
    [Nombres] nvarchar(max)  NOT NULL,
    [Apellidos] nvarchar(max)  NOT NULL,
    [Sexo] nvarchar(max)  NOT NULL,
    [Celular] nvarchar(max)  NOT NULL,
    [Direccion] nvarchar(max)  NOT NULL,
    [Especialidad] nvarchar(max)  NOT NULL,
    [FechaContratacion] datetime  NOT NULL,
    [HorarioTrabajo] nvarchar(max)  NOT NULL,
    [Nivel] nvarchar(max)  NOT NULL,
    [FechaModificacion] datetime  NOT NULL,
    [Activo] bit  NOT NULL
);
GO

-- Creating table 'CursosAcademicos'
CREATE TABLE [dbo].[CursosAcademicos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [FechaModificacion] datetime  NOT NULL,
    [Activo] bit  NOT NULL,
    [MaestroId] int  NOT NULL,
    [SeccionId] int  NOT NULL,
    [AñoAcademicoId] int  NOT NULL,
    [AñoId] int  NOT NULL
);
GO

-- Creating table 'EficienciasFisicas'
CREATE TABLE [dbo].[EficienciasFisicas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FechaModificacion] datetime  NOT NULL,
    [Activo] bit  NOT NULL,
    [CursoAcademicoId] int  NOT NULL,
    [ConsolidadoEficienciaFisicaId] int  NOT NULL,
    [SemestreId] int  NOT NULL,
    [AñoId] int  NOT NULL
);
GO

-- Creating table 'EficienciasFisicasEstudiantes'
CREATE TABLE [dbo].[EficienciasFisicasEstudiantes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [NombresApellidos] nvarchar(max)  NOT NULL,
    [Edad] int  NOT NULL,
    [Genero] nvarchar(max)  NOT NULL,
    [Peso_lb] decimal(18,0)  NULL,
    [Talla_cm] int  NULL,
    [Velocidad_seg] decimal(18,0)  NULL,
    [Resistencia_min_seg] decimal(18,0)  NULL,
    [Salto_cm] int  NULL,
    [Pechadas_repet] int  NULL,
    [Abdominales_repet] int  NULL,
    [FechaModificacion] datetime  NOT NULL,
    [Activo] bit  NOT NULL,
    [EficienciaFisicaId] int  NOT NULL,
    [EstudianteId] int  NOT NULL
);
GO

-- Creating table 'CursosAcademicosEstudiantes'
CREATE TABLE [dbo].[CursosAcademicosEstudiantes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Promedio] decimal(18,0)  NOT NULL,
    [Aprobado] bit  NOT NULL,
    [Estado] nvarchar(max)  NOT NULL,
    [FechaModificacion] datetime  NOT NULL,
    [Activo] bit  NOT NULL,
    [CursoAcademicoId] int  NOT NULL,
    [EstudianteId] int  NOT NULL
);
GO

-- Creating table 'AñosAcademicos'
CREATE TABLE [dbo].[AñosAcademicos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [Nivel] nvarchar(max)  NOT NULL,
    [Activo] bit  NOT NULL
);
GO

-- Creating table 'Calificaciones'
CREATE TABLE [dbo].[Calificaciones] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [NCuantitativa] decimal(18,0)  NOT NULL,
    [NCualitativa] nvarchar(max)  NULL,
    [Activo] bit  NOT NULL,
    [EstudianteId] int  NOT NULL,
    [MateriaId] int  NOT NULL,
    [ParcialId] int  NOT NULL
);
GO

-- Creating table 'Estudiantes'
CREATE TABLE [dbo].[Estudiantes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CodigoEstudiante] nvarchar(max)  NOT NULL,
    [Nombres] nvarchar(max)  NOT NULL,
    [Apellidos] nvarchar(max)  NOT NULL,
    [FechaNacimiento] datetime  NOT NULL,
    [Edad] int  NOT NULL,
    [Sexo] nvarchar(max)  NOT NULL,
    [Direccion] nvarchar(max)  NOT NULL,
    [FechaModificacion] datetime  NOT NULL,
    [Activo] bit  NOT NULL,
    [TutorId] int  NOT NULL
);
GO

-- Creating table 'Materias'
CREATE TABLE [dbo].[Materias] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CodigoMateria] nvarchar(max)  NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [FechaModificacion] datetime  NOT NULL,
    [Activo] bit  NOT NULL,
    [AñoAcademicoId] int  NOT NULL
);
GO

-- Creating table 'Secciones'
CREATE TABLE [dbo].[Secciones] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [CapacidadEstudiantes] int  NOT NULL,
    [FechaModificacion] datetime  NOT NULL,
    [Activo] bit  NOT NULL
);
GO

-- Creating table 'ConsolidadosEficienciasFisicas'
CREATE TABLE [dbo].[ConsolidadosEficienciasFisicas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Edad] int  NOT NULL,
    [Genero] nvarchar(max)  NOT NULL,
    [CantidadAlumnos] int  NOT NULL,
    [PromedioPeso] decimal(18,0)  NULL,
    [PromedioTalla] int  NULL,
    [PromedioVelocidad] decimal(18,0)  NULL,
    [PromedioResistencia] decimal(18,0)  NULL,
    [PromedioSalto] int  NULL,
    [PromedioPechadas] int  NULL,
    [PromedioAbdominales] int  NULL,
    [Observaciones] nvarchar(max)  NOT NULL,
    [FechaConsolidado] datetime  NOT NULL,
    [FechaModificacion] datetime  NOT NULL,
    [Activo] bit  NOT NULL,
    [SemestreId] int  NOT NULL,
    [AñoId] int  NOT NULL
);
GO

-- Creating table 'InventariosAlimentos'
CREATE TABLE [dbo].[InventariosAlimentos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Codigo] nvarchar(max)  NOT NULL,
    [NombreAlimento] nvarchar(max)  NOT NULL,
    [Stock] decimal(18,0)  NOT NULL,
    [UnidadDeMedida] nvarchar(max)  NOT NULL,
    [FechaReabastecimiento] datetime  NOT NULL,
    [FechaModificacion] datetime  NOT NULL,
    [Activo] bit  NOT NULL
);
GO

-- Creating table 'ControlesMeriendas'
CREATE TABLE [dbo].[ControlesMeriendas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Codigo] nvarchar(max)  NOT NULL,
    [FechaEntrega] datetime  NOT NULL,
    [AsistenciaEsperadaMujeres] int  NOT NULL,
    [AsistenciaEsperadaTotal] int  NOT NULL,
    [AsistenciaRealMujeres] int  NULL,
    [AsistenciaRealTotal] int  NOT NULL,
    [EAceite] decimal(18,0)  NOT NULL,
    [EArroz] decimal(18,0)  NOT NULL,
    [ECereal] decimal(18,0)  NOT NULL,
    [EFrijoles] decimal(18,0)  NOT NULL,
    [EMaiz] decimal(18,0)  NOT NULL,
    [FirmaDocente] nvarchar(max)  NOT NULL,
    [CedulaTutor] nvarchar(max)  NOT NULL,
    [FirmaTutor] nvarchar(max)  NOT NULL,
    [FechaModificacion] datetime  NOT NULL,
    [Activo] bit  NOT NULL,
    [CursoAcademicoId] int  NOT NULL,
    [InventarioAlimentoId] int  NOT NULL,
    [InventarioAlimentoId2] int  NOT NULL,
    [InventarioAlimentoId1] int  NOT NULL,
    [InventarioAlimentoId3] int  NOT NULL,
    [InventarioAlimentoId4] int  NOT NULL,
    [InventarioAlimentoId5] int  NOT NULL,
    [EstudianteId] int  NOT NULL
);
GO

-- Creating table 'Matriculas'
CREATE TABLE [dbo].[Matriculas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Codigo] nvarchar(max)  NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [Continuidad] bit  NOT NULL,
    [Traslado] bit  NOT NULL,
    [Repitente] bit  NOT NULL,
    [FechaMatricula] nvarchar(max)  NOT NULL,
    [FechaModificacion] datetime  NOT NULL,
    [Activo] bit  NOT NULL,
    [EstudianteId] int  NOT NULL,
    [TutorId] int  NOT NULL,
    [PeriodosId] int  NOT NULL,
    [AñoId] int  NOT NULL,
    [AñoAcademicoId] int  NOT NULL,
    [Aprobado] bit  NOT NULL
);
GO

-- Creating table 'Tutores'
CREATE TABLE [dbo].[Tutores] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombres] nvarchar(max)  NOT NULL,
    [Apellidos] nvarchar(max)  NOT NULL,
    [Cedula] nvarchar(max)  NOT NULL,
    [RelacionConElEstudiante] nvarchar(max)  NOT NULL,
    [Direccion] nvarchar(max)  NOT NULL,
    [Celular] nvarchar(max)  NOT NULL,
    [FechaModificacion] datetime  NOT NULL,
    [Activo] bit  NOT NULL
);
GO

-- Creating table 'Periodos'
CREATE TABLE [dbo].[Periodos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Numero] int  NOT NULL,
    [FechaModificacion] datetime  NOT NULL,
    [Activo] bit  NOT NULL,
    [AñoId] int  NOT NULL
);
GO

-- Creating table 'Parciales'
CREATE TABLE [dbo].[Parciales] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [SemestreId] int  NOT NULL
);
GO

-- Creating table 'Semestres'
CREATE TABLE [dbo].[Semestres] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Años'
CREATE TABLE [dbo].[Años] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Permisos'
ALTER TABLE [dbo].[Permisos]
ADD CONSTRAINT [PK_Permisos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Usuarios'
ALTER TABLE [dbo].[Usuarios]
ADD CONSTRAINT [PK_Usuarios]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Maestros'
ALTER TABLE [dbo].[Maestros]
ADD CONSTRAINT [PK_Maestros]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CursosAcademicos'
ALTER TABLE [dbo].[CursosAcademicos]
ADD CONSTRAINT [PK_CursosAcademicos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EficienciasFisicas'
ALTER TABLE [dbo].[EficienciasFisicas]
ADD CONSTRAINT [PK_EficienciasFisicas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EficienciasFisicasEstudiantes'
ALTER TABLE [dbo].[EficienciasFisicasEstudiantes]
ADD CONSTRAINT [PK_EficienciasFisicasEstudiantes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CursosAcademicosEstudiantes'
ALTER TABLE [dbo].[CursosAcademicosEstudiantes]
ADD CONSTRAINT [PK_CursosAcademicosEstudiantes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AñosAcademicos'
ALTER TABLE [dbo].[AñosAcademicos]
ADD CONSTRAINT [PK_AñosAcademicos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Calificaciones'
ALTER TABLE [dbo].[Calificaciones]
ADD CONSTRAINT [PK_Calificaciones]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Estudiantes'
ALTER TABLE [dbo].[Estudiantes]
ADD CONSTRAINT [PK_Estudiantes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Materias'
ALTER TABLE [dbo].[Materias]
ADD CONSTRAINT [PK_Materias]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Secciones'
ALTER TABLE [dbo].[Secciones]
ADD CONSTRAINT [PK_Secciones]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ConsolidadosEficienciasFisicas'
ALTER TABLE [dbo].[ConsolidadosEficienciasFisicas]
ADD CONSTRAINT [PK_ConsolidadosEficienciasFisicas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InventariosAlimentos'
ALTER TABLE [dbo].[InventariosAlimentos]
ADD CONSTRAINT [PK_InventariosAlimentos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id], [AsistenciaRealTotal] in table 'ControlesMeriendas'
ALTER TABLE [dbo].[ControlesMeriendas]
ADD CONSTRAINT [PK_ControlesMeriendas]
    PRIMARY KEY CLUSTERED ([Id], [AsistenciaRealTotal] ASC);
GO

-- Creating primary key on [Id] in table 'Matriculas'
ALTER TABLE [dbo].[Matriculas]
ADD CONSTRAINT [PK_Matriculas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tutores'
ALTER TABLE [dbo].[Tutores]
ADD CONSTRAINT [PK_Tutores]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Periodos'
ALTER TABLE [dbo].[Periodos]
ADD CONSTRAINT [PK_Periodos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Parciales'
ALTER TABLE [dbo].[Parciales]
ADD CONSTRAINT [PK_Parciales]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Semestres'
ALTER TABLE [dbo].[Semestres]
ADD CONSTRAINT [PK_Semestres]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Años'
ALTER TABLE [dbo].[Años]
ADD CONSTRAINT [PK_Años]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [RolId] in table 'Permisos'
ALTER TABLE [dbo].[Permisos]
ADD CONSTRAINT [FK_RolPermiso]
    FOREIGN KEY ([RolId])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RolPermiso'
CREATE INDEX [IX_FK_RolPermiso]
ON [dbo].[Permisos]
    ([RolId]);
GO

-- Creating foreign key on [RolId] in table 'Usuarios'
ALTER TABLE [dbo].[Usuarios]
ADD CONSTRAINT [FK_RolUsuario]
    FOREIGN KEY ([RolId])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RolUsuario'
CREATE INDEX [IX_FK_RolUsuario]
ON [dbo].[Usuarios]
    ([RolId]);
GO

-- Creating foreign key on [MaestroId] in table 'CursosAcademicos'
ALTER TABLE [dbo].[CursosAcademicos]
ADD CONSTRAINT [FK_MaestroCursoAcademico]
    FOREIGN KEY ([MaestroId])
    REFERENCES [dbo].[Maestros]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MaestroCursoAcademico'
CREATE INDEX [IX_FK_MaestroCursoAcademico]
ON [dbo].[CursosAcademicos]
    ([MaestroId]);
GO

-- Creating foreign key on [SeccionId] in table 'CursosAcademicos'
ALTER TABLE [dbo].[CursosAcademicos]
ADD CONSTRAINT [FK_SeccionCursoAcademico]
    FOREIGN KEY ([SeccionId])
    REFERENCES [dbo].[Secciones]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SeccionCursoAcademico'
CREATE INDEX [IX_FK_SeccionCursoAcademico]
ON [dbo].[CursosAcademicos]
    ([SeccionId]);
GO

-- Creating foreign key on [CursoAcademicoId] in table 'EficienciasFisicas'
ALTER TABLE [dbo].[EficienciasFisicas]
ADD CONSTRAINT [FK_CursoAcademicoEficienciaFisica]
    FOREIGN KEY ([CursoAcademicoId])
    REFERENCES [dbo].[CursosAcademicos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CursoAcademicoEficienciaFisica'
CREATE INDEX [IX_FK_CursoAcademicoEficienciaFisica]
ON [dbo].[EficienciasFisicas]
    ([CursoAcademicoId]);
GO

-- Creating foreign key on [EficienciaFisicaId] in table 'EficienciasFisicasEstudiantes'
ALTER TABLE [dbo].[EficienciasFisicasEstudiantes]
ADD CONSTRAINT [FK_EficienciaFisicaEficienciaFisicaEstudiante]
    FOREIGN KEY ([EficienciaFisicaId])
    REFERENCES [dbo].[EficienciasFisicas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EficienciaFisicaEficienciaFisicaEstudiante'
CREATE INDEX [IX_FK_EficienciaFisicaEficienciaFisicaEstudiante]
ON [dbo].[EficienciasFisicasEstudiantes]
    ([EficienciaFisicaId]);
GO

-- Creating foreign key on [ConsolidadoEficienciaFisicaId] in table 'EficienciasFisicas'
ALTER TABLE [dbo].[EficienciasFisicas]
ADD CONSTRAINT [FK_ConsolidadoEficienciaFisicaEficienciaFisica]
    FOREIGN KEY ([ConsolidadoEficienciaFisicaId])
    REFERENCES [dbo].[ConsolidadosEficienciasFisicas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ConsolidadoEficienciaFisicaEficienciaFisica'
CREATE INDEX [IX_FK_ConsolidadoEficienciaFisicaEficienciaFisica]
ON [dbo].[EficienciasFisicas]
    ([ConsolidadoEficienciaFisicaId]);
GO

-- Creating foreign key on [CursoAcademicoId] in table 'CursosAcademicosEstudiantes'
ALTER TABLE [dbo].[CursosAcademicosEstudiantes]
ADD CONSTRAINT [FK_CursoAcademicoCursoAcademicoEstudiante]
    FOREIGN KEY ([CursoAcademicoId])
    REFERENCES [dbo].[CursosAcademicos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CursoAcademicoCursoAcademicoEstudiante'
CREATE INDEX [IX_FK_CursoAcademicoCursoAcademicoEstudiante]
ON [dbo].[CursosAcademicosEstudiantes]
    ([CursoAcademicoId]);
GO

-- Creating foreign key on [EstudianteId] in table 'CursosAcademicosEstudiantes'
ALTER TABLE [dbo].[CursosAcademicosEstudiantes]
ADD CONSTRAINT [FK_EstudianteCursoAcademicoEstudiante]
    FOREIGN KEY ([EstudianteId])
    REFERENCES [dbo].[Estudiantes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EstudianteCursoAcademicoEstudiante'
CREATE INDEX [IX_FK_EstudianteCursoAcademicoEstudiante]
ON [dbo].[CursosAcademicosEstudiantes]
    ([EstudianteId]);
GO

-- Creating foreign key on [EstudianteId] in table 'Calificaciones'
ALTER TABLE [dbo].[Calificaciones]
ADD CONSTRAINT [FK_EstudianteCalificacion]
    FOREIGN KEY ([EstudianteId])
    REFERENCES [dbo].[Estudiantes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EstudianteCalificacion'
CREATE INDEX [IX_FK_EstudianteCalificacion]
ON [dbo].[Calificaciones]
    ([EstudianteId]);
GO

-- Creating foreign key on [MateriaId] in table 'Calificaciones'
ALTER TABLE [dbo].[Calificaciones]
ADD CONSTRAINT [FK_MateriaCalificacion]
    FOREIGN KEY ([MateriaId])
    REFERENCES [dbo].[Materias]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MateriaCalificacion'
CREATE INDEX [IX_FK_MateriaCalificacion]
ON [dbo].[Calificaciones]
    ([MateriaId]);
GO

-- Creating foreign key on [AñoAcademicoId] in table 'Materias'
ALTER TABLE [dbo].[Materias]
ADD CONSTRAINT [FK_AñoAcademicoMateria]
    FOREIGN KEY ([AñoAcademicoId])
    REFERENCES [dbo].[AñosAcademicos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AñoAcademicoMateria'
CREATE INDEX [IX_FK_AñoAcademicoMateria]
ON [dbo].[Materias]
    ([AñoAcademicoId]);
GO

-- Creating foreign key on [AñoAcademicoId] in table 'CursosAcademicos'
ALTER TABLE [dbo].[CursosAcademicos]
ADD CONSTRAINT [FK_AñoAcademicoCursoAcademico]
    FOREIGN KEY ([AñoAcademicoId])
    REFERENCES [dbo].[AñosAcademicos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AñoAcademicoCursoAcademico'
CREATE INDEX [IX_FK_AñoAcademicoCursoAcademico]
ON [dbo].[CursosAcademicos]
    ([AñoAcademicoId]);
GO

-- Creating foreign key on [EstudianteId] in table 'EficienciasFisicasEstudiantes'
ALTER TABLE [dbo].[EficienciasFisicasEstudiantes]
ADD CONSTRAINT [FK_EstudianteEficienciaFisicaEstudiante]
    FOREIGN KEY ([EstudianteId])
    REFERENCES [dbo].[Estudiantes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EstudianteEficienciaFisicaEstudiante'
CREATE INDEX [IX_FK_EstudianteEficienciaFisicaEstudiante]
ON [dbo].[EficienciasFisicasEstudiantes]
    ([EstudianteId]);
GO

-- Creating foreign key on [CursoAcademicoId] in table 'ControlesMeriendas'
ALTER TABLE [dbo].[ControlesMeriendas]
ADD CONSTRAINT [FK_CursoAcademicoControlMerienda]
    FOREIGN KEY ([CursoAcademicoId])
    REFERENCES [dbo].[CursosAcademicos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CursoAcademicoControlMerienda'
CREATE INDEX [IX_FK_CursoAcademicoControlMerienda]
ON [dbo].[ControlesMeriendas]
    ([CursoAcademicoId]);
GO

-- Creating foreign key on [InventarioAlimentoId1] in table 'ControlesMeriendas'
ALTER TABLE [dbo].[ControlesMeriendas]
ADD CONSTRAINT [FK_InventarioAlimentoControlMerienda]
    FOREIGN KEY ([InventarioAlimentoId1])
    REFERENCES [dbo].[InventariosAlimentos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InventarioAlimentoControlMerienda'
CREATE INDEX [IX_FK_InventarioAlimentoControlMerienda]
ON [dbo].[ControlesMeriendas]
    ([InventarioAlimentoId1]);
GO

-- Creating foreign key on [InventarioAlimentoId2] in table 'ControlesMeriendas'
ALTER TABLE [dbo].[ControlesMeriendas]
ADD CONSTRAINT [FK_InventarioAlimentoControlMerienda1]
    FOREIGN KEY ([InventarioAlimentoId2])
    REFERENCES [dbo].[InventariosAlimentos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InventarioAlimentoControlMerienda1'
CREATE INDEX [IX_FK_InventarioAlimentoControlMerienda1]
ON [dbo].[ControlesMeriendas]
    ([InventarioAlimentoId2]);
GO

-- Creating foreign key on [InventarioAlimentoId3] in table 'ControlesMeriendas'
ALTER TABLE [dbo].[ControlesMeriendas]
ADD CONSTRAINT [FK_InventarioAlimentoControlMerienda2]
    FOREIGN KEY ([InventarioAlimentoId3])
    REFERENCES [dbo].[InventariosAlimentos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InventarioAlimentoControlMerienda2'
CREATE INDEX [IX_FK_InventarioAlimentoControlMerienda2]
ON [dbo].[ControlesMeriendas]
    ([InventarioAlimentoId3]);
GO

-- Creating foreign key on [InventarioAlimentoId4] in table 'ControlesMeriendas'
ALTER TABLE [dbo].[ControlesMeriendas]
ADD CONSTRAINT [FK_InventarioAlimentoControlMerienda3]
    FOREIGN KEY ([InventarioAlimentoId4])
    REFERENCES [dbo].[InventariosAlimentos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InventarioAlimentoControlMerienda3'
CREATE INDEX [IX_FK_InventarioAlimentoControlMerienda3]
ON [dbo].[ControlesMeriendas]
    ([InventarioAlimentoId4]);
GO

-- Creating foreign key on [InventarioAlimentoId5] in table 'ControlesMeriendas'
ALTER TABLE [dbo].[ControlesMeriendas]
ADD CONSTRAINT [FK_InventarioAlimentoControlMerienda4]
    FOREIGN KEY ([InventarioAlimentoId5])
    REFERENCES [dbo].[InventariosAlimentos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InventarioAlimentoControlMerienda4'
CREATE INDEX [IX_FK_InventarioAlimentoControlMerienda4]
ON [dbo].[ControlesMeriendas]
    ([InventarioAlimentoId5]);
GO

-- Creating foreign key on [EstudianteId] in table 'Matriculas'
ALTER TABLE [dbo].[Matriculas]
ADD CONSTRAINT [FK_EstudianteMatricula]
    FOREIGN KEY ([EstudianteId])
    REFERENCES [dbo].[Estudiantes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EstudianteMatricula'
CREATE INDEX [IX_FK_EstudianteMatricula]
ON [dbo].[Matriculas]
    ([EstudianteId]);
GO

-- Creating foreign key on [TutorId] in table 'Matriculas'
ALTER TABLE [dbo].[Matriculas]
ADD CONSTRAINT [FK_TutorMatricula]
    FOREIGN KEY ([TutorId])
    REFERENCES [dbo].[Tutores]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TutorMatricula'
CREATE INDEX [IX_FK_TutorMatricula]
ON [dbo].[Matriculas]
    ([TutorId]);
GO

-- Creating foreign key on [PeriodosId] in table 'Matriculas'
ALTER TABLE [dbo].[Matriculas]
ADD CONSTRAINT [FK_PeriodosMatricula]
    FOREIGN KEY ([PeriodosId])
    REFERENCES [dbo].[Periodos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PeriodosMatricula'
CREATE INDEX [IX_FK_PeriodosMatricula]
ON [dbo].[Matriculas]
    ([PeriodosId]);
GO

-- Creating foreign key on [ParcialId] in table 'Calificaciones'
ALTER TABLE [dbo].[Calificaciones]
ADD CONSTRAINT [FK_ParcialCalificacion]
    FOREIGN KEY ([ParcialId])
    REFERENCES [dbo].[Parciales]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ParcialCalificacion'
CREATE INDEX [IX_FK_ParcialCalificacion]
ON [dbo].[Calificaciones]
    ([ParcialId]);
GO

-- Creating foreign key on [SemestreId] in table 'Parciales'
ALTER TABLE [dbo].[Parciales]
ADD CONSTRAINT [FK_SemestreParcial]
    FOREIGN KEY ([SemestreId])
    REFERENCES [dbo].[Semestres]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SemestreParcial'
CREATE INDEX [IX_FK_SemestreParcial]
ON [dbo].[Parciales]
    ([SemestreId]);
GO

-- Creating foreign key on [SemestreId] in table 'ConsolidadosEficienciasFisicas'
ALTER TABLE [dbo].[ConsolidadosEficienciasFisicas]
ADD CONSTRAINT [FK_SemestreConsolidadoEficienciaFisica]
    FOREIGN KEY ([SemestreId])
    REFERENCES [dbo].[Semestres]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SemestreConsolidadoEficienciaFisica'
CREATE INDEX [IX_FK_SemestreConsolidadoEficienciaFisica]
ON [dbo].[ConsolidadosEficienciasFisicas]
    ([SemestreId]);
GO

-- Creating foreign key on [SemestreId] in table 'EficienciasFisicas'
ALTER TABLE [dbo].[EficienciasFisicas]
ADD CONSTRAINT [FK_SemestreEficienciaFisica]
    FOREIGN KEY ([SemestreId])
    REFERENCES [dbo].[Semestres]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SemestreEficienciaFisica'
CREATE INDEX [IX_FK_SemestreEficienciaFisica]
ON [dbo].[EficienciasFisicas]
    ([SemestreId]);
GO

-- Creating foreign key on [AñoId] in table 'CursosAcademicos'
ALTER TABLE [dbo].[CursosAcademicos]
ADD CONSTRAINT [FK_AñoCursoAcademico]
    FOREIGN KEY ([AñoId])
    REFERENCES [dbo].[Años]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AñoCursoAcademico'
CREATE INDEX [IX_FK_AñoCursoAcademico]
ON [dbo].[CursosAcademicos]
    ([AñoId]);
GO

-- Creating foreign key on [AñoId] in table 'EficienciasFisicas'
ALTER TABLE [dbo].[EficienciasFisicas]
ADD CONSTRAINT [FK_AñoEficienciaFisica]
    FOREIGN KEY ([AñoId])
    REFERENCES [dbo].[Años]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AñoEficienciaFisica'
CREATE INDEX [IX_FK_AñoEficienciaFisica]
ON [dbo].[EficienciasFisicas]
    ([AñoId]);
GO

-- Creating foreign key on [AñoId] in table 'ConsolidadosEficienciasFisicas'
ALTER TABLE [dbo].[ConsolidadosEficienciasFisicas]
ADD CONSTRAINT [FK_AñoConsolidadoEficienciaFisica]
    FOREIGN KEY ([AñoId])
    REFERENCES [dbo].[Años]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AñoConsolidadoEficienciaFisica'
CREATE INDEX [IX_FK_AñoConsolidadoEficienciaFisica]
ON [dbo].[ConsolidadosEficienciasFisicas]
    ([AñoId]);
GO

-- Creating foreign key on [AñoId] in table 'Periodos'
ALTER TABLE [dbo].[Periodos]
ADD CONSTRAINT [FK_AñoPeriodos]
    FOREIGN KEY ([AñoId])
    REFERENCES [dbo].[Años]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AñoPeriodos'
CREATE INDEX [IX_FK_AñoPeriodos]
ON [dbo].[Periodos]
    ([AñoId]);
GO

-- Creating foreign key on [EstudianteId] in table 'ControlesMeriendas'
ALTER TABLE [dbo].[ControlesMeriendas]
ADD CONSTRAINT [FK_EstudianteControlMerienda]
    FOREIGN KEY ([EstudianteId])
    REFERENCES [dbo].[Estudiantes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EstudianteControlMerienda'
CREATE INDEX [IX_FK_EstudianteControlMerienda]
ON [dbo].[ControlesMeriendas]
    ([EstudianteId]);
GO

-- Creating foreign key on [AñoAcademicoId] in table 'Matriculas'
ALTER TABLE [dbo].[Matriculas]
ADD CONSTRAINT [FK_AñoAcademicoMatricula]
    FOREIGN KEY ([AñoAcademicoId])
    REFERENCES [dbo].[AñosAcademicos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AñoAcademicoMatricula'
CREATE INDEX [IX_FK_AñoAcademicoMatricula]
ON [dbo].[Matriculas]
    ([AñoAcademicoId]);
GO

-- Creating foreign key on [TutorId] in table 'Estudiantes'
ALTER TABLE [dbo].[Estudiantes]
ADD CONSTRAINT [FK_TutorEstudiante]
    FOREIGN KEY ([TutorId])
    REFERENCES [dbo].[Tutores]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TutorEstudiante'
CREATE INDEX [IX_FK_TutorEstudiante]
ON [dbo].[Estudiantes]
    ([TutorId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------