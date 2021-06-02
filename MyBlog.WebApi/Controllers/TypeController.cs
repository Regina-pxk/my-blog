using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.IService;
using MyBlog.Model;
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
    public class TypeController : ControllerBase
    {
        private readonly ITypeInfoService _iTypeInfoService;
        public TypeController(ITypeInfoService iTypeInfoService)
        {
            _iTypeInfoService = iTypeInfoService;
        }

        [HttpGet("Types")]
        public async Task<ActionResult<ApiResult>> GetTypes()
        {
            var types = await _iTypeInfoService.QueryAsync();
            if (types == null || types.Count == 0)
            {
                return ApiResultHelper.Error("没有更多的文章类型！");
            }
            return ApiResultHelper.Success(types);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<ApiResult>> Create(string name)
        {
            #region 数据验证
            if (string.IsNullOrWhiteSpace(name))
            {
                return ApiResultHelper.Error("文章类型名name不能为空");
            }
            TypeInfo typeInfo = new TypeInfo
            {
                Name = name
            };
            bool result = await _iTypeInfoService.CreateAsync(typeInfo);
            if (!result)
            {
                return ApiResultHelper.Error("添加失败！");
            }
            return ApiResultHelper.Success(typeInfo);
            #endregion
        }
        [HttpDelete("Delete")]
        public async Task<ActionResult<ApiResult>> Delete(int id)
        {
            bool result = await _iTypeInfoService.DeleteAsync(id);

            if (!result)
            {
                return ApiResultHelper.Error("删除失败！");
            }
            return ApiResultHelper.Success("删除成功！");
        }

        [HttpPut("Edit")]
        public async Task<ActionResult<ApiResult>> Edit(int id, string name)
        {
            TypeInfo typeInfo = await _iTypeInfoService.FindAsync(id);
            if (typeInfo == null)
            {
                return ApiResultHelper.Error("未找到该文章类型！");
            }
            typeInfo.Name = name;
            bool result = await _iTypeInfoService.EditAsync(typeInfo);
            if (!result)
            {
                return ApiResultHelper.Error("修改失败！");
            }
            return ApiResultHelper.Success(typeInfo);

        }

    }
}
