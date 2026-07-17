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
            public const int Validated = 3;
            public const int Cancelled = 4;
            public const int Overdue = 5;
        }

        public static class Application
        {
            public const int New = 1;
            public const int MatrixSelected = 2;
            public const int ProposalCreated = 3;
        }
    }
}
