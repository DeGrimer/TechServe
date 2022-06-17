CREATE TABLE client (
	id INT NOT NULL PRIMARY KEY IDENTITY,
	name VARCHAR (100) NOT NULL,
	surname VARCHAR (100) NOT NULL,
	email VARCHAR (150) NULL,
	phone VARCHAR (12) NOT NULL,
	address VARCHAR (100) NULL
	);
INSERT INTO client (name, surname, email, phone, address)
VALUES
('Ivan','Ívanov','ee@ro.ru','+78005553535', 'st. Pushkina 3-5'),
('Fedor','Valenko', 'gg@mail.ru', '+89127362440', 'st. Vorovskogo 12-3');