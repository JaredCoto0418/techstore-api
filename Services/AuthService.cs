using ApiTienda.Constants;
using ApiTienda.Database.Entities;
using ApiTienda.Dtos.Common;
using ApiTienda.Dtos.Security.Auth;
using ApiTienda.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HttpStatusCode = ApiTienda.Constants.CodigosDeEstadoHttp;

namespace ApiTienda.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<RoleEntity> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<UserEntity> _signInManager;

        public AuthService(
            UserManager<UserEntity> userManager, 
            RoleManager<RoleEntity> roleManager, 
            IConfiguration configuration,
            SignInManager<UserEntity> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        public async Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto dto)
        {
            var usuario = await _userManager.FindByEmailAsync(dto.Email);

            if (usuario == null)
            {
                return new ResponseDto<LoginResponseDto>
                {
                    Status = false,
                    StatusCode = HttpStatusCode.SOLICITUD_INCORRECTA,
                    Message = "Usuario o contraseña incorrectos."
                };
            }

            var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, dto.Password, false);

            if (!resultado.Succeeded)
            {
                return new ResponseDto<LoginResponseDto>
                {
                    Status = false,
                    StatusCode = HttpStatusCode.SOLICITUD_INCORRECTA,
                    Message = "Usuario o contraseña incorrectos."
                };
            }

            var rolesUsuario = await _userManager.GetRolesAsync(usuario);

            var claimsAuth = new List<Claim>
            {
                new Claim(ClaimTypes.Email, usuario.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id)
            };

            foreach (var rolUsuario in rolesUsuario)
            {
                claimsAuth.Add(new Claim(ClaimTypes.Role, rolUsuario));
            }

            var claveFirmaAuth = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: claimsAuth,
                signingCredentials: new SigningCredentials(claveFirmaAuth, SecurityAlgorithms.HmacSha256)
                );

            return new ResponseDto<LoginResponseDto>
            {
                Status = true,
                StatusCode = HttpStatusCode.OK,
                Data = new LoginResponseDto
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Email = usuario.Email!,
                    Roles = rolesUsuario.ToList()
                }
            };
        }
    }
} 