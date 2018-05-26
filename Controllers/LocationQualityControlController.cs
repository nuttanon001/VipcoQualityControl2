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
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class LocationQualityControlController : GenericController<LocationQualityControl>
    {
        readonly IRepositoryMachine<EmployeeGroupMis> repositoryGroupMis;
        readonly IRepositoryQualityControl<WorkGroupHasWorkShop> repositoryWorkShop;
        public LocationQualityControlController(IRepositoryQualityControl<LocationQualityControl> repo,
            IRepositoryMachine<EmployeeGroupMis> repoGroupMis,
            IRepositoryQualityControl<WorkGroupHasWorkShop> repoWorkShop,
            IMapper mapper) : base(repo, mapper) {
            this.repositoryGroupMis = repoGroupMis;
            this.repositoryWorkShop = repoWorkShop;
        }

        // GET: api/LocationQualityControl/5
        [HttpGet("GetKeyNumber")]
        public override async Task<IActionResult> Get(int key)
        {
            var HasData = await this.repository.GetAllAsQueryable().FirstOrDefaultAsync(x => x.LocationQualityControlId == key);
            if (HasData != null)
            {
                var MapData = this.mapper.Map<LocationQualityControl, LocationQualityControlViewModel>(HasData);
                foreach(var item in HasData.WorkGroupHasWorkShops)
                {
                    if (MapData.WorkGroupHasWorkShops == null)
                        MapData.WorkGroupHasWorkShops = new List<WorkGroupHasWorkShop>();

                    var MapItem = this.mapper.Map<WorkGroupHasWorkShop, WorkGroupHasWorkShopViewModel>(item);
                    if (!string.IsNullOrEmpty(MapItem.GroupMis))
                        MapItem.GroupMisString = (await this.repositoryGroupMis.GetAsync(MapItem.GroupMis))?.GroupDesc ?? "";
                    MapData.WorkGroupHasWorkShops.Add(MapItem);
                }
                return new JsonResult(MapData, this.DefaultJsonSettings);
            }
            else
                return BadRequest(new { Error = "Data not been found." });
        }

        // GET: api/LocationQualityControl/CheckGroupMis
        [HttpGet("CheckGroupMis")]
        public async Task<IActionResult> CheckGroupMis(string groupMis)
        {
            if (!string.IsNullOrEmpty(groupMis))
            {
                Expression<Func<WorkGroupHasWorkShop, bool>> match = e => e.GroupMis == groupMis;
                var HasValue = await this.repositoryWorkShop.AnyDataAsync(match);
                return new JsonResult(new { Has = HasValue }, this.DefaultJsonSettings);
            }
            return BadRequest();
        }

        // POST: api/LocationQualityControl/GetScroll
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
                                                 x.Description.ToLower().Contains(keyword));
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
                case "Description":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.Description);
                    else
                        QueryData = QueryData.OrderBy(e => e.Description);
                    break;
                default:
                    QueryData = QueryData.OrderBy(e => e.Name);
                    break;
            }
            // Get TotalRow
            Scroll.TotalRow = await QueryData.CountAsync();
            // Skip Take
            QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);
            // Mapper
            var HasMapper = new List<LocationQualityControlViewModel>();
            foreach (var item in await QueryData.ToListAsync())
            {
                var MapData = this.mapper.Map<LocationQualityControl, LocationQualityControlViewModel>(item);
                MapData.TotalGroup = item.WorkGroupHasWorkShops != null ? item.WorkGroupHasWorkShops.Count : 0;
                HasMapper.Add(MapData);
            }
            //JsonResult
            return new JsonResult(new ScrollDataViewModel<LocationQualityControlViewModel>(Scroll,
                HasMapper), this.DefaultJsonSettings);
        }

        // POST: api/LocationQualityControl
        [HttpPost]
        public override async Task<IActionResult> Create([FromBody] LocationQualityControl record)
        {
            var Message = "Some message.";

            try
            {
                // Set date for CrateDate Entity
                if (record == null)
                    return BadRequest();
                // +7 Hour
                record = this.helper.AddHourMethod(record);

                if (record.GetType().GetProperty("CreateDate") != null)
                    record.GetType().GetProperty("CreateDate").SetValue(record, DateTime.Now);

                if (record.WorkGroupHasWorkShops != null)
                {
                    foreach(var item in record.WorkGroupHasWorkShops)
                    {
                        if (item == null) continue;
                        item.CreateDate = record.CreateDate;
                        item.Creator = record.Creator;
                    }
                }

                if (await this.repository.AddAsync(record) == null)
                    return BadRequest();

                if (record.WorkGroupHasWorkShops != null)
                    record.WorkGroupHasWorkShops = null;

                return new JsonResult(record, this.DefaultJsonSettings);
            }
            catch (Exception ex)
            {
                Message = $"Has error message was {ex.ToString()}";
            }
            return BadRequest(new { Error = Message });
        }

        // PUT: api/LocationQualityControl/5
        [HttpPut]
        public override async Task<IActionResult> Update(int key, [FromBody] LocationQualityControl record)
        {
            if (key < 1)
                return BadRequest();
            if (record == null)
                return BadRequest();

            // +7 Hour
            record = this.helper.AddHourMethod(record);

            // Set date for CrateDate Entity
            if (record.GetType().GetProperty("ModifyDate") != null)
                record.GetType().GetProperty("ModifyDate").SetValue(record, DateTime.Now);

            // Set ItemMainHasEmployees
            if (record.WorkGroupHasWorkShops != null)
            {
                foreach (var item in record.WorkGroupHasWorkShops)
                {
                    if (item == null) continue;
                    if (item.WorkGroupHasWorkShopId > 0)
                    {
                        item.ModifyDate = record.ModifyDate;
                        item.Modifyer = record.Modifyer;
                    }
                    else
                    {
                        item.CreateDate = record.ModifyDate;
                        item.Creator = record.Modifyer;
                    }

                }
            }

            if (await this.repository.UpdateAsync(record, key) == null)
                return BadRequest();
            else
            {
                // Find requisition of item maintenance
                Expression<Func<WorkGroupHasWorkShop, bool>> condition = r => r.LocationQualityControlId == key;
                var dbChilds = await this.repositoryWorkShop.FindAllAsync(condition);

                //Remove requisition if edit remove it
                foreach (var item in dbChilds)
                {
                    if (!record.WorkGroupHasWorkShops.Any(x => x.WorkGroupHasWorkShopId == item.WorkGroupHasWorkShopId))
                        await this.repositoryWorkShop.DeleteAsync(item.WorkGroupHasWorkShopId);
                }
                //Update RequisitionStockSps or New RequisitionStockSps
                foreach (var item in record.WorkGroupHasWorkShops)
                {
                    if (item == null)
                        continue;
                    item.LocationQualityControlId = record.LocationQualityControlId;

                    if (item.WorkGroupHasWorkShopId > 0)
                        await this.repositoryWorkShop.UpdateAsync(item, item.WorkGroupHasWorkShopId);
                    else
                        await this.repositoryWorkShop.AddAsync(item);
                }
            }

            if (record.WorkGroupHasWorkShops != null)
                record.WorkGroupHasWorkShops = null;

            return new JsonResult(record, this.DefaultJsonSettings);
        }

    }
}
