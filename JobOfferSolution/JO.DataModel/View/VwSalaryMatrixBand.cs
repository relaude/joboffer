using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwSalaryMatrixBand
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }

        public int? SalaryMatrixId { get; set; }
        public int? JobLevelId { get; set; }
        public int? JobFamilyId { get; set; }
        public int? PositionGradeId { get; set; }
        public decimal? BandMinimum { get; set; }
        public decimal? BandMidpoint { get; set; }
        public decimal? BandMaximum { get; set; }
        public bool? BelowBandFlagEnabled { get; set; }
        public bool? AboveBandFlagEnabled { get; set; }

        public string? MatrixCode { get; set; }
        public string? MatrixName { get; set; }
        public string? JobLevelName { get; set; }
        public string? JobFamilyName { get; set; }
        public string? PositionGrade { get; set; }
    }
}
