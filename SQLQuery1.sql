
--Procedimientos almacenados para Usuarios

CREATE PROCEDURE MostrarUsuarios
AS
BEGIN
    SELECT IDUsuario, Nombre, CorreoElectronico
    FROM Usuarios;
END;
go

CREATE PROCEDURE MostrarUsuarioPorId
    @IDUsuario INT
AS
BEGIN
    SELECT IDUsuario, Nombre, CorreoElectronico
    FROM Usuarios
    WHERE IDUsuario = @IDUsuario
END;
go

CREATE PROCEDURE ActualizarUsuario
    @IDUsuario INT,
    @Nombre VARCHAR(100),
    @CorreoElectronico VARCHAR(100),
    @Contrase�a VARCHAR(50)
AS
BEGIN
    UPDATE Usuarios
    SET
        Nombre = @Nombre,
        CorreoElectronico = @CorreoElectronico,
        Contrase�a = @Contrase�a
    WHERE IDUsuario = @IDUsuario;
END;

go

CREATE PROCEDURE EliminarUsuario
    @IDUsuario INT
AS
BEGIN
    DELETE FROM Usuarios
    WHERE IDUsuario = @IDUsuario;
END;
go


--Procedimientos almacenados para Tareas

CREATE PROCEDURE MostrarTareas
AS
BEGIN
    SELECT IDTarea, Nombre, Descripcion, FechaVencimiento, Prioridad, Estado
    FROM Tareas;
END;
go

CREATE PROCEDURE MostrarTareasPorId
    @IDTarea INT
AS
BEGIN
    SELECT @IDTarea, Nombre, Descripcion, FechaVencimiento, Prioridad, Estado
    FROM Tareas
    WHERE IDTarea = @IDTarea
END;
go

CREATE PROCEDURE ActualizarTarea
    @IDTarea INT,
    @Nombre VARCHAR(255),
    @Descripcion VARCHAR(255),
    @FehaVencimiento date,
	@Prioridad  int,
	@Estado Varchar (50)
AS
BEGIN
    UPDATE Tareas
    SET
        Nombre = @Nombre,
        Descripcion = @Descripcion,
        FechaVencimiento = @FehaVencimiento,
		Prioridad = @Prioridad,
		Estado =@Estado
    WHERE IDTarea = @IDTarea;
END;

go

CREATE PROCEDURE EliminarTarea
    @IDTarea INT
AS
BEGIN
    DELETE FROM Tareas
    WHERE IDTarea = @IDTarea;
END;
go

CREATE PROCEDURE InsertarTareas
    @Nombre VARCHAR(255),
    @Descripcion VARCHAR(255),
    @FechaVencimiento date,
	@Prioridad int,
	@Estado Varchar (255)
AS
BEGIN
    INSERT INTO Tareas(Nombre, Descripcion,FechaVencimiento,Prioridad,Estado)
    VALUES (@Nombre,@Descripcion, @FechaVencimiento, @Prioridad, @Estado);
END;
go
--Procedimientos almacenados para Etiquetas


CREATE PROCEDURE MostrarEtiquetas
AS
BEGIN
    SELECT IDEtiqueta, Nombre
    FROM Etiquetas;
END;
go

CREATE PROCEDURE MostrarEtiquetaPorId
    @IDEtiqueta INT
AS
BEGIN
    SELECT IDEtiqueta, Nombre
    FROM Etiquetas
    WHERE IDEtiqueta = @IDEtiqueta
END;
go

CREATE PROCEDURE ActualizarEtiqueta
    @IDEtiqueta int,
	@Nombre Varchar (100)
AS
BEGIN
    UPDATE Etiquetas
    SET
        Nombre = @Nombre
    WHERE IDEtiqueta = @IDEtiqueta;
END;

go

CREATE PROCEDURE EliminarEtiqueta
    @IDetiqueta INT
AS
BEGIN
    DELETE FROM Etiquetas
    WHERE IDEtiqueta = @IDetiqueta;
END;
go

CREATE PROCEDURE InsertarEtiqueta
    @Nombre VARCHAR(100)
   
AS
BEGIN
    INSERT INTO Etiquetas(Nombre)
    VALUES (@Nombre);
END;
go


--Trigger para usuarios
Create Trigger Bitacorainsersionusuario
on Usuarios
after insert
as
begin
insert into BITACORA (HOST, USUARIO,TRANSACCION, FECHA_MOD,TABLA_MOD)
Values (@@SERVERNAME, SUSER_NAME(), 'Insert',GETDATE(),'Usuarios' )
end
go

Create Trigger Bitacoramostrarusuario
on Usuarios
after select
as
begin
insert into BITACORA (HOST, USUARIO,TRANSACCION, FECHA_MOD,TABLA_MOD)
Values (@@SERVERNAME, SUSER_NAME(), 'Insert',GETDATE(),'Usuarios' )
end
go

--Procedimientos almacenados para Asignaciones

CREATE PROCEDURE MostrarAsignaciones
AS
BEGIN
    SELECT ID, IDTarea, IDUsuario
    FROM Asignaciones;
END;
go


CREATE PROCEDURE MostrarAsignacionesPorId
    @ID INT
AS
BEGIN
    SELECT ID, IDTarea, IDUsuario
    FROM Asignaciones
    WHERE ID = @ID
END;
go

CREATE PROCEDURE Actualizarasignaciones
    @ID int,
	@IDTarea int,
	@IDUsuario int
AS
BEGIN
    UPDATE Asignaciones
    SET
      IDTarea=@IDTarea,
	  IDUsuario=@IDUsuario
    WHERE ID = @ID;
END;

go

CREATE PROCEDURE EliminarAsignaciones
    @ID INT
AS
BEGIN
    DELETE FROM Asignaciones
    WHERE ID = @ID;
END;
go

CREATE PROCEDURE InsertarAsignacion
    @IDTarea int,
	@IDUsuario int
   
AS
BEGIN
    INSERT INTO Asignaciones(IDTarea,IDUsuario)
    VALUES (@IDTarea,@IDUsuario);
END;
go


--Procedimientos almacenados para Asignaciones

CREATE PROCEDURE MostrarTareasEtiquetas
AS
BEGIN
    SELECT ID, IDTarea, IDEtiqueta
    FROM TareasEtiquetas;
END;
go


CREATE PROCEDURE MostrarTareasEtiquetasPorId
    @ID INT
AS
BEGIN
    SELECT ID, IDTarea, IDEtiqueta
    FROM TareasEtiquetas
    WHERE ID = @ID
END;
go

CREATE PROCEDURE ActualizarTareasEtiquetas
    @ID int,
	@IDTarea int,
	@IDEtiqueta int
AS
BEGIN
    UPDATE TareasEtiquetas
    SET
      IDTarea=@IDTarea,
	  IDEtiqueta=@IDEtiqueta
    WHERE ID = @ID;
END;

go

CREATE PROCEDURE EliminarTareasEtiquetas
    @ID INT
AS
BEGIN
    DELETE FROM TareasEtiquetas
    WHERE ID = @ID;
END;
go

CREATE PROCEDURE InsertarTareasEtiquetas
    @IDTarea int,
	@IDEtiqueta int
   
AS
BEGIN
    INSERT INTO TareasEtiquetas(IDTarea,IDEtiqueta)
    VALUES (@IDTarea,@IDEtiqueta);
END;
go