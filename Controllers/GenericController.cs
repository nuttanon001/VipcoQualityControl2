﻿// 3rd party
using AutoMapper;
using Newtonsoft.Json;
// Microsoft
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// System
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
// VipcoMaintenance
using VipcoQualityControl.Helper;
using VipcoQualityControl.Services;

namespace VipcoQualityControl.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public abstract class GenericController<Entity> : Controller where Entity : class
    {
        public IRepositoryQualityControl<Entity> repository;
        public IMapper mapper;
        public HelpersClass<Entity> helper;
        public JsonSerializerSettings DefaultJsonSettings =>
            new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

        public GenericController(IRepositoryQualityControl<Entity> repo,
            IMapper mapper)
        {
            this.repository = repo;
            this.mapper = mapper;
            this.helper = new HelpersClass<Entity>();
        }

        // GET: api/controller
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ListData = await this.repository.GetAllAsync();
            return new JsonResult(ListData, this.DefaultJsonSettings);
        }

        // GET: api/controller/5
        [HttpGet("GetKeyNumber")]
        public virtual async Task<IActionResult> Get(int key)
        {
            var HasData = await this.repository.GetAsync(key);
            return new JsonResult(HasData, this.DefaultJsonSettings);
        }

        // GET: api/controller/5
        [HttpGet("GetKeyString")]
        public async Task<IActionResult> Get(string key)
        {
            return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
        }

        //// GET: api/controller/5
        //[HttpGet()]
        //public async Task<IActionResult> Get2(string key)
        //{
        //    return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
        //}

        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] Entity record)
        {
            // Set date for CrateDate Entity
            if (record == null)
                return BadRequest();
            // +7 Hour
            record = this.helper.AddHourMethod(record);

            if (record.GetType().GetProperty("CreateDate") != null)
                record.GetType().GetProperty("CreateDate").SetValue(record, DateTime.Now);
            if (await this.repository.AddAsync(record) == null)
                return BadRequest();
            return new JsonResult(record, this.DefaultJsonSettings);
        }

        [HttpPut]
        public virtual async Task<IActionResult> Update(int key, [FromBody] Entity record)
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
            if (await this.repository.UpdateAsync(record, key) == null)
                return BadRequest();
            return new JsonResult(record, this.DefaultJsonSettings);
        }

        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete(int key)
        {
            if (await this.repository.DeleteAsync(key) == 0)
                return BadRequest();
            return NoContent();
        }
    }
}
