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
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MasterProjectListController : GenericController<MasterProjectList>
    {
        private readonly IRepositoryQualityControl<RequireHasMasterProject> repositoryRequireHasMl;

        public MasterProjectListController(IRepositoryQualityControl<MasterProjectList> repo,
            IRepositoryQualityControl<RequireHasMasterProject> repoRequireHasMl,
            IMapper mapper): base(repo,mapper)
        {
            this.repositoryRequireHasMl = repoRequireHasMl;
        }

        // POST: api/QualityControlWelding/Autocomplate
        [HttpPost("Autocomplate")]
        public async Task<IActionResult> GetAutocomplate([FromBody] AutoComplateViewModel autoComplate)
        {
            IQueryable<ResultAutoComplateViewModel> QueryData;

            if (autoComplate.ByColumn.IndexOf("MarkNo") != -1)
                QueryData = this.repository.GetAllAsQueryable().Where(x => x.MarkNo.ToLower().Contains(autoComplate.Filter.ToLower()))
                                .Select(x => new ResultAutoComplateViewModel { AutoComplate = x.MarkNo }).AsQueryable();
            else if (autoComplate.ByColumn.IndexOf("DrawingNo") != -1)
                QueryData = this.repository.GetAllAsQueryable().Where(x => x.DrawingNo.ToLower().Contains(autoComplate.Filter.ToLower()))
                                .Select(x => new ResultAutoComplateViewModel { AutoComplate = x.DrawingNo }).AsQueryable();
            else if (autoComplate.ByColumn.IndexOf("GradeMaterial1") != -1)
                QueryData = this.repository.GetAllAsQueryable().Where(x => x.GradeMaterial1.ToLower().Contains(autoComplate.Filter.ToLower()))
                                .Select(x => new ResultAutoComplateViewModel { AutoComplate = x.GradeMaterial1 }).AsQueryable();
            else if (autoComplate.ByColumn.IndexOf("GradeMaterial2") != -1)
                QueryData = this.repository.GetAllAsQueryable().Where(x => x.GradeMaterial2.ToLower().Contains(autoComplate.Filter.ToLower()) || x.GradeMaterial1.ToLower().Contains(autoComplate.Filter.ToLower()))
                                .Select(x => new ResultAutoComplateViewModel { AutoComplate = x.GradeMaterial2 }).AsQueryable();
            else if (autoComplate.ByColumn.IndexOf("TypeMaterial1") != -1)
                QueryData = this.repository.GetAllAsQueryable().Where(x => x.TypeMaterial1.ToLower().Contains(autoComplate.Filter.ToLower()))
                                .Select(x => new ResultAutoComplateViewModel { AutoComplate = x.TypeMaterial1 }).AsQueryable();
            else if (autoComplate.ByColumn.IndexOf("TypeMaterial2") != -1)
                QueryData = this.repository.GetAllAsQueryable().Where(x => x.TypeMaterial2.ToLower().Contains(autoComplate.Filter.ToLower()) || x.TypeMaterial1.ToLower().Contains(autoComplate.Filter.ToLower()))
                                .Select(x => new ResultAutoComplateViewModel { AutoComplate = x.TypeMaterial2 }).AsQueryable();
            else
                return NoContent();

            var HasAutoComplate = await QueryData.Distinct()
                                                .Take(10)
                                                .ToListAsync();
            if (HasAutoComplate.Any())
                return new JsonResult(HasAutoComplate, DefaultJsonSettings);

            return NoContent();
        }

        // POST: api/MasterProjectList/GetScroll
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            if (Scroll == null)
                return BadRequest();

            var QueryData = this.repository.GetAllAsQueryable()
                                //.AsNoTracking() Error EF-Core 2.1 Preview2
                                .AsQueryable();

            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.ToLower().Split(null);

            foreach (var keyword in filters)
            {
                QueryData = QueryData.Where(x => x.Name.ToLower().Contains(keyword) ||
                                                 x.MarkNo.ToLower().Contains(keyword) ||
                                                 x.DrawingNo.ToLower().Contains(keyword) ||
                                                 x.Remark.ToLower().Contains(keyword) ||
                                                 x.Description.ToLower().Contains(keyword));
            }

            // Order
            switch (Scroll.SortField)
            {
                case "MarkNo":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.MarkNo);
                    else
                        QueryData = QueryData.OrderBy(e => e.MarkNo);
                    break;
                case "Name":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.Name);
                    else
                        QueryData = QueryData.OrderBy(e => e.Name);
                    break;
                case "Quantity":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.Quantity);
                    else
                        QueryData = QueryData.OrderBy(e => e.Quantity);
                    break;
                default:
                    QueryData = QueryData.OrderBy(e => e.MarkNo);
                    break;
            }
            // Get TotalRow
            Scroll.TotalRow = await QueryData.CountAsync();
            // Skip Take
            QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);
            try
            {
                var HasData = await QueryData.ToListAsync();
                var listData = new List<MasterProjectListViewModel>();
                foreach (var item in HasData)
                {
                    var MapItem = this.mapper.Map<MasterProjectList, MasterProjectListViewModel>(item);
                    listData.Add(MapItem);
                }

                return new JsonResult(new ScrollDataViewModel<MasterProjectListViewModel>(Scroll, listData), this.DefaultJsonSettings);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = $"{ex.ToString()}" });
            }
        }

        // GET:api/MasterProjectList/GetMasterProjectListByRequireQualityControl/5
        [HttpGet("GetMasterProjectListByRequireQualityControl")]
        public async Task<IActionResult> GetMasterProjectListByRequireQualityControl(int key)
        {
            if (key > 0)
            {
                var HasData = await this.repositoryRequireHasMl.GetToListAsync(
                        selector: x => x,
                        predicate: z => z.RequireQualityControlId == key,
                        include: c => c.Include(z => z.MasterProjectList).Include(z => z.RequireHasWelder));
                                        
                if (HasData != null)
                {
                    var ListData = new List<MasterProjectListViewModel>();
                    foreach (var item in HasData)
                    {
                        if (item.MasterProjectList != null)
                        {
                            var MapData = this.mapper.Map<MasterProjectList, MasterProjectListViewModel>(item.MasterProjectList);
                            MapData.Quantity = item.Quantity;
                            MapData.RequireHasWelder = item.RequireHasWelder != null ? this.mapper.Map<RequireHasWelder, RequireHasWelderViewModel>(item.RequireHasWelder) : null;
                            ListData.Add(MapData);
                        }
                    }
                    return new JsonResult(ListData, this.DefaultJsonSettings);
                }
            }
            return BadRequest();
        }

        // GET:api/MasterProjectList/GetMasterProjectListByRequireQualityControl/5
        [HttpGet("GetMasterProjectListByRequireQualityControlForFail")]
        public async Task<IActionResult> GetMasterProjectListByRequireQualityControlForFail(int key)
        {
            if (key > 0)
            {
                var HasData = await this.repositoryRequireHasMl.GetToListAsync(
                       selector: x => x,
                       predicate: z => z.RequireQualityControlId == key && 
                                       z.RequireQualityControl.RequireStatus == RequireStatus.QcFail,
                       include: c => c.Include(z => z.MasterProjectList));
                if (HasData != null)
                {
                    var ListData = new List<MasterProjectListViewModel>();
                    foreach (var item in HasData)
                    {
                        if (item.MasterProjectList != null)
                        {
                            var MapData = this.mapper.Map<MasterProjectList, MasterProjectListViewModel>(item.MasterProjectList);
                            MapData.Quantity = item.Quantity;
                            MapData.FailQuantity = item.Quantity - item.PassQuantity;
                            MapData.RemarkExter = item.QualityControlReason != null ? item.QualityControlReason.Name : "";
                            ListData.Add(MapData);
                        }
                    }
                    return new JsonResult(ListData, this.DefaultJsonSettings);
                }
            }
            return BadRequest();
        }
    }
}
