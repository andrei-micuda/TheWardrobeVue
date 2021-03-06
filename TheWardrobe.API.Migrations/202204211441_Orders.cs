using System;
using FluentMigrator;

namespace TheWardrobe.API.Migrations
{
  [Migration(2022042111441)]
  public class Orders : Migration
  {
    public override void Up()
    {
      Create.Table("favorite")
        .WithColumn("account_id").AsGuid().PrimaryKey().ForeignKey("account", "id").OnDelete(System.Data.Rule.Cascade)
        .WithColumn("item_id").AsGuid().PrimaryKey().ForeignKey("item", "id").OnDelete(System.Data.Rule.Cascade);

      Create.Table("cart")
        .WithColumn("account_id").AsGuid().PrimaryKey().ForeignKey("account", "id").OnDelete(System.Data.Rule.Cascade)
        .WithColumn("item_id").AsGuid().PrimaryKey().ForeignKey("item", "id").OnDelete(System.Data.Rule.Cascade);

      Create.Table("delivery_address")
        .WithColumn("id").AsGuid().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
        .WithColumn("account_id").AsGuid().ForeignKey("account", "id").OnDelete(System.Data.Rule.Cascade)
        .WithColumn("address").AsString()
        .WithColumn("city").AsString()
        .WithColumn("country").AsString()
        .WithColumn("postal_code").AsString();

      Create.Table("order")
        .WithColumn("id").AsGuid().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
        .WithColumn("seller_id").AsGuid().ForeignKey("account", "id")
        .WithColumn("buyer_id").AsGuid().ForeignKey("account", "id")
        .WithColumn("delivery_address_id").AsGuid().ForeignKey("delivery_address", "id")
        .WithColumn("when_placed").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
        .WithColumn("status").AsInt64();

      Create.Table("order_item")
        .WithColumn("id").AsGuid().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
        .WithColumn("item_id").AsGuid().ForeignKey("item", "id").OnDelete(System.Data.Rule.SetNull)
        .WithColumn("order_id").AsGuid().ForeignKey("order", "id").OnDelete(System.Data.Rule.SetNull);

      Create.Table("review")
        .WithColumn("order_id").AsGuid().ForeignKey("order", "id").PrimaryKey()
        .WithColumn("buyer_rating").AsInt32().Nullable()
        .WithColumn("seller_rating").AsInt32().Nullable();
    }

    public override void Down()
    {
      Delete.Table("review");
      Delete.Table("order_item");
      Delete.Table("order");
      Delete.Table("delivery_address");
      Delete.Table("cart");
      Delete.Table("favorite");
    }
  }
}