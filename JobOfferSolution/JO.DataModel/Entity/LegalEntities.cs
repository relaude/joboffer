using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class LegalEntities
    {
        [Key] public int Id { get; set; }
        public int? JobOfferId { get; set; }
        public int? CandidateId { get; set; }
        public int? CompanyId { get; set; }
        public int? DivisionId { get; set; }
        public int? MatrixId { get; set; }
        public int? JobPositionId { get; set; }
    }
}
