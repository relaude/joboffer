using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwLegalEntities
    {
        public int Id { get; set; }
        public int? JobOfferId { get; set; }
        public int? MatrixId { get; set; }
        public string? RefNum { get; set; }
        public string? CompanyCodeName { get; set; }
        public string? DivisionCodeName { get; set; }
        public string? MatrixCodeName { get; set; }
        public string? PositionName { get; set; }
    }
}
