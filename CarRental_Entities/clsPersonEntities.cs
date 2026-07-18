using System;
using SharedClass;

namespace CarRental_Entities
{
    public class clsPersonEntities
    {
        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; } // allows null
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public enGenderType Gender { get; set; }
        public string Email { get; set; } // allows null
        public string Phone { get; set; } // allows null
        public string Address { get; set; } // allows null
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime? EditedDate { get; set; }
        public int? EditedByUserID { get; set; }
    }
}
