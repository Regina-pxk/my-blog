using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.IService;
using MyBlog.Model;
using MyBlog.Model.DTO;
using MyBlog.WebApi.Utility._MD5;
using MyBlog.WebApi.Utility.ApiResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class WriterInfoController : ControllerBase
    {
        private readonly IWriterInfoService _iWriterInfoService;

        public WriterInfoController(IWriterInfoService writerInfoService)
        {
            _iWriterInfoService = writerInfoService;
        }

        [HttpGet("WritrInfo")]
        public async Task<ApiResult> GetWritrInfo()
        {
            var writrInfo = await _iWriterInfoService.QueryAsync();
            if (writrInfo == null || writrInfo.Count == 0)
            {
                return ApiResultHelper.Error("没有更多的用户！");
            }
            return ApiResultHelper.Success(writrInfo);
        }

        [HttpPost("Create")]
        public async Task<ApiResult> Create(string name, string username, string userpwd)
        {
            #region 数据验证
            if (string.IsNullOrWhiteSpace(name))
            {
                return ApiResultHelper.Error("文章类型名name不能为空");
            }
            WriterInfo writerInfo = new WriterInfo
            {
                Name = name,
                UserName = username,
                //加密密码
                UserPwd = MD5Helper.MD5Encrypt32(userpwd)
            };
            //判断数据库中是否已经存在与要添加的账号相同的账号数据
            var oldWriter = await _iWriterInfoService.FindAsync(c => c.UserName == username);
            if (oldWriter != null)
            {
                return ApiResultHelper.Error("账号已存在，请勿重复添加！");
            }
            bool result = await _iWriterInfoService.CreateAsync(writerInfo);
            if (!result)
            {
                return ApiResultHelper.Error("添加失败！");
            }
            return ApiResultHelper.Success(writerInfo);
            #endregion
        }

        //[HttpDelete("Delete")]
        //public async Task<ActionResult<ApiResult>> Delete(int id)
        //{
        //    bool result = await _iWriterInfoService.DeleteAsync(id);

        //    if (!result)
        //    {
        //        return ApiResultHelper.Error("删除失败！");
        //    }
        //    return ApiResultHelper.Success("删除成功！");
        //}

        [HttpPut("EditName")]
        public async Task<ActionResult<ApiResult>> Edit(string name)
        {
            int id = Convert.ToInt32(this.User.FindFirst("Id").Value);
            var writerInfo = await _iWriterInfoService.FindAsync(id);
            if (writerInfo == null)
            {
                return ApiResultHelper.Error("未查询到对应的用户！");
            }
            writerInfo.Name = name;
            bool result =await _iWriterInfoService.EditAsync(writerInfo);
            if (!result)
            {
                return ApiResultHelper.Error("修改失败！");
            }
            return ApiResultHelper.Success(writerInfo);

            //WriterInfo writerInfo = await _iWriterInfoService.FindAsync(id);
            //if (writerInfo == null)
            //{
            //    return ApiResultHelper.Error("未找到该用户！");
            //}
            //writerInfo.Name = name;
            //bool result = await _iWriterInfoService.EditAsync(writerInfo);
            //if (!result)
            //{
            //    return ApiResultHelper.Error("修改失败！");
            //}
            //return ApiResultHelper.Success(writerInfo);
        }
        [AllowAnonymous]
        [HttpGet("FindWriter")]
        public async Task<ApiResult> FindWriter([FromServices]IMapper iMapper, int id)
        {
            var writer =await _iWriterInfoService.FindAsync(id);
            if (writer == null)
            {
                return ApiResultHelper.Error("未找到该作者");
            }
            var writerDTO = iMapper.Map<WriterDTO>(writer);
            return ApiResultHelper.Success(writerDTO);
        }
        //[HttpPut("EditPwd")]
        //public async Task<ActionResult<ApiResult>> Edit(int id, string userpwd)
        //{
        //    WriterInfo writerInfo = await _iWriterInfoService.FindAsync(id);
        //    if (writerInfo == null)
        //    {
        //        return ApiResultHelper.Error("未找到该用户！");
        //    }
        //    writerInfo.UserPwd = userpwd;
        //    bool result = await _iWriterInfoService.EditAsync(writerInfo);
        //    if (!result)
        //    {
        //        return ApiResultHelper.Error("修改失败！");
        //    }
        //    return ApiResultHelper.Success(writerInfo);
        //}
    }
}
