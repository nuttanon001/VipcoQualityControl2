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
    public class BranchController : GenericController<Branch>
    {
        public BranchController(IRepositoryQualityControl<Branch> repo,
            IMapper mapper) : 
            base(repo, mapper) { }

        // POST: api/Branch/GetScroll
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            if (Scroll == null)
                return BadRequest();

            var QueryData = this.repository.GetAllAsQueryable();

            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.ToLower().Split(null);

            foreach (var keyword in filters)
            {
                QueryData = QueryData.Where(x => x.Name.ToLower().Contains(keyword) ||
                                                 x.Address.ToLower().Contains(keyword));
            }

            // Order
            switch (Scroll.SortField)
            {
                case "Name":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.Name);
                    else
                        QueryData = QueryData.OrderBy(e => e.Name);
                    break;
                case "Address":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.Address);
                    else
                        QueryData = QueryData.OrderBy(e => e.Address);
                    break;
                default:
                    QueryData = QueryData.OrderBy(e => e.Name);
                    break;
            }
            // Get TotalRow
            Scroll.TotalRow = await QueryData.CountAsync();
            // Skip Take
            QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);

            return new JsonResult(new ScrollDataViewModel<Branch>(Scroll,
                await QueryData.ToListAsync()), this.DefaultJsonSettings);
        }
    }
}
