namespace JO.BlazorApp.Components.Pages
{
    public static class JORoutes
    {
        private const string PrefixAdmin = "/admin/manage";
        private const string PrefixTA = "/ta";
        private const string PrefixJobOffer = "/joboffer";
        private const string PrefixHROD = "/hrod";
        private const string PrefixDH = "/dh";

        public static class Public
        {
            public const string Login = "/login";
            public const string Denied = "/denied";
        }

        public static class Transaction
        {
            public const string JobOffer = PrefixJobOffer;
        }

        public static class Admin
        {
            public const string Users = PrefixAdmin + "/users";
        }

        public static class TA
        {
            public const string Dashboard = PrefixTA + "/dashboard";
            public const string Candidates = PrefixTA + "/candidates";
            public const string JobOffer = PrefixTA + "/joboffer";
            public const string Accept = PrefixTA + "/joboffer/accept";
            public const string Email = PrefixTA + "/email";
        }

        public static class HROD
        {
            public const string Dashboard = PrefixHROD + "/dashboard";
            public const string Approvals = PrefixHROD + "/approvals";
            public const string Approve = PrefixHROD + "/approve";
        }

        public static class DH
        {
            public const string Dashboard = PrefixDH + "/dashboard";
            public const string Approvals = PrefixDH + "/approvals";
            public const string Approve = PrefixDH + "/approve";
        }
    }
}
