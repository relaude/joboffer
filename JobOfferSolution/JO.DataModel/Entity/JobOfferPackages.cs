using JO.DataModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.Entity
{
    public class JobOfferPackages : BaseEntity
    {
        public int Transaction_Id { get; set; }
        public string PackageName { get; set; }
        public decimal PackageAmount { get; set; }
        public int Priority { get; set; }
    }
}
