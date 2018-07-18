using System;
using System.IO;
using System.Linq;
using System.Dynamic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

using VipcoQualityControl.Helper;
using VipcoQualityControl.Services;
using VipcoQualityControl.ViewModels;
using VipcoQualityControl.Models.Machines;
using VipcoQualityControl.Models.QualityControls;

using AutoMapper;

namespace VipcoQualityControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WelderHasProjectController : GenericController<WelderHasProject>
    {
        private readonly IRepositoryQualityControl<WelderHasProjectViewModel> repositoryView;

        public WelderHasProjectController(IRepositoryQualityControl<WelderHasProject> repo,
            IRepositoryQualityControl<WelderHasProjectViewModel> repoView,
            IMapper mapper)
            : base(repo: repo, mapper: mapper)
        {
            //Repository
            this.repositoryView = repoView;
        }

        // GET: api/WelderHasProject/GetByMaster
        [HttpGet("GetByMaster/")]
        public async Task<IActionResult> GetByMaster(int key)
        {
            var QueryData = await this.repositoryView.GetToListAsync(
                selector: x => x,
                predicate: x => x.ProjectCodeMasterId == key);
            if (QueryData.Any())
                return new JsonResult(QueryData, this.DefaultJsonSettings);
            else
                return BadRequest(new { error = "Data not been found." });
        }

        // GET: api/controller/5
        [HttpGet("GetKeyNumber")]
        public override async Task<IActionResult> Get(int key)
        {
            var HasData = await this.repositoryView.GetToListAsync(x => x,x => x.ProjectCodeMasterId == key);
            if (HasData != null)
            {
                var GroupData = new WelderSummanyProject()
                {
                    ProjectCodeMasterId = HasData.FirstOrDefault().ProjectCodeMasterId,
                    ProjectCodeMasterString = HasData.FirstOrDefault().ProjectCodeMasterString,
                    WelderProjects = HasData.ToList()
                };
                return new JsonResult(GroupData, this.DefaultJsonSettings);
            }

            return BadRequest(new { error = "Data not been found." });
        }

        // POST: api/WelderHasProjectViewModel/GetScroll
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            if (Scroll == null)
                return BadRequest();
            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.Split(null);

            var predicate = PredicateBuilder.False<WelderHasProjectViewModel>();
            foreach (string keyword in filters)
            {
                string temp = keyword;
                predicate = predicate.Or(p => p.ProjectCodeMasterString.Contains(temp));
            }
            if (!string.IsNullOrEmpty(Scroll.Where))
                predicate = predicate.And(p => p.Creator == Scroll.Where);
            // Where
            //Order by
            Func<ICollection<WelderHasProjectViewModel>, IOrderedEnumerable<WelderHasProjectViewModel>> order;
            // Order
            switch (Scroll.SortField)
            {
                case "ProjectCodeMasterString":
                    if (Scroll.SortOrder == -1)
                        order = o => o.OrderByDescending(x => x.ProjectCodeMasterString);
                    else
                        order = o => o.OrderBy(x => x.ProjectCodeMasterString);
                    break;
                default:
                    order = o => o.OrderByDescending(x => x.ProjectCodeMasterString);
                    break;
            }

            var QueryData = await this.repositoryView.GetToListAsync(
                                    selector: x => x, // Selected
                                    predicate: predicate); // Where 

            var GroupData = order(QueryData).GroupBy(x => new { x.ProjectCodeMasterString, x.ProjectCodeMasterId});

            // Get TotalRow
            Scroll.TotalRow = GroupData.Count();

            return new JsonResult(new ScrollDataViewModel<WelderSummanyProject>(Scroll,
                GroupData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 10).Select(x => new WelderSummanyProject
                {
                    ProjectCodeMasterId = x.Key.ProjectCodeMasterId,
                    ProjectCodeMasterString = x.Key.ProjectCodeMasterString,
                    TotalWelder = x.Count()
                })), this.DefaultJsonSettings);
        }
    }

    public class WelderSummanyProject
    {
        public string ProjectCodeMasterString { get; set; }
        public int? ProjectCodeMasterId { get; set; }
        public int? TotalWelder { get; set; }
        public List<WelderHasProjectViewModel> WelderProjects { get; set; } = new List<WelderHasProjectViewModel>();
    }
}
