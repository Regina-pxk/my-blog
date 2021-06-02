using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.IService;
using MyBlog.Model;
using MyBlog.Model.DTO;
using MyBlog.WebApi.Utility.ApiResult;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogNewsController : ControllerBase
    {
        private readonly IBlogNewsService _iBlogNewsService;

        public BlogNewsController(IBlogNewsService blogNewsService)
        {
            _iBlogNewsService = blogNewsService;
        }

        [HttpGet("BlogNews")]
        public async Task<ActionResult<ApiResult>> GetBlogNews()
        {
            int id = Convert.ToInt32(this.User.FindFirst("Id").Value);
            var data = await _iBlogNewsService.QueryAsync(c => c.WriterId == id);
            if (data == null || data.Count == 0)
            {
                return ApiResultHelper.Error("没有更多的文章可供展示！");
            }
            return ApiResultHelper.Success(data);
        }

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<ActionResult<ApiResult>> Create(string title, string content, int typeId)
        {
            //数据验证
            BlogNews blogNews = new BlogNews
            {
                Title = title,
                Content = content,
                BrowseCount = 0,
                LikeCount = 0,
                Time = DateTime.Now,
                TypeId = typeId,
                WriterId = Convert.ToInt32(this.User.FindFirst("Id").Value)
            };
            bool result = await _iBlogNewsService.CreateAsync(blogNews);
            if (!result)
            {
                return ApiResultHelper.Error("添加失败，服务器错误！");
            }
            return ApiResultHelper.Success(blogNews);
        }
        [HttpDelete("Delete")]
        public async Task<ActionResult<ApiResult>> Delete(int id)
        {
            bool result = await _iBlogNewsService.DeleteAsync(id);
            if (!result)
            {
                return ApiResultHelper.Error("删除失败！");
            }
            return ApiResultHelper.Success("删除成功！");
        }

        [HttpPut("Edit")]
        public async Task<ActionResult<ApiResult>> Edit(int id, string title, string content, int typeId)
        {
            BlogNews blogNews = await _iBlogNewsService.FindAsync(id);
            if (blogNews == null)
            {
                return ApiResultHelper.Error("修改失败，未找到该id对应的新闻！");
            }
            blogNews.Title = title;
            blogNews.Content = content;
            blogNews.TypeId = typeId;
            bool result = await _iBlogNewsService.EditAsync(blogNews);
            if (!result)
            {
                return ApiResultHelper.Error("修改失败！");
            }
            return ApiResultHelper.Success(blogNews);

        }

        [HttpGet("GetBlogNewsPage")]
        public async Task<ApiResult> GetBlogNewsPage([FromServices] IMapper iMapper, int page, int size)
        {
            RefAsync<int> total = 0;
            List<BlogNews> blogNews = await _iBlogNewsService.QueryAsync(page, size, total);
            try
            {
                var blogNewDTO = iMapper.Map<List<BlogNewsDTO>>(blogNews);
                return ApiResultHelper.Success(blogNewDTO, total);
            }
            catch (Exception ex)
            {
                return ApiResultHelper.Error("AutoMapper映射错误，错误原因：" + ex.Message);
            }
        }
    }
}
