namespace JO.BlazorApp.Components.Pages
{
    public static class JORoutes
    {
        private const string PrefixTransaction = "/transaction";
        private const string PrefixAdmin = "/admin/manage";
        private const string PrefixCandidate = "/candidate";
        private const string PrefixTA = "/ta";

        public static class Public
        {
            public const string Login = "/login";
            public const string Denied = "/denied";
        }

        public static class Transaction
        {
            public const string Details = PrefixTransaction + "/details";
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

        public static class TA
        {
            public const string Dashboard = PrefixTA + "/dashboard";
            public const string Analysis = PrefixTA + "/analysis";
        }
    }
}
