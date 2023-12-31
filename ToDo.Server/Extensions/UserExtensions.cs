﻿using System.Security.Claims;

namespace Todo.Server.Extensions;

public static class UserExtensions
{
    public static Guid  GetUserAccountId(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
    
    public static string? GetUserName(this ClaimsPrincipal user)
    {
        return user.Claims.First(c => c.Type == ClaimTypes.Name).Value;
    }
}