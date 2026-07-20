using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwJobOfferWorkFlow
    {
        public int Id { get; set; }
        public int? JobOfferId { get; set; }
        public int? ActionId { get; set; }
        public int? DisplayOrder { get; set; }
        public string? ShortName { get; set; }
        public string? Icon { get; set; }
        public string? ActionName { get; set; }
        public string? ActionClass { get; set; }
    }
}
