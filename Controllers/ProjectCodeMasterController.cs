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
using VipcoQualityControl.Models.Machines;
using AutoMapper;

namespace VipcoQualityControl.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ProjectCodeMasterController : GenericMachineController<ProjectCodeMaster>
    {
        public ProjectCodeMasterController(IRepositoryMachine<ProjectCodeMaster> repo,
            IMapper mapper) : base(repo,mapper) { }

        // POST: api/Branch/GetScroll
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            var QueryData = this.repository.GetAllAsQueryable();
            // Where
            if (!string.IsNullOrEmpty(Scroll.Where))
            {
                // QueryData = QueryData.Where(x => x.GroupCode == Scroll.Where);
            }
            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.ToLower().Split(null);
            foreach (var keyword in filters)
            {
                QueryData = QueryData.Where(x => x.ProjectCode.ToLower().Contains(keyword) ||
                                                 x.ProjectName.ToLower().Contains(keyword));
            }

            // Order
            switch (Scroll.SortField)
            {
                case "ProjectCode":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.ProjectCode);
                    else
                        QueryData = QueryData.OrderBy(e => e.ProjectCode);
                    break;

                case "ProjectName":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.ProjectName);
                    else
                        QueryData = QueryData.OrderBy(e => e.ProjectName);
                    break;

                default:
                    QueryData = QueryData.OrderByDescending(e => e.ProjectCode)
                                         .ThenBy(e => e.ProjectName);
                    break;
            }
            // Get TotalRow
            Scroll.TotalRow = await QueryData.CountAsync();
            // Skip and Take
            QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);

            var ListData = new List<ProjectMasterViewModel>();
            foreach (var item in await QueryData.ToListAsync())
                ListData.Add(this.mapper.Map<ProjectCodeMaster, ProjectMasterViewModel>(item));

            return new JsonResult(new ScrollDataViewModel<ProjectMasterViewModel>
                (Scroll, ListData), this.DefaultJsonSettings);
        }
    }
}
