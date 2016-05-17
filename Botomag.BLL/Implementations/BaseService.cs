using System;
using Botomag.DAL;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using AutoMapper.QueryableExtensions;

using Botomag.BLL.Contracts;
using Botomag.DAL.Model;
using Botomag.BLL.Models;

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

        protected IEnumerable<TDestination> Get<TSource, TDestination, TKey>(
            IRepository<TSource, TKey> repo,
            Expression<Func<TSource, bool>> filter = null,
            Expression<Func<TSource, object>>[] include = null,
            params Expression<Func<TDestination, object>>[] membersToExpand) 
            where TSource : BaseEntity<TKey>
            where TDestination : BaseModel<TKey>
            where TKey : struct
        {
            if (repo == null)
            {
                throw new ArgumentNullException("repo.");
            }

            IQueryable<TSource> query = repo.Get();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (include != null)
            {
                foreach(Expression<Func<TSource, object>> inc in include)
                {
                    query = query.Include(inc);
                }
            }

            IQueryable<TDestination> mappedQuery = query.ProjectTo<TDestination>(membersToExpand);

            return mappedQuery.ToArray();
        }
    }
}
