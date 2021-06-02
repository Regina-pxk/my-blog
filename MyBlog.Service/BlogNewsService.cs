using MyBlog.IRespository;
using MyBlog.IService;
using MyBlog.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog.Service
{
    public class BlogNewsService : BaseService<BlogNews>, IBlogNewsService
    {
        private readonly IBlogNewsRespository _iblogNewsRespository;

        public BlogNewsService(IBlogNewsRespository iblogNewsRespository)
        {
            base._baseRespository = iblogNewsRespository;
            _iblogNewsRespository = iblogNewsRespository;
        }
    }
}
