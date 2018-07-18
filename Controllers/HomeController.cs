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
using VipcoQualityControl.Models.Machines;
using VipcoQualityControl.Models.QualityControls;
using AutoMapper;


namespace VipcoQualityControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : GenericController<DashBoardDaily>
    {
        #region IReposiotory

        private readonly IRepositoryQualityControl<RequireQualityControl> repositoyRequireQC;
        private readonly IRepositoryMachine<EmployeeGroupMis> repositoryWorkGroup;
        #endregion

        public HomeController(IRepositoryQualityControl<DashBoardDaily> repo,
            IRepositoryQualityControl<RequireQualityControl> repoRequireQC,
            IRepositoryMachine<EmployeeGroupMis> repoWorGroup,
            IMapper mapper):
            base(repo, mapper)
        {
            //IRepository
            this.repositoyRequireQC = repoRequireQC;
            this.repositoryWorkGroup = repoWorGroup;
        }

        // GET: api/Home
        [HttpPost("GetDashBoardDaily")]
        public async Task<IActionResult> GetDashBoardDaily([FromBody]DashBoardOptionViewModel Option)
        {
            if (Option != null && Option.PickDate != null)
            {
                var DashData = await this.repository.GetFirstOrDefaultAsync(
                    selector: x => x,
                    predicate: x => x.DashBoardDate.Value.Date == Option.PickDate.Value.Date);
                if (DashData != null)
                    return new JsonResult(DashData, this.DefaultJsonSettings);
                else
                {
                    var requireQCData = await this.repositoyRequireQC.GetToListAsync(
                        selector: x => x,
                        predicate: x => x.CreateDate.Value.Date == Option.PickDate.Value.Date,
                        include: x => x.Include(z => z.WorkGroupQualityControl));

                    var Top3 = requireQCData.GroupBy(z => z.GroupMIS).
                        Select(x => new
                        {
                           GroupMis = x.Key,
                           GroupName = this.repositoryWorkGroup.GetFirstOrDefault(z => z.GroupDesc,z => z.GroupMis == x.Key) ?? "-",
                           TotalRequire = x.Count(z => z.RequireStatus != RequireStatus.Cancel)
                        }).OrderByDescending(x => x.TotalRequire).Take(3).ToList();


                    if (requireQCData != null)
                    {
                        var newDashData = new DashBoardDaily()
                        {
                            CreateDate = DateTime.Now,
                            Creator = Option.UserName ?? "Someone",
                            DashBoardDate = DateTime.Now,
                            Top1Name = Top3 != null && Top3.Count > 0 ? Top3[0].GroupName : "-",
                            Top1Require = Top3 != null && Top3.Count > 0 ? Top3[0].TotalRequire : 0,
                            Top2Name = Top3 != null && Top3.Count > 1 ? Top3[1].GroupName : "-",
                            Top2Require = Top3 != null && Top3.Count > 1 ? Top3[1].TotalRequire : 0,
                            Top3Name = Top3 != null && Top3.Count > 2 ? Top3[2].GroupName : "-",
                            Top3Require = Top3 != null && Top3.Count > 2 ? Top3[2].TotalRequire : 0,
                            TotalRequire = requireQCData.Count(x => x.RequireStatus != RequireStatus.Cancel),
                            TotalRequireFail = requireQCData.Count(x => x.RequireStatus != RequireStatus.Cancel && (x.RequireStatus == RequireStatus.QcFail || x.RequireStatus == RequireStatus.Revise)),
                            TotalRequirePass = requireQCData.Count(x => x.RequireStatus != RequireStatus.Cancel && x.RequireStatus == RequireStatus.Complate)
                        };

                        await this.repository.AddAsync(newDashData);
                        return new JsonResult(newDashData, this.DefaultJsonSettings);
                    }
                }
            }

            return BadRequest(new { Error = "Data not been found." });
        }
      
    }
}
