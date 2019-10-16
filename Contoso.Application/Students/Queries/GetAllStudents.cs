using System;
using System.Collections.Generic;
using MediatR;

namespace Contoso.Application.Students.Queries
{
    public class GetAllStudents : IRequest<IEnumerable<StudentViewModel>>
    {

    }
}
