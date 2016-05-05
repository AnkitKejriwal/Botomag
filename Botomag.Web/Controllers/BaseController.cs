using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using AutoMapper;

namespace Botomag.Web.Controllers
{
    /// <summary>
    /// Base controller for all controllers in application
    /// </summary>
    public class BaseController : Controller
    {

        #region Properties and Fields

        protected IMapper _mapper { get; private set; }

        #endregion Properties and Fields

        #region Constructors

        public BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }

        #endregion Constructors

        #region Methods

        #endregion Methods
    }
}