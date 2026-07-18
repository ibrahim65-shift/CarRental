using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.People;
using CarRental_Buisness.Models.Users;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.People
{
    public class clsPersonService
    {
        private readonly clsPersonValidator _validator = new clsPersonValidator();

        public async Task<clsServiceResult<clsPersonDto>> GetPersonByIDAsync(int personID)
        {
            var entity = await clsPersonData.GetPersonByPersonIDAsync(personID);
            if (entity == null)
                return clsServiceResult<clsPersonDto>.Fail("الشخص غير موجود");

            return clsServiceResult<clsPersonDto>.OK(clsPersonMapper.ToDto(entity));
        }

        public async Task<clsServiceResult<clsPersonDto>> GetPersonByNationalNoAsync(string nationalNo)
        {
            var entity = await clsPersonData.GetPersonByNationalNoAsync(nationalNo);
            if (entity == null)
                return clsServiceResult<clsPersonDto>.Fail("الشخص غير موجود");

            return clsServiceResult<clsPersonDto>.OK(clsPersonMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsPagedResult<DataTable>>> GetPeoplePageAsync
             (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            var result = await clsPersonData.GetPeoplePageAsync(PageNumber, PageSize, FilterColumn, FilterValue);

            if (result.peopleData.Rows.Count == 0)
                return clsServiceResult<clsPagedResult<DataTable>>.Fail("لايوجد اشخاص");

           
            var paged = new clsPagedResult<DataTable>
            {
               Data = result.peopleData,
               TotalPages = result.TotalPages
            };


            return clsServiceResult<clsPagedResult<DataTable>>.OK(paged);
        }
        public async Task<clsServiceResult<clsPersonDto>> AddNewAsync(clsPersonAddNewModel model)
        {
            var validation = await _validator.ValidateAddNewAsync(model);
            if (!validation.IsValid)
                return clsServiceResult<clsPersonDto>.Invalid(validation);

            var entity = new clsPersonEntities
            {
                NationalNo = model.NationalNo,
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                ThirdName = model.ThirdName,
                LastName = model.LastName,
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                IsDeleted=false,
                CreatedDate = DateTime.Now,
                CreatedByUserID = clsCurrentUser.UserID,
                EditedDate=null,
                EditedByUserID= null
            };

            var newID = await clsPersonData.AddNewAsync(entity);
            if (newID == null)
                return clsServiceResult<clsPersonDto>.Fail("فشل إضافة شخص");

            entity.PersonID = newID.Value;
            return clsServiceResult<clsPersonDto>.OK(clsPersonMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsPersonDto>> UpdateAsync(int personID , clsPersonUpdateModel model)
        {
            var entity = await clsPersonData.GetPersonByPersonIDAsync(personID);
            if (entity == null)
                return clsServiceResult<clsPersonDto>.Fail("الشخص غير موجود");

            var validation = await _validator.ValidateUpdateAsync(personID,model);
            if (!validation.IsValid)
                return clsServiceResult<clsPersonDto>.Invalid(validation);

            entity.Email=model.Email;
            entity.Phone=model.Phone;
            entity.Address=model.Address;
            entity.EditedDate = DateTime.Now;
            entity.EditedByUserID = clsCurrentUser.UserID;

            bool udpate = await clsPersonData.UpdateAsync(entity);

            return udpate ? clsServiceResult<clsPersonDto>.OK(clsPersonMapper.ToDto(entity))
                : clsServiceResult<clsPersonDto>.Fail("فشل تحديث بيانات الشخص");
        }
        public async Task<clsServiceResult<bool>> DeleteAsync(int personID)
        {
            if(!await clsPersonData.PersonExists(personID))
                return clsServiceResult<bool>.Fail("الشخص غير موجود");

            bool delete = await clsPersonData.DeleteAsync(personID);
            return delete ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل حذف الشخص");
        }
        public async Task<clsServiceResult<bool>> IsCustomerAsync(int personID)
        {
            bool exists = await clsCustomersData.ExistsByPersonIDAsync(personID);

            return exists ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail($"ليس عميل{ personID} الشخص الذي يحمل الرقم التعريفي");
        }
        public async Task<clsServiceResult<bool>> IsUserAsync(int personID)
        {
            bool exists = await clsUsersData.IsPersonIDExistsAsync(personID);

            return exists ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail($"ليس مستخدم{ personID} الشخص الذي يحمل الرقم التعريفي");
        }


    }
}
