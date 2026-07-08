using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Constants
{
    public class JORoutes
    {
        private const string PrefixAdmin = "/admin";
        private const string PrefixTA = "/ta";
        private const string PrefixJobOffer = "/joboffer";
        private const string PrefixHROD = "/hrod";
        private const string PrefixDH = "/dh";
        private const string PrefixMock = "/mock";

        public static class Public
        {
            public const string Login = "/login";
            public const string Denied = "/denied";
            public const string Landing = "/";
        }

        public static class Private
        {
            public const string Home = "/home";
        }

        public static class Transaction
        {
            public const string JobOffer = PrefixJobOffer;
        }

        public static class Admin
        {
            public const string Users = PrefixAdmin + "/users";
            public const string UserDetails = PrefixAdmin + "/user";
            public const string NewUser = PrefixAdmin + "/new-user";
            public const string EditUser = PrefixAdmin + "/edit-user";
            public const string ToggleActive = PrefixAdmin + "/toggle-user";
            public const string SalaryMatrix = PrefixAdmin + "/salary-matrix";
            public const string SalaryMatrixNew = PrefixAdmin + "/salary-matrix/new";
        }

        public static class TA
        {
            public const string Dashboard = PrefixTA + "/dashboard";
            public const string Candidates = PrefixTA + "/candidates";

            public const string MSFormCandidate = PrefixTA + "/msform-candidate";
            public const string MSFormDetails = PrefixTA + "/msform-details";
            public const string MSFormFileUpload = PrefixTA + "/msform-file-upload";

            //Legal Entity
            public const string DivisionSetup = PrefixTA + "/division-setup";

            //JO Analysis
            public const string ApplicationDetails = PrefixTA + "/application-details";
            public const string JOAnalysis = PrefixTA + "/joboffer-analysis";
            public const string JOAnalysisDetails = PrefixTA + "/analysis-details";

            public const string RequestTracker = PrefixTA + "/request-tracker";
            public const string JobOfferTracker = PrefixTA + "/joboffer-tracker";

            public const string JobOffer = PrefixTA + "/joboffer";
            public const string Accept = PrefixTA + "/joboffer/accept";
            public const string Return = PrefixTA + "/joboffer/return";
            public const string Email = PrefixTA + "/email";
            public const string MassUpload = PrefixTA + "/mass-upload";
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

        public static class MockUp
        {
            public const string FormRequest = PrefixMock + "/form-request";
            public const string SendRequest = PrefixMock + "/send-request";
            public const string Legal = PrefixMock + "/legal";
            public const string Analysis = PrefixMock + "/analysis";
            public const string Approval = PrefixMock + "/approval";
            public const string Accept = PrefixMock + "/accept";
            public const string Negotiate = PrefixMock + "/negotiate";
            public const string TADashboard = PrefixMock + "/ta-dashboard";
            public const string ApproverDashboard = PrefixMock + "/approver-dashboard";
            public const string Users = PrefixMock + "/users";
            public const string NewUser = PrefixMock + "/new-user";
            public const string ARBAC = PrefixMock + "/arbac";
        }
    }
}
