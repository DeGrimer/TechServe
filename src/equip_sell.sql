﻿CREATE TABLE equip_sell (
	id INT NOT NULL PRIMARY KEY IDENTITY,
	equip_id INT NOT NULl,
	sell_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	client_id INT NOT NULL,
	CONSTRAINT FK_equip_id_equip FOREIGN KEY (equip_id)
	REFERENCES equip (id)
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	CONSTRAINT FK_client_id FOREIGN KEY (client_id)
	REFERENCES client (id)
	ON DELETE CASCADE
	ON UPDATE CASCADE
	);