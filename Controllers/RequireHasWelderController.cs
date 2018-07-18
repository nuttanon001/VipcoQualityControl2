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
using Microsoft.EntityFrameworkCore.Query;

namespace VipcoQualityControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequireHasWelderController : GenericController<RequireHasWelder>
    {
        private readonly IRepositoryQualityControl<RequireQualityControl> repositoryRequireQc;
        private readonly IRepositoryQualityControl<WelderNoViewModel> repositoryWelderNoView;
        private readonly IRepositoryQualityControl<RequireHasMasterProject> repositoryRequireHasMaster;
        private readonly IRepositoryQualityControl<RequireQcMoreWorkActvity> repositoryRequireMoreActvity;
        private readonly IRepositoryQualityControl<WorkGroupQualityControl> repositoryQcGroup;
        //Repository Machine
        private readonly IRepositoryMachine<Employee> repositoryEmployee;
        private readonly IRepositoryMachine<ProjectCodeDetail> repositoryProjectCodeDetail;
        private readonly IRepositoryMachine<EmployeeGroupMis> repositoryGroupMis;
        //ViewRazor
        private readonly IViewRenderService viewRenderService;
        // Private 
        private readonly EmailClass EmailClass;
        private readonly Func<IQueryable<RequireQualityControl>, IIncludableQueryable<RequireQualityControl, object>> includes;

        public RequireHasWelderController(
            IRepositoryQualityControl<RequireHasWelder> repo,
            IRepositoryQualityControl<RequireQualityControl> repoRequireQc,
            IRepositoryQualityControl<WelderNoViewModel> repoWelderNoView,
            IRepositoryQualityControl<RequireHasMasterProject> repoRequireHasMaster,
            IRepositoryQualityControl<RequireQcMoreWorkActvity> reoRequireMoreActivity,
            IRepositoryQualityControl<WorkGroupQualityControl> repoQcGroup,
            IRepositoryMachine<ProjectCodeDetail> repoProjectCodeDetail,
            IRepositoryMachine<Employee> repoEmployee,
            IRepositoryMachine<EmployeeGroupMis> repoGroupMis,
            IMapper mapper,
            IViewRenderService viewRender) :
            base(repo, mapper)
        {
            //Repostory
            this.repositoryWelderNoView = repoWelderNoView;
            this.repositoryRequireQc = repoRequireQc;
            this.repositoryRequireHasMaster = repoRequireHasMaster;
            this.repositoryRequireMoreActvity = reoRequireMoreActivity;
            this.repositoryQcGroup = repoQcGroup;
            //Repository Machine
            this.repositoryEmployee = repoEmployee;
            this.repositoryProjectCodeDetail = repoProjectCodeDetail;
            this.repositoryGroupMis = repoGroupMis;
            // include
            this.includes = e => e.Include(z => z.Branch)
                                  .Include(z => z.InspectionPoint)
                                  .Include(z => z.WorkActivity)
                                  .Include(z => z.WorkGroupQualityControl);
            //Razor to string
            this.viewRenderService = viewRender;
            //Helper
            this.EmailClass = new EmailClass();
        }
        #region Private
        // Send Mail
        private async Task<bool> SendMail(RequireQualityControl HasData, int Option = 0)
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
                    var EmpName = (await this.repositoryEmployee.GetAsync(HasData.RequireEmp)).NameThai ?? "ไม่ระบุ";

                    var BodyMessage = "<body style=font-size:11pt;font-family:Tahoma>" +
                                            "<h4 style='color:steelblue;'>เมล์ฉบับนี้เป็นแจ้งเตือนจากระบบงาน VIPCO Quality control SYSTEM</h4>" +
                                            $"เรียน คุณ{EmpName}" +
                                            $"<p>เรื่อง การเปิดคำขอตรวจคุณภาพงานเลขที่ {HasData.RequireQualityNo} วันเวลาที่ร้องขอเข้าตรวจสอบ {HasData.RequireDate.ToString("dd/MM/yy HH:mm")}น.</p>" +
                                            $"<p style='color:blue;'><b>ณ.ขณะนี้หน่วยงานเชื่อม ได้ทำการตรวจ Visual Examination(VT) และได้ดำเนินการเรียกตรวจ NDE</b></p>" +
                                            $"<p>กับทางหน่วยงานควบคุมคุณภาพ ในวันที่ <b>{HasData.WelderDate.Value.ToString("dd/MM/yy HH:mm")}น.</b>" +
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
                var QcGroup = await this.repositoryQcGroup.GetAsync(HasData.WorkGroupQualityControlId.Value);
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
                                $"<p>เรื่อง ระบบตรวจพบการร้องยื่นคำร้องขอตรวจสอบคุณภาพ NDE</p>" +
                                $"<p>เอกสารงานเลขที่ {HasData.RequireQualityNo} วันเวลาที่ร้องขอเข้าตรวจสอบ {HasData.WelderDate.Value.ToString("dd/MM/yy HH:mm")}น.</p>" +
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
            return false;
        }
        private async Task<bool> SendMail2(RequireQualityControl HasData, bool ShellYouCanPass)
        {
            if (HasData.RequireQualityControlId > 0)
            {
                var HasRequire = await this.repositoryRequireQc.GetFirstOrDefaultAsync(x => x,x => x.RequireQualityControlId == HasData.RequireQualityControlId);
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
                        var EmpName = (await this.repositoryEmployee.GetFirstOrDefaultAsync(x => x,x => x.EmpCode == HasRequire.RequireEmp)).NameThai ?? "ไม่ระบุ";
                        var ItemLists = await this.repositoryRequireHasMaster.GetToListAsync(
                            selector: x => new RequireHasMasterProjectViewModel()
                            {
                                DrawingNo = x.MasterProjectList.DrawingNo,
                                MarkNoString = x.MasterProjectList.MarkNo,
                                Box = x.MasterProjectList.Box,
                                UnitNo = x.MasterProjectList.UnitNo,
                                Quantity = x.Quantity,
                                PassQuantity = x.PassQuantity,
                                QualityControlReasonString = x.QualityControlReason != null ? x.QualityControlReason.Name : "-"
                            }, predicate: x => x.RequireQualityControlId == HasData.RequireQualityControlId);

                        var EmailTemplate = new EmailTemplateViewModel()
                        {
                            ToEmployeeName = $"คุณ{EmpName}",
                            ItemLists = ItemLists,
                            LinkToApp = $"http://{Request.Host}/qualitycontrol/require-qc/link-email/{HasData.RequireQualityControlId}",
                            RequireNo = HasRequire.RequireQualityNo,
                            ResultDate = HasData.WelderDate.Value.ToString("dd MMM yy"),
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

        #endregion
        // GET: api/RequireQualityControl/GenarateFromFailRequireQualityControl/5
        [HttpGet("GenarateFromFailRequireQualityControl")]
        public async Task<IActionResult> GenarateFromFailRequireQualityControl(int key)
        {
            if (key > 0)
            {
                var HasData = await this.repositoryRequireQc.GetFirstOrDefaultAsync(
                       selector: x => x,
                       predicate: z => z.RequireQualityControlId == key,
                       orderBy: null,
                       include: this.includes);

                if (HasData != null)
                {
                    var MapItem = this.mapper.Map<RequireQualityControl, RequireQualityControlViewModel>(HasData);
                    // RequireEmpString
                    if (!string.IsNullOrEmpty(MapItem.RequireEmp))
                        MapItem.RequireEmpString = (await this.repositoryEmployee.GetFirstOrDefaultAsync
                            (x => x,x => x.EmpCode == MapItem.RequireEmp)).NameThai;
                    // ProjectCodeDetail
                    if (MapItem.ProjectCodeDetailId.HasValue)
                    {
                        var HasProject = await this.repositoryProjectCodeDetail.GetFirstOrDefaultAsync
                            (x => x, x => x.ProjectCodeDetailId == (MapItem.ProjectCodeDetailId ?? 0), null, x => x.Include(z => z.ProjectCodeMaster));

                        MapItem.ProjectCodeDetailString = HasProject != null ? $"{HasProject.ProjectCodeMaster.ProjectCode}/{HasProject.ProjectCodeDetailCode}" : "-";
                    }
                    // GroupMIS
                    if (!string.IsNullOrEmpty(MapItem.GroupMIS))
                        MapItem.GroupMISString = (await this.repositoryGroupMis.GetFirstOrDefaultAsync(x => x,x => x.GroupMis == MapItem.GroupMIS)).GroupDesc;

                    var id = HasData.ParentRequireQcId != null ? HasData.ParentRequireQcId : HasData.RequireQualityControlId;
                    var RunNumber = (await this.repositoryRequireQc.GetLengthWithAsync(x => x.RequireDate.Year == HasData.RequireDate.Year &&
                                                                                            x.ParentRequireQcId == id)) + 1;
                    var RequireQualityNo = HasData.RequireQualityNo.IndexOf("/Rev") != -1 ?
                                           HasData.RequireQualityNo.Remove(HasData.RequireQualityNo.Length - 2, 2) + RunNumber.ToString("00") :
                                           $"{HasData.RequireQualityNo}/Rev{RunNumber.ToString("00")}";

                    var GenarateData = new RequireQualityControlViewModel()
                    {
                        RequireQualityNo = RequireQualityNo,
                        RequireDate = HasData.RequireDate,
                        BranchId = HasData.BranchId,
                        Description = HasData.Description,
                        GroupMIS = HasData.GroupMIS,
                        RequireEmp = HasData.RequireEmp,
                        InspectionPointId = HasData.InspectionPointId,
                        MailReply = HasData.MailReply,
                        ParentRequireQcId = id,
                        ProjectCodeDetailId = HasData.ProjectCodeDetailId,
                        RequireStatus = RequireStatus.WeldingReq,
                        WorkActivityId = HasData.WorkActivityId,
                        WorkGroupQualityControlId = HasData.WorkGroupQualityControlId,
                        //ViewModel
                        RequireEmpString = MapItem.RequireEmpString,
                        GroupMISString = MapItem.GroupMISString,
                        ProjectCodeDetailString = MapItem.ProjectCodeDetailString,
                        LocationQualityControlString = MapItem.LocationQualityControlString,
                    };

                    var HasDataMasterList = await this.repositoryRequireHasMaster.GetToListAsync(
                        selector:x => x,
                        predicate:x => x.RequireQualityControlId == MapItem.RequireQualityControlId,
                        include:x => x.Include(z => z.MasterProjectList).Include(z => z.RequireHasWelder));

                    if (HasDataMasterList != null)
                    {
                        foreach (var item in HasDataMasterList)
                        {
                            if (item.RequireHasWelder != null)
                                item.RequireHasWelder.RequireHasMasterProject = null;

                            if (item.Quantity - item.PassQuantity > 0)
                            {
                                if (GenarateData.RequireHasMasterProjects == null)
                                    GenarateData.RequireHasMasterProjects = new List<RequireHasMasterProject>();

                                GenarateData.RequireHasMasterProjects.Add(new RequireHasMasterProject()
                                {
                                    MasterProjectList = item.MasterProjectList,
                                    MasterProjectListId = item.MasterProjectListId,
                                    Quantity = item.Quantity - item.PassQuantity,
                                    RequireHasWelder = new RequireHasWelder()
                                    {
                                        VTStaus = item.RequireHasWelder.VTStaus,
                                        PercentNDE = item.RequireHasWelder.PercentNDE,
                                        WelderDate = item.RequireHasWelder.WelderDate,
                                        WelderNo1Id = item.RequireHasWelder.WelderNo1Id,
                                        WelderNo1Name = item.RequireHasWelder.WelderNo1Name,
                                        WelderNo2Id = item.RequireHasWelder.WelderNo2Id,
                                        WelderNo2Name = item.RequireHasWelder.WelderNo2Name,
                                        WelderProcess = item.RequireHasWelder.WelderProcess,
                                        Remark = item.RequireHasWelder.Remark,
                                    }
                                });
                            }
                        }
                    }

                    return new JsonResult(GenarateData, this.DefaultJsonSettings);
                }
            }
            return BadRequest(new { Error = "Key not been found." });
        }

        // GET:  api/RequireHasWelder/GetMaster
        [HttpGet("GetByMaster")]
        public async Task<IActionResult> GetByMaster(int key)
        {
            if (key > 0)
            {
                var hasData = await this.repositoryRequireHasMaster.GetToListAsync(
                    selector: s => s,
                    predicate: p => p.RequireQualityControlId == key,
                    include: z => z.Include(x => x.RequireHasWelder).Include(x => x.MasterProjectList));

                return new JsonResult(hasData, this.DefaultJsonSettings);
            }
            return BadRequest(new { error = "Not been found data." });
        }

        // POST: api/RequireHasWelder/GetScroll
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

            var QueryData = await this.repositoryRequireQc.GetToListAsync(
                                    selector: selected => selected,  // Selected
                                    predicate: predicate, // Where
                                    orderBy: order, // Order
                                    include: this.includes, // Include
                                    skip: Scroll.Skip ?? 0, // Skip
                                    take: Scroll.Take ?? 10); // Take

            // Get TotalRow
            Scroll.TotalRow = await this.repositoryRequireQc.GetLengthWithAsync(predicate: predicate);

            var mapDatas = new List<RequireQualityControlViewModel>();
            foreach (var item in QueryData)
            {
                var MapItem = this.mapper.Map<RequireQualityControl, RequireQualityControlViewModel>(item);
                mapDatas.Add(MapItem);
            }

            return new JsonResult(new ScrollDataViewModel<RequireQualityControlViewModel>(Scroll, mapDatas), this.DefaultJsonSettings);
        }

        // POST: api/RequireHasWelder/GetScroll
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
            predicate = predicate.And(p => p.RequireStatus == RequireStatus.WeldingFail);
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

            var QueryData = await this.repositoryRequireQc.GetToListAsync(
                                    selector: selected => selected,  // Selected
                                    predicate: predicate, // Where
                                    orderBy: order, // Order
                                    include: this.includes, // Include
                                    skip: Scroll.Skip ?? 0, // Skip
                                    take: Scroll.Take ?? 10); // Take

            // Get TotalRow
            Scroll.TotalRow = await this.repositoryRequireQc.GetLengthWithAsync(predicate: predicate);

            var mapDatas = new List<RequireQualityControlViewModel>();
            foreach (var item in QueryData)
            {
                var MapItem = this.mapper.Map<RequireQualityControl, RequireQualityControlViewModel>(item);
                mapDatas.Add(MapItem);
            }

            return new JsonResult(new ScrollDataViewModel<RequireQualityControlViewModel>(Scroll, mapDatas), this.DefaultJsonSettings);
        }

        // POST: api/RequireQualityControl/CreateV2/
        [HttpPost("CreateV2")]
        public async Task<IActionResult> CreateV2([FromBody] RequireQualityControlViewModel recordViewModel)
        {
            // Set date for CrateDate Entity
            if (recordViewModel == null)
                return BadRequest();
            var record = this.mapper.Map<RequireQualityControlViewModel, RequireQualityControl>(recordViewModel);
            // +7 Hour
            var helpRequireQc = new Helper.HelpersClass<RequireQualityControl>();
            record = helpRequireQc.AddHourMethod(record);
            record.RequireStatus = RequireStatus.WeldingReq;
            if (recordViewModel.WelderTime.HasValue)
            {
                record.WelderDate = new DateTime(record.WelderDate.Value.Year, record.WelderDate.Value.Month, record.WelderDate.Value.Day,
                                                  recordViewModel.WelderTime.Value.Hour, recordViewModel.WelderTime.Value.Minute, 0);

                if (record.RequireStatus == RequireStatus.Welding)
                {
                    if (record.WelderDate <= DateTime.Now.AddHours(1))
                        record.WelderDate = DateTime.Now.AddHours(1);
                }
            }
           
            record.CreateDate = DateTime.Now;
            if (record.RequireHasMasterProjects == null)
                record.RequireHasMasterProjects = new List<RequireHasMasterProject>();
            // Set ItemMainHasEmployees
            if (recordViewModel.RequireQcWelder != null)
            {
                foreach (var item in recordViewModel.RequireQcWelder)
                {
                    if (item == null)
                        continue;

                    item.CreateDate = record.CreateDate;
                    item.Creator = record.Creator;

                    if (item.RequireHasMasterProject != null)
                    {
                        item.RequireHasMasterProject.CreateDate = record.CreateDate;
                        item.RequireHasMasterProject.Creator = record.Creator;

                        record.RequireHasMasterProjects.Add(new RequireHasMasterProject()
                        {
                            CreateDate = record.CreateDate,
                            Creator = record.Creator,
                            Quantity = item.RequireHasMasterProject.Quantity,
                            MasterProjectListId = item.RequireHasMasterProject.MasterProjectListId,
                            RequireHasWelder = new RequireHasWelder()
                            {
                                CreateDate = item.CreateDate,
                                Creator = item.Creator,
                                VTStaus = item.VTStaus,
                                PercentNDE = item.PercentNDE,
                                WelderDate = item.WelderDate,
                                WelderNo1Id = item.WelderNo1Id,
                                WelderNo1Name = item.WelderNo1Name,
                                WelderNo2Id = item.WelderNo2Id,
                                WelderNo2Name = item.WelderNo2Name,
                                WelderProcess = item.WelderProcess,
                                Remark = item.Remark,
                            }
                        });
                    }
                }
            }

            if (recordViewModel.MoreWorkActvities != null)
            {
                foreach (var item in recordViewModel.MoreWorkActvities)
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

            if (await this.repositoryRequireQc.AddAsync(record) == null)
                return BadRequest();
            if (record.ParentRequireQcId.HasValue)
            {
                var ParentRequireQc = await this.repositoryRequireQc.GetFirstOrDefaultAsync
                    (x => x,x => x.RequireQualityControlId == record.ParentRequireQcId.Value);

                if (ParentRequireQc != null)
                {
                    if (ParentRequireQc.RequireStatus == RequireStatus.WeldingFail)
                    {
                        ParentRequireQc.RequireStatus = RequireStatus.Revise;
                        ParentRequireQc.ModifyDate = DateTime.Now;
                        ParentRequireQc.Modifyer = record.Creator;
                        //Update Parent
                        await this.repositoryRequireQc.UpdateAsync(ParentRequireQc, ParentRequireQc.RequireQualityControlId);
                    }
                }
                // AllChlid
                var ChlidRequireQc = await this.repositoryRequireQc.GetToListAsync
                                        (selector: x => x,
                                         predicate: x => x.ParentRequireQcId == record.ParentRequireQcId.Value &&
                                                         x.RequireQualityControlId != record.RequireQualityControlId);
                if (ChlidRequireQc != null)
                {
                    foreach (var item in ChlidRequireQc)
                    {
                        if (item.RequireStatus == RequireStatus.WeldingFail)
                        {
                            item.RequireStatus = RequireStatus.Revise;
                            item.ModifyDate = DateTime.Now;
                            item.Modifyer = record.Creator;
                            //Update Parent
                            await this.repositoryRequireQc.UpdateAsync(item, item.RequireQualityControlId);
                        }
                    }
                }
            }

            //Send mail to groupqc
            if (record.RequireStatus == RequireStatus.WeldingReq)
            {
                // mail to employee require
                await this.SendMail(record);
                // mail to work group qc
                await this.SendMail(record, 1);
            }

            return new JsonResult(record, this.DefaultJsonSettings);
        }

        // PUT: api/RequireHasWelder/UpdateV2/
        [HttpPut("UpdateV2")]
        public async Task<IActionResult> UpdateV2(int key, [FromBody] RequireQualityControlViewModel recordViewModel)
        {
            if (key < 1)
                return BadRequest();
            if (recordViewModel == null)
                return BadRequest();

            var record = this.mapper.Map<RequireQualityControlViewModel, RequireQualityControl>(recordViewModel);
            // +7 Hour
            var helpRequireQc = new Helper.HelpersClass<RequireQualityControl>();
            record = helpRequireQc.AddHourMethod(record);
            // set 
            record.RequireStatus = RequireStatus.WeldingReq;
            if (recordViewModel.WelderTime.HasValue)
            {
                record.WelderDate = new DateTime(record.WelderDate.Value.Year, record.WelderDate.Value.Month, record.WelderDate.Value.Day,
                                                  recordViewModel.WelderTime.Value.Hour, recordViewModel.WelderTime.Value.Minute, 0);

                if (record.RequireStatus == RequireStatus.Welding)
                {
                    if (record.WelderDate <= DateTime.Now.AddHours(1))
                        record.WelderDate = DateTime.Now.AddHours(1);
                }
            }

            // Set date for CrateDate Entity
            if (record.GetType().GetProperty("ModifyDate") != null)
                record.GetType().GetProperty("ModifyDate").SetValue(record, DateTime.Now);
            if (await this.repositoryRequireQc.UpdateAsync(record, key) == null)
                return BadRequest();
            else
            {
                if (recordViewModel.RequireQcWelder != null)
                {
                    // Record
                    foreach (var item in recordViewModel.RequireQcWelder.ToList())
                    {
                        var requireQcWelder = this.helper.AddHourMethod(item);
                        if (requireQcWelder.RequireHasMasterProject != null)
                            requireQcWelder.RequireHasMasterProject = null;

                        if (requireQcWelder.RequireHasWelderId > 0)
                        {
                            requireQcWelder.ModifyDate = record.ModifyDate;
                            requireQcWelder.Modifyer = record.Modifyer;
                            await this.repository.UpdateAsync(item, item.RequireHasWelderId);
                        }
                        else
                        {
                            requireQcWelder.CreateDate = record.ModifyDate;
                            requireQcWelder.Creator = record.Modifyer;
                            await this.repository.AddAsync(item);
                        }
                    }

                }

                var dbRequireMoreActivity = await this.repositoryRequireMoreActvity.GetToListAsync(
                    selector: x => x,
                    predicate: x => x.RequireQualityControlId == key);

                if (dbRequireMoreActivity != null)
                {
                    //Remove requisition if edit remove it
                    foreach (var item in dbRequireMoreActivity)
                    {
                        if (!recordViewModel.MoreWorkActvities.Any(x => x.WorkActivityId == item.WorkActivityId))
                            await this.repositoryRequireMoreActvity.DeleteAsync(item.RequireQcMoreWorkActvityId);
                    }
                }

                if (recordViewModel.MoreWorkActvities != null)
                {
                    foreach (var myitem in recordViewModel.MoreWorkActvities)
                    {
                        if (myitem == null)
                            continue;

                        var dbMoreWorkActivity = await this.repositoryRequireMoreActvity.GetFirstOrDefaultAsync(
                            selector: x => x,
                            predicate: x => x.RequireQualityControlId == record.RequireQualityControlId &&
                             x.WorkActivityId == myitem.WorkActivityId);

                        if (dbMoreWorkActivity != null)
                        {
                            dbMoreWorkActivity.ModifyDate = record.ModifyDate;
                            dbMoreWorkActivity.Modifyer = record.Modifyer;

                            await this.repositoryRequireMoreActvity.UpdateAsync(dbMoreWorkActivity, dbMoreWorkActivity.RequireQcMoreWorkActvityId);
                        }
                        else
                        {
                            await this.repositoryRequireMoreActvity.AddAsync(new RequireQcMoreWorkActvity()
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
                var ParentRequireQc = await this.repositoryRequireQc.GetFirstOrDefaultAsync(x => x, x => x.RequireQualityControlId == record.ParentRequireQcId.Value);
                if (ParentRequireQc != null)
                {
                    if (ParentRequireQc.RequireStatus == RequireStatus.QcFail)
                    {
                        ParentRequireQc.RequireStatus = RequireStatus.Revise;
                        ParentRequireQc.ModifyDate = DateTime.Now;
                        ParentRequireQc.Modifyer = record.Creator;
                        //Update Parent
                        await this.repositoryRequireQc.UpdateAsync(ParentRequireQc, ParentRequireQc.RequireQualityControlId);
                    }
                }
                // For All chlid
                var ChlidRequireQc = await this.repositoryRequireQc.GetToListAsync
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
                            await this.repositoryRequireQc.UpdateAsync(item, item.RequireQualityControlId);
                        }
                    }
                }
            }

            //Send mail to groupqc
            if (record.RequireStatus == RequireStatus.WeldingReq)
            {
                // mail to employee require
                await this.SendMail(record);
                // mail to work group qc
                await this.SendMail(record, 1);
            }

            return new JsonResult(record, this.DefaultJsonSettings);
        }
    }
}
