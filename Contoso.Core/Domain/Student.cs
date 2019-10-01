using System;
namespace Contoso.Core.Domain
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => LastName + ", " + FirstName;
        public DateTime EnrollmentDate { get; set; }
    }
}
