using DFramework.AspNet.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DFramework.Infrastructure;

namespace DFramework.KendoUI.ApiControllers
{
    public class FileController : ApiControllerBase
    {
        public FileController(IExceptionManager exceptionManager) : base(exceptionManager)
        {
        }
    }
}