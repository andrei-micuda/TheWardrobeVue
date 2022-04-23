class Order:
    def __init__(self, name, price, brand, gender, category, size, images, seller_id=1):
        self.name = name
        self.price = price
        self.brand = brand
        self.gender = gender
        self.category = category
        self.size = size
        self.images = images
        self.seller_id = seller_id

    def generate_sql(self):
        global BRANDS, CATEGORIES, GENDERS, SIZES
        brand_id = BRANDS.index(self.brand) + 1
        category_id = CATEGORIES.index(self.category) + 1
        gender_id = GENDERS.index(self.gender) + 1
        size_id = SIZES.index(self.size) + 1

        item_insert_sql = f"""
        item_id = uuid_generate_v4();

        INSERT INTO item (id, seller_id, product_name, price, brand_id, gender_id, category_id, size_id)
        VALUES (item_id,
            account_id_{self.seller_id},
            '{self.name}',
            {self.price}, {brand_id}, {gender_id}, {category_id}, {size_id});
        """

        item_images_sql = "\n".join(
            [f"INSERT INTO item_image(item_id, url) VALUES (item_id, '{image}');" for image in self.images])

        return f"""
        {item_insert_sql}

        {item_images_sql}
        """


items_sql = '\n'.join([i.generate_sql() for i in items_arr])

final_sql = f"""
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

    {items_sql}
    END $$;
"""

with open("item_inserts.sql", 'w') as fin:
    fin.write(final_sql)
