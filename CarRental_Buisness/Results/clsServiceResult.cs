using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Results
{
    public class clsServiceResult<T>
    {
        public T Data { get; private set; }
        public bool Success { get; private set; }
        public clsValidationResult Validation { get; private set; }
        public string ErrorMessage { get; private set; }

        public static clsServiceResult<T> OK(T data) 
            => new clsServiceResult<T> { Success = true, Data = data };
        public static clsServiceResult<T> Invalid(clsValidationResult validation)
            => new clsServiceResult<T> { Success = false, Validation = validation };
        public static clsServiceResult<T> Fail(string message  )
            => new clsServiceResult<T> { Success = false, ErrorMessage = message };
    }
}
