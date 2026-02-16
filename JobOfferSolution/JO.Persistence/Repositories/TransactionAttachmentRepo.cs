using JO.DataModel.Entity;
using JO.Persistence.DataAccess;
using JO.Persistence.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Persistence.Repositories
{
    public class TransactionAttachmentRepo : GenericRepository<TransactionAttachments>, ITransactionAttachmentRepo
    {
        public TransactionAttachmentRepo(JobOfferDbContext context) : base(context) { }
    }
}
