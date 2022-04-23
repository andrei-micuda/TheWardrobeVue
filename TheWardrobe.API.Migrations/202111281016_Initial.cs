using FluentMigrator;

namespace TheWardrobe.API.Migrations
{
  [Migration(202111281016)]
  public class Initial : Migration
  {
    public override void Up()
    {
      Create.Table("role")
        .WithColumn("id").AsGuid().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
        .WithColumn("name").AsString();

      Create.Table("account")
        .WithColumn("id").AsGuid().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
        .WithColumn("first_name").AsString().Nullable()
        .WithColumn("last_name").AsString().Nullable()
        .WithColumn("phone_number").AsString().Nullable()
        .WithColumn("email").AsString()
        .WithColumn("password_hash").AsString()
        .WithColumn("role_id").AsGuid().ForeignKey("role", "id").Nullable()
        .WithColumn("verification_token").AsString().Nullable()
        .WithColumn("reset_token").AsString().Nullable()
        .WithColumn("when_verified").AsDateTime().Nullable()
        .WithColumn("when_password_reset").AsDateTime().Nullable()
        .WithColumn("when_reset_token_expires").AsDateTime().Nullable()
        .WithColumn("when_created").AsDateTime()
        .WithColumn("when_updated").AsDateTime();

      Create.Table("refresh_token")
        .WithColumn("id").AsGuid().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
        .WithColumn("account_id").AsGuid().ForeignKey("account", "id")
        .WithColumn("token").AsString()
        .WithColumn("when_expires").AsDateTime()
        .WithColumn("when_created").AsDateTime()
        .WithColumn("created_by_ip").AsString().Nullable()
        .WithColumn("when_revoked").AsDateTime().Nullable()
        .WithColumn("revoked_by_ip").AsString().Nullable()
        .WithColumn("replaced_by_token").AsString().Nullable();
    }

    public override void Down()
    {
      Delete.Table("refresh_token");
      Delete.Table("account");
      Delete.Table("role");
    }
  }
}