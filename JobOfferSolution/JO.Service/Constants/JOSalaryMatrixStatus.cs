using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Constants
{
    public static class JOSalaryMatrixStatus
    {
        public const int Draft = 1;
        public const int PendingApproval = 2;
        public const int Approved = 3;
        public const int Expired = 4;
        public const int Archived = 5;
    }
}
