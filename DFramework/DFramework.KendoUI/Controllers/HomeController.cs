using System.Linq;
using System.Web.Mvc;
using DFramework.KendoUI.Domain;
using DFramework.KendoUI.Models;
using DFramework.UnitOfWork;

namespace DFramework.KendoUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly KendoDbContext _kendoDbContext;
        private readonly IUnitOfWork _appUnitOfWork;
        private readonly IKendoUIRepository _domainRepository;

        public HomeController(KendoDbContext kendoDbContext,
            IUnitOfWork appUnitOfWork,
            IKendoUIRepository domainRepository)
        {
            _kendoDbContext = kendoDbContext;
            _appUnitOfWork = appUnitOfWork;
            _domainRepository = domainRepository;
        }

        // GET: Home
        public ActionResult Index()
        {
            _kendoDbContext.Files.FirstOrDefault();
            var node = new Node();
            _domainRepository.Add(node);
            _kendoDbContext.Nodes.Add(node);
            _appUnitOfWork.Commit();
            return View();
        }

        public ActionResult Mvvm()
        {
            return View();
        }

        public ActionResult Alert()
        {
            return View();
        }

        public ActionResult Select2()
        {
            return View();
        }
    }
}