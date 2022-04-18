DO $$
DECLARE item_id uuid := uuid_generate_v4();
BEGIN
INSERT INTO item
VALUES (
        item_id,
        'e6041016-240b-4c4b-b9da-bdb3bcc9d599',
        'Tricou puma',
        60,
        17,
        1,
        18,
        4
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://images.asos-media.com/products/puma-essentials-small-logo-t-shirt-in-white/23815930-1-white?$n_480w$&wid=476&fit=constrain'
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://images.asos-media.com/products/puma-essentials-small-logo-t-shirt-in-white/23815930-3?$n_640w$&wid=513&fit=constrain'
    );
END $$;
DO $$
DECLARE item_id uuid := uuid_generate_v4();
BEGIN
INSERT INTO item
VALUES (
        item_id,
        'e6041016-240b-4c4b-b9da-bdb3bcc9d599',
        'Pantaloni Scurti Tommy Jeans',
        50,
        10,
        1,
        36,
        2
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T140611.632.png?v=1650280069'
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T140720.851.png?v=1650280069'
    );
END $$;
DO $$
DECLARE item_id uuid := uuid_generate_v4();
BEGIN
INSERT INTO item
VALUES (
        item_id,
        'e6041016-240b-4c4b-b9da-bdb3bcc9d599',
        'Camasa',
        75,
        10,
        1,
        5,
        5
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-15T134333.418.png?v=1650019511'
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-15T134447.364.png?v=1650019511'
    );
END $$;
DO $$
DECLARE item_id uuid := uuid_generate_v4();
BEGIN
INSERT INTO item
VALUES (
        item_id,
        'e6041016-240b-4c4b-b9da-bdb3bcc9d599',
        'Bluza Calvin Klein',
        125,
        18,
        1,
        16,
        2
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-14T111913.210.png?v=1649924463'
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-14T112041.054.png?v=1649924463'
    );
END $$;
DO $$
DECLARE item_id uuid := uuid_generate_v4();
BEGIN
INSERT INTO item
VALUES (
        item_id,
        'e6041016-240b-4c4b-b9da-bdb3bcc9d599',
        'Fusta',
        50,
        5,
        2,
        33,
        2
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T161031.477.png?v=1650287504'
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T161126.478.png?v=1650287503'
    );
END $$;
DO $$
DECLARE item_id uuid := uuid_generate_v4();
BEGIN
INSERT INTO item
VALUES (
        item_id,
        'e6041016-240b-4c4b-b9da-bdb3bcc9d599',
        'Colanti',
        25,
        17,
        2,
        30,
        5
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T153828.039.png?v=1650285522'
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T153659.432.png?v=1650285523'
    );
END $$;
DO $$
DECLARE item_id uuid := uuid_generate_v4();
BEGIN
INSERT INTO item
VALUES (
        item_id,
        'e6041016-240b-4c4b-b9da-bdb3bcc9d599',
        'Rochie',
        50,
        4,
        2,
        41,
        2
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T153447.858.png?v=1650285365'
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T153542.510.png?v=1650285365'
    );
END $$;
DO $$
DECLARE item_id uuid := uuid_generate_v4();
BEGIN
INSERT INTO item
VALUES (
        item_id,
        'e6041016-240b-4c4b-b9da-bdb3bcc9d599',
        'adidas Originals',
        400,
        2,
        3,
        51,
        16
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://images.asos-media.com/products/adidas-originals-niteball-trainers-in-off-white-and-orbit-green/201264575-1-white?$n_640w$&wid=513&fit=constrain'
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://images.asos-media.com/products/adidas-originals-niteball-trainers-in-off-white-and-orbit-green/201264575-3?$n_640w$&wid=513&fit=constrain'
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://images.asos-media.com/products/adidas-originals-niteball-trainers-in-off-white-and-orbit-green/201264575-4?$n_640w$&wid=513&fit=constrain'
    );
END $$;
DO $$
DECLARE item_id uuid := uuid_generate_v4();
BEGIN
INSERT INTO item
VALUES (
        item_id,
        'e6041016-240b-4c4b-b9da-bdb3bcc9d599',
        'Nike Daybreak',
        250,
        1,
        2,
        51,
        8
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://images.asos-media.com/products/nike-daybreak-trainers-in-black-and-white/20585414-1-offnoir?$n_640w$&wid=513&fit=constrain'
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://images.asos-media.com/products/nike-daybreak-trainers-in-black-and-white/20585414-2?$n_640w$&wid=513&fit=constrain'
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://images.asos-media.com/products/nike-daybreak-trainers-in-black-and-white/20585414-4?$n_640w$&wid=513&fit=constrain'
    );
END $$;
DO $$
DECLARE item_id uuid := uuid_generate_v4();
BEGIN
INSERT INTO item
VALUES (
        item_id,
        'e6041016-240b-4c4b-b9da-bdb3bcc9d599',
        'Geanta Tommy',
        500,
        10,
        3,
        52,
        6
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://images.asos-media.com/products/tommy-jeans-flag-logo-shoulder-bag-in-check/200896105-1-checkmulti?$n_640w$&wid=513&fit=constrain'
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://images.asos-media.com/products/tommy-jeans-flag-logo-shoulder-bag-in-check/200896105-2?$n_640w$&wid=513&fit=constrain'
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://images.asos-media.com/products/tommy-jeans-flag-logo-shoulder-bag-in-check/200896105-4?$n_640w$&wid=513&fit=constrain'
    );
INSERT INTO item_image(item_id, url)
VALUES (
        item_id,
        'https://images.asos-media.com/products/tommy-jeans-flag-logo-shoulder-bag-in-check/200896105-3?$n_640w$&wid=513&fit=constrain'
    );
END $$;