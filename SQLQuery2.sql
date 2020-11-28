CREATE TABLE ramtipo(
	idRamTipo SMALLINT IDENTITY(1,1) PRIMARY KEY,
	descripcion VARCHAR(10) NOT NULL,
);

CREATE TABLE marcas(
	idMarca INT IDENTITY(1,1) PRIMARY KEY,
	descripcion VARCHAR(50) NOT NULL
);
CREATE TABLE unidadAlmacenamiento(
	idUnidad SMALLINT IDENTITY(1,1) PRIMARY KEY,
	descripcion VARCHAR(10) NOT NULL
);
CREATE TABLE tipoPerifericos(
	idTipoPeriferico INT IDENTITY(1,1) PRIMARY KEY,
	descripcion VARCHAR(25) NOT NULL
);

CREATE TABLE departamentos(
	idDepartamento INT IDENTITY(1,1) PRIMARY KEY,	
	nombre VARCHAR(50) NOT NULL
);
CREATE TABLE proveedores(
	idProveedor INT IDENTITY(1,1) PRIMARY KEY,
	cuit VARCHAR(25) NOT NULL,
	razSoc VARCHAR(50) NOT NULL,
	telefono VARCHAR(25),
	direccion VARCHAR(50) NOT NULL,
	email VARCHAR(25)
);

CREATE TABLE equipos(
	idEquipo INT IDENTITY(1,1) PRIMARY KEY,
	nombre VARCHAR(100) NOT NULL,
	descripcion VARCHAR(500) NOT NULL,
	modelo VARCHAR(100) NOT NULL,
	fecCompra DATETIME NOT NULL,
	garantia DATE NULL,

	ram INT NOT NULL,
	-- 1: DDR3 2: DDR4
	idRamTipo SMALLINT NOT NULL,
	idMarca INT NULL,
	idDepartamento INT NULL,
	idProveedor INT NOT NULL,

	motherboard VARCHAR(100) NOT NULL,
	cpu VARCHAR(100) NOT NULL,
	gpu VARCHAR(100) NOT NULL,
	hdd INT NOT NULL,
	ssd INT,
	hddUnidad SMALLINT NOT NULL,
	ssdUnidad SMALLINT NOT NULL,

	FOREIGN KEY (idRamTipo) REFERENCES ramtipo (idRamTipo),
	FOREIGN KEY (idMarca) REFERENCES marcas (idMarca),
	FOREIGN KEY (idDepartamento) REFERENCES departamentos (idDepartamento), 
	FOREIGN KEY (IdProveedor) REFERENCES proveedores(IdProveedor),
	FOREIGN KEY (hddUnidad) REFERENCES unidadAlmacenamiento (idUnidad),
	FOREIGN KEY (ssdUnidad) REFERENCES unidadAlmacenamiento (idUnidad)
)

CREATE TABLE perifericos(
	idPeriferico INT IDENTITY(1,1) PRIMARY KEY,
	fecCompra DATETIME NOT NULL,
	garantia DATE NULL,
	modelo VARCHAR(100) NOT NULL,
	caracteristicas varchar(1000) NOT NULL,
	--1: Scanner 2: Teclado 3: Mouse 4: Cámara 5: Micrófono 6: Proyector--
	idTipoPeriferico INT NOT NULL,
	idMarca INT NOT NULL,
	idProveedor INT NOT NULL,

	FOREIGN KEY (idTipoPeriferico) REFERENCES tipoPerifericos (idTipoPeriferico),

	FOREIGN KEY (idMarca) REFERENCES marcas (idMarca),
	FOREIGN KEY (idProveedor) REFERENCES proveedores(idProveedor)
);



CREATE TABLE historialCambios(
	idHistorialCambio INT IDENTITY(1,1) PRIMARY KEY,
	descripcion VARCHAR(1000) NOT NULL,
	observaciones VARCHAR(1000),
	fecha DATE NOT NULL,
	idEquipo INT NOT NULL,
	FOREIGN KEY (idEquipo) REFERENCES equipos (idEquipo) ON DELETE CASCADE
	);




CREATE TABLE usuarios(
	idUsuario INT IDENTITY(1,1) PRIMARY KEY,
	nombreYapellido VARCHAR(50) NOT NULL,
	dni VARCHAR(25) NOT NULL,
	idDepartamento INT NOT NULL,
	FOREIGN KEY (idDepartamento) REFERENCES departamentos (idDepartamento) ON DELETE CASCADE ON UPDATE CASCADE
);



CREATE TABLE proveedor_marca(
	idProveedor INT,
	idMarca INT,
	PRIMARY KEY(idProveedor,idMarca),
	FOREIGN KEY(idProveedor) REFERENCES proveedores (idProveedor),
	FOREIGN KEY(idMarca) REFERENCES marcas (idMarca) ON DELETE CASCADE
);

CREATE TABLE usuario_equipo(
	idUsuario INT,
	idEquipo INT,
	PRIMARY KEY(idUsuario,idEquipo),
	FOREIGN KEY (idUsuario) REFERENCES usuarios (idUsuario),
	FOREIGN KEY (idEquipo) REFERENCES equipos (idEquipo)
);