DO $$
DECLARE account_id_1 uuid;
DECLARE account_id_2 uuid;
DECLARE order_id uuid;
DECLARE address_id uuid;
BEGIN
SELECT id INTO account_id_1
FROM account
LIMIT 1;
SELECT id INTO account_id_2
FROM account OFFSET 1
LIMIT 1;
order_id = uuid_generate_v4();
address_id = uuid_generate_v4();
INSERT INTO delivery_address (
    id,
    account_id,
    address,
    city,
    country,
    postal_code
  )
VALUES (
    address_id,
    account_id_2,
    'Unirii 1',
    'Bucuresti',
    'Romania',
    '123456'
  );
INSERT INTO "order" (
    seller_id,
    buyer_id,
    delivery_address_id,
    total,
    status
  )
VALUES (
    account_id_1,
    account_id_2,
    address_id,
    floor(random() * 200 + 1)::int,
    2
  );
INSERT INTO "order" (
    seller_id,
    buyer_id,
    delivery_address_id,
    total,
    status
  )
VALUES (
    account_id_1,
    account_id_2,
    address_id,
    floor(random() * 200 + 1)::int,
    2
  );
INSERT INTO "order" (
    seller_id,
    buyer_id,
    delivery_address_id,
    total,
    status
  )
VALUES (
    account_id_1,
    account_id_2,
    address_id,
    floor(random() * 200 + 1)::int,
    2
  );
INSERT INTO "order" (
    seller_id,
    buyer_id,
    delivery_address_id,
    total,
    status
  )
VALUES (
    account_id_1,
    account_id_2,
    address_id,
    floor(random() * 200 + 1)::int,
    2
  );
INSERT INTO "order" (
    seller_id,
    buyer_id,
    delivery_address_id,
    total,
    status
  )
VALUES (
    account_id_1,
    account_id_2,
    address_id,
    floor(random() * 200 + 1)::int,
    2
  );
INSERT INTO "order" (
    seller_id,
    buyer_id,
    delivery_address_id,
    total,
    status
  )
VALUES (
    account_id_1,
    account_id_2,
    address_id,
    floor(random() * 200 + 1)::int,
    2
  );
END $$