using JO.DataModel.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.DTOs
{
    public class CandidateJobOffersDto
    {
        public VwCandidates Candidates { get; set; } = new();
        public List<VwJobOffers> JobOffers { get; set; } = new();
    }
}
