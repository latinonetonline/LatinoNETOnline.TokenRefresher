
using FluentMigrator;

using LatinoNETOnline.TokenRefresher.Web.Entities;

namespace LatinoNETOnline.TokenRefresher.Web.Migrations
{
    [Migration(180520201950)]
    public class CreateTokenTable : Migration
    {
        public override void Up()
        {
            Create.Table("Tokens")
                .WithColumn(nameof(Token.Id)).AsGuid().PrimaryKey()
                .WithColumn(nameof(Token.Name)).AsString(150).NotNullable()
                .WithColumn(nameof(Token.Value)).AsString().NotNullable()
                .WithColumn(nameof(Token.Expires)).AsDateTime()
                .WithColumn(nameof(Token.TokenType)).AsString()
                .WithColumn(nameof(Token.RefreshToken)).AsString()
                .WithColumn(nameof(Token.ClientId)).AsString().NotNullable();

            Create.UniqueConstraint("IX_Token_ClientId_Name").OnTable("Tokens").Columns(nameof(Token.ClientId), nameof(Token.Name));
            Create.UniqueConstraint("IX_Token_ClientId_Value").OnTable("Tokens").Columns(nameof(Token.ClientId), nameof(Token.Value));
        }

        public override void Down()
        {
            Delete.Table("Tokens");
        }
    }
}
