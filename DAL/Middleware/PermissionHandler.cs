using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using DAL.ViewModel;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly PizzaShopContext _context;

    public PermissionHandler(IHttpContextAccessor httpContextAccessor, PizzaShopContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var user = _httpContextAccessor.HttpContext.User;

        if (user == null)
        {
            Console.WriteLine("❌ No user found in context.");
            return Task.CompletedTask;
        }

        var roleClaim = user.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
        if (roleClaim == null)
        {
            Console.WriteLine("❌ Role claim not found in user claims.");
            return Task.CompletedTask;
        }

        Console.WriteLine($"🔍 User Role: {roleClaim}");

        // 🔥 Fetch user permissions
        var userPermissions = GetPermissionsForRole(roleClaim);

        Console.WriteLine($"🟢 Permissions found for role {roleClaim}: {userPermissions.Count}");

        // 🔥 Log all permissions fetched
        foreach (var perm in userPermissions)
        {
            Console.WriteLine($"🔎 Fetched Permission: {perm.PermissionName} | CanView: {perm.CanView} | CanAddEdit: {perm.CanAddEdit} | CanDelete: {perm.CanDelete}");
        }

        // 🔥 Check if user has the required permission based on action type
        if (requirement.Permission.EndsWith("_View"))
        {
            if (userPermissions.Any(p => p.PermissionName == requirement.Permission.Replace("_View", "") && p.CanView == true))
            {
                Console.WriteLine($"✅ Permission granted to VIEW: {requirement.Permission}");
                context.Succeed(requirement);
            }
        }
        else if (requirement.Permission.EndsWith("_Edit"))
        {
            if (userPermissions.Any(p => p.PermissionName == requirement.Permission.Replace("_Edit", "") && p.CanAddEdit == true))
            {
                Console.WriteLine($"✅ Permission granted to EDIT: {requirement.Permission}");
                context.Succeed(requirement);
            }
        }
        else if (requirement.Permission.EndsWith("_Delete"))
        {
            if (userPermissions.Any(p => p.PermissionName == requirement.Permission.Replace("_Delete", "") && p.CanDelete == true))
            {
                Console.WriteLine($"✅ Permission granted to DELETE: {requirement.Permission}");
                context.Succeed(requirement);
            }
        }
        else
        {
            Console.WriteLine($"❌ Permission denied for: {requirement.Permission}");
        }

        return Task.CompletedTask;
    }

    private List<PermissionVM> GetPermissionsForRole(string role)
    {
        var permissions = _context.Permissions
            .Include(p => p.Role)
            .Where(p => p.Role != null && p.Role.RoleName == role)
            .Select(p => new PermissionVM
            {
                RoleId = p.RoleId,
                PermissionId = p.PermissionId,
                PermissionName = p.PermissionName,
                CanView = p.CanView,
                CanAddEdit = p.CanAddEdit,
                CanDelete = p.CanDelete
            })
            .ToList();

        Console.WriteLine($"🟢 Permissions found for role {role}: {permissions.Count}");

        return permissions;
    }
}
