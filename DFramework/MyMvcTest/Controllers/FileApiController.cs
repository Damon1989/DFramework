using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MyMvcTest.Controllers
{
    public class FileApiController : ApiController
    {
        //public HttpResponseMessage Upload()
        //{
        //    HttpResponseMessage response = null;
        //    var request = HttpContext.Current.Request;
        //    var projectId = request.Form["projectId"];
        //    string filePath = $"fileupload/{projectId}/10";
        //    if (request.Files.Count>0)
        //    {
        //        var fileNameList = new List<string>();
        //        //request.Files客户端传过来的文件
        //        foreach (var file in request.Files)
        //        {
                    
        //        }
        //    }
        //}

        //public bool UploadFile()
        //{
        //    var request = new RestRequest(Method.POST);
        //    request.AddParameter("projectId", Guid.NewGuid().ToString("n"));
        //    request.AddFile("file", "");
        //}
    }
}
