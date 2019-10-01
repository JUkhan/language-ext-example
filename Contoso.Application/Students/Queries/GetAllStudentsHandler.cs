using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Contoso.Core.Interfaces.Repositories;
using MediatR;
using LanguageExt;
using static Contoso.Application.Students.Mapper;
using System.Linq;

namespace Contoso.Application.Students.Queries
{
    public class GetAllStudentsHandler: IRequestHandler<GetAllStudents, List<StudentViewModel>>
    {
        private readonly IStudentRepository _studentRepository;
        public GetAllStudentsHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<List<StudentViewModel>> Handle(GetAllStudents request, CancellationToken cancellationToken)
        {
            return (await this._studentRepository.GetAll()).Map(ProjectToViewModel).ToList();
        }
    }
}
