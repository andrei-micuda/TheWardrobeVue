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


def generateSqlScript(product_name, price, brand, gender, category, size, images):
    global BRANDS, CATEGORIES, GENDERS, SIZES
    brand_id = BRANDS.index(brand) + 1
    category_id = CATEGORIES.index(category) + 1
    gender_id = GENDERS.index(gender) + 1
    size_id = SIZES.index(size) + 1

    item_insert_sql = f"""
      DECLARE item_id uuid := uuid_generate_v4();
      BEGIN
      INSERT INTO item
      VALUES (item_id,
          'e6041016-240b-4c4b-b9da-bdb3bcc9d599',
          '{product_name}',
          {price}, {brand_id}, {gender_id}, {category_id}, {size_id});
      """

    item_images_sql = "\n".join(
        [f"INSERT INTO item_image(item_id, url) VALUES (item_id, '{image}');" for image in images])

    final_sql = f"""
    DO $$
    {item_insert_sql}

    {item_images_sql}
    END $$;
    """

    with open("item_inserts.sql", 'a') as fin:
        fin.write(final_sql)


generateSqlScript("Tricou puma", 60, "Puma", "male", "Tee", "L", [
    "https://images.asos-media.com/products/puma-essentials-small-logo-t-shirt-in-white/23815930-1-white?$n_480w$&wid=476&fit=constrain",
    "https://images.asos-media.com/products/puma-essentials-small-logo-t-shirt-in-white/23815930-3?$n_640w$&wid=513&fit=constrain"])
generateSqlScript("Pantaloni Scurti Tommy Jeans", 50, "Tommy Hilfiger", "male", "Trunks", "S", [
    "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T140611.632.png?v=1650280069",
    "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T140720.851.png?v=1650280069"])
generateSqlScript("Camasa", 75, "Tommy Hilfiger", "male", "Button-Down", "XL", [
    "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-15T134333.418.png?v=1650019511",
    "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-15T134447.364.png?v=1650019511"])
generateSqlScript("Bluza Calvin Klein", 125, "Calvin Klein", "male", "Sweater", "S", [
    "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-14T111913.210.png?v=1649924463",
    "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-14T112041.054.png?v=1649924463"])
generateSqlScript("Fusta", 50, "C&A", "female", "Skirt", "S", [
    "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T161031.477.png?v=1650287504",
    "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T161126.478.png?v=1650287503"])
generateSqlScript("Colanti", 25, "Puma", "female", "Leggings", "XL", [
    "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T153828.039.png?v=1650285522",
    "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T153659.432.png?v=1650285523"])
generateSqlScript("Rochie", 50, "H&M", "female", "Dress", "S", [
    "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T153447.858.png?v=1650285365",
    "https://cdn.shopify.com/s/files/1/0340/2222/8012/products/Alex-2022-04-18T153542.510.png?v=1650285365"])
generateSqlScript("adidas Originals", 400, "Adidas", "unisex", "Footwear", "46", [
    "https://images.asos-media.com/products/adidas-originals-niteball-trainers-in-off-white-and-orbit-green/201264575-1-white?$n_640w$&wid=513&fit=constrain",
    "https://images.asos-media.com/products/adidas-originals-niteball-trainers-in-off-white-and-orbit-green/201264575-3?$n_640w$&wid=513&fit=constrain",
    "https://images.asos-media.com/products/adidas-originals-niteball-trainers-in-off-white-and-orbit-green/201264575-4?$n_640w$&wid=513&fit=constrain"])
generateSqlScript("Nike Daybreak", 250, "Nike", "female", "Footwear", "38", [
    "https://images.asos-media.com/products/nike-daybreak-trainers-in-black-and-white/20585414-1-offnoir?$n_640w$&wid=513&fit=constrain",
    "https://images.asos-media.com/products/nike-daybreak-trainers-in-black-and-white/20585414-2?$n_640w$&wid=513&fit=constrain",
    "https://images.asos-media.com/products/nike-daybreak-trainers-in-black-and-white/20585414-4?$n_640w$&wid=513&fit=constrain"])
generateSqlScript("Geanta Tommy", 500, "Tommy Hilfiger", "unisex", "Accessories", "one-size", [
    "https://images.asos-media.com/products/tommy-jeans-flag-logo-shoulder-bag-in-check/200896105-1-checkmulti?$n_640w$&wid=513&fit=constrain",
    "https://images.asos-media.com/products/tommy-jeans-flag-logo-shoulder-bag-in-check/200896105-2?$n_640w$&wid=513&fit=constrain",
    "https://images.asos-media.com/products/tommy-jeans-flag-logo-shoulder-bag-in-check/200896105-4?$n_640w$&wid=513&fit=constrain",
    "https://images.asos-media.com/products/tommy-jeans-flag-logo-shoulder-bag-in-check/200896105-3?$n_640w$&wid=513&fit=constrain"])
