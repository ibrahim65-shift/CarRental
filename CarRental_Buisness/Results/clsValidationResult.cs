using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Results
{
    public class clsValidationResult
    {
        public List<clsValidationError> Errors { get; } = new List<clsValidationError>();
        public bool IsValid => !Errors.Any();
        public void Add(string fieldName , string message)
        {
            Errors.Add(new clsValidationError
            { 
               FieldName = fieldName,
               Message = message
            });

        }
    }
}
