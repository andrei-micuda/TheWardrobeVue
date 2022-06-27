using FluentMigrator;

namespace TheWardrobe.API.Migrations
{
  [Migration(202204031827)]
  public class Items : Migration
  {
    public override void Up()
    {
      Create.Table("brand")
        .WithColumn("id").AsInt64().PrimaryKey().Identity()
        .WithColumn("name").AsString();
      Insert.IntoTable("brand").Row(new { name = "Nike" });
      Insert.IntoTable("brand").Row(new { name = "Adidas" });
      Insert.IntoTable("brand").Row(new { name = "Zara" });
      Insert.IntoTable("brand").Row(new { name = "H&M" });
      Insert.IntoTable("brand").Row(new { name = "C&A" });
      Insert.IntoTable("brand").Row(new { name = "Pull&Bear" });
      Insert.IntoTable("brand").Row(new { name = "Collin's" });
      Insert.IntoTable("brand").Row(new { name = "Stradivarius" });
      Insert.IntoTable("brand").Row(new { name = "U.S. Polo Assn." });
      Insert.IntoTable("brand").Row(new { name = "Tommy Hilfiger" });
      Insert.IntoTable("brand").Row(new { name = "Champion" });
      Insert.IntoTable("brand").Row(new { name = "The North Face" });
      Insert.IntoTable("brand").Row(new { name = "Hugo Boss" });
      Insert.IntoTable("brand").Row(new { name = "Superdry" });
      Insert.IntoTable("brand").Row(new { name = "Lacoste" });
      Insert.IntoTable("brand").Row(new { name = "New Balance" });
      Insert.IntoTable("brand").Row(new { name = "Puma" });
      Insert.IntoTable("brand").Row(new { name = "Calvin Klein" });
      Insert.IntoTable("brand").Row(new { name = "Other" });

      Create.Table("gender")
        .WithColumn("id").AsInt64().PrimaryKey().Identity()
        .WithColumn("name").AsString();
      Insert.IntoTable("gender").Row(new { name = "male" });
      Insert.IntoTable("gender").Row(new { name = "female" });
      Insert.IntoTable("gender").Row(new { name = "unisex" });

      Create.Table("category")
        .WithColumn("id").AsInt64().PrimaryKey().Identity()
        .WithColumn("name").AsString();
      Insert.IntoTable("category").Row(new { name = "Anorak" });
      Insert.IntoTable("category").Row(new { name = "Blazer" });
      Insert.IntoTable("category").Row(new { name = "Blouse" });
      Insert.IntoTable("category").Row(new { name = "Bomber" });
      Insert.IntoTable("category").Row(new { name = "Button-Down" });
      Insert.IntoTable("category").Row(new { name = "Cardigan" });
      Insert.IntoTable("category").Row(new { name = "Flannel" });
      Insert.IntoTable("category").Row(new { name = "Halter" });
      Insert.IntoTable("category").Row(new { name = "Henley" });
      Insert.IntoTable("category").Row(new { name = "Hoodie" });
      Insert.IntoTable("category").Row(new { name = "Jacket" });
      Insert.IntoTable("category").Row(new { name = "Jersey" });
      Insert.IntoTable("category").Row(new { name = "Parka" });
      Insert.IntoTable("category").Row(new { name = "Peacoat" });
      Insert.IntoTable("category").Row(new { name = "Poncho" });
      Insert.IntoTable("category").Row(new { name = "Sweater" });
      Insert.IntoTable("category").Row(new { name = "Tank" });
      Insert.IntoTable("category").Row(new { name = "Tee" });
      Insert.IntoTable("category").Row(new { name = "Top" });
      Insert.IntoTable("category").Row(new { name = "Turtleneck" });
      Insert.IntoTable("category").Row(new { name = "Capris" });
      Insert.IntoTable("category").Row(new { name = "Chinos" });
      Insert.IntoTable("category").Row(new { name = "Culottes" });
      Insert.IntoTable("category").Row(new { name = "Cutoffs" });
      Insert.IntoTable("category").Row(new { name = "Gauchos" });
      Insert.IntoTable("category").Row(new { name = "Jeans" });
      Insert.IntoTable("category").Row(new { name = "Jeggings" });
      Insert.IntoTable("category").Row(new { name = "Jodhpurs" });
      Insert.IntoTable("category").Row(new { name = "Joggers" });
      Insert.IntoTable("category").Row(new { name = "Leggings" });
      Insert.IntoTable("category").Row(new { name = "Sarong" });
      Insert.IntoTable("category").Row(new { name = "Shorts" });
      Insert.IntoTable("category").Row(new { name = "Skirt" });
      Insert.IntoTable("category").Row(new { name = "Sweatpants" });
      Insert.IntoTable("category").Row(new { name = "Sweatshorts" });
      Insert.IntoTable("category").Row(new { name = "Trunks" });
      Insert.IntoTable("category").Row(new { name = "Caftan" });
      Insert.IntoTable("category").Row(new { name = "Cape" });
      Insert.IntoTable("category").Row(new { name = "Coat" });
      Insert.IntoTable("category").Row(new { name = "Coverup" });
      Insert.IntoTable("category").Row(new { name = "Dress" });
      Insert.IntoTable("category").Row(new { name = "Jumpsuit" });
      Insert.IntoTable("category").Row(new { name = "Kaftan" });
      Insert.IntoTable("category").Row(new { name = "Kimono" });
      Insert.IntoTable("category").Row(new { name = "Nightdress" });
      Insert.IntoTable("category").Row(new { name = "Onesie" });
      Insert.IntoTable("category").Row(new { name = "Robe" });
      Insert.IntoTable("category").Row(new { name = "Romper" });
      Insert.IntoTable("category").Row(new { name = "Shirtdress" });
      Insert.IntoTable("category").Row(new { name = "Sundress" });
      Insert.IntoTable("category").Row(new { name = "Footwear" });
      Insert.IntoTable("category").Row(new { name = "Accessories" });

      Create.Table("size")
        .WithColumn("id").AsInt64().PrimaryKey().Identity()
        .WithColumn("name").AsString();
      Insert.IntoTable("size").Row(new { name = "XS" });
      Insert.IntoTable("size").Row(new { name = "S" });
      Insert.IntoTable("size").Row(new { name = "M" });
      Insert.IntoTable("size").Row(new { name = "L" });
      Insert.IntoTable("size").Row(new { name = "XL" });
      Insert.IntoTable("size").Row(new { name = "one-size" });
      Insert.IntoTable("size").Row(new { name = "37" });
      Insert.IntoTable("size").Row(new { name = "38" });
      Insert.IntoTable("size").Row(new { name = "39" });
      Insert.IntoTable("size").Row(new { name = "40" });
      Insert.IntoTable("size").Row(new { name = "41" });
      Insert.IntoTable("size").Row(new { name = "42" });
      Insert.IntoTable("size").Row(new { name = "43" });
      Insert.IntoTable("size").Row(new { name = "44" });
      Insert.IntoTable("size").Row(new { name = "45" });
      Insert.IntoTable("size").Row(new { name = "46" });
      Insert.IntoTable("size").Row(new { name = "Other" });

      Create.Table("item")
        .WithColumn("id").AsGuid().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
        .WithColumn("when_added").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
        .WithColumn("seller_id").AsGuid().ForeignKey("account", "id")
        .WithColumn("is_available").AsBoolean().WithDefaultValue(true)
        .WithColumn("product_name").AsString()
        .WithColumn("price").AsFloat()
        .WithColumn("brand_id").AsInt64().ForeignKey("brand", "id")
        .WithColumn("gender_id").AsInt64().ForeignKey("gender", "id")
        .WithColumn("category_id").AsInt64().ForeignKey("category", "id")
        .WithColumn("size_id").AsInt64().ForeignKey("size", "id");

      Create.Table("item_image")
        .WithColumn("id").AsGuid().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
        .WithColumn("item_id").AsGuid().ForeignKey("item", "id").OnDelete(System.Data.Rule.Cascade)
        .WithColumn("url").AsString();
    }

    public override void Down()
    {
      Delete.Table("item_image");
      Delete.Table("item");
      Delete.Table("brand");
      Delete.Table("gender");
      Delete.Table("category");
      Delete.Table("size");
    }
  }
}