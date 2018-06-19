﻿using System;
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

        // GET: api/MasterProjectList/Autocomplate
        [HttpGet("Autocomplate")]
        public async Task<IActionResult> GetAutocomplate(string Filter)
        {
            // Expression<Func<MasterProjectList, bool>> condition = m => m.MarkNo.ToLower().Contains(Filter.ToLower());
            // return new JsonResult(await this.repository.FindAllAsync(condition), this.DefaultJsonSettings);

            var QueryData = await this.repository.GetAllAsQueryable()
                                                 .Where(x => x.MarkNo.ToLower().Contains(Filter.ToLower()))
                                                 .Select(x => new
                                                     {
                                                         x.MarkNo,
                                                         x.Name
                                                     })
                                                 .Distinct()
                                                 .Take(10)
                                                 .ToListAsync();
            if (QueryData != null)
                return new JsonResult(QueryData, this.DefaultJsonSettings);

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
                var HasData = await this.repositoryRequireHasMl.GetAllAsQueryable()
                                        .Where(x => x.RequireQualityControlId == key)
                                        .ToListAsync();
                if (HasData != null)
                {
                    var ListData = new List<MasterProjectListViewModel>();
                    foreach (var item in HasData)
                    {
                        if (item.MasterProjectList != null)
                        {
                            var MapData = this.mapper.Map<MasterProjectList, MasterProjectListViewModel>(item.MasterProjectList);
                            MapData.Quantity = item.Quantity;
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
                var HasData = await this.repositoryRequireHasMl.GetAllAsQueryable()
                                        .Where(x => x.RequireQualityControlId == key && 
                                                    x.RequireQualityControl.RequireStatus == RequireStatus.QcFail)
                                        .ToListAsync();
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
