using System;
using System.Linq;
using System.Security.Cryptography;
using AutoMapper;

using Botomag.BLL.Contracts;
using Botomag.DAL;
using Botomag.BLL.Model;
using Botomag.DAL.Model;

namespace Botomag.BLL.Implementations
{
    /// <summary>
    /// Struct for return result of createing user and info message
    /// </summary>
    public struct CreateUserMessages
    {
        public const string UserAlreadyExists = "Пользователь с Email: {0} уже существует.";

        public const string UserCreated = "Пользователь с Email: {0} успешно создан.";
    }

    /// <summary>
    /// Service for authentication and authorization features in application
    /// </summary>
    public class UserService : BaseService, IUserService
    {
        #region Properties and Fields

        #endregion Properties and Fields

        #region Constructors

        public UserService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

        #endregion Constructors

        #region Public Methods

        public bool TryCreateUser(UserModel model, out string Message)
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

            User user = userRepo.Get(n => n.Email == model.Email).FirstOrDefault();

            if (user != null)
            {
                Message = string.Format(CreateUserMessages.UserAlreadyExists, model.Email);
                return false;
            }

            model.PasswordHash = HashPassword(model.Password);

            user = Mapper.Map<User>(model);

            userRepo.Add(user);

            _unitOfWork.Save();

            Message = string.Format(CreateUserMessages.UserCreated, model.Email);

            return true;
        }

        #endregion Public Methods

        #region Private Methods

        // get from here: http://stackoverflow.com/questions/20621950/asp-net-identity-default-password-hasher-how-does-it-work-and-is-it-secure
        private string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        private bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return buffer3.SequenceEqual(buffer4);
        }

        #endregion Private Methods
    }
}
