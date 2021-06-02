using MyBlog.IRespository;
using MyBlog.IService;
using MyBlog.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog.Service
{
    public class WriterInfoService : BaseService<WriterInfo>, IWriterInfoService
    {
        private readonly IWriterInfoRespository _iWriterInfoRespository;

        public WriterInfoService(IWriterInfoRespository iWriterInfoRespository)
        {
            base._baseRespository = iWriterInfoRespository;
            _iWriterInfoRespository = iWriterInfoRespository;
        }
    }
}
