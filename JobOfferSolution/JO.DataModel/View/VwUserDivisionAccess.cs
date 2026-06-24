using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwUserDivisionAccess
    {
        public int Id { get; set; }
        public int? JobOfferUserId { get; set; }
        public int? DivisionId { get; set; }
        public string? JOUserName { get; set; }
        public string? CompanyCode { get; set; }
        public string? CompanyName { get; set; }
        public string? DivisionCode { get; set; }
        public string? DivisionName { get; set; }
    }
}
