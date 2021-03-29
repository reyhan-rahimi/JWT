using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWT.Controllers
{
    public class LoginController : Controller
    {
        private IConfiguration _config;
        public readonly User user1;
        

        public LoginController(IConfiguration config  , User user)
        {
            _config = config;
            this.user1 = user;
        }
        //به همه اجازه داده شود تا بتوانند لاگین کنند.
        [AllowAnonymous]
        [HttpPost]
        //تولید jwt
        //برای تولید جی دبلیو تی از کاربر انتظار دریافت یکسری اطلاعات هویتی دارد
        //مثل نام کاربری و ایمیل
        public IActionResult Login([FromBody] UserModel login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);
            //اگر کاربری یافت شود و برگشت داده شود ، ای پی آی یک رمز جدید تولید میکند
            if (user != null)
            {
                //تولید رمز با استفاده از این تابع است
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

       
        private string GenerateJSONWebToken(UserModel userInfo)
        {
            //برای تولید یک جی دبلیو تی از کلاس JwtSecurityToken استفاده شده است.
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        //برای تایید اعتبار یک کاربر که قصد لاگین کردن را دارد به کار میرود
        //
        private UserModel AuthenticateUser(UserModel login)
        {
            UserModel user = null;
           UserModel  user2= user1.search(login);
            //Validate the User Credentials    
            //Demo Purpose, I have Passed HardCoded User Information    
            if (user2 != null)
            {
                return user;
            }
            return null;
        }
        
    }
}
