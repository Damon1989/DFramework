using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DFramework.EntityFramework;
using DFramework.EntityFramework.Repositories;
using DFramework.IoC;
using DFramework.KendoUI.Domain;
using DFramework.KendoUI.Models;
using DFramework.UnitOfWork;

namespace DFramework.KendoUI.Repositories.Impl
{
    public class KendoUIRepository : DomainRepository, IKendoUIRepository
    {
        protected KendoDbContext _db;

        public KendoUIRepository(KendoDbContext dbContext, IUnitOfWork unitOfWork, IContainer container)
            : base(dbContext, unitOfWork, container)
        {
        }
    }
}