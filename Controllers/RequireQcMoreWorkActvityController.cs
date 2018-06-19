using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using VipcoQualityControl.Services;
using VipcoQualityControl.ViewModels;
using VipcoQualityControl.Models.QualityControls;
using AutoMapper;

namespace VipcoQualityControl.Controllers
{
    //api/RequireQcMoreWorkActvity
    [Route("api/[controller]")]
    [ApiController]
    public class RequireQcMoreWorkActvityController : GenericController<RequireQcMoreWorkActvity>
    {
        public RequireQcMoreWorkActvityController(IRepositoryQualityControl<RequireQcMoreWorkActvity> repo,
           IMapper mapper) :
           base(repo, mapper)
        { }

        // GET: api/RequireQcMoreWorkActvity/GetByMaster
        [HttpGet("GetByMaster")]
        public async Task<IActionResult> GetByMaster(int key)
        {
            if (key > 0)
            {
                Expression<Func<RequireQcMoreWorkActvity, bool>>
                    expression = e => e.RequireQualityControlId == key;

                var RequireHasMaster = (await this.repository.FindAllAsync(expression, true));
                if (RequireHasMaster != null)
                {
                    var DataMapper = new List<RequireQcMoreWorkActvityViewModel>();
                    foreach (var item in RequireHasMaster)
                        DataMapper.Add(this.mapper.Map<RequireQcMoreWorkActvity, RequireQcMoreWorkActvityViewModel>(item));

                    return new JsonResult(DataMapper, this.DefaultJsonSettings);
                }
            }
            return BadRequest();
        }
    }
}
