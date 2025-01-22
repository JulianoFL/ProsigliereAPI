using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProsigliereAPI.Models.DBModels;

namespace ProsigliereAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public abstract class BaseController : ControllerBase
    {
        internal IMapper Mapper;
        internal IBlogPostRepository DBRepo;
        internal ILogger<ControllerBase> Logger;

        public BaseController(IBlogPostRepository DBRepo, ILogger<ControllerBase> Logger, IMapper Mapper) 
        {
            this.Mapper = Mapper;
            this.DBRepo = DBRepo;
            this.Logger = Logger;
        }
    }
}
