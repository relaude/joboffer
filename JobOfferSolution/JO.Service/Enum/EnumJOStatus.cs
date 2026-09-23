using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Enum
{
    public enum EnumJOStatus
    {
        Draft=1,
        Created=2,
        ForTALeadReview=3,
        ForPEHeadApproval=4,
        ForDivisionHeadApproval=5,
        ForHRODHeadApproval=6,
        ForPresidentApproval=7,
        ForDiscussion=8,
        AcceptedAndCompleted=9,
        SendBack=10,
        ForNegotiation=11,
        Declined=12,
        ForDivisionHeadL2Approval=13,
        ForPEHeadReview=14,
        ForHRODHead2ndApproval=15
    }
    public enum EnumJOEmailStatus
    {
        Draft = 1,
        ForApproval = 2,
        Approved = 3,
    }

    public enum EnumJODocumentType
    {
        JOLetter = 1,
        Benefits = 2,
        MaskedJOLetter = 3
    }
}
