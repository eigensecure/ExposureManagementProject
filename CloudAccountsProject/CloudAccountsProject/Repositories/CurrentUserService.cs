using CloudAccountsProject.Repositories.Contracts;

namespace CloudAccountsProject.Repositories;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string? Username =>
        _httpContextAccessor.HttpContext?
        .Request.Headers["X-Username"]
        .FirstOrDefault();
}
