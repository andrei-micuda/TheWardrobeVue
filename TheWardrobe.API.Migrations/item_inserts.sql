
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
            account_id_2,
            'Blugi Tommy Jeans',
            230, 10, 2, 26, 2);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/f149ba97-2a5f-401b-9a57-3a8b5983c8dc.jpeg');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/81ccd5c4-d33b-4780-b0f8-f2a673fb1260.jpeg');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_2,
            'Pulover calduros',
            90, 4, 2, 16, 4);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/400470b5-b5ad-49a7-9156-9df730486be1.png');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/d67f567e-7dac-47b2-a9f9-208a06160de0.png');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Pantaloni eleganti',
            120, 7, 1, 22, 4);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/19f3f893-93b3-4f34-8c82-5be53353ba79.jpg');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Jacheta bomber',
            80, 6, 2, 4, 2);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/b7b94a31-5f1e-4507-9b03-063c66056964.jpg');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Maieu Nirvana',
            25, 4, 2, 17, 1);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/ef26dd33-5ffe-4d1b-9e49-9e7000239c12.jpeg');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/f6d12540-6b82-4faa-a4d1-c1e7083cd361.jpeg');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Tricou sport',
            65, 2, 1, 18, 3);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/80acb5b2-d977-4cf9-86fc-e10026272340.png');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Poncho casual',
            100, 8, 2, 15, 6);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/76f5653b-0447-40c3-9429-165abf19a261.jpg');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Bluza eleganta',
            70, 5, 2, 5, 2);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/8f6250f9-9341-4a12-b607-c2a13d81670e.png');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Rochie plaja',
            50, 3, 2, 48, 3);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/4b6dfa11-d833-4595-a1d3-7560b4063374.png');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Pantaloni scurti',
            50, 3, 2, 32, 1);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/9ade4957-3844-4875-b05b-69dfc62a2b12.jpg');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/a5070d99-fe38-44f6-86e9-5470ae3d2adc.jpg');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Croptop',
            20, 4, 2, 19, 1);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/6e0af294-446f-4bf6-9e02-6de366d46e48.jpg');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/b3599a3f-8f09-4e20-aada-fce5575f4384.jpg');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/b7048444-b4e5-4756-8fb7-10e623214ff2.jpg');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Camasa casual',
            60, 3, 2, 5, 3);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/bbf9ddd8-d950-41cf-a088-16acf51df88d.jpg');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/5f95967c-cc12-4858-9a43-2a2fab07a274.jpg');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/d04f5f86-3db5-4c79-b305-c616c9876599.jpg');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Rochie eveniment',
            95, 8, 2, 41, 4);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/aa19f200-2355-4bd1-b70e-5955937c4b25.jpg');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/32f12a4a-c54a-49dc-b96e-6334a71cec2a.jpg');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Maieu',
            15, 5, 2, 17, 2);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/f801eca0-f47e-4257-aceb-b332edc732a1.jpg');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Cercei eleganti',
            35, 8, 2, 52, 6);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/ed29e5a9-7b04-43e4-bc70-623ec24d0c4a.jpg');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/ad131c52-1d43-40c1-bc4f-1bc64da22df2.jpg');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Pulover pe gat',
            60, 3, 2, 20, 3);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/4f850fa5-089b-4443-9f43-5e16b8c9d996.jpg');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Bluza deosebita',
            45, 5, 2, 3, 3);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/189763fc-6a1c-4481-9fca-6014edc6c030.jpg');
        

        
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_1,
            'Tricou imprimeu',
            35, 4, 2, 18, 3);
        

        INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/7d8a3c20-8604-419b-8289-cd7c934bae1f.jpg');
INSERT INTO item_image(item_id, url) VALUES (item_id, 'https://thewardrobe-media.s3.eu-central-1.amazonaws.com/6faf9aef-94c5-488f-8310-d9c58a13f998.jpg');
        
    END $$;
