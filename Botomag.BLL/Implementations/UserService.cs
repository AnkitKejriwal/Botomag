using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using AutoMapper;
using Microsoft.AspNet.Identity;

using Botomag.BLL.Contracts;
using Botomag.DAL;
using Botomag.BLL.Models;
using Botomag.DAL.Model;

namespace Botomag.BLL.Implementations
{
    /// <summary>
    /// Service for authentication features in application
    /// </summary>
    public class UserService : BaseService, IUserService
    {
        #region Properties and Fields

        /// <summary>
        /// Struct for return result of createing user and info message
        /// </summary>
        private struct CreateUserMessages
        {
            public const string UserAlreadyExists = "Пользователь с Email: {0} уже существует.";

            public const string UserCreated = "Пользователь с Email: {0} успешно создан.";
        }

        #endregion Properties and Fields

        IPasswordHasher _hasher;

        #region Constructors

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher hasher) : base(unitOfWork, mapper) 
        {
            _hasher = hasher;
        }

        #endregion Constructors

        #region Public Methods

        public Task<CreateUserResult> CreateUserAsync(UserModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model.");
            }

            if (model.Email == null || model.Password == null)
            {
                throw new ArgumentNullException("one or few of required properties are missed.");
            }

            return Task<CreateUserResult>.Factory.StartNew(() => _CreateUserAsync(model).Result);
        }

        public Task<VerifyUserResult> IsUserValidAsync(UserModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model.");
            }

            if (model.Email == null || model.Password == null)
            {
                throw new ArgumentNullException("one or few of required properties are missed.");
            }

            return Task<VerifyUserResult>.Factory.StartNew(() => _IsUserValidAsync(model).Result);
        }

        #endregion Public Methods

        #region Private Methods

        private async Task<CreateUserResult> _CreateUserAsync(UserModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model.");
            }

            if (model.Email == null || model.Password == null)
            {
                throw new ArgumentNullException("one or few of required properties are missed.");
            }

            IRepository<User, Guid> userRepo = _unitOfWork.GetRepository<User, Guid>();

            User user = await Task<User>.Factory.StartNew(() => userRepo.Get().Where(n => n.Email == model.Email).FirstOrDefault());

            string message = null;

            if (user != null)
            {
                message = string.Format(CreateUserMessages.UserAlreadyExists, model.Email);
                return new CreateUserResult(null, message, false);
            }

            model.PasswordHash = _hasher.HashPassword(model.Password);
            model.Id = Guid.NewGuid();

            user = _mapper.Map<User>(model);

            userRepo.Add(user);

            await _unitOfWork.SaveAsync();

            message = string.Format(CreateUserMessages.UserCreated, model.Email);

            model.Password = null;
            model.PasswordHash = null;

            return new CreateUserResult(model, message, true);
        }

        private async Task<VerifyUserResult> _IsUserValidAsync(UserModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model.");
            }

            if (model.Email == null || model.Password == null)
            {
                throw new ArgumentNullException("one or few of required properties are missed.");
            }

            VerifyUserResult result = new VerifyUserResult { User = model, IsValid = false };

            User user = await Task<User>.Factory.StartNew(() => _unitOfWork.GetRepository<User, Guid>().Get().
                Where(n => n.Email == model.Email).FirstOrDefault());

            if (user != null)
            {
                result.User = _mapper.Map<UserModel>(user);
                PasswordVerificationResult verification = _hasher.VerifyHashedPassword(user.PasswordHash, model.Password);
                if (verification == PasswordVerificationResult.Success || verification == PasswordVerificationResult.SuccessRehashNeeded)
                {

                    result.IsValid = true;
                }
            }

            return result;
        }

        #endregion Private Methods
    }
}
