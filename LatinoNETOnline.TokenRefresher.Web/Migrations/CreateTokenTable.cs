
using FluentMigrator;

using LatinoNETOnline.TokenRefresher.Web.Enitities;

namespace LatinoNETOnline.TokenRefresher.Web.Migrations
{
    [Migration(1)]
    public class CreateTokenTable : Migration
    {
        public override void Up()
        {
            Create.Table("AccessTokens")
                .WithColumn(nameof(AccessToken.Id)).AsGuid().PrimaryKey()
                .WithColumn(nameof(AccessToken.Name)).AsString(150).NotNullable().Unique()
                .WithColumn(nameof(AccessToken.Token)).AsString().NotNullable().Unique()
                .WithColumn(nameof(AccessToken.Expires)).AsDateTime()
                .WithColumn(nameof(AccessToken.TokenType)).AsString()
                .WithColumn(nameof(AccessToken.RefreshToken)).AsString();
        }

        public override void Down()
        {
            Delete.Table("AccessTokens");
        }
    }
}
