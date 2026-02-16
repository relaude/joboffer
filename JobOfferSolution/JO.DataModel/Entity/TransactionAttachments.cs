using JO.DataModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.Entity
{
    public class TransactionAttachments : BaseEntity
    {
        public int Transaction_Id { get; set; }
        public int FileType_Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
