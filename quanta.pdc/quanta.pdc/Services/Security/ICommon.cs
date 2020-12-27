using cns.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cns.Services.Security
{
    public interface ICommon
    {
        Task CreateDefaultSuperAdmin();

        List<String> GetAllRoles();

        List<ApplicationUser> GetAllMembers();

        ApplicationUser GetMemberByApplicationId(string applicationId);

        Task<ApplicationUser> CreateApplicationUser(ApplicationUser applicationUser, string password);
        
    }
}
