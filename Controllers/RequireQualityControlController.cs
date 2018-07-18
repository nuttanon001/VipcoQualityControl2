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

using VipcoQualityControl.Services;
using VipcoQualityControl.ViewModels;
using VipcoQualityControl.Models.Machines;
using VipcoQualityControl.Models.QualityControls;

using AutoMapper;
using VipcoQualityControl.Helper;
using Microsoft.EntityFrameworkCore.Query;

namespace VipcoQualityControl.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class RequireQualityControlController : GenericController<RequireQualityControl>
    {
        // Repository
        private readonly IRepositoryMachine<ProjectCodeMaster> RepositoryProject;
        private readonly IRepositoryMachine<ProjectCodeDetail> RepositoryProjectDetail;
        private readonly IRepositoryMachine<Employee> RepositoryEmployee;
        private readonly IRepositoryMachine<EmployeeGroupMis> RepositoryGroupMis;
        private readonly IRepositoryMachine<AttachFile> RepositoryAttach;
        private readonly IRepositoryQualityControl<RequireHasAttach> RepositoryHasAttach;
        private readonly IRepositoryQualityControl<MasterProjectList> RepositoryMasterList;
        private readonly IRepositoryQualityControl<RequireHasMasterProject> RepositoryRequireHasMaster;
        private readonly IRepositoryQualityControl<WorkGroupHasWorkShop> RepositoryWorkShop;
        private readonly IRepositoryQualityControl<WorkGroupQualityControl> RepositoryQcGroup;
        private readonly IRepositoryQualityControl<RequireQcMoreWorkActvity> RepositoryRequireMoreActvity;
        // Helper
        private readonly Helper.EmailClass EmailClass;
        // IHost
        private readonly IHostingEnvironment HostEnvironment;
        // Private 
        private readonly Func<IQueryable<RequireQualityControl>, IIncludableQueryable<RequireQualityControl, object>> includes;
        public RequireQualityControlController(
            IRepositoryQualityControl<RequireQualityControl> repo,
            IRepositoryMachine<ProjectCodeMaster> repoProMaster,
            IRepositoryMachine<ProjectCodeDetail> repoProDetail,
            IRepositoryMachine<Employee> repoEmp,
            IRepositoryMachine<EmployeeGroupMis> repoGroupMis,
            IRepositoryMachine<AttachFile> repoAttach,
            IRepositoryQualityControl<RequireHasAttach> repoHasAttach,
            IRepositoryQualityControl<RequireHasMasterProject> repoRequireHasMaster,
            IRepositoryQualityControl<MasterProjectList> repoMasterList,
            IRepositoryQualityControl<WorkGroupHasWorkShop> repoWorkShop,
            IRepositoryQualityControl<WorkGroupQualityControl> repoQcGroup,
            IRepositoryQualityControl<RequireQcMoreWorkActvity> repoRequireMoreActvity,
            IMapper map,
            IHostingEnvironment hostEnv) : base(repo, map)
        {
            // Repository Machine
            this.RepositoryEmployee = repoEmp;
            this.RepositoryProject = repoProMaster;
            this.RepositoryProjectDetail = repoProDetail;
            this.RepositoryGroupMis = repoGroupMis;
            this.RepositoryAttach = repoAttach;
            // Repository QuailtyControl
            this.RepositoryHasAttach = repoHasAttach;
            this.RepositoryMasterList = repoMasterList;
            this.RepositoryRequireHasMaster = repoRequireHasMaster;
            this.RepositoryWorkShop = repoWorkShop;
            this.RepositoryQcGroup = repoQcGroup;
            this.RepositoryRequireMoreActvity = repoRequireMoreActvity;
            // Helper
            this.EmailClass = new Helper.EmailClass();
            // IHost
            this.HostEnvironment = hostEnv;
            // include
            this.includes = e => e.Include(z => z.Branch)
                                  .Include(z => z.InspectionPoint)
                                  .Include(z => z.WorkActivity)
                                  .Include(z => z.WorkGroupQualityControl);
        }

        #region Property
        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        private async Task<bool> SendMail(RequireQualityControl HasData,int Option = 0)
        {
            if (Option == 0)
            {
                if (string.IsNullOrEmpty(HasData.MailReply))
                    return false;

                var ListMail = new List<string>();
                if (HasData.MailReply.IndexOf(',') != -1)
                    ListMail = HasData.MailReply.Split(',').ToList();
                else if (HasData.MailReply.IndexOf(';') != -1)
                    ListMail = HasData.MailReply.Split(';').ToList();
                else
                    ListMail.Add(HasData.MailReply);

                if (ListMail.Any())
                {
                    var EmpName = (await this.RepositoryEmployee.GetAsync(HasData.RequireEmp)).NameThai ?? "ไม่ระบุ";

                    var BodyMessage = "<body style=font-size:11pt;font-family:Tahoma>" +
                                            "<h4 style='color:steelblue;'>เมล์ฉบับนี้เป็นแจ้งเตือนจากระบบงาน VIPCO Quality control SYSTEM</h4>" +
                                            $"เรียน คุณ{EmpName}" +
                                            $"<p>เรื่อง การเปิดคำขอตรวจคุณภาพงานเลขที่ {HasData.RequireQualityNo} วันเวลาที่ร้องขอเข้าตรวจสอบ {HasData.RequireDate.ToString("dd/MM/yy HH:mm")}น.</p>" +
                                            $"<p style='color:blue;'><b>ณ.ขณะนี้ได้รับการยืนยันเวลาการเข้าตรวจสอบ</b></p>" +
                                            $"<p>จากทางหน่วยงานควบคุมคุณภาพ ในวันที่ <b>{HasData.ResponseDate.Value.ToString("dd/MM/yy HH:mm")}น.</b>" +
                                            $"หากกำหนดเวลาดังกล่าวไม่สามารถเตรียมการได้ทันโปรดติดต่อ หน่วยงานควบคุมคุณภาพ</p><hr/>" +
                                            $"<p>\"คุณ{EmpName}\" " +
                                            $"สามารถเข้าไปตรวจติดตามข้อมูลได้ <a href='http://{Request.Host}/qualitycontrol/require-qc/link-email/{HasData.RequireQualityControlId}'>ที่นี้</a> </p>" +
                                            "<span style='color:steelblue;'>This mail auto generated by VIPCO Quality control SYSTEM. Do not reply this email.</span>" +
                                        "</body>";

                    await this.EmailClass.SendMailMessage(ListMail[0], EmpName, ListMail,
                                               BodyMessage, "Notification mail from VIPCO Quality Control system.");

                    return true;
                }
            }
            else if (Option == 1 && HasData.WorkGroupQualityControlId != null) //Send to workgroup of qualitycontrol
            {
                var QcGroup = await this.RepositoryQcGroup.GetAsync(HasData.WorkGroupQualityControlId.Value);
                if (QcGroup != null)
                {
                    if (!string.IsNullOrEmpty(QcGroup.Email))
                    {
                        var ListMail = new List<string>();
                        if (QcGroup.Email.IndexOf(',') != -1)
                            ListMail = QcGroup.Email.Split(',').ToList();
                        else if (QcGroup.Email.IndexOf(';') != -1)
                            ListMail = QcGroup.Email.Split(';').ToList();
                        else
                            ListMail.Add(QcGroup.Email);

                        if (ListMail.Any())
                        {
                            var BodyMessage =
                                "<body style=font-size:11pt;font-family:Tahoma>" +
                                "<h4 style='color:steelblue;'>เมล์ฉบับนี้เป็นแจ้งเตือนจากระบบงาน VIPCO Quality control SYSTEM</h4>" +
                                $"เรียน กลุ่มงาน {QcGroup.Name}" +
                                $"<p>เรื่อง ระบบตรวจพบการร้องยื่นคำร้องขอตรวจสอบคุณภาพ</p>" +
                                $"<p>เอกสารงานเลขที่ {HasData.RequireQualityNo} วันเวลาที่ร้องขอเข้าตรวจสอบ {HasData.RequireDate.ToString("dd/MM/yy HH:mm")}น.</p>" +
                                $"สามารถเข้าไปตรวจติดตามข้อมูลได้ <a href='http://{Request.Host}/qualitycontrol/require-qc/require-waiting/1'>ที่นี้</a> </p>" +
                                "<span style='color:steelblue;'>This mail auto generated by VIPCO Quality control SYSTEM. Do not reply this email.</span>" +
                                "</body>";

                            await this.EmailClass.SendMailMessage(ListMail[0], QcGroup.Name, ListMail,
                                        BodyMessage, "Notification mail from VIPCO Quality Control system.");

                            return true;
                        }
                    }
                }
            }
            else if (Option == 2)
            {
                var QcGroup = await this.RepositoryQcGroup.GetAsync(HasData.WorkGroupQualityControlId.Value);
                if (QcGroup != null)
                {
                    if (!string.IsNullOrEmpty(QcGroup.SubEmail))
                    {
                        var ListMail = new List<string>();
                        if (QcGroup.SubEmail.IndexOf(',') != -1)
                            ListMail = QcGroup.SubEmail.Split(',').ToList();
                        else if (QcGroup.SubEmail.IndexOf(';') != -1)
                            ListMail = QcGroup.SubEmail.Split(';').ToList();
                        else
                            ListMail.Add(QcGroup.SubEmail);

                        if (ListMail.Any())
                        {
                            var BodyMessage =
                                "<body style=font-size:11pt;font-family:Tahoma>" +
                                "<h4 style='color:steelblue;'>เมล์ฉบับนี้เป็นแจ้งเตือนจากระบบงาน VIPCO Quality control SYSTEM</h4>" +
                                $"เรียน หน่วยงานเชิ่อม" +
                                $"<p>เรื่อง ระบบตรวจพบการร้องยื่นคำร้องขอตรวจสอบคุณภาพ NDE</p>" +
                                $"<p>เอกสารงานเลขที่ {HasData.RequireQualityNo} วันเวลาที่ร้องขอเข้าตรวจสอบ {HasData.RequireDate.ToString("dd/MM/yy HH:mm")}น.</p>" +
                                $"<p>โปรดดำเนินการระบุข้อมูลเพิ่มเติมเพื่อดำเนินการเรียกตรวจ NDE ในขั้นตอนต่อไปและ</p>" +
                                $"สามารถเข้าไปตรวจติดตามข้อมูลได้ <a href='http://{Request.Host}/qualitycontrol/require-qc/require-waiting/2'>ที่นี้</a> </p>" +
                                "<span style='color:steelblue;'>This mail auto generated by VIPCO Quality control SYSTEM. Do not reply this email.</span>" +
                                "</body>";

                            await this.EmailClass.SendMailMessage(ListMail[0], "Welder WorkGroup", ListMail,
                                        BodyMessage, "Notification mail from VIPCO Quality Control system.");

                            return true;
                        }
                    }
                }
            }
            return false;
        }

        #endregion

        #region Action

        // GET: api/RequireQualityControl/GetKeyNumber/5
        [HttpGet("GetKeyNumber")]
        public override async Task<IActionResult> Get(int key)
        {
            var HasItem = await this.repository.GetFirstOrDefaultAsync(
                        selector:x => x,
                        predicate: z => z.RequireQualityControlId == key,
                        orderBy:null,
                        include:this.includes);

            if (HasItem != null)
            {
                var MapItem = this.mapper.Map<RequireQualityControl, RequireQualityControlViewModel>(HasItem);
                // RequireEmpString
                if (!string.IsNullOrEmpty(MapItem.RequireEmp))
                    MapItem.RequireEmpString = (await this.RepositoryEmployee.GetAsync(MapItem.RequireEmp)).NameThai;
                // ProjectCodeDetail
                if (MapItem.ProjectCodeDetailId.HasValue)
                {
                    var HasProject = await this.RepositoryProjectDetail
                                            .GetAllAsQueryable()
                                            .Include(x => x.ProjectCodeMaster)
                                            .FirstOrDefaultAsync(x => x.ProjectCodeDetailId == (MapItem.ProjectCodeDetailId ?? 0));
                    MapItem.ProjectCodeDetailString = HasProject != null ? $"{HasProject.ProjectCodeMaster.ProjectCode}/{HasProject.ProjectCodeDetailCode}" : "-";
                }
                // RequireTime
                MapItem.RequireQcTimeString = MapItem.RequireDate.ToString("HH:mm");
                // GroupMIS
                if (!string.IsNullOrEmpty(MapItem.GroupMIS))
                {
                    MapItem.GroupMISString = (await this.RepositoryGroupMis.GetAsync(MapItem.GroupMIS)).GroupDesc;

                    var workShop = (await this.RepositoryWorkShop.GetAllAsQueryable()
                                                    .Include(x => x.LocationQualityControl)
                                                    .FirstOrDefaultAsync(x => x.GroupMis == MapItem.GroupMIS));
                    if (workShop != null)
                        MapItem.GroupMISString += $"/{(workShop?.LocationQualityControl?.Name ?? "")}";
                }

                return new JsonResult(MapItem, this.DefaultJsonSettings);
            }
            return BadRequest();
        }

        // GET: api/RequireQualityControl/GenarateFromFailRequireQualityControl/5
        [HttpGet("GenarateFromFailRequireQualityControl")]
        public async Task<IActionResult> GenarateFromFailRequireQualityControl(int key)
        {
            if (key > 0)
            {
                var HasData = await this.repository.GetFirstOrDefaultAsync(
                       selector: x => x,
                       predicate: z => z.RequireQualityControlId == key,
                       orderBy: null,
                       include: this.includes);

                if (HasData != null)
                {
                    var MapItem = this.mapper.Map<RequireQualityControl, RequireQualityControlViewModel>(HasData);
                    // RequireEmpString
                    if (!string.IsNullOrEmpty(MapItem.RequireEmp))
                        MapItem.RequireEmpString = (await this.RepositoryEmployee.GetAsync(MapItem.RequireEmp)).NameThai;
                    // ProjectCodeDetail
                    if (MapItem.ProjectCodeDetailId.HasValue)
                    {
                        var HasProject = await this.RepositoryProjectDetail
                                                .GetAllAsQueryable()
                                                .Include(x => x.ProjectCodeMaster)
                                                .FirstOrDefaultAsync(x => x.ProjectCodeDetailId == (MapItem.ProjectCodeDetailId ?? 0));
                        MapItem.ProjectCodeDetailString = HasProject != null ? $"{HasProject.ProjectCodeMaster.ProjectCode}/{HasProject.ProjectCodeDetailCode}" : "-";
                    }
                    // GroupMIS
                    if (!string.IsNullOrEmpty(MapItem.GroupMIS))
                        MapItem.GroupMISString = (await this.RepositoryGroupMis.GetAsync(MapItem.GroupMIS)).GroupDesc;

                    var id = HasData.ParentRequireQcId != null ? HasData.ParentRequireQcId : HasData.RequireQualityControlId;
                    var RunNumber = (await this.repository.GetAllAsQueryable().CountAsync(x => x.RequireDate.Year == HasData.RequireDate.Year && 
                                                                                               x.ParentRequireQcId == id)) + 1;
                    var RequireQualityNo = HasData.RequireQualityNo.IndexOf("/Rev") != -1 ?
                                           HasData.RequireQualityNo.Remove(HasData.RequireQualityNo.Length - 2, 2) + RunNumber.ToString("00") :
                                           $"{HasData.RequireQualityNo}/Rev{RunNumber.ToString("00")}";

                    var GenarateData = new RequireQualityControlViewModel()
                    {
                        RequireQualityNo = RequireQualityNo,
                        BranchId = HasData.BranchId,
                        Description = HasData.Description,
                        GroupMIS = HasData.GroupMIS,
                        RequireEmp = HasData.RequireEmp,
                        InspectionPointId = HasData.InspectionPointId,
                        MailReply = HasData.MailReply,
                        ParentRequireQcId = id,
                        ProjectCodeDetailId = HasData.ProjectCodeDetailId,
                        RequireStatus = RequireStatus.Waiting,
                        WorkActivityId = HasData.WorkActivityId,
                        WorkGroupQualityControlId = HasData.WorkGroupQualityControlId,
                        //ViewModel
                        RequireEmpString = MapItem.RequireEmpString,
                        GroupMISString = MapItem.GroupMISString,
                        ProjectCodeDetailString = MapItem.ProjectCodeDetailString,
                        LocationQualityControlString = MapItem.LocationQualityControlString,
                        MasterLists = new List<MasterProjectList>()
                    };
                    var HasDataMasterList = await this.RepositoryRequireHasMaster.GetAllAsQueryable()
                                 .Include(x => x.MasterProjectList)
                                 .Where(x => x.RequireQualityControlId == MapItem.RequireQualityControlId)
                                 .ToListAsync();

                    if (HasDataMasterList != null)
                    {
                        foreach (var item in HasDataMasterList)
                        {
                            if (item.MasterProjectList != null)
                            {
                                if (item.Quantity - item.PassQuantity > 0)
                                {
                                    var MapMaster = this.mapper.Map<MasterProjectList, MasterProjectListViewModel>(item.MasterProjectList);
                                    GenarateData.MasterLists.Add(new MasterProjectList()
                                    {
                                        CreateDate = MapMaster.CreateDate,
                                        Creator = MapMaster.Creator,
                                        MarkNo = MapMaster.MarkNo,
                                        Description = MapMaster.Description,
                                        Box = MapMaster.Box,
                                        DrawingNo = MapMaster.DrawingNo,
                                        UnitNo = MapMaster.UnitNo,
                                        Name = MapMaster.Name,
                                        Quantity = item.Quantity - item.PassQuantity,
                                        MasterProjectListId = MapMaster.MasterProjectListId
                                    });
                                }
                            }
                        }
                    }

                    return new JsonResult(GenarateData, this.DefaultJsonSettings);
                }
            }
            return BadRequest(new { Error = "Key not been found." });
        }

        // GET: api/RequireQualityControl/ActionRequireMaintenance/5
        [HttpGet("ActionRequireQualityControl")]
        public async Task<IActionResult> ActionRequireQualityControl(int key, string byEmp)
        {
            if (key > 0)
            {
                var HasData = await this.repository.GetFirstOrDefaultAsync(x => x,x => x.RequireQualityControlId == key);
                if (HasData != null)
                {
                    // Comfirm require date
                    HasData.ResponseDate = HasData.RequireDate.AddHours(1);
                    HasData.ModifyDate = DateTime.Now;
                    HasData.Modifyer = byEmp;
                    // Send mail
                    var Complate = await this.repository.UpdateAsync(HasData, key);

                    if (Complate != null)
                    {
                        await this.SendMail(Complate);
                        return new JsonResult(Complate, this.DefaultJsonSettings);
                    }
                }
            }
            return BadRequest();
        }

        // POST: api/RequireQualityControl/RequireQualityControlChange
        [HttpPost("RequireQualityControlChange")]
        public async Task<IActionResult> RequireQualityControlChange([FromBody] RequireQualityControlChangeViewModel Option)
        {
            if (Option != null && Option.ChangeDate != null)
            {
                var HasData = await this.repository.GetFirstOrDefaultAsync
                    (x => x,x => x.RequireQualityControlId == Option.RequireQualityControlId.Value);

                // Add Hour
                Option.ChangeDate = Option.ChangeDate.Value.AddHours(7);

                if (HasData != null)
                {
                    HasData.ResponseDate = new DateTime(Option.ChangeDate.Value.Year, Option.ChangeDate.Value.Month, Option.ChangeDate.Value.Day,
                                                        Option.ChangeTime.Value.Hour, Option.ChangeTime.Value.Minute, 0);
                    HasData.ModifyDate = DateTime.Now;
                    HasData.Modifyer = Option.UserName;
                    // Send Mail
                    var Complate = await this.repository.UpdateAsync(HasData, HasData.RequireQualityControlId);
                    if (Complate != null)
                    {
                        await this.SendMail(Complate);
                        return new JsonResult(Complate, this.DefaultJsonSettings);
                    }
                }
            }
            return BadRequest();
        }

        // POST: api/RequireQualityControl/RequireQualityControlWaiting
        [HttpPost("RequireQualityControlWaiting")]
        public async Task<IActionResult> RequireQualityControlWaiting([FromBody] OptionRequireQualityControl Option)
        {
            string Message = "";
            try
            {
                if (Option == null)
                    return BadRequest(new { Error = "Date not beed found." });
                var TotalRow = 0;
                var filters = string.IsNullOrEmpty(Option.Filter) ? new string[] { "" }
                                : Option.Filter.Split(null);

                var predicate = PredicateBuilder.False<RequireQualityControl>();
                foreach (string keyword in filters)
                {
                    string temp = keyword;
                    predicate = predicate.Or(p => p.Description.Contains(temp) ||
                                                  p.RequireQualityNo.Contains(temp) ||
                                                  p.InspectionPoint.Name.Contains(temp) ||
                                                  p.WorkActivity.Name.Contains(temp) ||
                                                  p.WorkGroupQualityControl.Name.Contains(temp));
                }
                // Option ProjectCodeMaster
                if (Option.ProjectId.HasValue)
                    predicate = predicate.And(p => p.ProjectCodeDetailId == Option.ProjectId);

                if (Option.Status.HasValue)
                {
                    if (Option.Status == 1)
                        predicate = predicate.And(x => x.RequireStatus == RequireStatus.Waiting || x.RequireStatus == RequireStatus.WeldingReq);
                    else if (Option.Status == 2)
                        predicate = predicate.And(x => x.RequireStatus == RequireStatus.Welding);
                    else
                        predicate = predicate.And(x => x.RequireStatus != RequireStatus.Cancel);
                }
                else
                    predicate = predicate.And(x => x.RequireStatus == RequireStatus.Waiting);

                TotalRow = await this.repository.GetLengthWithAsync(predicate);

                var GetData = await this.repository.GetToListAsync(
                    selector: x => x,
                    predicate: predicate,
                    include: x =>
                     x.Include(z => z.WorkGroupQualityControl)
                     .Include(z => z.InspectionPoint)
                     .Include(z => z.WorkActivity)
                     .Include(z => z.WorkGroupQualityControl),
                    skip: Option.Skip,
                    take: Option.Take);

                if (GetData.Any())
                {
                    List<string> ColumnUpper = new List<string>();
                    IDictionary<DateTime, string> ColumnLower = new Dictionary<DateTime, string>();

                    var MinDate = GetData.Min(x => x.RequireDate);
                    var MaxDate = GetData.Max(x => x.RequireDate);
                    var countCol = 1;
                    if (MinDate == null && MaxDate == null)
                        return NotFound(new { Error = "Data not found" });

                    foreach (DateTime day in EachDay(MinDate, MaxDate))
                    {
                        if (GetData.Any(x => x.RequireDate.Date == day.Date))
                        {
                            ColumnUpper.Add(day.Date.ToString("dd/MM/yy"));
                            ColumnLower.Add(new DateTime(day.Year, day.Month, day.Day, 8, 0, 0), $"Col{countCol.ToString("00")}");
                            countCol++;
                            ColumnLower.Add(new DateTime(day.Year, day.Month, day.Day, 13, 0, 0), $"Col{countCol.ToString("00")}");
                            countCol++;
                        }
                    }

                    var DataTable = new List<IDictionary<string, object>>();

                    foreach (var Data in GetData.OrderBy(x => x.WorkGroupQualityControl.Name).ThenBy(x => x.RequireDate).ThenBy(x => x.ResponseDate))
                    {
                        var WorkGroupQcName = Option.Status == 2 ? "Welding WorkGroup" : $"{Data.WorkGroupQualityControl.Name ?? "No-Data"}";
                        IDictionary<string, object> rowData;
                        bool update = false;
                        if (DataTable.Any(x => (string)x["WorkGroupQcName"] == WorkGroupQcName))
                        {
                            var FirstData = DataTable.FirstOrDefault(x => (string)x["WorkGroupQcName"] == WorkGroupQcName);
                            if (FirstData != null)
                            {
                                rowData = FirstData;
                                update = true;
                            }
                            else
                                rowData = new ExpandoObject();
                        }
                        else
                            rowData = new ExpandoObject();
                       
                        // Get ProjectNo
                        var ProjectName = "-";
                        if (Data.ProjectCodeDetailId != null)
                        {
                            var ProjectDetail = await this.RepositoryProjectDetail.GetFirstOrDefaultAsync(
                                selector:x => x,
                                predicate:x => x.ProjectCodeDetailId == Data.ProjectCodeDetailId.Value,
                                include:x => x.Include(z => z.ProjectCodeMaster));

                            ProjectName = $"{ProjectDetail.ProjectCodeMaster.ProjectCode} / {ProjectDetail.ProjectCodeDetailCode}";
                        }

                        var Key = "";
                        if (ColumnLower.Any(x => x.Key.Date == Data.RequireDate.Date))
                        {
                            if (Data.RequireDate.Hour > 12)
                                Key = ColumnLower.FirstOrDefault(x => x.Key.Date == Data.RequireDate.Date && x.Key.Hour > 12).Value;
                            else
                                Key = ColumnLower.FirstOrDefault(x => x.Key.Date == Data.RequireDate.Date && x.Key.Hour <= 12).Value;
                        }
                        // New Data
                        var Master = new RequireQualityControlViewModel()
                        {
                            RequireQualityControlId = Data.RequireQualityControlId,
                            MailReply = Data.MailReply ?? null,
                            ResponseDate = Data.ResponseDate,
                            // RequireString = $"{EmployeeReq} | No.{Data.RequireNo}",
                            ProjectCodeDetailString = ProjectName,
                            InspectionPointString = Data?.InspectionPoint?.Name ?? "",
                            WorkActivityString = Data?.WorkActivity?.Name ?? "",
                            WorkGroupQualityControlString = Data?.WorkGroupQualityControl?.Name ?? "",
                            RequireEmpString = string.IsNullOrEmpty(Data.RequireEmp) ? "-" : "คุณ" + (await this.RepositoryEmployee.GetAsync(Data.RequireEmp)).NameThai
                                                
                        };

                        Master.RequireEmpString += Data.RequireDate != null ? $" {Data.RequireDate.ToString("HH:mm")}น." : "";

                        if (rowData.Any(x => x.Key == Key))
                        {
                            // New Value
                            var ListMaster = (List<RequireQualityControlViewModel>)rowData[Key];
                            ListMaster.Add(Master);
                            // add to row data
                            rowData[Key] = ListMaster;
                        }
                        else // add new
                            rowData.Add(Key, new List<RequireQualityControlViewModel>() { Master });

                        if (!update)
                        {
                            rowData.Add("WorkGroupQcName", WorkGroupQcName);
                            DataTable.Add(rowData);
                        }
                    }

                    if (!DataTable.Any())
                        return BadRequest(new { Error = "Data not been found." });

                    return new JsonResult(new
                    {
                        TotalRow,
                        ColumnUpper,
                        ColumnLower = ColumnLower.OrderBy(x => x.Key.Date)
                                                 .Select(x => x.Key.Hour < 12 ? $"AM" : $"PM"),
                        ColumnAll = ColumnLower.OrderBy(x => x.Key.Date).Select(x => x.Value),
                        DataTable
                    }, this.DefaultJsonSettings);
                }
            }
            catch (Exception ex)
            {
                Message = $"Has error {ex.ToString()}";
            }

            return BadRequest(new { Error = Message });
        }

        [HttpPost("RequireQualityControlSchedule")]
        public async Task<IActionResult> RequireQualityControlSchedule([FromBody] RequireQualityControlSchedule Schedule)
        {
            var Message = "Data has been found.";
            try
            {
                var QueryData = this.repository.GetAllAsQueryable()
                                    .Include(x => x.QualityControlResult)
                                    .Include(x => x.WorkGroupQualityControl)
                                    .Include(x => x.InspectionPoint)
                                    .Where(x => x.RequireStatus != RequireStatus.Cancel)
                                    .OrderByDescending(x => x.RequireDate).ThenByDescending(x => x.ResponseDate)
                                    .AsNoTracking()
                                    .AsQueryable();
                int TotalRow;

                if (Schedule != null)
                {
                    // Option Filter
                    if (!string.IsNullOrEmpty(Schedule.Filter))
                    {
                        var filters = string.IsNullOrEmpty(Schedule.Filter) ? new string[] { "" }
                                   : Schedule.Filter.ToLower().Split(null);
                        foreach (var keyword in filters)
                        {
                            QueryData = QueryData.Where(x => x.Description.ToLower().Contains(keyword) ||
                                                             x.Remark.ToLower().Contains(keyword) ||
                                                             x.RequireQualityNo.ToLower().Contains(keyword) ||
                                                             x.WorkGroupQualityControl.Name.ToLower().Contains(keyword) ||
                                                             x.InspectionPoint.Name.ToLower().Contains(keyword) ||
                                                             x.WorkGroupQualityControl.Name.ToLower().Contains(keyword) ||
                                                             x.RequireHasMasterProjects.Any(z => z.MasterProjectList.Name.ToLower().Contains(keyword)));
                        }
                    }
                    // Option Mode
                    if (Schedule.Mode.HasValue)
                    {
                        if (Schedule.Mode == 1)
                            QueryData = QueryData.OrderByDescending(x => x.RequireDate);
                    }
                    // Option ProjectMasterId
                    if (Schedule.ProjectMasterId.HasValue)
                        QueryData = QueryData.Where(x => x.ProjectCodeDetailId == Schedule.ProjectMasterId);
                    // Option Create
                    if (!string.IsNullOrEmpty(Schedule.Creator))
                        QueryData = QueryData.Where(x => x.RequireEmp == Schedule.Creator);
                    // Option RequireMaintenance
                    if (Schedule.RequireQuailtyControlId.HasValue)
                        QueryData = QueryData.Where(x => x.RequireQualityControlId == Schedule.RequireQuailtyControlId);
                    // Option WorkGroupMaintenance
                    if (Schedule.WorkGroupQuailtyControlId.HasValue)
                        QueryData = QueryData.Where(x => x.WorkGroupQualityControlId == Schedule.WorkGroupQuailtyControlId);

                    TotalRow = await QueryData.CountAsync();
                    // Option Skip and Task
                    // if (Scehdule.Skip.HasValue && Scehdule.Take.HasValue)
                    QueryData = QueryData.Skip(Schedule.Skip ?? 0).Take(Schedule.Take ?? 20);
                }
                else
                    TotalRow = await QueryData.CountAsync();

                var GetData = await QueryData.ToListAsync();
                if (GetData.Any())
                {
                    IDictionary<string, int> ColumnGroupTop = new Dictionary<string, int>();
                    IDictionary<DateTime, string> ColumnGroupLow = new Dictionary<DateTime, string>();
                    List<string> ColumnsAll = new List<string>();
                    // PlanDate
                    List<DateTime?> ListDate = new List<DateTime?>()
                    {
                        //START Date
                        GetData.Min(x => x.RequireDate),
                        GetData.Min(x => x?.ResponseDate) ?? null,
                        GetData.Min(x => x?.QualityControlResult?.QualityControlResultDate) ?? null,
                        //END Date
                        GetData.Max(x => x.RequireDate),
                        GetData.Max(x => x?.ResponseDate) ?? null,
                        GetData.Max(x => x?.QualityControlResult?.QualityControlResultDate) ?? null,
                    };

                    DateTime? MinDate = ListDate.Min();
                    DateTime? MaxDate = ListDate.Max();

                    if (MinDate == null && MaxDate == null)
                        return NotFound(new { Error = "Data not found" });

                    int countCol = 1;
                    // add Date to max
                    MaxDate = MaxDate.Value.AddDays(1);
                    MinDate = MinDate.Value.AddDays(-1);

                    // If Range of date below then 15 day add more
                    var RangeDay = (MaxDate.Value - MinDate.Value).Days;
                    if (RangeDay < 5)
                    {
                        MaxDate = MaxDate.Value.AddDays((5 - RangeDay) / 2);
                        MinDate = MinDate.Value.AddDays((((5 - RangeDay) / 2) * -1));
                    }

                    // EachDay
                    var EachDate = new Helper.LoopEachDate();
                    // Foreach Date
                    foreach (DateTime day in EachDate.EachDate(MinDate.Value, MaxDate.Value))
                    {
                        // Get Month
                        if (ColumnGroupTop.Any(x => x.Key == day.ToString("MMMM")))
                            ColumnGroupTop[day.ToString("MMMM")] += 2;
                        else
                            ColumnGroupTop.Add(day.ToString("MMMM"), 2);

                        ColumnGroupLow.Add(new DateTime(day.Year, day.Month, day.Day, 8, 0, 0), $"Col{countCol.ToString("00")}");
                        countCol++;
                        ColumnGroupLow.Add(new DateTime(day.Year, day.Month, day.Day, 13, 0, 0), $"Col{countCol.ToString("00")}");
                        countCol++;
                    }

                    var DataTable = new List<IDictionary<String, Object>>();
                    // OrderBy(x => x.Machine.TypeMachineId).ThenBy(x => x.Machine.MachineCode)
                    foreach (var Data in GetData.OrderByDescending(x => x.RequireDate).ThenBy(x => x.CreateDate))
                    {
                        IDictionary<String, Object> rowData = new ExpandoObject();
                        var Progress = Data?.RequireStatus != null ? System.Enum.GetName(typeof(RequireStatus), Data.RequireStatus) : "NoAction";
                        var ProjectMaster = "NoData";
                        if (Data?.ProjectCodeDetailId != null)
                        {
                            var ProjectData = await this.RepositoryProjectDetail.GetFirstOrDefaultAsync(
                                x => x,x => x.ProjectCodeDetailId == Data.ProjectCodeDetailId,null,x => x.Include(z => z.ProjectCodeMaster));
                            ProjectMaster = ProjectData != null ? ($"{ProjectData.ProjectCodeMaster.ProjectCode}/{ProjectData.ProjectCodeDetailCode}") : "-";
                            if (ProjectMaster.Length > 25)
                                ProjectMaster = ProjectMaster.Substring(0, 25) + "...";
                        }

                        // add column time
                        rowData.Add("ProjectMaster", ProjectMaster);
                        rowData.Add("WorkGroupQualityControl", Data?.WorkGroupQualityControl?.Name ?? "Not-Assign");
                        rowData.Add("InspectionPoint", $"{Data?.RequireQualityNo}/{Data?.InspectionPoint?.Name ?? "Not Found"}"  );
                        rowData.Add("Progress", Progress);
                        rowData.Add("QualityControlStatus", Data?.RequireStatus != null ? Data.RequireStatus : RequireStatus.Cancel);
                        rowData.Add("QualityControlResultId", Data?.QualityControlResult?.QualityControlResultId != null ? Data?.QualityControlResult.QualityControlResultId : 0);
                        rowData.Add("RequireQualityControlId", Data.RequireQualityControlId);

                        // Add new
                        if (Data.ResponseDate.HasValue)
                        {
                            if (ColumnGroupLow.Any(x => x.Key.Date == Data.ResponseDate.Value.Date))
                            {
                                DateTime HasDate = Data.ResponseDate.Value.Hour > 12 ? new DateTime(Data.ResponseDate.Value.Year, Data.ResponseDate.Value.Month, Data.ResponseDate.Value.Day, 13, 0, 0) :
                                        new DateTime(Data.ResponseDate.Value.Year, Data.ResponseDate.Value.Month, Data.ResponseDate.Value.Day, 8, 0, 0);

                                rowData.Add("Response", ColumnGroupLow.FirstOrDefault(x => x.Key == HasDate).Value);
                            }
                        }
                        // End new

                        // Data is 1:Plan,2:Actual,3:PlanAndActual
                        if (Data != null)
                        {
                            // For Plan1
                            if (Data.RequireDate != null)
                            {
                                if (ColumnGroupLow.Any(x => x.Key.Date == Data.RequireDate.Date))
                                {
                                    DateTime HasDate = Data.RequireDate.Hour > 12 ? new DateTime(Data.RequireDate.Year, Data.RequireDate.Month, Data.RequireDate.Day, 13, 0, 0) : 
                                        new DateTime(Data.RequireDate.Year, Data.RequireDate.Month, Data.RequireDate.Day, 8, 0, 0);
                                    // Add row data
                                    rowData.Add(ColumnGroupLow.FirstOrDefault(x => x.Key == HasDate).Value, 1);
                                }
                            }

                            // For Actual
                            if (Data?.QualityControlResult?.QualityControlResultDate != null)
                            {
                                if (ColumnGroupLow.Any(x => x.Key.Date == Data?.QualityControlResult?.QualityControlResultDate.Value.Date))
                                {
                                    DateTime HasDate = Data.QualityControlResult?.QualityControlResultDate.Value.Hour > 12 ? 
                                        new DateTime(Data.QualityControlResult.QualityControlResultDate.Value.Year, Data.QualityControlResult.QualityControlResultDate.Value.Month, Data.QualityControlResult.QualityControlResultDate.Value.Day, 13, 0, 0) :
                                        new DateTime(Data.QualityControlResult.QualityControlResultDate.Value.Year, Data.QualityControlResult.QualityControlResultDate.Value.Month, Data.QualityControlResult.QualityControlResultDate.Value.Day, 8, 0, 0);

                                    var Col = ColumnGroupLow.FirstOrDefault(x => x.Key == HasDate);
                                    // if Have Plan change value to 3
                                    if (rowData.Keys.Any(x => x == Col.Value))
                                        rowData[Col.Value] = 3;
                                    else // else Don't have plan value is 2
                                        rowData.Add(Col.Value, 2);
                                }
                            }
                        }
                        DataTable.Add(rowData);
                    }

                    if (DataTable.Any())
                        ColumnGroupLow.OrderByDescending(x => x.Key.Date).Select(x => x.Value)
                            .ToList().ForEach(item => ColumnsAll.Add(item));

                    return new JsonResult(new
                    {
                        TotalRow,
                        ColumnsTop = ColumnGroupTop.Select(x => new
                        {
                            Name = x.Key,
                            x.Value
                        }),
                        ColumnsLow = ColumnGroupLow.GroupBy(x => x.Key.Date).OrderByDescending(x => x.Key.Date)
                                        .Select(x => x.Key.Day),
                        ColumnsLower = ColumnGroupLow.OrderByDescending(x => x.Key.Date)
                                        .Select(x => x.Key.Hour < 12 ? $"AM" : $"PM"),
                        ColumnsAll,
                        DataTable
                    }, this.DefaultJsonSettings);
                }
            }
            catch(Exception ex)
            {
                Message = $"Has error {ex.ToString()}";
            }
            return BadRequest(new { Error = Message });
        }

        [HttpGet("CancelRequireQualityControl")]
        public async Task<IActionResult> CancelRequireQualityControl(int key)
        {
            if (key > 0)
            {
                var hasData = await this.repository.FindAsync(x => x.RequireQualityControlId == key);
                if (hasData != null)
                {
                    hasData.RequireStatus = RequireStatus.Cancel;
                    await this.repository.UpdateAsync(hasData, hasData.RequireQualityControlId);
                    return new JsonResult(hasData, this.DefaultJsonSettings);
                }
            }

            return BadRequest(new { error = "Data not been found." });
        }
        #endregion

        #region GetScroll

        // POST: api/RequireQualityControl/GetScroll
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            if (Scroll == null)
                return BadRequest();
            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.Split(null);

            var predicate = PredicateBuilder.False<RequireQualityControl>();
            foreach (string keyword in filters)
            {
                string temp = keyword;
                predicate = predicate.Or(p => p.Description.Contains(temp) || 
                                              p.RequireQualityNo.Contains(temp) ||
                                              p.InspectionPoint.Name.Contains(temp) ||
                                              p.WorkActivity.Name.Contains(temp) ||
                                              p.WorkGroupQualityControl.Name.Contains(temp));
            }
            if (!string.IsNullOrEmpty(Scroll.Where))
                predicate = predicate.And(p => p.Creator == Scroll.Where);
            // Where
            //Order by
            Func<IQueryable<RequireQualityControl>, IOrderedQueryable<RequireQualityControl>> order;
            // Order
            switch (Scroll.SortField)
            {
                case "RequireQualityNo":
                    if (Scroll.SortOrder == -1)
                        order = o => o.OrderByDescending(x => x.RequireQualityNo);
                    else
                        order = o => o.OrderBy(x => x.RequireQualityNo);
                    break;
                case "InspectionPointString":
                    if (Scroll.SortOrder == -1)
                        order = o => o.OrderByDescending(x => x.InspectionPoint.Name);
                    else
                        order = o => o.OrderBy(x => x.InspectionPoint.Name);
                    break;
                case "RequireDate":
                    if (Scroll.SortOrder == -1)
                        order = o => o.OrderByDescending(x => x.RequireDate);
                    else
                        order = o => o.OrderBy(x => x.RequireDate);
                    break;
                default:
                    order = o => o.OrderByDescending(x => x.RequireDate);
                    break;
            }

            var QueryData = await  this.repository.GetToListAsync(
                                    selector: selected => selected,  // Selected
                                    predicate: predicate, // Where
                                    orderBy: order, // Order
                                    include: this.includes, // Include
                                    skip: Scroll.Skip ?? 0, // Skip
                                    take: Scroll.Take ?? 10); // Take

            // Get TotalRow
            Scroll.TotalRow = await this.repository.GetLengthWithAsync(predicate: predicate);

            var mapDatas = new List<RequireQualityControlViewModel>();
            foreach (var item in QueryData)
            {
                var MapItem = this.mapper.Map<RequireQualityControl, RequireQualityControlViewModel>(item);
                mapDatas.Add(MapItem);
            }

            return new JsonResult(new ScrollDataViewModel<RequireQualityControlViewModel>(Scroll, mapDatas), this.DefaultJsonSettings);
        }

        // POST: api/RequireQualityControl/GetScrollOnFail
        [HttpPost("GetScrollOnFail")]
        public async Task<IActionResult> GetScrollOnFail([FromBody] ScrollViewModel Scroll)
        {

            if (Scroll == null)
                return BadRequest();
            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.Split(null);

            var predicate = PredicateBuilder.False<RequireQualityControl>();
            foreach (string keyword in filters)
            {
                string temp = keyword;
                predicate = predicate.Or(p => p.Description.Contains(temp) ||
                                              p.RequireQualityNo.Contains(temp) ||
                                              p.InspectionPoint.Name.Contains(temp) ||
                                              p.WorkActivity.Name.Contains(temp) ||
                                              p.WorkGroupQualityControl.Name.Contains(temp));
            }
            if (!string.IsNullOrEmpty(Scroll.Where))
                predicate = predicate.And(p => p.Creator == Scroll.Where);
            // Where
            predicate = predicate.And(p => p.RequireStatus == RequireStatus.QcFail);
            //Order by
            Func<IQueryable<RequireQualityControl>, IOrderedQueryable<RequireQualityControl>> order;
            // Order
            switch (Scroll.SortField)
            {
                case "RequireQualityNo":
                    if (Scroll.SortOrder == -1)
                        order = o => o.OrderByDescending(x => x.RequireQualityNo);
                    else
                        order = o => o.OrderBy(x => x.RequireQualityNo);
                    break;
                case "InspectionPointString":
                    if (Scroll.SortOrder == -1)
                        order = o => o.OrderByDescending(x => x.InspectionPoint.Name);
                    else
                        order = o => o.OrderBy(x => x.InspectionPoint.Name);
                    break;
                case "RequireDate":
                    if (Scroll.SortOrder == -1)
                        order = o => o.OrderByDescending(x => x.RequireDate);
                    else
                        order = o => o.OrderBy(x => x.RequireDate);
                    break;
                default:
                    order = o => o.OrderByDescending(x => x.RequireDate);
                    break;
            }

            var QueryData = await this.repository.GetToListAsync(
                                    selector: selected => selected,  // Selected
                                    predicate: predicate, // Where
                                    orderBy: order, // Order
                                    include: this.includes, // Include
                                    skip: Scroll.Skip ?? 0, // Skip
                                    take: Scroll.Take ?? 10); // Take

            // Get TotalRow
            Scroll.TotalRow = await this.repository.GetLengthWithAsync(predicate: predicate);

            var mapDatas = new List<RequireQualityControlViewModel>();
            foreach (var item in QueryData)
            {
                var MapItem = this.mapper.Map<RequireQualityControl, RequireQualityControlViewModel>(item);
                mapDatas.Add(MapItem);
            }

            return new JsonResult(new ScrollDataViewModel<RequireQualityControlViewModel>(Scroll, mapDatas), this.DefaultJsonSettings);
        }

        // POST: api/RequireQualityControl/GetScroll
        [HttpPost("GetScrollOnWelder")]
        public async Task<IActionResult> GetScrollOnWelder([FromBody] ScrollViewModel Scroll)
        {
            if (Scroll == null)
                return BadRequest();
            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.Split(null);

            var predicate = PredicateBuilder.False<RequireQualityControl>();
            foreach (string keyword in filters)
            {
                string temp = keyword;
                predicate = predicate.Or(p => p.Description.Contains(temp) ||
                                              p.RequireQualityNo.Contains(temp) ||
                                              p.InspectionPoint.Name.Contains(temp) ||
                                              p.WorkActivity.Name.Contains(temp) ||
                                              p.WorkGroupQualityControl.Name.Contains(temp));
            }
            if (!string.IsNullOrEmpty(Scroll.Where))
                predicate = predicate.And(p => p.Creator == Scroll.Where);
            // Where
            predicate = predicate.And(p => p.RequireStatus == RequireStatus.WeldingReq);
            //Order by
            Func<IQueryable<RequireQualityControl>, IOrderedQueryable<RequireQualityControl>> order;
            // Order
            switch (Scroll.SortField)
            {
                case "RequireQualityNo":
                    if (Scroll.SortOrder == -1)
                        order = o => o.OrderByDescending(x => x.RequireQualityNo);
                    else
                        order = o => o.OrderBy(x => x.RequireQualityNo);
                    break;
                case "InspectionPointString":
                    if (Scroll.SortOrder == -1)
                        order = o => o.OrderByDescending(x => x.InspectionPoint.Name);
                    else
                        order = o => o.OrderBy(x => x.InspectionPoint.Name);
                    break;
                case "RequireDate":
                    if (Scroll.SortOrder == -1)
                        order = o => o.OrderByDescending(x => x.RequireDate);
                    else
                        order = o => o.OrderBy(x => x.RequireDate);
                    break;
                default:
                    order = o => o.OrderByDescending(x => x.RequireDate);
                    break;
            }

            var QueryData = await this.repository.GetToListAsync(
                                    selector: selected => selected,  // Selected
                                    predicate: predicate, // Where
                                    orderBy: order, // Order
                                    include: this.includes, // Include
                                    skip: Scroll.Skip ?? 0, // Skip
                                    take: Scroll.Take ?? 10); // Take

            // Get TotalRow
            Scroll.TotalRow = await this.repository.GetLengthWithAsync(predicate: predicate);

            var mapDatas = new List<RequireQualityControlViewModel>();
            foreach (var item in QueryData)
            {
                var MapItem = this.mapper.Map<RequireQualityControl, RequireQualityControlViewModel>(item);
                mapDatas.Add(MapItem);
            }

            return new JsonResult(new ScrollDataViewModel<RequireQualityControlViewModel>(Scroll, mapDatas), this.DefaultJsonSettings);
        }
        #endregion

        #region CreateAndUpdateModel

        // POST: api/RequireQualityControl/CreateV2/
        [HttpPost("CreateV2")]
        public async Task<IActionResult> CreateV2([FromBody] RequireQualityControlViewModel recordViewModel)
        {
            // Set date for CrateDate Entity
            if (recordViewModel == null)
                return BadRequest();
            var record = this.mapper.Map<RequireQualityControlViewModel, RequireQualityControl>(recordViewModel);
            // +7 Hour
            record = this.helper.AddHourMethod(record);
            if (recordViewModel.RequireQcTime.HasValue)
            {
                record.RequireDate = new DateTime(record.RequireDate.Year, record.RequireDate.Month, record.RequireDate.Day,
                                                  recordViewModel.RequireQcTime.Value.Hour, recordViewModel.RequireQcTime.Value.Minute, 0);

                if (record.RequireDate <= DateTime.Now.AddHours(1))
                    record.RequireDate = DateTime.Now.AddHours(1);
            }

            if (string.IsNullOrEmpty(record.RequireQualityNo))
            {
                var RunNumber = (await this.repository.GetAllAsQueryable().CountAsync(x => x.RequireDate.Year == record.RequireDate.Year && x.ParentRequireQcId == null)) + 1;
                record.RequireQualityNo = $"{record.RequireDate.ToString("yy")}-{RunNumber.ToString("0000")}";
            }
            record.CreateDate = DateTime.Now;

            // Set ItemMainHasEmployees
            if (recordViewModel.MasterLists != null)
            {
                foreach (var item in recordViewModel.MasterLists.ToList())
                {
                    if (item == null)
                        continue;
                    if (item.MasterProjectListId > 0)
                    {
                        record.RequireHasMasterProjects.Add(new RequireHasMasterProject
                        {
                            CreateDate = record.CreateDate,
                            Creator = record.Creator,
                            Quantity = item.Quantity,
                            MasterProjectListId = item.MasterProjectListId,
                        });
                    }
                    else
                    {
                        item.CreateDate = record.CreateDate;
                        item.Creator = record.Creator;

                        record.RequireHasMasterProjects.Add(new RequireHasMasterProject
                        {
                            CreateDate = record.CreateDate,
                            Creator = record.Creator,
                            Quantity = item.Quantity,
                            MasterProjectList = item
                        });
                    }

                }
            }

            if (recordViewModel.MoreWorkActvities != null)
            {
                if (record.RequireQualityControlId < 1 && record.RequireStatus == RequireStatus.Waiting)
                {
                    if (!recordViewModel.MoreWorkActvities.Any(x => x.TypeWorkActivity == TypeWorkActivity.QC_QA))
                        record.RequireStatus = RequireStatus.Welding;
                }

                foreach(var item in recordViewModel.MoreWorkActvities)
                {
                    if (item == null)
                        continue;

                    record.RequireQcMoreWorkActvities.Add(new RequireQcMoreWorkActvity
                    {
                        CreateDate = record.CreateDate,
                        Creator = record.Creator,
                        WorkActivityId = item.WorkActivityId,
                    });
                }
            }

            if (await this.repository.AddAsync(record) == null)
                return BadRequest();
            if (record.ParentRequireQcId.HasValue)
            {
                var ParentRequireQc = await this.repository.GetAsync(record.ParentRequireQcId.Value);
                if (ParentRequireQc != null)
                {
                    if (ParentRequireQc.RequireStatus == RequireStatus.QcFail)
                    {
                        ParentRequireQc.RequireStatus = RequireStatus.Revise;
                        ParentRequireQc.ModifyDate = DateTime.Now;
                        ParentRequireQc.Modifyer = record.Creator;
                        //Update Parent
                        await this.repository.UpdateAsync(ParentRequireQc, ParentRequireQc.RequireQualityControlId);
                    }
                }
                // AllChlid
                var ChlidRequireQc = await this.repository.GetToListAsync
                                        (selector:x => x,
                                         predicate: x => x.ParentRequireQcId == record.ParentRequireQcId.Value &&
                                                         x.RequireQualityControlId != record.RequireQualityControlId);
                if (ChlidRequireQc != null)
                {
                    foreach(var item in ChlidRequireQc)
                    {
                        if (item.RequireStatus == RequireStatus.QcFail)
                        {
                            item.RequireStatus = RequireStatus.Revise;
                            item.ModifyDate = DateTime.Now;
                            item.Modifyer = record.Creator;
                            //Update Parent
                            await this.repository.UpdateAsync(item, item.RequireQualityControlId);
                        }
                    }
                }
            }

            //Send mail to groupqc
            if (record.RequireStatus != RequireStatus.Welding)
                await this.SendMail(record, 1);
            else
                await this.SendMail(record, 2);

            return new JsonResult(record, this.DefaultJsonSettings);
        }

        // PUT: api/RequireQualityControl/UpdateV2/
        [HttpPut("UpdateV2")]
        public async Task<IActionResult> UpdateV2(int key, [FromBody] RequireQualityControlViewModel recordViewModel)
        {
            if (key < 1)
                return BadRequest();
            if (recordViewModel == null)
                return BadRequest();

            var record = this.mapper.Map<RequireQualityControlViewModel, RequireQualityControl>(recordViewModel);
            // +7 Hour
            record = this.helper.AddHourMethod(record);
            if (recordViewModel.RequireQcTime.HasValue)
            {
                record.RequireDate = new DateTime(record.RequireDate.Year, record.RequireDate.Month, record.RequireDate.Day,
                                                  recordViewModel.RequireQcTime.Value.Hour, recordViewModel.RequireQcTime.Value.Minute, 0);

                if (record.RequireStatus == RequireStatus.Waiting)
                {
                    if (record.RequireDate <= DateTime.Now.AddHours(1))
                        record.RequireDate = DateTime.Now.AddHours(1);
                }
            }

            // Check Status Welding
            if (record.RequireStatus == RequireStatus.Waiting)
            {
                if (!recordViewModel.MoreWorkActvities.Any(x => x.TypeWorkActivity == TypeWorkActivity.QC_QA))
                    record.RequireStatus = RequireStatus.Welding;
            }

            // Set date for CrateDate Entity
            if (record.GetType().GetProperty("ModifyDate") != null)
                record.GetType().GetProperty("ModifyDate").SetValue(record, DateTime.Now);
            if (await this.repository.UpdateAsync(record, key) == null)
                return BadRequest();
            else
            {
                // Find requisition of item maintenance
                Expression<Func<RequireHasMasterProject, bool>> condition = r => r.RequireQualityControlId == key;
                var dbRequireHasMaster = await this.RepositoryRequireHasMaster.FindAllAsync(condition);

                //Remove requisition if edit remove it
                foreach (var item in dbRequireHasMaster)
                {
                    if (!recordViewModel.MasterLists.Any(x => x.MasterProjectListId == item.MasterProjectListId))
                        await this.RepositoryRequireHasMaster.DeleteAsync(item.RequireHasMasterProjectId);
                }

                if (recordViewModel.MasterLists != null)
                {
                    var ListMasterProject = new List<MasterProjectList>();
                    // Record
                    foreach (var item in recordViewModel.MasterLists)
                    {
                        if (item == null)
                            continue;
                        if (item.MasterProjectListId > 0 &&
                            (!recordViewModel.ParentRequireQcId.HasValue ||
                              recordViewModel.ParentRequireQcId < 1))
                        {
                            var dbMasterList = await this.RepositoryMasterList.GetAsync(item.MasterProjectListId, true);
                            if (dbMasterList != null)
                            {
                                dbMasterList.MarkNo = item.MarkNo;
                                dbMasterList.Name = item.Name;
                                dbMasterList.DrawingNo = item.DrawingNo;
                                dbMasterList.UnitNo = item.UnitNo;
                                dbMasterList.Box = item.Box;
                                dbMasterList.Quantity = item.Quantity;
                                dbMasterList.ModifyDate = record.ModifyDate;
                                dbMasterList.Modifyer = record.Modifyer;
                                // Await
                                await this.RepositoryMasterList.UpdateAsync(dbMasterList, dbMasterList.MasterProjectListId);
                                // Temp
                                ListMasterProject.Add(new MasterProjectList()
                                {
                                    MasterProjectListId = dbMasterList.MasterProjectListId,
                                    Quantity = dbMasterList.Quantity
                                });
                            }
                        }
                        else
                        {
                            var RequireHasMaster = new RequireHasMasterProject
                            {
                                CreateDate = record.ModifyDate,
                                Creator = record.Modifyer,
                                RequireQualityControlId = record.RequireQualityControlId,
                                Quantity = item.Quantity,
                                MasterProjectList = new MasterProjectList()
                                {
                                    CreateDate = record.ModifyDate,
                                    Creator = record.Modifyer,
                                    Quantity = item.Quantity,
                                    Name = item.Name,
                                    MarkNo = item.MarkNo,
                                    DrawingNo = item.DrawingNo,
                                    UnitNo = item.UnitNo,
                                    Box = item.Box,

                                }
                            };
                            await this.RepositoryRequireHasMaster.AddAsync(RequireHasMaster);

                            // Temp
                            ListMasterProject.Add(new MasterProjectList()
                            {
                                MasterProjectListId = RequireHasMaster.MasterProjectList.MasterProjectListId,
                                Quantity = RequireHasMaster.MasterProjectList.Quantity
                            });
                        }
                    }
                    if (ListMasterProject != null)
                    {
                        // Update require has master
                        foreach (var item in await this.RepositoryRequireHasMaster
                                                      .GetAllAsQueryable()
                                                      .Where(x => x.RequireQualityControlId == record.RequireQualityControlId)
                                                      .ToListAsync())
                        {
                            var MasterProject = ListMasterProject.FirstOrDefault(x => x.MasterProjectListId == item.MasterProjectListId);
                            if (MasterProject != null)
                            {
                                item.Quantity = MasterProject.Quantity;
                                await this.RepositoryRequireHasMaster.UpdateAsync(item, item.RequireHasMasterProjectId);
                            }
                        }
                    }
                }

                Expression<Func<RequireQcMoreWorkActvity, bool>> expression = e => e.RequireQualityControlId == key;
                var dbRequireMoreActivity = await this.RepositoryRequireMoreActvity.FindAllAsync(expression);

                if (dbRequireMoreActivity != null)
                {
                    //Remove requisition if edit remove it
                    foreach (var item in dbRequireMoreActivity)
                    {
                        if (!recordViewModel.MoreWorkActvities.Any(x => x.WorkActivityId == item.WorkActivityId))
                            await this.RepositoryRequireMoreActvity.DeleteAsync(item.RequireQcMoreWorkActvityId);
                    }
                }

                if (recordViewModel.MoreWorkActvities != null)
                {
                    foreach(var myitem in recordViewModel.MoreWorkActvities)
                    {
                        if (myitem == null)
                            continue;

                        Expression<Func<RequireQcMoreWorkActvity, bool>> expressionIn = e => e.RequireQualityControlId == record.RequireQualityControlId && 
                                                                                           e.WorkActivityId == myitem.WorkActivityId;
                        var dbMoreWorkActivity = await this.RepositoryRequireMoreActvity.FindAsync(expressionIn);
                        if (dbMoreWorkActivity != null)
                        {
                            dbMoreWorkActivity.ModifyDate = record.ModifyDate;
                            dbMoreWorkActivity.Modifyer = record.Modifyer;

                            await this.RepositoryRequireMoreActvity.UpdateAsync(dbMoreWorkActivity, dbMoreWorkActivity.RequireQcMoreWorkActvityId);
                        }
                        else
                        {
                            await this.RepositoryRequireMoreActvity.AddAsync(new RequireQcMoreWorkActvity()
                            {
                                CreateDate = record.ModifyDate,
                                Creator = record.Modifyer,
                                RequireQualityControlId = record.RequireQualityControlId,
                                WorkActivityId = myitem.WorkActivityId
                            });
                        }
                    }
                }
            }

            if (record.ParentRequireQcId.HasValue)
            {
                // For Parent
                var ParentRequireQc = await this.repository.GetAsync(record.ParentRequireQcId.Value);
                if (ParentRequireQc != null)
                {
                    if (ParentRequireQc.RequireStatus == RequireStatus.QcFail)
                    {
                        ParentRequireQc.RequireStatus = RequireStatus.Revise;
                        ParentRequireQc.ModifyDate = DateTime.Now;
                        ParentRequireQc.Modifyer = record.Creator;
                        //Update Parent
                        await this.repository.UpdateAsync(ParentRequireQc, ParentRequireQc.RequireQualityControlId);
                    }
                }
                // For All chlid
                var ChlidRequireQc = await this.repository.GetToListAsync
                                       (selector: x => x,
                                        predicate: x => x.ParentRequireQcId == record.ParentRequireQcId.Value &&
                                                        x.RequireQualityControlId != record.RequireQualityControlId);
                if (ChlidRequireQc != null)
                {
                    foreach (var item in ChlidRequireQc)
                    {
                        if (item.RequireStatus == RequireStatus.QcFail)
                        {
                            item.RequireStatus = RequireStatus.Revise;
                            item.ModifyDate = DateTime.Now;
                            item.Modifyer = record.Creator;
                            //Update Parent
                            await this.repository.UpdateAsync(item, item.RequireQualityControlId);
                        }
                    }
                }
            }

            //Send mail to groupqc
            if (record.RequireStatus == RequireStatus.WeldingReq)
                await this.SendMail(record, 1);

            return new JsonResult(record, this.DefaultJsonSettings);
        }

        #endregion

        #region ATTACH

        // GET: api/RequireQualityControl/GetAttach/5
        [HttpGet("GetAttach")]
        public async Task<IActionResult> GetAttach(int key)
        {
            var AttachIds = await this.RepositoryHasAttach.GetAllAsQueryable()
                                  .Where(x => x.RequireQualityControlId == key)
                                  .Select(x => x.AttachFileId).ToListAsync();
            if (AttachIds != null)
            {
                var DataAttach = await this.RepositoryAttach.GetAllAsQueryable()
                                       .Where(x => AttachIds.Contains(x.AttachFileId))
                                       .ToListAsync();

                return new JsonResult(DataAttach, this.DefaultJsonSettings);
            }

            return NotFound(new { Error = "Attatch not been found." });
        }

        // POST: api/RequireQualityControl/PostAttach/5/Someone
        [HttpPost("PostAttach")]
        public async Task<IActionResult> PostAttac(int key, string CreateBy, IEnumerable<IFormFile> files)
        {
            string Message = "";
            try
            {
                long size = files.Sum(f => f.Length);

                // full path to file in temp location
                var filePath1 = Path.GetTempFileName();

                foreach (var formFile in files)
                {
                    string FileName = Path.GetFileName(formFile.FileName).ToLower();
                    // create file name for file
                    string FileNameForRef = $"{DateTime.Now.ToString("ddMMyyhhmmssfff")}{ Path.GetExtension(FileName).ToLower()}";
                    // full path to file in temp location
                    var filePath = Path.Combine(this.HostEnvironment.WebRootPath + "/files", FileNameForRef);

                    if (formFile.Length > 0)
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                            await formFile.CopyToAsync(stream);
                    }

                    var returnData = await this.RepositoryAttach.AddAsync(new AttachFile()
                    {
                        FileAddress = $"/qualitycontrol/files/{FileNameForRef}",
                        FileName = FileName,
                        CreateDate = DateTime.Now,
                        Creator = CreateBy ?? "Someone"
                    });

                    await this.RepositoryHasAttach.AddAsync(new RequireHasAttach()
                    {
                        AttachFileId = returnData.AttachFileId,
                        CreateDate = DateTime.Now,
                        Creator = CreateBy ?? "Someone",
                        RequireQualityControlId = key
                    });
                }

                return Ok(new { count = 1, size, filePath1 });

            }
            catch (Exception ex)
            {
                Message = ex.ToString();
            }

            return NotFound(new { Error = "Not found " + Message });
        }

        // DELETE: api/RequireQualityControl/DeleteAttach/5
        [HttpDelete("DeleteAttach")]
        public async Task<IActionResult> DeleteAttach(int AttachFileId)
        {
            if (AttachFileId > 0)
            {
                var AttachFile = await this.RepositoryAttach.GetAsync(AttachFileId);
                if (AttachFile != null)
                {
                    var filePath = Path.Combine(this.HostEnvironment.WebRootPath + AttachFile.FileAddress);
                    FileInfo delFile = new FileInfo(filePath);

                    if (delFile.Exists)
                        delFile.Delete();
                    // Condition
                    Expression<Func<RequireHasAttach, bool>> condition = c => c.AttachFileId == AttachFile.AttachFileId;
                    var RequireMaitenanceHasAttach = this.RepositoryHasAttach.FindAsync(condition).Result;
                    if (RequireMaitenanceHasAttach != null)
                        this.RepositoryHasAttach.Delete(RequireMaitenanceHasAttach.RequireHasAttachId);
                    // remove attach
                    return new JsonResult(await this.RepositoryAttach.DeleteAsync(AttachFile.AttachFileId), this.DefaultJsonSettings);
                }
            }
            return NotFound(new { Error = "Not found attach file." });
        }

        #endregion

        #region NoUser
        public async Task<IActionResult> GetScroll2([FromBody] ScrollViewModel Scroll)
        {
            if (Scroll == null)
                return BadRequest();

            var QueryData = this.repository.GetAllAsQueryable()
                                // .AsNoTracking()
                                .AsQueryable();

            if (!string.IsNullOrEmpty(Scroll.Where))
                QueryData = QueryData.Where(x => x.Creator == Scroll.Where);

            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.ToLower().Split(null);

            foreach (var keyword in filters)
            {
                QueryData = QueryData.Where(x => x.RequireQualityNo.ToLower().Contains(keyword) ||
                                                 x.InspectionPoint.Name.ToLower().Contains(keyword) ||
                                                 x.WorkActivity.Name.ToLower().Contains(keyword) ||
                                                 x.WorkGroupQualityControl.Name.ToLower().Contains(keyword) ||
                                                 x.Remark.ToLower().Contains(keyword) ||
                                                 x.Description.ToLower().Contains(keyword));
            }

            // Order
            switch (Scroll.SortField)
            {
                case "RequireQualityNo":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.RequireQualityNo);
                    else
                        QueryData = QueryData.OrderBy(e => e.RequireQualityNo);
                    break;
                case "InspectionPointString":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.InspectionPoint.Name);
                    else
                        QueryData = QueryData.OrderBy(e => e.InspectionPoint.Name);
                    break;
                case "RequireDate":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.RequireDate);
                    else
                        QueryData = QueryData.OrderBy(e => e.RequireDate);
                    break;
                default:
                    QueryData = QueryData.OrderByDescending(e => e.RequireDate);
                    break;
            }
            // Get TotalRow
            Scroll.TotalRow = await QueryData.CountAsync();
            // Skip Take
            QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);

            var HasData = await QueryData.ToListAsync();
            var listData = new List<RequireQualityControlViewModel>();
            foreach (var item in HasData)
            {
                var MapItem = this.mapper.Map<RequireQualityControl, RequireQualityControlViewModel>(item);
                listData.Add(MapItem);
            }

            return new JsonResult(new ScrollDataViewModel<RequireQualityControlViewModel>(Scroll, listData), this.DefaultJsonSettings);
        }

        public async Task<IActionResult> GetScrollOnFail2([FromBody] ScrollViewModel Scroll)
        {
            if (Scroll == null)
                return BadRequest();

            var QueryData = this.repository.GetAllAsQueryable()
                                .Where(x => x.RequireStatus == RequireStatus.QcFail)
                                // .AsNoTracking()
                                .AsQueryable();

            if (!string.IsNullOrEmpty(Scroll.Where))
                QueryData = QueryData.Where(x => x.Creator == Scroll.Where);

            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.ToLower().Split(null);

            foreach (var keyword in filters)
            {
                QueryData = QueryData.Where(x => x.RequireQualityNo.ToLower().Contains(keyword) ||
                                                 x.InspectionPoint.Name.ToLower().Contains(keyword) ||
                                                 x.WorkActivity.Name.ToLower().Contains(keyword) ||
                                                 x.WorkGroupQualityControl.Name.ToLower().Contains(keyword) ||
                                                 x.Remark.ToLower().Contains(keyword) ||
                                                 x.Description.ToLower().Contains(keyword));
            }

            // Order
            switch (Scroll.SortField)
            {
                case "RequireQualityNo":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.RequireQualityNo);
                    else
                        QueryData = QueryData.OrderBy(e => e.RequireQualityNo);
                    break;
                case "InspectionPointString":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.InspectionPoint.Name);
                    else
                        QueryData = QueryData.OrderBy(e => e.InspectionPoint.Name);
                    break;
                case "RequireDate":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.RequireDate);
                    else
                        QueryData = QueryData.OrderBy(e => e.RequireDate);
                    break;
                default:
                    QueryData = QueryData.OrderByDescending(e => e.RequireDate);
                    break;
            }
            // Get TotalRow
            Scroll.TotalRow = await QueryData.CountAsync();
            // Skip Take
            QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);

            var HasData = await QueryData.ToListAsync();
            var listData = new List<RequireQualityControlViewModel>();
            foreach (var item in HasData)
            {
                var MapItem = this.mapper.Map<RequireQualityControl, RequireQualityControlViewModel>(item);
                listData.Add(MapItem);
            }

            return new JsonResult(new ScrollDataViewModel<RequireQualityControlViewModel>(Scroll, listData), this.DefaultJsonSettings);
        }

        public async Task<IActionResult> RequireQualityControlWaiting2([FromBody] OptionRequireQualityControl Option)
        {
            string Message = "";
            try
            {
                var QueryData = this.repository.GetAllAsQueryable()
                                               .Where(x => x.RequireStatus != RequireStatus.Cancel)
                                               .AsQueryable();
                int TotalRow;

                if (Option != null)
                {
                    if (!string.IsNullOrEmpty(Option.Filter))
                    {
                        // Filter
                        var filters = string.IsNullOrEmpty(Option.Filter) ? new string[] { "" }
                                            : Option.Filter.ToLower().Split(null);
                        foreach (var keyword in filters)
                        {
                            QueryData = QueryData.Where(x => x.Description.ToLower().Contains(keyword) ||
                                                             x.Remark.ToLower().Contains(keyword) ||
                                                             x.RequireQualityNo.ToLower().Contains(keyword) ||
                                                             x.WorkActivity.Name.ToLower().Contains(keyword) ||
                                                             x.InspectionPoint.Name.ToLower().Contains(keyword) ||
                                                             x.WorkGroupQualityControl.Name.ToLower().Contains(keyword));
                        }
                    }

                    // Option ProjectCodeMaster
                    if (Option.ProjectId.HasValue)
                        QueryData = QueryData.Where(x => x.ProjectCodeDetailId == Option.ProjectId);

                    // Option Status
                    if (Option.Status.HasValue)
                    {
                        if (Option.Status == 1)
                            QueryData = QueryData.Where(x => x.RequireStatus == RequireStatus.Waiting || x.RequireStatus == RequireStatus.WeldingReq);
                        else if (Option.Status == 2)
                            QueryData = QueryData.Where(x => x.RequireStatus == RequireStatus.Welding);
                        else
                            QueryData = QueryData.Where(x => x.RequireStatus != RequireStatus.Cancel);
                    }
                    else
                        QueryData = QueryData.Where(x => x.RequireStatus == RequireStatus.Waiting);

                    TotalRow = await QueryData.CountAsync();

                    // Option Skip and Task
                    if (Option.Skip.HasValue && Option.Take.HasValue)
                        QueryData = QueryData.Skip(Option.Skip ?? 0).Take(Option.Take ?? 50);
                    else
                        QueryData = QueryData.Skip(0).Take(50);
                }
                else
                    TotalRow = await QueryData.CountAsync();

                var GetData = await QueryData.ToListAsync();
                if (GetData.Any())
                {
                    List<string> ColumnUpper = new List<string>();
                    IDictionary<DateTime, string> ColumnLower = new Dictionary<DateTime, string>();

                    var MinDate = GetData.Min(x => x.RequireDate);
                    var MaxDate = GetData.Max(x => x.RequireDate);
                    var countCol = 1;
                    if (MinDate == null && MaxDate == null)
                        return NotFound(new { Error = "Data not found" });

                    foreach (DateTime day in EachDay(MinDate, MaxDate))
                    {
                        if (GetData.Any(x => x.RequireDate.Date == day.Date))
                        {
                            ColumnUpper.Add(day.Date.ToString("dd/MM/yy"));
                            ColumnLower.Add(new DateTime(day.Year, day.Month, day.Day, 8, 0, 0), $"Col{countCol.ToString("00")}");
                            countCol++;
                            ColumnLower.Add(new DateTime(day.Year, day.Month, day.Day, 13, 0, 0), $"Col{countCol.ToString("00")}");
                            countCol++;
                        }
                    }

                    var DataTable = new List<IDictionary<string, object>>();

                    foreach (var Data in GetData.OrderBy(x => x.WorkGroupQualityControl.Name).ThenBy(x => x.RequireDate).ThenBy(x => x.ResponseDate))
                    {
                        var WorkGroupQcName = Option.Status == 2 ? "Welding WorkGroup" : $"{Data.WorkGroupQualityControl.Name ?? "No-Data"}";
                        IDictionary<string, object> rowData;
                        bool update = false;
                        if (DataTable.Any(x => (string)x["WorkGroupQcName"] == WorkGroupQcName))
                        {
                            var FirstData = DataTable.FirstOrDefault(x => (string)x["WorkGroupQcName"] == WorkGroupQcName);
                            if (FirstData != null)
                            {
                                rowData = FirstData;
                                update = true;
                            }
                            else
                                rowData = new ExpandoObject();
                        }
                        else
                            rowData = new ExpandoObject();

                        // Get ProjectNo
                        var ProjectName = "-";
                        if (Data.ProjectCodeDetailId != null)
                        {
                            var ProjectDetail = await this.RepositoryProjectDetail.GetAsync(Data.ProjectCodeDetailId.Value, true);
                            ProjectName = $"{ProjectDetail.ProjectCodeMaster.ProjectCode} / {ProjectDetail.ProjectCodeDetailCode}";
                        }

                        var Key = "";
                        if (ColumnLower.Any(x => x.Key.Date == Data.RequireDate.Date))
                        {
                            if (Data.RequireDate.Hour > 12)
                                Key = ColumnLower.FirstOrDefault(x => x.Key.Date == Data.RequireDate.Date && x.Key.Hour > 12).Value;
                            else
                                Key = ColumnLower.FirstOrDefault(x => x.Key.Date == Data.RequireDate.Date && x.Key.Hour <= 12).Value;
                        }
                        // New Data
                        var Master = new RequireQualityControlViewModel()
                        {
                            RequireQualityControlId = Data.RequireQualityControlId,
                            MailReply = Data.MailReply ?? null,
                            ResponseDate = Data.ResponseDate,
                            // RequireString = $"{EmployeeReq} | No.{Data.RequireNo}",
                            ProjectCodeDetailString = ProjectName,
                            InspectionPointString = Data?.InspectionPoint?.Name ?? "",
                            WorkActivityString = Data?.WorkActivity?.Name ?? "",
                            WorkGroupQualityControlString = Data?.WorkGroupQualityControl?.Name ?? "",
                            RequireEmpString = string.IsNullOrEmpty(Data.RequireEmp) ? "-" : "คุณ" + (await this.RepositoryEmployee.GetAsync(Data.RequireEmp)).NameThai

                        };

                        Master.RequireEmpString += Data.RequireDate != null ? $" {Data.RequireDate.ToString("HH:mm")}น." : "";

                        if (rowData.Any(x => x.Key == Key))
                        {
                            // New Value
                            var ListMaster = (List<RequireQualityControlViewModel>)rowData[Key];
                            ListMaster.Add(Master);
                            // add to row data
                            rowData[Key] = ListMaster;
                        }
                        else // add new
                            rowData.Add(Key, new List<RequireQualityControlViewModel>() { Master });

                        if (!update)
                        {
                            rowData.Add("WorkGroupQcName", WorkGroupQcName);
                            DataTable.Add(rowData);
                        }
                    }

                    if (!DataTable.Any())
                        return BadRequest(new { Error = "Data not been found." });

                    return new JsonResult(new
                    {
                        TotalRow,
                        ColumnUpper,
                        ColumnLower = ColumnLower.OrderBy(x => x.Key.Date)
                                                 .Select(x => x.Key.Hour < 12 ? $"AM" : $"PM"),
                        ColumnAll = ColumnLower.OrderBy(x => x.Key.Date).Select(x => x.Value),
                        DataTable
                    }, this.DefaultJsonSettings);
                }
            }
            catch (Exception ex)
            {
                Message = $"Has error {ex.ToString()}";
            }

            return BadRequest(new { Error = Message });
        }


        #endregion
    }
}
