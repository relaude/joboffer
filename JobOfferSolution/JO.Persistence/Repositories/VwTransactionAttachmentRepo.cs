using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Persistence.DataAccess;
using JO.Persistence.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Persistence.Repositories
{
    public class VwTransactionAttachmentRepo : GenericRepository<VwTransactionAttachments>, IVwTransactionAttachmentRepo
    {
        public VwTransactionAttachmentRepo(JobOfferDbContext context) : base(context) { }
    }
}
