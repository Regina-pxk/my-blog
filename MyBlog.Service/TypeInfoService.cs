using MyBlog.IRespository;
using MyBlog.IService;
using MyBlog.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog.Service
{
    public class TypeInfoService : BaseService<TypeInfo>, ITypeInfoService
    {
        private readonly ITypeInfoRespository _iTypeInfoRespository;

        public TypeInfoService(ITypeInfoRespository iTypeInfoRespository)
        {
            base._baseRespository = iTypeInfoRespository;
            _iTypeInfoRespository = iTypeInfoRespository;
        }
    }
}
