using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.People;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.People
{
    public class clsPersonValidator
    {
        public async Task<clsValidationResult> ValidateAddNewAsync(clsPersonAddNewModel model)
        {
            var list = new clsValidationResult();

            if(string.IsNullOrWhiteSpace(model.NationalNo))
            {
                list.Add("NationalNo", "رقم الهوية لايمكن أن يكون فارغ");
            }
            else if (model.NationalNo.Length>20)
            {
                list.Add("NationalNo", "رقم الهوية تجاوز الحد المسموح به");
            }

            if(!string.IsNullOrWhiteSpace(model.NationalNo))
            {
                if (await clsPersonData.IsNationalNoExistsAsync(model.NationalNo))
                {
                    list.Add("NationalNo", "هذه الهوية موجود بالفعل");
                }
            }

            if(model.BirthDate==null)
            {
                list.Add("BirthDate", "يرجى إدخال تاريخ الميلاد");
            }

            if(model.BirthDate.Date > DateTime.Today.AddYears(-18))
            {
                list.Add("BirthDate", "العمر يجب أن يكون 18 سنة أو أكبر");
            }
    
            if (string.IsNullOrWhiteSpace(model.FirstName))
            {
                list.Add("FirstName", "الاسم الأول لايمكن أن يكون فارغ");
            }
            else if (model.FirstName.Length>100)
            {
                list.Add("FirstName", "الاسم الأول تجاوز الحد المسموح به");
            }


            if(string.IsNullOrWhiteSpace(model.SecondName))
            {
                list.Add("SecondName", "الاسم الثاني لايمكن أن يكون فارغ");
            }
            else if (model.SecondName.Length>100)
            {
                list.Add("SecondName", "الاسم الثاني تجاوز الحد المسموح به");
            }

            if (!string.IsNullOrWhiteSpace(model.ThirdName))
            {
                if (model.ThirdName.Length > 100)
                {
                    list.Add("ThirdName", "الاسم الثالث تجاوز الحد المسموح به");
                }
            }

            if (string.IsNullOrWhiteSpace(model.LastName))
            {
                list.Add("LastName", "الاسم الأخير لايمكن أن يكون فارغ");
            }
            else if (model.LastName.Length>100)
            {
                list.Add("LastName", "الاسم الأخير تجاوز الحد المسموح به");
            }

            if(!string.IsNullOrWhiteSpace(model.Email))
            {
                if (model.Email.Length > 200)
                {
                    list.Add("Email", "البريد الإلكتروني تجاوز الحد المسموح به");
                }

                if (!clsUtil.IsValidEmail(model.Email))
                {
                    list.Add("Email", "البريد الإلكتروني غير صحيح");
                }

                if (await clsPersonData.IsDuplicateEmailAsync(null,model.Email))
                {
                    list.Add("Email", "هذا البريد الإلكتروني موجود بالفعل");
                }

            }

            if(!string.IsNullOrWhiteSpace(model.Phone))
            {
                if (model.Phone.Length > 20)
                {
                    list.Add("Phone", "الهاتف تجاوز الحد المسموح به");
                }

                if (await clsPersonData.IsDuplicatePhoneAsync(null,model.Phone))
                {
                    list.Add("Phone", "هذا الهاتف موجود بالفعل");
                }
            }

            if(!string.IsNullOrWhiteSpace(model.Address))
            {
                if(model.Address.Length > 250)
                {
                    list.Add("Address", "العنوان تجاوز الحد المسموح به");
                }
            }

            


            return list;
        }
        public async Task<clsValidationResult> ValidateUpdateAsync(int personID , clsPersonUpdateModel model)
        {
            var list = new clsValidationResult();

         
            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                if (model.Email.Length > 200)
                {
                    list.Add("Email", "البريد الإلكتروني تجاوز الحد المسموح به");
                }

                if (!clsUtil.IsValidEmail(model.Email))
                {
                    list.Add("Email", "البريد الإلكتروني غير صحيح");
                }

                if (await clsPersonData.IsDuplicateEmailAsync(personID, model.Email))
                {
                    list.Add("Email", "هذا البريد الإلكتروني موجود بالفعل");
                }
            }

            if (!string.IsNullOrWhiteSpace(model.Phone))
            {
                if (model.Phone.Length > 20)
                {
                    list.Add("Phone", "الهاتف تجاوز الحد المسموح به");
                }

                if (await clsPersonData.IsDuplicatePhoneAsync(personID, model.Phone))
                {
                    list.Add("Phone", "هذا الهاتف موجود بالفعل");
                }
            }

            return list;
        }
    }
}
