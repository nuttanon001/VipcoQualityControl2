using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VipcoQualityControl.Models.QualityControls;

namespace VipcoQualityControl.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class SessionValuesController : Controller
    {
        [HttpGet("QualityControlResult")]
        public IActionResult GetCart()
        {
            return Ok(HttpContext.Session.GetString("QualityControlResult"));
        }

        [HttpPost("QualityControlResult")]
        public void StoreCart([FromBody] List<QualityControlResult> records)
        {
            var jsonData = JsonConvert.SerializeObject(records);
            HttpContext.Session.SetString("QualityControlResult", jsonData);
        }
    }
}
