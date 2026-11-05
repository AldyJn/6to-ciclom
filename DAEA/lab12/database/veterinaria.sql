DROP TABLE IF EXISTS mascotas;
DROP TABLE IF EXISTS propietarios;

CREATE TABLE propietarios (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    dni CHAR(8) NOT NULL UNIQUE,
    telefono VARCHAR(15) NOT NULL,
    email VARCHAR(100) NULL
);

CREATE TABLE mascotas (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(60) NOT NULL,
    especie VARCHAR(30) NOT NULL,
    raza VARCHAR(60) NULL,
    peso_kg NUMERIC(6,2) NOT NULL CHECK (peso_kg > 0),
    fecha_nacimiento DATE NOT NULL,
    activa BOOLEAN NOT NULL DEFAULT TRUE,
    propietario_id INT NOT NULL REFERENCES propietarios(id)
);

INSERT INTO propietarios (nombre, dni, telefono, email) VALUES
('Carlos Ramirez', '45678123', '987654321', 'carlos@correo.com'),
('Lucia Fernandez', '40123456', '912345678', 'lucia@correo.com'),
('Jorge Salazar', '47891234', '956781234', NULL);

INSERT INTO mascotas (nombre, especie, raza, peso_kg, fecha_nacimiento, activa, propietario_id) VALUES
('Rocky', 'Perro', 'Labrador', 28.50, '2019-03-12', TRUE, 1),
('Michi', 'Gato', 'Siames', 4.20, '2021-07-01', TRUE, 1),
('Luna', 'Perro', 'Poodle', 6.80, '2020-11-23', TRUE, 2),
('Pelusa', 'Conejo', NULL, 2.10, '2022-05-15', FALSE, 3);
