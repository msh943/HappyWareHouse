using System.Security.Claims;

namespace HappyWarehouse.Tests.Unit.Tests.Helpers
{
    public static class FakeUser
    {
        public static ClaimsPrincipal Admin(int userId = 1)
        => Build(userId, "Admin");

        public static ClaimsPrincipal Manager(int userId = 2)
            => Build(userId, "Management");

        private static ClaimsPrincipal Build(int userId, string role)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Name, "Unit Test User"),
            new Claim(ClaimTypes.Email, "test@hw.local")
        };
            return new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));
        }
    }
}
