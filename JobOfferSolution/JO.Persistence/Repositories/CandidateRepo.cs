using JO.DataModel.Entity;
using JO.Persistence.DataAccess;
using JO.Persistence.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Persistence.Repositories
{
    public class CandidateRepo : GenericRepository<Candidates>, ICandidateRepo
    {
        public CandidateRepo(JobOfferDbContext context) : base(context) { }
    }
}
