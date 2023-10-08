using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RnD.Angular.Backend.Models;

namespace RnD.Angular.Backend.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("[controller]")]
    [ApiController] 
    public class CustomerController : ControllerBase
    {
        private readonly LearnDbContext learnDbContext;

        public CustomerController(LearnDbContext learnDb)
        {
            learnDbContext = learnDb;
        }
        
        [HttpGet]
        public IEnumerable<CustomerModel> Get()
        {
            return learnDbContext.TblCustomers.ToList(); 
        }
    }
}
