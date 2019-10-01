using System;
using System.Collections.Generic;
using Contoso.Core.Domain;

namespace Contoso.Infrastructure.Data
{
    public interface IDBContext
    {
        List<Student> GetStudents();
    }
}
