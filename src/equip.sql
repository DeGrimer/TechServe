CREATE TABLE equip (
	id INT NOT NULL PRIMARY KEY IDENTITY,
	producer VARCHAR (100) NOT NULL,
	model VARCHAR (100) NOT NULL,
	guarantee_period_month INT NOT NULL
	);
INSERT INTO equip (producer, model, guarantee_period_month)
VALUES
('HP', 'Envy 17', 24),
('HP', 'Chromebook x360', 24),
('Lenovo', 'Legion Slim 7', 36);