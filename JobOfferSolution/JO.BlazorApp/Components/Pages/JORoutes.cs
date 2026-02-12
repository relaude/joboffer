namespace JO.BlazorApp.Components.Pages
{
    public static class JORoutes
    {
        private const string PrefixAdmin = "/admin/manage";
        public static class Prototype
        {
            public const string Index = "/prototype";
        }

        public static class Admin
        {
            public const string Users = PrefixAdmin + "/users";
        }
    }
}
