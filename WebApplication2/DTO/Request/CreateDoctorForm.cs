using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.DTO.Request
{
    public class CreateDoctorForm
    {

        public string IndexNumber
        {
            get; set;
        }

        public string FirstName
        {
            get; set;
        }

        public string LastName
        {
            get; set;
        }

        public string Email
        {
            get; set;
        }

    }
}
