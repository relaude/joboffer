using JO.DataModel.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwTransactionAttachments : TransactionAttachments
    {
        public string TransactionNumber { get; set; }
        public string TypeName { get; set; }
    }
}
