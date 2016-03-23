using System;
using Botomag.DAL;
using AutoMapper;

namespace Botomag.BLL.Implementations
{
    /// <summary>
    /// Base service class for all services in app
    /// </summary>
    public abstract class BaseService
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
