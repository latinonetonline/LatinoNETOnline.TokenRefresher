
using FluentMigrator;

namespace LatinoNETOnline.TokenRefresher.Web.Migrations
{
    [Migration(220520200504)]
    public class SetTimeZoneUtc : Migration
    {
        public override void Down()
        {
            Execute.Sql("SET TIMEZONE='UTC';");
        }

        public override void Up()
        {
            Execute.Sql("SET TIMEZONE='UTC';");
        }
    }
}
