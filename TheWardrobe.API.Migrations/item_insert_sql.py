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


items_arr = [Item("Bluza noua", 60, "Zara", "female", "Blouse", "L", [
    "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/949e16a7-2013-4d99-9d52-b4c55fc68326.jpeg",
    "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/bfa6a3da-59d4-4fca-b3be-54bd9a87872e.jpeg"], 2),
    Item("Blugi Tommy Jeans", 230, "Tommy Hilfiger", "female", "Jeans", "S", [
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/232a457d-fa7c-4c6a-aff4-09cbbcf7efdf.jpeg",
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/14b4ee28-13a3-4b14-afe1-510669953ec3.jpeg"]),
    Item("Pulover calduros", 90, "H&M", "female", "Sweater", "L", [
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/a5d269ca-ed5b-4f55-ac2f-890d45242eaa.png",
        "https://thewardrobe-media.s3.eu-central-1.amazonaws.com/37d6f218-c5cd-4494-bb6b-c790a36a9065.png"]),
    Item("Tricou puma", 60, "Puma", "male", "Tee", "L", [
        "https://images.asos-media.com/products/puma-essentials-small-logo-t-shirt-in-white/23815930-1-white?$n_480w$&wid=476&fit=constrain",
        "https://images.asos-media.com/products/puma-essentials-small-logo-t-shirt-in-white/23815930-3?$n_640w$&wid=513&fit=constrain"]),
    Item("Pantaloni Scurti Tommy Jeans", 50, "Tommy Hilfiger", "male", "Trunks", "S", [
        "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T140611.632.png?v=1650280069",
        "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T140720.851.png?v=1650280069"]),
    Item("Camasa", 75, "Tommy Hilfiger", "male", "Button-Down", "XL", [
        "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-15T134333.418.png?v=1650019511",
        "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-15T134447.364.png?v=1650019511"]),
    Item("Bluza Calvin Klein", 125, "Calvin Klein", "male", "Sweater", "S", [
        "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-14T111913.210.png?v=1649924463",
        "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-14T112041.054.png?v=1649924463"]),
    Item("Fusta", 50, "C&A", "female", "Skirt", "S", [
        "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T161031.477.png?v=1650287504",
        "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T161126.478.png?v=1650287503"]),
    Item("Colanti", 25, "Puma", "female", "Leggings", "XL", [
        "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T153828.039.png?v=1650285522",
        "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T153659.432.png?v=1650285523"]),
    Item("Rochie", 50, "H&M", "female", "Dress", "S", [
        "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T153447.858.png?v=1650285365",
        "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T153542.510.png?v=1650285365"]),
    Item("adidas Originals", 400, "Adidas", "unisex", "Footwear", "46", [
        "https://images.asos-media.com/products/adidas-originals-niteball-trainers-in-off-white-and-orbit-green/201264575-1-white?$n_640w$&wid=513&fit=constrain",
        "https://images.asos-media.com/products/adidas-originals-niteball-trainers-in-off-white-and-orbit-green/201264575-3?$n_640w$&wid=513&fit=constrain",
        "https://images.asos-media.com/products/adidas-originals-niteball-trainers-in-off-white-and-orbit-green/201264575-4?$n_640w$&wid=513&fit=constrain"]),
    Item("Nike Daybreak", 250, "Nike", "female", "Footwear", "38", [
        "https://images.asos-media.com/products/nike-daybreak-trainers-in-black-and-white/20585414-1-offnoir?$n_640w$&wid=513&fit=constrain",
        "https://images.asos-media.com/products/nike-daybreak-trainers-in-black-and-white/20585414-2?$n_640w$&wid=513&fit=constrain",
        "https://images.asos-media.com/products/nike-daybreak-trainers-in-black-and-white/20585414-4?$n_640w$&wid=513&fit=constrain"]),
    Item("Geanta Tommy", 500, "Tommy Hilfiger", "unisex", "Accessories", "one-size", [
        "https://images.asos-media.com/products/tommy-jeans-flag-logo-shoulder-bag-in-check/200896105-1-checkmulti?$n_640w$&wid=513&fit=constrain",
        "https://images.asos-media.com/products/tommy-jeans-flag-logo-shoulder-bag-in-check/200896105-2?$n_640w$&wid=513&fit=constrain",
        "https://images.asos-media.com/products/tommy-jeans-flag-logo-shoulder-bag-in-check/200896105-4?$n_640w$&wid=513&fit=constrain",
        "https://images.asos-media.com/products/tommy-jeans-flag-logo-shoulder-bag-in-check/200896105-3?$n_640w$&wid=513&fit=constrain"])]

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
