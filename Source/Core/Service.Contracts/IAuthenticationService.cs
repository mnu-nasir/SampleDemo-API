﻿using Microsoft.AspNetCore.Identity;
using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
    Task<bool> ValidateUser(UserForAuthenticationDto userForAuthentication);
    Task<string> CreateToken();
    Task<TokenDto> CreateAllToken(bool populateExp);
    Task<TokenDto> RefreshToken(TokenDto tokenDto);
}
