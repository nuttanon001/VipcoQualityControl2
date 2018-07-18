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
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;

using VipcoQualityControl.Services;
using VipcoQualityControl.ViewModels;
using VipcoQualityControl.Models.Machines;
using VipcoQualityControl.Models.QualityControls;
using VipcoQualityControl.Helper;

using AutoMapper;

namespace VipcoQualityControl.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class QualityControlResultController : GenericController<QualityControlResult>
    {
        private readonly IRepositoryQualityControl<RequireHasMasterProject> repositoryRequireHasMaster;
        private readonly IRepositoryQualityControl<RequireQualityControl> repositoryRequireQualityControl;
        private readonly IRepositoryQualityControl<InspectionPoint> repositoryInspection;
        private readonly IRepositoryQualityControl<WorkActivity> repositoryWorkActivity;
        private readonly IRepositoryQualityControl<WorkGroupQualityControl> repositoryQcGroup;
        private readonly IRepositoryQualityControl<WorkGroupHasWorkShop> repositoryWorkShop;
        private readonly IRepositoryQualityControl<RequireHasWelder> repositoryRequireWelder;
        private readonly IRepositoryMachine<ProjectCodeDetail> repositoryProject;
        private readonly IRepositoryMachine<Employee> repositoryEmployee;
        private readonly IRepositoryMachine<EmployeeGroupMis> repositoryGroupMis;
        private readonly IViewRenderService viewRenderService;
        private readonly EmailClass EmailClass;
        public QualityControlResultController(IRepositoryQualityControl<QualityControlResult> repo,
            IRepositoryQualityControl<RequireHasMasterProject> repoRequireHasMaster,
            IRepositoryQualityControl<RequireQualityControl> repoRequireRequireQualityControl,
            IRepositoryQualityControl<InspectionPoint> repoInspacetion,
            IRepositoryQualityControl<WorkActivity> repoWorkActivity,
            IRepositoryQualityControl<WorkGroupQualityControl> repoQcGroup,
            IRepositoryQualityControl<WorkGroupHasWorkShop> repoWorkShop,
            IRepositoryQualityControl<RequireHasWelder> repoRequireWelder,
            IRepositoryMachine<Employee> repoEmployee,
            IRepositoryMachine<ProjectCodeDetail> repoProject,
            IRepositoryMachine<EmployeeGroupMis> repoGroupMis,
            IViewRenderService viewRender,
            IMapper mapper) : base(repo, mapper) {
            //Repository Machine
            this.repositoryEmployee = repoEmployee;
            this.repositoryProject = repoProject;
            this.repositoryGroupMis = repoGroupMis;
            //Repository Quality Control
            this.repositoryRequireQualityControl = repoRequireRequireQualityControl;
            this.repositoryRequireHasMaster = repoRequireHasMaster;
            this.repositoryInspection = repoInspacetion;
            this.repositoryWorkActivity = repoWorkActivity;
            this.repositoryWorkShop = repoWorkShop;
            this.repositoryQcGroup = repoQcGroup;
            this.repositoryRequireWelder = repoRequireWelder;
            //Helper
            this.EmailClass = new EmailClass();
            //Razor to string
            this.viewRenderService = viewRender;
        }

        #region Private

        // Require qualitycontrol update
        private async Task<bool> RequireQualityControlUpdate(QualityControlResult QualityControl, bool Status, string ByUser)
        {
            if (QualityControl.RequireQualityControlId.HasValue)
            {
                var HasData = await this.repositoryRequireQualityControl
                    .GetFirstOrDefaultAsync(x => x,x => x.RequireQualityControlId == QualityControl.RequireQualityControlId.Value);
                if (HasData != null)
                {
                    // Comfirm require date
                    if (!HasData.ResponseDate.HasValue)
                        HasData.ResponseDate = HasData.RequireDate.AddHours(2);
                    var toWelder = false;
                    if (HasData.RequireStatus == RequireStatus.WeldingReq)
                    {
                        toWelder = true;
                        HasData.RequireStatus = Status ? RequireStatus.Complate : RequireStatus.WeldingFail;
                    }
                    else
                        HasData.RequireStatus = Status ? RequireStatus.Complate : RequireStatus.QcFail;

                    HasData.Modifyer = ByUser;
                    HasData.ModifyDate = DateTime.Now;

                    await this.repositoryRequireQualityControl.UpdateAsync(HasData, HasData.RequireQualityControlId);
                    await this.SendMail2(QualityControl, Status);
                    if (toWelder)
                        await this.SendMailWelder(QualityControl, Status);

                    return true;
                }
            }
            return false;
        }

        // Send Mail
        private async Task<bool> SendMail(QualityControlResult HasData,bool ShellYouCanPass)
        {
            if (HasData.RequireQualityControlId.HasValue)
            {
                var HasRequire = await this.repositoryRequireQualityControl
                    .GetFirstOrDefaultAsync(x => x,x => x.RequireQualityControlId == HasData.RequireQualityControlId.Value);
                if (HasRequire != null)
                {
                    if (this.EmailClass.IsValidEmail(HasRequire.MailReply))
                    {
                        var EmpName = (await this.repositoryEmployee.GetAsync(HasRequire.RequireEmp)).NameThai ?? "ไม่ระบุ";

                        var BodyMessage = "<body style=font-size:11pt;font-family:Tahoma>" +
                                            "<h4 style='color:steelblue;'>เมล์ฉบับนี้เป็นแจ้งเตือนจากระบบงาน VIPCO Quality Control System</h4>" +
                                            $"เรียน คุณ{EmpName}" +
                                            $"<p>เรื่อง ผลการตรวจสอบคุณภาพงานเลขที่ {HasRequire.RequireQualityNo} </p>" +
                                            $"{(ShellYouCanPass ? "<p style='color:steelblue;'><b>ผลการตรวจสอบผ่านเงื่อนไข</b></p>" : "<p style='color:red;'><b>ไม่ผ่านเงื่อนไขการตรวจสอบ</b></p>")}" +
                                            $"<p>รายการเรียกตรวจสอบคุณภาพดังนี้</p><hr/><span>Mark-No</span>&emsp;&emsp;&emsp;&emsp;&emsp;<span>Status</span>";
                        // RequireQualityControl
                        var MarkNos = await this.repositoryRequireHasMaster.GetAllAsQueryable()
                                                .Where(x => x.RequireQualityControlId == HasData.RequireQualityControlId)
                                                .ToListAsync();
                        var RowIndex = 1;
                        foreach (var item in MarkNos)
                        {
                            BodyMessage += $"<br/><span style='font-size:13px;'>{RowIndex}. {item.MasterProjectList.MarkNo}</span>&emsp;&emsp;&emsp;&emsp;&emsp;" +
                                           $"<span style='font-size:13px;color:lightsteelblue;'>{(item.Quantity - item.PassQuantity > 0 ? "ไม่ผ่าน(" + (item.Quantity - item.PassQuantity).ToString() + " ea.)" : "ผ่าน")}" +
                                           $"{(item.QualityControlReason != null ? "( " + item?.QualityControlReason?.Name + " )" : "")}</span>";
                            RowIndex++;
                        }

                        BodyMessage += $"<hr/><p>จากทางหน่วยงานควบคุมคุณภาพ ในวันที่ <b>{HasData.QualityControlResultDate.Value.ToString("dd/MM/yy")}</b>" +
                                        $"<p>\"คุณ{EmpName}\" " +
                                        $"สามารถเข้าไปตรวจติดตามข้อมูลได้ <a href='http://{Request.Host}/qualitycontrol/require-qc/link-email/{HasData.RequireQualityControlId}'>ที่นี้</a> </p>" +
                                        "<span style='color:steelblue;'>This mail auto generated by VIPCO Quality Control SYSTEM. Do not reply this email.</span>" +
                                        "</body>";

                        await this.EmailClass.SendMailMessage(HasRequire.MailReply, EmpName,
                                                   new List<string> { HasRequire.MailReply },
                                                   BodyMessage, "Notification mail from VIPCO Quality Control system.");

                        return true;
                    }
                }
            }
            return false;
        }

        // Send Mail
        private async Task<bool> SendMail2(QualityControlResult HasData, bool ShellYouCanPass)
        {
            if (HasData.RequireQualityControlId.HasValue)
            {
                var HasRequire = await this.repositoryRequireQualityControl.
                    GetFirstOrDefaultAsync(x => x,x => x.RequireQualityControlId == HasData.RequireQualityControlId.Value);
                if (HasRequire != null)
                {
                    if (string.IsNullOrEmpty(HasRequire.MailReply))
                        return false;

                    var ListMail = new List<string>();
                    if (HasRequire.MailReply.IndexOf(',') != -1)
                        ListMail = HasRequire.MailReply.Split(',').ToList();
                    else if (HasRequire.MailReply.IndexOf(';') != -1)
                        ListMail = HasRequire.MailReply.Split(';').ToList();
                    else
                        ListMail.Add(HasRequire.MailReply);

                    if (ListMail.Any())
                    {
                        var EmpName = (await this.repositoryEmployee
                            .GetFirstOrDefaultAsync(x => x,x => x.EmpCode == HasRequire.RequireEmp)).NameThai ?? "ไม่ระบุ";
                        var ItemLists = await this.repositoryRequireHasMaster.GetToListAsync(x => new RequireHasMasterProjectViewModel()
                        {
                            DrawingNo = x.MasterProjectList.DrawingNo,
                            MarkNoString = x.MasterProjectList.MarkNo,
                            Box = x.MasterProjectList.Box,
                            UnitNo = x.MasterProjectList.UnitNo,
                            Quantity = x.Quantity,
                            PassQuantity = x.PassQuantity,
                            QualityControlReasonString = x.QualityControlReason != null ? x.QualityControlReason.Name : "-"
                        },x => x.RequireQualityControlId == HasData.RequireQualityControlId,null,
                        x => x.Include(z => z.MasterProjectList));

                        var EmailTemplate = new EmailTemplateViewModel()
                        {
                            ToEmployeeName = $"คุณ{EmpName}",
                            ItemLists = ItemLists,
                            LinkToApp = $"http://{Request.Host}/qualitycontrol/require-qc/link-email/{HasData.RequireQualityControlId}",
                            RequireNo = HasRequire.RequireQualityNo,
                            ResultDate = HasData.QualityControlResultDate.Value.ToString("dd MMM yy"),
                            Status = ShellYouCanPass
                        };

                        var result = await viewRenderService.RenderToStringAsync("Email/Result", EmailTemplate);

                        await this.EmailClass.SendMailMessage(ListMail[0], EmpName,
                                                   ListMail,
                                                   result, "Notification mail from VIPCO Quality Control system.");

                        return true;
                    }
                }
            }
            return false;
        }

        private async Task<bool> SendMailWelder(QualityControlResult HasData, bool ShellYouCanPass)
        {
            if (HasData.RequireQualityControlId.HasValue)
            {
                var HasRequire = await this.repositoryRequireQualityControl.
                    GetFirstOrDefaultAsync(x => x, x => x.RequireQualityControlId == HasData.RequireQualityControlId.Value);
                if (HasRequire != null)
                {
                    var QcGroup = await this.repositoryQcGroup.GetAsync(HasRequire.WorkGroupQualityControlId.Value);
                    if (string.IsNullOrEmpty(QcGroup.SubEmail))
                        return false;

                    var ListMail = new List<string>();
                    if (QcGroup.SubEmail.IndexOf(',') != -1)
                        ListMail = QcGroup.SubEmail.Split(',').ToList();
                    else if (QcGroup.SubEmail.IndexOf(';') != -1)
                        ListMail = QcGroup.SubEmail.Split(';').ToList();
                    else
                        ListMail.Add(QcGroup.SubEmail);

                    if (ListMail.Any())
                    {
                        var ItemLists = await this.repositoryRequireHasMaster.GetToListAsync(x => new RequireHasMasterProjectViewModel()
                        {
                            DrawingNo = x.MasterProjectList.DrawingNo,
                            MarkNoString = x.MasterProjectList.MarkNo,
                            Box = x.MasterProjectList.Box,
                            UnitNo = x.MasterProjectList.UnitNo,
                            Quantity = x.Quantity,
                            PassQuantity = x.PassQuantity,
                            QualityControlReasonString = x.QualityControlReason != null ? x.QualityControlReason.Name : "-"
                        }, x => x.RequireQualityControlId == HasData.RequireQualityControlId, null,
                        x => x.Include(z => z.MasterProjectList));

                        var EmailTemplate = new EmailTemplateViewModel()
                        {
                            ToEmployeeName = $"หน่วยงานเชื่อม",
                            ItemLists = ItemLists,
                            LinkToApp = $"http://{Request.Host}/qualitycontrol/require-qc/link-email/{HasData.RequireQualityControlId}",
                            RequireNo = HasRequire.RequireQualityNo,
                            ResultDate = HasData.QualityControlResultDate.Value.ToString("dd MMM yy"),
                            Status = ShellYouCanPass
                        };

                        var result = await viewRenderService.RenderToStringAsync("Email/Result", EmailTemplate);

                        await this.EmailClass.SendMailMessage(ListMail[0], EmailTemplate.ToEmployeeName,
                                                   ListMail,
                                                   result, "Notification mail from VIPCO Quality Control system.");

                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        // GET: api/QualityControlResult/GetKeyNumber/5
        [HttpGet("GetKeyNumber")]
        public override async Task<IActionResult> Get(int key)
        {
            var HasItem = await this.repository.GetFirstOrDefaultAsync(
                selector:x => x,
                predicate:x => x.QualityControlResultId == key,
                include:x => x.Include(z => z.RequireQualityControl.WorkGroupQualityControl));

            if (HasItem != null)
            {
                var MapItem = this.mapper.Map<QualityControlResult, QualityControlResultViewModel>(HasItem);
                // RequireEmpString
                if (!string.IsNullOrEmpty(MapItem.EmpCode))
                    MapItem.EmpQualityControlString = (await this.repositoryEmployee.GetFirstOrDefaultAsync(x => x,x => x.EmpCode == MapItem.EmpCode)).NameThai;
               
                return new JsonResult(MapItem, this.DefaultJsonSettings);
            }
            return BadRequest();
        }

        // POST: api/QualityControlResult/GetScroll
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            if (Scroll == null)
                return BadRequest();

            var QueryData = this.repository.GetAllAsQueryable()
                                //.AsNoTracking() Error EF-Core 2.1 Preview2
                                .AsQueryable();

            if (!string.IsNullOrEmpty(Scroll.Where))
                QueryData = QueryData.Where(x => x.Creator == Scroll.Where);

            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.ToLower().Split(null);
            foreach (var keyword in filters)
            {
                QueryData = QueryData.Where(x => x.RequireQualityControl.InspectionPoint.Name.ToLower().Contains(keyword) ||
                                                 x.RequireQualityControl.RequireQualityNo.ToLower().Contains(keyword) ||
                                                 x.RequireQualityControl.WorkGroupQualityControl.Name.ToLower().Contains(keyword) ||
                                                 x.Remark.ToLower().Contains(keyword) ||
                                                 x.Description.ToLower().Contains(keyword));
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
                case "WorkGroupQualityControlString":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.RequireQualityControl.WorkGroupQualityControl.Name);
                    else
                        QueryData = QueryData.OrderBy(e => e.RequireQualityControl.WorkGroupQualityControl.Name);
                    break;
                case "QualityControlResultDate":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.QualityControlResultDate);
                    else
                        QueryData = QueryData.OrderBy(e => e.QualityControlResultDate);
                    break;
                default:
                    QueryData = QueryData.OrderByDescending(e => e.QualityControlResultDate);
                    break;
            }

            var Message = "";

            try
            {
                // Get TotalRow
                Scroll.TotalRow = await QueryData.CountAsync();
                // Skip Take
                QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);
                // Mapper
                var HasMapper = new List<QualityControlResultViewModel>();
                foreach (var item in await QueryData.ToListAsync())
                    HasMapper.Add(this.mapper.Map<QualityControlResult, QualityControlResultViewModel>(item));
                //JsonResult
                return new JsonResult(new ScrollDataViewModel<QualityControlResultViewModel>(Scroll,
                    HasMapper), this.DefaultJsonSettings);
            }
            catch (Exception ex)
            {
                Message = $"Has error {ex.ToString()}";
            }

            return BadRequest(new { Error = Message });
        }

        //POST: api/QualityControlResult/CreateV2
        [HttpPost("CreateV2")]
        public async Task<IActionResult> CreateV2([FromBody] QualityControlResultViewModel recordViewModel)
        {
            if (ModelState.IsValid)
            {
                // Set date for CrateDate Entity
                if (recordViewModel == null)
                    return BadRequest();
                var record = this.mapper.Map<QualityControlResultViewModel, QualityControlResult>(recordViewModel);
                // +7 Hour
                record = this.helper.AddHourMethod(record);
                record.CreateDate = DateTime.Now;

                if (recordViewModel.QualityControlResultTime.HasValue)
                {
                    record.QualityControlResultDate = new DateTime(record.QualityControlResultDate.Value.Year, record.QualityControlResultDate.Value.Month, record.QualityControlResultDate.Value.Day,
                                                      recordViewModel.QualityControlResultTime.Value.Hour, recordViewModel.QualityControlResultTime.Value.Minute, 0);
                }

                var RequireQc = await this.repositoryRequireQualityControl.GetFirstOrDefaultAsync(
                    x => x, x => x.RequireQualityControlId == record.RequireQualityControlId);

                // Check HasFail or Pass
                var HasFail = false;
                // Set ItemMainHasEmployees
                if (recordViewModel.QualityHasMasterLists != null)
                {
                    foreach (var item in recordViewModel.QualityHasMasterLists)
                    {
                        if (item == null)
                            continue;

                        var HasData = await this.repositoryRequireHasMaster
                            .GetFirstOrDefaultAsync(
                            selector: x => x,
                            predicate: x => x.RequireHasMasterProjectId == item.RequireHasMasterProjectId,
                            include: x => x.Include(z => z.RequireHasWelder));

                        if (HasData != null)
                        {
                            HasData.QualityControlReasonId = item.QualityControlReasonId;
                            HasData.PassQuantity = item.PassQuantity;
                            HasData.ModifyDate = DateTime.Now;
                            HasData.Modifyer = record.Creator;
                            // Update
                            HasFail = HasData.PassQuantity != HasData.Quantity;
                            await this.repositoryRequireHasMaster.UpdateAsync(HasData, HasData.RequireHasMasterProjectId);
                            // If Welder
                            if (HasData.RequireHasWelder != null)
                            {
                                HasData.RequireHasWelder.QcStatus = HasFail ? WelderStatus.Rejected : 
                                    (RequireQc.ParentRequireQcId == null ? WelderStatus.Accepted : WelderStatus.AcceptedAfterRepaired);

                                HasData.RequireHasWelder.ModifyDate = record.CreateDate;
                                HasData.RequireHasWelder.Modifyer = record.Creator;
                                // Update RequireHasWelder
                                await this.repositoryRequireWelder.UpdateAsync(HasData.RequireHasWelder, HasData.RequireHasWelder.RequireHasWelderId);
                            }
                        }
                    }
                }
                // Status
                record.QualityControlStatus = HasFail ? QualityControlStatus.Failed : QualityControlStatus.Approved;

                if (await this.repository.AddAsync(record) == null)
                    return BadRequest();
                // Update Require qualitycontrol
                await this.RequireQualityControlUpdate(record, !HasFail, record.Creator);
                if (record.RequireQualityControl != null)
                    record.RequireQualityControl = null;

                return new JsonResult(record, this.DefaultJsonSettings);
            }
            else
                return BadRequest(ModelState);
        }

        //PUT: api/QualityControlResult/UpdateV2
        [HttpPut("UpdateV2")]
        public async Task<IActionResult> UpdateV2(int key, [FromBody] QualityControlResultViewModel recordViewModel)
        {
            if (ModelState.IsValid)
            {
                if (key < 1)
                    return BadRequest();
                if (recordViewModel == null)
                    return BadRequest();

                var record = this.mapper.Map<QualityControlResultViewModel, QualityControlResult>(recordViewModel);
                // +7 Hour
                record = this.helper.AddHourMethod(record);
                if (recordViewModel.QualityControlResultTime.HasValue)
                {
                    record.QualityControlResultDate = new DateTime(record.QualityControlResultDate.Value.Year, record.QualityControlResultDate.Value.Month, record.QualityControlResultDate.Value.Day,
                                                      recordViewModel.QualityControlResultTime.Value.Hour, recordViewModel.QualityControlResultTime.Value.Minute, 0);

                }

                var HasFail = false;
                // Set date for CrateDate Entity
                if (record.GetType().GetProperty("ModifyDate") != null)
                    record.GetType().GetProperty("ModifyDate").SetValue(record, DateTime.Now);
                if (await this.repository.UpdateAsync(record, key) == null)
                    return BadRequest();
                else
                {
                    if (recordViewModel.QualityHasMasterLists != null)
                    {
                        foreach (var item in recordViewModel.QualityHasMasterLists)
                        {
                            if (item == null)
                                continue;

                            var HasData = await this.repositoryRequireHasMaster
                                .GetFirstOrDefaultAsync(x => x,x => x.RequireHasMasterProjectId == item.RequireHasMasterProjectId);
                            if (HasData != null)
                            {
                                HasData.PassQuantity = item.PassQuantity;
                                HasData.ModifyDate = DateTime.Now;
                                HasData.Modifyer = record.Creator;
                                //Update
                                HasFail = HasData.PassQuantity != HasData.Quantity;
                                await this.repositoryRequireHasMaster.UpdateAsync(HasData, HasData.RequireHasMasterProjectId);
                            }
                        }

                        // Update Require qualitycontrol
                        await this.RequireQualityControlUpdate(record, !HasFail, record.Creator);
                        if (record.RequireQualityControl != null)
                            record.RequireQualityControl = null;
                    }

                    // Status
                    record.QualityControlStatus = HasFail ? QualityControlStatus.Failed : QualityControlStatus.Approved;
                    await this.repository.UpdateAsync(record, key);
                }
                return new JsonResult(record, this.DefaultJsonSettings);
            }
            else
                return BadRequest(ModelState);
        }

        #region Report

        [HttpGet("QuailtyControlReport")]
        public async Task<IActionResult> QuailtyControlReport(int key,string type = "RequireQc")
        {
            if (key > 0)
            {
                var RequireQC = new RequireQualityControl();
                if (type == "RequireQc")
                    RequireQC = await this.repositoryRequireQualityControl.GetAsync(key, true);
                else
                {
                    var QuailtyControlData = await this.repository.GetAsync(key, true);
                    RequireQC = QuailtyControlData.RequireQualityControl;
                }

                if (RequireQC != null)
                {
                    var Project = await this.repositoryProject.GetAsync(RequireQC.ProjectCodeDetailId ?? 0, true);
                    var workActivities = (await this.repositoryWorkActivity.GetAllAsync()).ToList();

                    var WorkActivitiesCheck = new List<WorkActivitiesCheckViewModel>();
                    var WorkActivityCheck = new WorkActivitiesCheckViewModel();
                    var Index = 0;
                    var Count = 0;
                    foreach (var itemWork in workActivities)
                    {
                        var Already = false;
                        if (Index % 4 == 0)
                        {
                            WorkActivityCheck = new WorkActivitiesCheckViewModel();
                            Index = 0;
                        }

                        if (Index == 0)
                        {
                            WorkActivityCheck.InspectionCheck1 = RequireQC.RequireQcMoreWorkActvities.Any(x => x.WorkActivityId == itemWork.WorkActivityId) ? "1" : "";
                            WorkActivityCheck.InspectionName1 = itemWork.Name;
                        }
                        else if (Index == 1)
                        {
                            WorkActivityCheck.InspectionCheck2 = RequireQC.RequireQcMoreWorkActvities.Any(x => x.WorkActivityId == itemWork.WorkActivityId) ? "1" : "";
                            WorkActivityCheck.InspectionName2 = itemWork.Name;
                        }
                        else if (Index == 2)
                        {
                            WorkActivityCheck.InspectionCheck3 = RequireQC.RequireQcMoreWorkActvities.Any(x => x.WorkActivityId == itemWork.WorkActivityId) ? "1" : "";
                            WorkActivityCheck.InspectionName3 = itemWork.Name;
                        }
                        else if (Index == 3)
                        {
                            WorkActivityCheck.InspectionCheck4 = RequireQC.RequireQcMoreWorkActvities.Any(x => x.WorkActivityId == itemWork.WorkActivityId) ? "1" : "";
                            WorkActivityCheck.InspectionName4 = itemWork.Name;
                            Already = true;
                            WorkActivitiesCheck.Add(new WorkActivitiesCheckViewModel()
                            {
                                InspectionCheck1 = WorkActivityCheck.InspectionCheck1,
                                InspectionCheck2 = WorkActivityCheck.InspectionCheck2,
                                InspectionCheck3 = WorkActivityCheck.InspectionCheck3,
                                InspectionCheck4 = WorkActivityCheck.InspectionCheck4,
                                InspectionName1 = WorkActivityCheck.InspectionName1,
                                InspectionName2 = WorkActivityCheck.InspectionName2,
                                InspectionName3 = WorkActivityCheck.InspectionName3,
                                InspectionName4 = WorkActivityCheck.InspectionName4
                            });
                        }
                        Index++;
                        Count++;

                        if (Count >= workActivities.Count())
                        {
                            if (!Already)
                            {
                                WorkActivitiesCheck.Add(new WorkActivitiesCheckViewModel()
                                {
                                    InspectionCheck1 = WorkActivityCheck.InspectionCheck1,
                                    InspectionCheck2 = WorkActivityCheck.InspectionCheck2,
                                    InspectionCheck3 = WorkActivityCheck.InspectionCheck3,
                                    InspectionCheck4 = WorkActivityCheck.InspectionCheck4,
                                    InspectionName1 = WorkActivityCheck.InspectionName1,
                                    InspectionName2 = WorkActivityCheck.InspectionName2,
                                    InspectionName3 = WorkActivityCheck.InspectionName3,
                                    InspectionName4 = WorkActivityCheck.InspectionName4
                                });
                            }
                        }
                    }

                    var MarkNos = (await this.repositoryRequireHasMaster.GetAllAsQueryable()
                                            .Where(x => x.RequireQualityControlId == RequireQC.RequireQualityControlId)
                                            .ToListAsync())
                                            .Select((item, index) => new
                                            {
                                                RowNumber = index + 1,
                                                item.MasterProjectList.DrawingNo,
                                                item.MasterProjectList.MarkNo,
                                                item.MasterProjectList.Name,
                                                item.MasterProjectList.Box,
                                                item.MasterProjectList.UnitNo,
                                                item.Quantity,
                                                TestResult = item.PassQuantity,
                                                Remark = item.QualityControlReason != null ? item.QualityControlReason.Name : ""
                                            }).ToList();

                    var workActivityId = RequireQC?.RequireQcMoreWorkActvities.Select(x => x.WorkActivityId).ToList();
                    var WorkShop = (await this.repositoryWorkShop.GetAllAsQueryable()
                                              .FirstOrDefaultAsync(x => x.GroupMis == RequireQC.GroupMIS));
                    var locationInfo = "";
                    if (RequireQC.Branch != null)
                    {
                        if (RequireQC.Branch.Name.IndexOf("Subcontract") != -1)
                        {
                            var groupName = await this.repositoryGroupMis.GetAsync(RequireQC.GroupMIS);
                            locationInfo = groupName != null ? groupName.GroupDesc : (WorkShop != null ? WorkShop.LocationQualityControl.Name : "-");
                        }
                        else
                            locationInfo = WorkShop != null ? WorkShop.LocationQualityControl.Name : "-";
                    }

                    var HasDataReport = new
                    {
                        JobNo = Project?.ProjectCodeMaster?.ProjectCode ?? "",
                        Project = $"{Project?.ProjectCodeMaster?.ProjectName ?? ""} / {Project?.ProjectCodeDetailCode ?? ""}",
                        Branch = RequireQC?.Branch?.Name ?? "",
                        RequireDate = RequireQC.RequireDate.ToString("dd/MM/yy"),
                        RequireTime = RequireQC.RequireDate.ToString("HH:mm"),
                        Location = locationInfo,
                        Inspections = WorkActivitiesCheck,
                        ListMarkNos = MarkNos,
                    };

                    if (HasDataReport != null)
                        return new JsonResult(HasDataReport, this.DefaultJsonSettings);
                }
            }

            return BadRequest(new { Error = "Not been found." });
        }

        #endregion
    }
}
