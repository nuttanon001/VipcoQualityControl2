using AutoMapper;
using VipcoQualityControl.Models.Machines;
using VipcoQualityControl.Models.QualityControls;

using VipcoQualityControl.ViewModels;

namespace VipcoQualityControl.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region WorkGroupQc
            CreateMap<WorkGroupQualityControl, WorkGroupQualityControlViewModel>()
                .ForMember(x => x.RequireQualityControls, o => o.Ignore());
            #endregion

            #region WorkActivity
            CreateMap<WorkActivity, WorkActivityViewModel>();
            #endregion

            #region WorkGroupHasWorkShop

            CreateMap<WorkGroupHasWorkShop, WorkGroupHasWorkShopViewModel>()
                .ForMember(x => x.LocationQualityControl, o => o.Ignore());

            #endregion

            #region LocationQC
            CreateMap<LocationQualityControl, LocationQualityControlViewModel>()
                .ForMember(x => x.WorkGroupHasWorkShops, o => o.Ignore());
            #endregion

            #region InspectionPoint
            CreateMap<InspectionPoint, InspectionPointViewModel>();
            #endregion

            #region MasterProjectList
            CreateMap<MasterProjectList, MasterProjectListViewModel>();
            #endregion

            #region RequireQuality
            CreateMap<RequireQualityControl, RequireQualityControlViewModel>()
                .ForMember(x => x.BranchString,
                            o => o.MapFrom(s => s.Branch == null ? "-" : s.Branch.Name))
                .ForMember(x => x.Branch, o => o.Ignore())
                .ForMember(x => x.InspectionPointString,
                            o => o.MapFrom(s => s.InspectionPoint == null ? "-" :  s.InspectionPoint.Name))
                .ForMember(x => x.InspectionPoint, o => o.Ignore())
                .ForMember(x => x.RequireStatusString, o => o.MapFrom(s => System.Enum.GetName(typeof(RequireStatus), s.RequireStatus)))
                .ForMember(x => x.WorkActivityString,
                            o => o.MapFrom(s => s.WorkActivity == null ? "-" : s.WorkActivity.Name))
                .ForMember(x => x.WorkActivity, o => o.Ignore())
                .ForMember(x => x.WorkGroupQualityControlString,
                            o => o.MapFrom(s => s.WorkGroupQualityControl == null ? "-" : s.WorkGroupQualityControl.Name))
                .ForMember(x => x.RequireHasMasterProjects, o => o.Ignore())
                .ForMember(x => x.QualityControlResult, o => o.Ignore())
                .ForMember(x => x.ParentRequireQc,o => o.Ignore())
                .ForMember(x => x.WorkQualityControl,o => o.Ignore())
                .ForMember(x => x.RequireHasAttaches,o => o.Ignore())
                .ForMember(x => x.RequireQcMoreWorkActvities,o => o.Ignore())
                .ForMember(x => x.WorkGroupQualityControl, o => o.Ignore());
            CreateMap<RequireQualityControlViewModel, RequireQualityControl>();
            #endregion

            #region RequireHasMasterProject

            CreateMap<RequireHasMasterProject, RequireHasMasterProjectViewModel>()
                .ForMember(x => x.RequireQualityControl,o => o.Ignore());

            #endregion

            #region User

            //User
            CreateMap<User, UserViewModel>()
                // CuttingPlanNo
                .ForMember(x => x.NameThai,
                           o => o.MapFrom(s => s.EmpCodeNavigation == null ? "-" : $"คุณ{s.EmpCodeNavigation.NameThai}"))
                .ForMember(x => x.EmpCodeNavigation, o => o.Ignore());

            #endregion User

            #region Employee

            //Employee
            CreateMap<Employee, EmployeeViewModel>()
                .ForMember(x => x.User, o => o.Ignore())
                .ForMember(x => x.GroupMisNavigation, o => o.Ignore());

            #endregion

            #region EmployeeGroupMis

            CreateMap<EmployeeGroupMis, GroupMisViewModel>()
                .ForMember(x => x.Employee, o => o.Ignore());

            #endregion

            #region ProjectCodeMaster

            CreateMap<ProjectCodeMaster, ProjectMasterViewModel>()
                .ForMember(x => x.ProjectCodeDetail, o => o.Ignore());

            #endregion

            #region ProjectCodeDetail

            CreateMap<ProjectCodeDetail, ProjectDetailViewModel>()
                .ForMember(x => x.ProjectCodeMaster, o => o.Ignore());

            #endregion

            #region QualityControlReasons

            CreateMap<QualityControlReason, QualityControlReasonViewModel>()
                .ForMember(x => x.RequireHasMasterProjects, o => o.Ignore());

            #endregion

            #region QualityControlResult

            CreateMap<QualityControlResult, QualityControlResultViewModel>()
                .ForMember(x => x.RequireQualityControlNo,
                            o => o.MapFrom(s => s.RequireQualityControl == null ? "NoData" : s.RequireQualityControl.RequireQualityNo))
                .ForMember(x => x.WorkGroupQualityControlString,
                            o => o.MapFrom(s => s.RequireQualityControl == null ? "NoData" : s.RequireQualityControl.WorkGroupQualityControl.Name))
                .ForMember(x => x.QualityControlStatusString, o => o.MapFrom(s => System.Enum.GetName(typeof(QualityControlStatus), s.QualityControlStatus)))
                .ForMember(x => x.RequireQualityControl,o => o.Ignore());

            #endregion

            #region QualityControlWelding

            CreateMap<QualityControlWelding, QualityControlWeldingViewModel>()
                .ForMember(x => x.ParentQcWelding, o => o.Ignore())
                .ForMember(x => x.QualityControlReason, o => o.Ignore())
                .ForMember(x => x.RequireQualityControlNo,o => 
                                o.MapFrom(s => s.RequireQualityControl == null ? "-" : s.RequireQualityControl.RequireQualityNo))
                .ForMember(x => x.RequireQualityControl, o => o.Ignore());

            #endregion

            #region RequireQcMoreWorkActvity

            CreateMap<RequireQcMoreWorkActvity, RequireQcMoreWorkActvityViewModel>()
                .ForMember(x => x.WorkActivityName, o => o.MapFrom(s => s.WorkActivity.Name))
                .ForMember(x => x.RequireQualityControl, o => o.Ignore())
                .ForMember(x => x.WorkActivity, o => o.Ignore());

            #endregion

            #region WelderNo
            CreateMap<WelderNo, WelderNoViewModel>();
            #endregion

            #region WelderHasProject
            CreateMap<WelderHasProject, WelderHasProjectViewModel>();
            #endregion

            #region RequireHasWelder

            CreateMap<RequireHasWelder, RequireHasWelderViewModel>()
                .ForMember(x => x.RequireHasMasterProject, o => o.Ignore());

            #endregion
        }
    }
}
