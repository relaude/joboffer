using JO.DataModel.View;
using JO.Persistence.DataAccess;
using JO.Persistence.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Persistence.Repositories
{
    public class VwJobOfferTransactionRepo : GenericRepository<VwJobOfferTransactions>, IVwJobOfferTransactionRepo
    {
        public VwJobOfferTransactionRepo(JobOfferDbContext context) : base(context) { }
    }
}
