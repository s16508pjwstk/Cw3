using System;
using System.Collections.Generic;

namespace WebApplication2.GeneratedModels
{
    public partial class Studies
    {
        public Studies()
        {
            Enrollment = new HashSet<Enrollment>();
        }

        public int IdStudy { get; set; }
        public string Name { get; set; }

        public ICollection<Enrollment> Enrollment { get; set; }
    }
}
