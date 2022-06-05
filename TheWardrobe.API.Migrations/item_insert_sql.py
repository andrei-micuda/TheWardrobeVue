BRANDS = [
    "Nike",
    "Adidas",
    "Zara",
    "H&M",
    "C&A",
    "Pull&Bear",
    "Collin's",
    "Stradivarius",
    "U.S. Polo Assn.",
    "Tommy Hilfiger",
    "Champion",
    "The North Face",
    "Hugo Boss",
    "Superdry",
    "Lacoste",
    "New Balance",
    "Puma",
    "Calvin Klein"
]

CATEGORIES = ["Anorak",
              "Blazer",
              "Blouse",
              "Bomber",
              "Button-Down",
              "Cardigan",
              "Flannel",
              "Halter",
              "Henley",
              "Hoodie",
              "Jacket",
              "Jersey",
              "Parka",
              "Peacoat",
              "Poncho",
              "Sweater",
              "Tank",
              "Tee",
              "Top",
              "Turtleneck",
              "Capris",
              "Chinos",
              "Culottes",
              "Cutoffs",
              "Gauchos",
              "Jeans",
              "Jeggings",
              "Jodhpurs",
              "Joggers",
              "Leggings",
              "Sarong",
              "Shorts",
              "Skirt",
              "Sweatpants",
              "Sweatshorts",
              "Trunks",
              "Caftan",
              "Cape",
              "Coat",
              "Coverup",
              "Dress",
              "Jumpsuit",
              "Kaftan",
              "Kimono",
              "Nightdress",
              "Onesie",
              "Robe",
              "Romper",
              "Shirtdress",
              "Sundress",
              "Footwear",
              "Accessories"]

GENDERS = ["male", "female", "unisex"]

SIZES = ["XS",
         "S",
         "M",
         "L",
         "XL",
         "one-size",
         "37",
         "38",
         "39",
         "40",
         "41",
         "42",
         "43",
         "44",
         "45",
         "46"]


class Item:
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


items_arr = [
    Item("Bluza noua", 60, "Zara", "female", "Blouse", "L", [
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/949e16a7-2013-4d99-9d52-b4c55fc68326.jpeg",
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/bfa6a3da-59d4-4fca-b3be-54bd9a87872e.jpeg"], 2),
    Item("Blugi Tommy Jeans", 230, "Tommy Hilfiger", "female", "Jeans", "S", [
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/f149ba97-2a5f-401b-9a57-3a8b5983c8dc.jpeg",
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/81ccd5c4-d33b-4780-b0f8-f2a673fb1260.jpeg"], 2),
    Item("Pulover calduros", 90, "H&M", "female", "Sweater", "L", [
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/400470b5-b5ad-49a7-9156-9df730486be1.png",
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/d67f567e-7dac-47b2-a9f9-208a06160de0.png"], 2),
    Item("Pantaloni eleganti", 120, "Collin's", "male", "Chinos", "L", [
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/19f3f893-93b3-4f34-8c82-5be53353ba79.jpg"]),
    Item("Jacheta bomber", 80, "Pull&Bear", "female", "Bomber", "S", [
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/b7b94a31-5f1e-4507-9b03-063c66056964.jpg"]),
    Item("Maieu Nirvana", 25, "H&M", "female", "Tank", "XS", [
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/ef26dd33-5ffe-4d1b-9e49-9e7000239c12.jpeg",
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/f6d12540-6b82-4faa-a4d1-c1e7083cd361.jpeg"]),
    Item("Tricou sport", 65, "Adidas", "male", "Tee", "M", [
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/80acb5b2-d977-4cf9-86fc-e10026272340.png"]),
    Item("Poncho casual", 100, "Stradivarius", "female", "Poncho", "one-size", [
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/76f5653b-0447-40c3-9429-165abf19a261.jpg"]),
    Item("Bluza eleganta", 70, "C&A", "female", "Button-Down", "S", [
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/8f6250f9-9341-4a12-b607-c2a13d81670e.png"]),
    Item("Rochie plaja", 50, "Zara", "female", "Romper", "M", [
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/4b6dfa11-d833-4595-a1d3-7560b4063374.png"]),
    Item("Pantaloni scurti", 50, "Zara", "female", "Shorts", "XS", [
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/9ade4957-3844-4875-b05b-69dfc62a2b12.jpg",
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/a5070d99-fe38-44f6-86e9-5470ae3d2adc.jpg"]),
    Item("Croptop", 20, "H&M", "female", "Top", "XS", [
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/6e0af294-446f-4bf6-9e02-6de366d46e48.jpg",
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/b3599a3f-8f09-4e20-aada-fce5575f4384.jpg",
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/b7048444-b4e5-4756-8fb7-10e623214ff2.jpg",
    ]),
    Item("Camasa casual", 60, "Zara", "female", "Button-Down", "M", [
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/bbf9ddd8-d950-41cf-a088-16acf51df88d.jpg",
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/5f95967c-cc12-4858-9a43-2a2fab07a274.jpg",
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/d04f5f86-3db5-4c79-b305-c616c9876599.jpg"
    ]),
    Item("Rochie eveniment", 95, "Stradivarius", "female", "Dress", "L", [
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/aa19f200-2355-4bd1-b70e-5955937c4b25.jpg",
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/32f12a4a-c54a-49dc-b96e-6334a71cec2a.jpg",
    ]),
    Item("Maieu", 15, "C&A", "female", "Tank", "S", [
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/f801eca0-f47e-4257-aceb-b332edc732a1.jpg"]),
    Item("Cercei eleganti", 35, "Stradivarius", "female", "Accessories", "one-size", [
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/ed29e5a9-7b04-43e4-bc70-623ec24d0c4a.jpg",
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/ad131c52-1d43-40c1-bc4f-1bc64da22df2.jpg",
    ]),
    Item("Pulover pe gat", 60, "Zara", "female", "Turtleneck", "M", [
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/4f850fa5-089b-4443-9f43-5e16b8c9d996.jpg"]),
    Item("Bluza deosebita", 45, "C&A", "female", "Blouse", "M", [
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/189763fc-6a1c-4481-9fca-6014edc6c030.jpg"]),
    Item("Tricou imprimeu", 35, "H&M", "female", "Tee", "M", [
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/7d8a3c20-8604-419b-8289-cd7c934bae1f.jpg",
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/6faf9aef-94c5-488f-8310-d9c58a13f998.jpg",
    ]),
]

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
