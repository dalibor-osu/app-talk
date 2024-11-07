using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppTalk.API.DatabaseService.Interfaces;
using AppTalk.Core.Enums;
using AppTalk.Core.Helpers;
using AppTalk.Models.Convertors;
using AppTalk.Models.DataTransferObjects.Login;
using AppTalk.Models.DataTransferObjects.User;
using AppTalk.Models.Models;
using Microsoft.IdentityModel.Tokens;

namespace AppTalk.API.Managers;

public class UserManager(
    IConfiguration configuration,
    IUserDatabaseService userDatabaseService)
{
    public async Task<FullUserDto> GetFullAsync(Guid userId)
    {
        var result = await userDatabaseService.GetUserAsync(userId);
        return result?.ToFullDto();
    }

    public async Task<UserDto> GetAsync(Guid userId)
    {
        var result = await userDatabaseService.GetUserAsync(userId);
        return result?.ToDto();
    }
    
    public async Task<LoginResultDto> RegisterAsync(NewUserDto user)
    {
        bool exists = await userDatabaseService.UsernameOrEmailExistsAsync(user.Username, user.Email);

        if (exists)
        {
            return null;
        }
        
        var newUser = new User
        {
            Username = user.Username,
            Email = user.Email,
            PasswordHash = PasswordHelper.GetPasswordHash(user.Password)
        };
        
        var createdUser = await userDatabaseService.CreateUserAsync(newUser);
        
        return new LoginResultDto
        {
            Token = GenerateJwt(createdUser),
            ResultType = LoginResultType.Success,
            User = createdUser.ToFullDto()
        };
    }

    public async Task<LoginResultDto> LoginAsync(LoginDto loginDto)
    {
        var user = await userDatabaseService.GetUserAsync(loginDto.Username);

        if (user == null)
        {
            return new LoginResultDto
            {
                ResultType = LoginResultType.UserNotFound
            };
        }

        bool validPassword = PasswordHelper.ValidatePassword(loginDto.Password, user.PasswordHash);

        if (!validPassword)
        {
            return new LoginResultDto
            {
                ResultType = LoginResultType.InvalidPassword
            };
        }
        
        return new LoginResultDto
        {
            Token = GenerateJwt(user),
            ResultType = LoginResultType.Success,
            User = user.ToFullDto()
        };
    }

    public async Task<bool> ExistsAsync(Guid userId)
    {
        return await userDatabaseService.ExistsAsync(userId);
    }

    private string GenerateJwt(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);
        
        var claims = new List<Claim>() {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Issuer"],
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}