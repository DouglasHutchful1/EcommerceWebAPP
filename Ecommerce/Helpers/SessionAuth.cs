namespace Ecommerce.Helpers;

public static class SessionAuth
{
    public static bool IsLoggedIn(ISession session)
        => (session.GetInt32("UserId") ?? 0) > 0;
}