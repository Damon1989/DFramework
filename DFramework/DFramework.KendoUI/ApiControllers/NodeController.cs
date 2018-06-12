using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DFramework.AspNet.Controllers;
using DFramework.EntityFramework.Repositories;
using DFramework.Infrastructure;
using DFramework.KendoUI.Domain;
using DFramework.KendoUI.Models;

namespace DFramework.KendoUI.ApiControllers
{
    [RoutePrefix("api/node")]
    public class NodeController : ApiControllerBase
    {
        private readonly KendoDbContext _kendoDbContext;

        public NodeController(IExceptionManager exceptionManager,
            KendoDbContext kendoDbContext)
            : base(exceptionManager)
        {
            _kendoDbContext = kendoDbContext;
        }

        [Route("getnode")]
        public async Task<ApiResult<Node>> GetNodes()
        {
            return await ProcessAsync(async () =>
                await _kendoDbContext.Nodes.FirstOrDefaultAsync().ConfigureAwait(false));
        }
    }
}