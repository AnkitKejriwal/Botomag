using System;
using Botomag.DAL;
using AutoMapper;

using Botomag.BLL.Contracts;

namespace Botomag.BLL.Implementations
{
    /// <summary>
    /// Base service class for all services in app
    /// </summary>
    public abstract class BaseService : IBaseService
    {
        protected IUnitOfWork _unitOfWork { get; private set; }
        protected IMapper _mapper { get; private set; }

        public BaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
