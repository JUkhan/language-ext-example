using System;
using System.Collections.Generic;
using Contoso.Core.Domain;

namespace Contoso.Infrastructure.Data
{
    public class DBContext:IDBContext
    {
        public DBContext()
        {
        }

        List<Student> _students = new List<Student>();

        public List<Student> GetStudents()
        {
            return _students;
        }
    }
}
