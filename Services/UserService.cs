using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControllerDeAcesso.Data;
using ControllerDeAcesso.Data.DTO;
using ControllerDeAcesso.Entity.DTO;
using ControllerDeAcesso.Migrations;
using Microsoft.AspNetCore.Identity;

namespace ControllerDeAcesso.Services
{
    public class UserService
    {

        private UserManager<UserModel> _userManager;
        private SignInManager<UserModel> _signInManager;
        private TokenService _tokenService;

        public UserService(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<IdentityResult> CreateUser(CreateUserDto userDto)
        {
            UserModel userModel = new UserModel
            {
                UserName = userDto.UserName,
                BirthDay = userDto.BirthDay
            };

            IdentityResult result = await _userManager.CreateAsync(userModel, userDto.Password);

            return result;
        }

        public async Task<string> Login(LoginDto loginDto)
        {


            var result = await _signInManager.PasswordSignInAsync(loginDto.UserName, loginDto.Password, false, false);


            if (!result.Succeeded)
            {
                throw new ApplicationException("Error");
            }

            var user = _signInManager.UserManager.Users.FirstOrDefault(user => user.NormalizedUserName == loginDto.UserName.ToUpper());
           var token = _tokenService.GenerateToken(user);


            return token;

        }
    }

    
}