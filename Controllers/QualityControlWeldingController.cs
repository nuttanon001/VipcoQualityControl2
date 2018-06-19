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
using VipcoQualityControl.Models.Machines;
using AutoMapper;

namespace VipcoQualityControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QualityControlWeldingController : GenericController<QualityControlWelding>
    {
        //Repository
        private readonly IRepositoryMachine<ProjectCodeMaster> repositoryProjectMaster;

        public QualityControlWeldingController(IRepositoryQualityControl<QualityControlWelding> repo,
            IRepositoryMachine<ProjectCodeMaster> repoProjectMaster,
            IMapper mapper) :
            base(repo, mapper)
        {
            // Repository Machine
            this.repositoryProjectMaster = repoProjectMaster;
        }

        // GET: api/RequireHasMasterProject/GetByMaster
        [HttpGet("GetByMaster")]
        public async Task<IActionResult> GetByMaster(int key)
        {
            if (key > 0)
            {
                Expression<Func<QualityControlWelding, bool>>
                    expression = e => e.RequireQualityControlId == key;

                var QcWelding = (await this.repository.FindAllAsync(expression));
                if (QcWelding != null)
                {
                    var DataMapper = new List<QualityControlWeldingViewModel>();
                    foreach (var item in QcWelding)
                    {
                        if (!item.Reject.HasValue)
                            item.Reject = 0;

                        DataMapper.Add(this.mapper.Map<QualityControlWelding, QualityControlWeldingViewModel>(item));
                    }

                    return new JsonResult(DataMapper, this.DefaultJsonSettings);
                }
            }
            return BadRequest();
        }

        // GET: api/controller/5
        [HttpGet("GetKeyNumber")]
        public override async Task<IActionResult> Get(int key)
        {
            var HasData = await this.repository.GetAsync(key,true);
            var MapData = this.mapper.Map<QualityControlWelding, QualityControlWeldingViewModel>(HasData);
            if (MapData.ProjectCodeMasterId != null && MapData.ProjectCodeMasterId > 0)
            {
                var HasProject = await this.repositoryProjectMaster.GetAsync(MapData.ProjectCodeMasterId.Value);
                if (HasProject != null)
                    MapData.ProjectCodeMasterString = $"{HasProject.ProjectCode}/{HasProject.ProjectName}";
            }
            return new JsonResult(MapData, this.DefaultJsonSettings);
        }

        // POST: api/QualityControlWelding/Autocomplate
        [HttpPost("Autocomplate")]
        public async Task<IActionResult> GetAutocomplate([FromBody] AutoComplateViewModel autoComplate)
        {
            IQueryable<ResultAutoComplateViewModel> QueryData;

            if (autoComplate.ByColumn.IndexOf("MarkNoPreview") != -1)
                QueryData = this.repository.GetAllAsQueryable().Where(x => x.MarkNoPreview.ToLower().Contains(autoComplate.Filter.ToLower()))
                                .Select(x => new ResultAutoComplateViewModel { AutoComplate = x.MarkNoPreview }).AsQueryable();
            else if (autoComplate.ByColumn.IndexOf("WelderNo") != -1)
                QueryData = this.repository.GetAllAsQueryable().Where(x => x.WelderNo.ToLower().Contains(autoComplate.Filter.ToLower()))
                                .Select(x => new ResultAutoComplateViewModel { AutoComplate = x.WelderNo }).AsQueryable();
            else if (autoComplate.ByColumn.IndexOf("ProcessWeld") != -1)
                QueryData = this.repository.GetAllAsQueryable().Where(x => x.ProcessWeld.ToLower().Contains(autoComplate.Filter.ToLower()))
                                .Select(x => new ResultAutoComplateViewModel { AutoComplate = x.ProcessWeld }).AsQueryable();
            else
                return NoContent();

            var HasAutoComplate = await QueryData.Distinct()
                                                .Take(10)
                                                .ToListAsync();
            if (HasAutoComplate.Any())
                return new JsonResult(HasAutoComplate, DefaultJsonSettings);

            return NoContent();
        }


        // POST: api/QualityControlWelding/GetScroll
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            if (Scroll == null)
                return BadRequest();

            var QueryData = this.repository.GetAllAsQueryable().AsQueryable();

            //Where
            if (!string.IsNullOrEmpty(Scroll.Where))
                QueryData = QueryData.Where(x => x.Creator == Scroll.Where);

            QueryData = QueryData.Where(x => x.RequireQualityControlId == null);

            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.ToLower().Split(null);

            foreach (var keyword in filters)
            {
                QueryData = QueryData.Where(x => x.MarkNoPreview.ToLower().Contains(keyword) ||
                                                 x.MarkNo.ToLower().Contains(keyword) ||
                                                 x.WelderNo.ToLower().Contains(keyword) ||
                                                 x.RequireQualityControl.RequireQualityNo.ToLower().Contains(keyword));
            }

            // Order
            switch (Scroll.SortField)
            {
                case "RequireQualityControlNo":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.RequireQualityControl.RequireQualityNo);
                    else
                        QueryData = QueryData.OrderBy(e => e.RequireQualityControl.RequireQualityNo);
                    break;
                case "WeldingDate":
                    QueryData = Scroll.SortOrder == -1 ?
                            QueryData.OrderByDescending(e => e.WeldingDate) :
                            QueryData.OrderBy(e => e.WeldingDate);
                    break;
                case "MarkNo":
                    QueryData = Scroll.SortOrder == -1 ?
                            QueryData.OrderByDescending(e => e.MarkNo) :
                            QueryData.OrderBy(e => e.MarkNo);
                    break;
                case "MarkNoPreview":
                    QueryData = Scroll.SortOrder == -1 ?
                            QueryData.OrderByDescending(e => e.MarkNoPreview) :
                            QueryData.OrderBy(e => e.MarkNoPreview);
                    break;
                case "WelderNo":
                    QueryData = Scroll.SortOrder == -1 ?
                            QueryData.OrderByDescending(e => e.WelderNo) :
                            QueryData.OrderBy(e => e.WelderNo);
                    break;
                case "ProcessWeld":
                    QueryData = Scroll.SortOrder == -1 ?
                            QueryData.OrderByDescending(e => e.ProcessWeld) :
                            QueryData.OrderBy(e => e.ProcessWeld);
                    break;
                default:
                    QueryData = QueryData.OrderBy(e => e.MarkNo);
                    break;
            }
            // Get TotalRow
            Scroll.TotalRow = await QueryData.CountAsync();
            // Skip Take
            QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);

            var HasMapper = new List<QualityControlWeldingViewModel>();

            foreach (var item in await QueryData.ToListAsync())
            {
                var MapData = this.mapper.Map<QualityControlWelding, QualityControlWeldingViewModel>(item);
                if (MapData.ProjectCodeMasterId != null && MapData.ProjectCodeMasterId > 0)
                {
                    var HasProject = await this.repositoryProjectMaster.GetAsync(MapData.ProjectCodeMasterId.Value);
                    if (HasProject != null)
                        MapData.ProjectCodeMasterString = $"{HasProject.ProjectCode}/{HasProject.ProjectName}";
                }
                HasMapper.Add(MapData);
            }

            return new JsonResult(new ScrollDataViewModel<QualityControlWeldingViewModel>(Scroll, HasMapper), this.DefaultJsonSettings);
        }
    }
}
