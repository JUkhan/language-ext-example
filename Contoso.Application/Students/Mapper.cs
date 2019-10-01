using System;
using Contoso.Core.Domain;
namespace Contoso.Application.Students
{
    internal class Mapper
    {
        internal static StudentViewModel ProjectToViewModel(Student student) =>
            new StudentViewModel(student.StudentId, student.FirstName, student.LastName,
                student.EnrollmentDate);
    }
}
