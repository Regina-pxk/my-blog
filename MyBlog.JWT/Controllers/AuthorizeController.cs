using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyBlog.IService;
using MyBlog.JWT.Utility._MD5;
using MyBlog.JWT.Utility.ApiResult;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly IWriterInfoService _iWriterInfoService;

        public AuthorizeController(IWriterInfoService iWriterInfoService)
        {
            _iWriterInfoService = iWriterInfoService;
        }
        [HttpPost("Login")]
        public async Task<ApiResult> Login(string username, string userpwd)
        {
            //加密密码
            string pwd = MD5Helper.MD5Encrypt32(userpwd);
            var writerInfo = await _iWriterInfoService.FindAsync(c => c.UserName == username && c.UserPwd == pwd);
            if (writerInfo != null)
            {
                //登录成功
                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, "张三"),
                    new Claim("Id", writerInfo.Id.ToString()),
                    new Claim("UserName", writerInfo.UserName)
                    //不能放入敏感信息，可被反编译
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SDMC-CJAS1-SAD-DFSFA-SADHJVF-VF"));
                //issuer代表颁发Token的Web应用程序，audience是Token的受理者
                var token = new JwtSecurityToken(
                    issuer: "http://localhost:6060",//授权项目地址
                    audience: "http://localhost:5000",//webapi地址
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddHours(1),//过期时间
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return ApiResultHelper.Success(jwtToken);
            }
            else
            {
                return ApiResultHelper.Error("账号或密码错误");
            }

        }
    }
}
