using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using VipcoQualityControl.Models.QualityControls;

namespace VipcoQualityControl.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var Context = new QualityControlContext(
                serviceProvider.GetRequiredService<DbContextOptions<QualityControlContext>>()))
            {
                // Look for any movies.
                if (Context.Database.EnsureCreated())
                {
                    #region View
                    // WelderNo with employee
                    Context.Database.ExecuteSqlCommand(
                        @"CREATE VIEW View_WelderNo_Employee AS 
                            SELECT e.NameThai AS EmployeeString, w.*
                            FROM dbo.WelderNo AS w 
                            INNER JOIN VipcoMachineDataBase.dbo.Employee AS e 
                            ON w.EmpCode = e.EmpCode");

                    Context.Database.ExecuteSqlCommand(
                        @"CREATE VIEW View_WelderNo_Project_Employee AS 
                            SELECT	welder.*,
		                            job.ProjectCode + ' ' + job.ProjectName as ProjectCodeMasterString,
		                            wno.EmployeeString as EmployeeString
                            FROM [dbo].[WelderHasProject] as welder
                            INNER JOIN [VipcoMachineDataBase].[dbo].[ProjectCodeMaster] as job
                            ON welder.ProjectCodeMasterId = job.ProjectCodeMasterId
                            INNER JOIN [dbo].[View_WelderNo_Employee] as wno
                            ON wno.WelderNoId = welder.WelderNoId");

                    #endregion
                }

            }
        }
    }
}