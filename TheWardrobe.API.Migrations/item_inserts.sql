
    DO $$
    DECLARE account_id_1 uuid;
    DECLARE account_id_2 uuid;
    DECLARE item_id uuid;
    BEGIN
    SELECT id INTO account_id_1
    FROM account
    LIMIT 1;

    SELECT id INTO account_id_2
    FROM account
    OFFSET 1
    LIMIT 1;

    
        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_2,
            'Bluza noua',
            60, 3, 2, 3, 4);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/949e16a7-2013-4d99-9d52-b4c55fc68326.jpeg');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/bfa6a3da-59d4-4fca-b3be-54bd9a87872e.jpeg');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Blugi Tommy Jeans',
            230, 10, 2, 26, 2);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/232a457d-fa7c-4c6a-aff4-09cbbcf7efdf.jpeg');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/14b4ee28-13a3-4b14-afe1-510669953ec3.jpeg');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Pulover calduros',
            90, 4, 2, 16, 4);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/a5d269ca-ed5b-4f55-ac2f-890d45242eaa.png');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/37d6f218-c5cd-4494-bb6b-c790a36a9065.png');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Tricou puma',
            60, 17, 1, 18, 4);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://images.asos-media.com/products/puma-essentials-small-logo-t-shirt-in-white/23815930-1-white?$n_480w$&wid=476&fit=constrain');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://images.asos-media.com/products/puma-essentials-small-logo-t-shirt-in-white/23815930-3?$n_640w$&wid=513&fit=constrain');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Pantaloni Scurti Tommy Jeans',
            50, 10, 1, 36, 2);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T140611.632.png?v=1650280069');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T140720.851.png?v=1650280069');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Camasa',
            75, 10, 1, 5, 5);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-15T134333.418.png?v=1650019511');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-15T134447.364.png?v=1650019511');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Bluza Calvin Klein',
            125, 18, 1, 16, 2);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-14T111913.210.png?v=1649924463');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-14T112041.054.png?v=1649924463');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Fusta',
            50, 5, 2, 33, 2);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T161031.477.png?v=1650287504');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T161126.478.png?v=1650287503');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Colanti',
            25, 17, 2, 30, 5);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T153828.039.png?v=1650285522');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T153659.432.png?v=1650285523');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Rochie',
            50, 4, 2, 41, 2);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T153447.858.png?v=1650285365');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T153542.510.png?v=1650285365');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'adidas Originals',
            400, 2, 3, 51, 16);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://images.asos-media.com/products/adidas-originals-niteball-trainers-in-off-white-and-orbit-green/201264575-1-white?$n_640w$&wid=513&fit=constrain');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://images.asos-media.com/products/adidas-originals-niteball-trainers-in-off-white-and-orbit-green/201264575-3?$n_640w$&wid=513&fit=constrain');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://images.asos-media.com/products/adidas-originals-niteball-trainers-in-off-white-and-orbit-green/201264575-4?$n_640w$&wid=513&fit=constrain');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Nike Daybreak',
            250, 1, 2, 51, 8);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://images.asos-media.com/products/nike-daybreak-trainers-in-black-and-white/20585414-1-offnoir?$n_640w$&wid=513&fit=constrain');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://images.asos-media.com/products/nike-daybreak-trainers-in-black-and-white/20585414-2?$n_640w$&wid=513&fit=constrain');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://images.asos-media.com/products/nike-daybreak-trainers-in-black-and-white/20585414-4?$n_640w$&wid=513&fit=constrain');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Geanta Tommy',
            500, 10, 3, 52, 6);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://images.asos-media.com/products/tommy-jeans-flag-logo-shoulder-bag-in-check/200896105-1-checkmulti?$n_640w$&wid=513&fit=constrain');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://images.asos-media.com/products/tommy-jeans-flag-logo-shoulder-bag-in-check/200896105-2?$n_640w$&wid=513&fit=constrain');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://images.asos-media.com/products/tommy-jeans-flag-logo-shoulder-bag-in-check/200896105-4?$n_640w$&wid=513&fit=constrain');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://images.asos-media.com/products/tommy-jeans-flag-logo-shoulder-bag-in-check/200896105-3?$n_640w$&wid=513&fit=constrain');
        
    END $$;
