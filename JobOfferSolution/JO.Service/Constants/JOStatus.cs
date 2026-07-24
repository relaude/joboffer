using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Constants
{
    public class JOStatus
    {
        public static class Proposal
        {
            public const int New = 1;
            public const int Approve = 2;
            public const int Return = 3;
            public const int Reject = 4;
        }

        public static class Action
        {
            public const int Open = 1;
            public const int Current = 2;
            public const int Next = 3;
            public const int Done = 4;
        }

        public static class Request
        {
            public const int Awaiting = 1;
            public const int Submitted = 2;
            public const int Cancelled = 4;
            public const int Overdue = 5;
        }

        public static class Application
        {
            public const int MatrixSelected = 3;
            public const int ProposalCreated = 4;
            public const int TRApproved = 5;
            public const int HRODApproved = 6;
            public const int DHApproved = 7;
            public const int Approved = 8;
        }

        public static class SalaryBand
        {
            public const int Within = 1;
            public const int Midpoint = 2;
            public const int Upper = 3;
            public const int Exceed = 4;
            public const int Lower = 5;
        }
    }
}
