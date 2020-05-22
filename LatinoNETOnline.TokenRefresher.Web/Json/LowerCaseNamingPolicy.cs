using System.Text.Json;

namespace LatinoNETOnline.TokenRefresher.Web.Json
{
    public class LowerCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) =>
            name.ToLower();
    }
}
