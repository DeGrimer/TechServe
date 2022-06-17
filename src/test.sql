SELECT name,producer, model,sell_date 
FROM equip_sell INNER JOIN equip ON equip_sell.equip_id = equip.id
INNER JOIN client ON equip_sell.client_id = client.id;