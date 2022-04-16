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

      Create.Table("gender")
        .WithColumn("id").AsInt64().PrimaryKey().Identity()
        .WithColumn("name").AsString();

      Create.Table("category")
        .WithColumn("id").AsInt64().PrimaryKey().Identity()
        .WithColumn("name").AsString();

      Create.Table("size")
        .WithColumn("id").AsInt64().PrimaryKey().Identity()
        .WithColumn("name").AsString();

      Create.Table("item")
        .WithColumn("id").AsGuid().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
        .WithColumn("seller_id").AsGuid().ForeignKey("account", "id")
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