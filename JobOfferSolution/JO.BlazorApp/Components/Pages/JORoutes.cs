namespace JO.BlazorApp.Components.Pages
{
    public static class JORoutes
    {
        private const string PrefixAdmin = "/admin/manage";
        private const string PrefixCandidate = "/candidate";
        public static class Public
        {
            public const string Login = "/login";
            public const string Denied = "/denied";
        }

        public static class Admin
        {
            public const string Users = PrefixAdmin + "/users";
        }

        public static class Candidate
        {
            public const string Index = PrefixCandidate;
            public const string Details = PrefixCandidate + "/details";
        }
    }
}
