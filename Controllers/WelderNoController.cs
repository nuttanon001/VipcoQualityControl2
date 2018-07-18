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
using VipcoQualityControl.Helper;

namespace VipcoQualityControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WelderNoController : GenericController<WelderNo>
    {
        private readonly IRepositoryMachine<Employee> repositoryEmp;
        private readonly IRepositoryQualityControl<WelderNoViewModel> repositoryView;
        public WelderNoController(
            IRepositoryQualityControl<WelderNo> repo,
            IRepositoryQualityControl<WelderNoViewModel> repoView,
            IRepositoryMachine<Employee> repoEmp,
            IMapper mapper):
            base(repo,mapper)
        {
            //RepositoryQualityControl
            this.repositoryView = repoView;
            //RepositoryMachine
            this.repositoryEmp = repoEmp;
        }
        // GET: api/WelderNo/GetTeamWelder
        [HttpGet("GetTeamWelder")]
        public async Task<IActionResult> GetTeamWelder()
        {
            var HasData = await this.repository.GetToListAsync(selector: x => x.TeamWelderNo);
            if (HasData.Any())
                return new JsonResult(HasData.Distinct(), this.DefaultJsonSettings);
            return BadRequest(new { Error = "Data not been found." });
        }

        // GET: api/WelderNo/5
        [HttpGet("GetKeyNumber")]
        public override async Task<IActionResult> Get(int key)
        {
            var HasData = await this.repository.GetFirstOrDefaultAsync(
                selector: x => x,
                predicate: x => x.WelderNoId == key);

            if (HasData != null)
                return new JsonResult(HasData, this.DefaultJsonSettings);
            else
                return BadRequest(new { error = "Data not been found." });
        }

        // POST: api/WelderNo/GetScroll
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            if (Scroll == null)
                return BadRequest();
            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.Split(null);

            var predicate = PredicateBuilder.False<WelderNo>();
            foreach (string keyword in filters)
            {
                string temp = keyword;
                predicate = predicate.Or(x => x.NameThai.ToLower().Contains(temp) ||
                                              x.NameEnglish.ToLower().Contains(temp) ||
                                                 x.Remark.ToLower().Contains(temp) ||
                                                 x.WelderNoCode.ToLower().Contains(temp) ||
                                                 x.Description.ToLower().Contains(temp));
            }
            if (!string.IsNullOrEmpty(Scroll.Where))
                predicate = predicate.And(p => p.Creator == Scroll.Where);
            // Where
            //Order by
            Func<IQueryable<WelderNo>, IOrderedQueryable<WelderNo>> order;
            // Order
            switch (Scroll.SortField)
            {
                case "NameThai":
                    if (Scroll.SortOrder == -1)
                        order = o => o.OrderByDescending(x => x.NameThai);
                    else
                        order = o => o.OrderBy(x => x.NameThai);
                    break;
                case "EmpCode":
                    if (Scroll.SortOrder == -1)
                        order = o => o.OrderByDescending(x => x.EmpCode);
                    else
                        order = o => o.OrderBy(x => x.EmpCode);
                    break;
                case "WelderNoCode":
                    if (Scroll.SortOrder == -1)
                        order = o => o.OrderByDescending(x => x.WelderNoCode);
                    else
                        order = o => o.OrderBy(x => x.WelderNoCode);
                    break;
                default:
                    order = o => o.OrderByDescending(x => x.WelderNoCode);
                    break;
            }

            var QueryData = await this.repository.GetToListAsync(
                                    selector: selected => selected,  // Selected
                                    predicate: predicate, // Where
                                    orderBy: order, // Order
                                    skip: Scroll.Skip ?? 0, // Skip
                                    take: Scroll.Take ?? 10); // Take

            // Get TotalRow
            Scroll.TotalRow = await this.repository.GetLengthWithAsync(predicate: predicate);

            return new JsonResult(new ScrollDataViewModel<WelderNo>(Scroll, QueryData), this.DefaultJsonSettings);
        }

        #region NoUse
        public async Task<IActionResult> GetScroll2([FromBody] ScrollViewModel Scroll)
        {
            if (Scroll == null)
                return BadRequest();

            var QueryData = this.repositoryView.GetAllAsQueryable();
            var QueryDataEmp = this.repositoryEmp.GetAllAsQueryable();
            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.ToLower().Split(null);
            //QueryData = QueryData.Where(x => Employees.Contains(x.EmpCode));
            // filter
            foreach (var keyword in filters)
            {
                QueryData = QueryData.Where(x => x.EmployeeString.ToLower().Contains(keyword) ||
                                                 x.Remark.ToLower().Contains(keyword) ||
                                                 x.WelderNoCode.ToLower().Contains(keyword) ||
                                                 x.Description.ToLower().Contains(keyword));
            }

            // Order
            switch (Scroll.SortField)
            {
                case "EmployeeString":
                    QueryData = Scroll.SortOrder == 1 ?
                        QueryData.OrderByDescending(e => e.EmployeeString) :
                        QueryData.OrderBy(e => e.EmployeeString);
                    break;
                case "EmpCode":
                    QueryData = Scroll.SortOrder == 1 ?
                        QueryData.OrderByDescending(e => e.EmpCode) :
                        QueryData.OrderBy(e => e.EmpCode);
                    break;
                case "WelderNoCode":
                    QueryData = Scroll.SortOrder == 1 ?
                        QueryData.OrderByDescending(e => e.WelderNoCode) :
                        QueryData.OrderBy(e => e.WelderNoCode);
                    break;
                default:
                    QueryData = QueryData.OrderBy(e => e.WelderNoCode);
                    break;
            }
            // Get TotalRow
            Scroll.TotalRow = await QueryData.CountAsync();
            // Skip Take
            QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);

            return new JsonResult(new ScrollDataViewModel<WelderNoViewModel>(Scroll,
                await QueryData.ToListAsync()), this.DefaultJsonSettings);
        }

        #endregion
    }
}
