using JO.DataModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.Entity
{
    public class SalaryMatrixBand : BaseEntity
    {
        public int? SalaryMatrixId { get; set; }
        public int? JobLevelId { get; set; }
        public int? JobFamilyId { get; set; }
        public int? PositionGradeId { get; set; }
        public decimal? BandMinimum { get; set; }
        public decimal? BandMidpoint { get; set; }
        public decimal? BandMaximum { get; set; }
        public bool? BelowBandFlagEnabled { get; set; }
        public bool? AboveBandFlagEnabled { get; set; }
    }
}
