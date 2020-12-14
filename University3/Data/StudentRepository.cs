using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace University3.Data
{
    public class StudentRepository
    {
        private University3Context db;
        public StudentRepository(University3Context db)
        {
            this.db = db;
        }
    }
}
