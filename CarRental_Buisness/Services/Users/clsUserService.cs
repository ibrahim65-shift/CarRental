using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.Maintenance;
using CarRental_Buisness.Models.Users;
using CarRental_Buisness.Results;
using CarRental_Buisness.Validators;
using CarRental_DataAccess;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.Users
{
    public class clsUserService
    {
        private readonly clsUserValidator _validator = new clsUserValidator();

        public async Task<clsServiceResult<clsUserDto>> GetUserByUserIDAsync(int userID)
        {
            var entity = await clsUsersData.GetUserByUserIDAsync(userID);
            if (entity == null)
                return clsServiceResult<clsUserDto>.Fail("المستخدم غير موجود");

            return clsServiceResult<clsUserDto>.OK(clsUserMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsUserDto>> GetUserByPersonIDAsync(int personID)
        {
            var entity = await clsUsersData.GetUserByPersonIDAsync(personID);
            if (entity == null)
                return clsServiceResult<clsUserDto>.Fail("المستخدم غير موجود");

            return clsServiceResult<clsUserDto>.OK(clsUserMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsUserDto>> GetUserByUserNameAsync(string userName)
        {
            var entity = await clsUsersData.GetUserByUserNameAsync(userName);
            if (entity == null)
                return clsServiceResult<clsUserDto>.Fail("المستخدم غير موجود");

            return clsServiceResult<clsUserDto>.OK(clsUserMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<List<clsUserDto>>> GetUsersByRoleIDAsync(int roleID)
        {
            var entities = await clsUsersData.GetUsersByRoleIDAsync(roleID);
            if (entities == null)
                return clsServiceResult<List<clsUserDto>>.Fail("حدث خطأ أثناء جلب البيانات");

            if (!entities.Any())
                return clsServiceResult<List<clsUserDto>>.OK(new List<clsUserDto>());

            var list = entities.Select(e=>clsUserMapper.ToDto(e)).ToList();
            return clsServiceResult<List<clsUserDto>>.OK(list);
        }
        public async Task<clsServiceResult<clsPagedResult<DataTable>>> GetUsersPageAsync
            (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            var (dt,totalPages) = await clsUsersData.GetUsersPageAsync(PageNumber, PageSize, FilterColumn , FilterValue);
            if (dt.Rows.Count == 0 || dt==null)
                return clsServiceResult<clsPagedResult<DataTable>>.Fail("لايوجد مستخدمين");

            var result = new clsPagedResult<DataTable>
            {
                Data = dt,
                TotalPages = totalPages
            };

            return clsServiceResult<clsPagedResult<DataTable>>.OK(result);
        }
        public async Task<clsServiceResult<clsUserDto>> AddNewAsync(clsUserAddNewModel model)
        {
            var validation = await _validator.ValidateAddNewAsync(model);

            if (!validation.IsValid)
                return clsServiceResult<clsUserDto>.Invalid(validation);

            var entity = new clsUsersEntities
            {
                PersonID = model.PersonID,
                RoleID = model.RoleID,
                UserName = model.UserName,
                Password = clsSecurity.EncryptedPassword(model.Password),
                IsActive = true,
                IsDeleted= false,
                IsLockedOut = false,
                FailedLoginAttempts= 0,
                LastFailedLoginDate = null,
                CreatedDate = DateTime.Now,
                CreatedByUserID =clsCurrentUser.UserID,
                EditedDate = null,
                EditedByUserID =null
            };

            var newID = await clsUsersData.AddNewAsync(entity);
            if (newID == null)
                return clsServiceResult<clsUserDto>.Fail("فشل إضافة المستخدم");

            entity.UserID = newID.Value;
            return clsServiceResult<clsUserDto>.OK(clsUserMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsUserDto>> UpdateAsync(int userID , clsUserUpdateModel model)
        {
            var validation =  _validator.ValidateUpdateAsync(model);
            if (!validation.IsValid)
                return clsServiceResult<clsUserDto>.Invalid(validation);

            var entity = await clsUsersData.GetUserByUserIDAsync(userID);
            if (entity == null)
                return clsServiceResult<clsUserDto>.Fail("المستخدم غير موجود");

            entity.UserName = model.UserName;
            entity.RoleID = model.RoleID;
            entity.IsActive = model.IsActive;
            entity.EditedDate= DateTime.Now;
            entity.EditedByUserID = clsCurrentUser.UserID;

            bool update = await clsUsersData.UpdateAsync(entity);
            if (!update)
                return clsServiceResult<clsUserDto>.Fail("فشل تحديث بيانات المستخدم");

            return clsServiceResult<clsUserDto>.OK(clsUserMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<bool>> DeleteAsync(int userID)
        {
            bool deleted = await clsUsersData.DeleteAsync(userID);

            return deleted ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل حذف المستخدم");
        }
        public async Task<clsServiceResult<bool>> UpdateUsePasswordAsync(int userID,string password)
        {
           bool update = await clsUsersData.UpdateUserPasswordAsync(userID,clsSecurity.EncryptedPassword(password));

            return update ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل تحديث كلمة السر");
        }
    }
}
