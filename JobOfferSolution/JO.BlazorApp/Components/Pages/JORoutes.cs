namespace JO.BlazorApp.Components.Pages
{
    public static class JORoutes
    {
        private const string PrefixTransaction = "/transaction";
        private const string PrefixAdmin = "/admin/manage";
        private const string PrefixTA = "/ta";
        private const string PrefixJobOffer = "/joboffer";

        public static class Public
        {
            public const string Login = "/login";
            public const string Denied = "/denied";
        }

        public static class Transaction
        {
            public const string Details = PrefixTransaction + "/details";
            public const string JobOffer = PrefixJobOffer;
        }

        public static class Admin
        {
            public const string Users = PrefixAdmin + "/users";
        }

        public static class TA
        {
            public const string Dashboard = PrefixTA + "/dashboard";
            public const string Analysis = PrefixTA + "/analysis";
            public const string Candidates = PrefixTA + "/candidates";
            public const string JobOffer = PrefixTA + "/joboffer";
        }
    }
}
