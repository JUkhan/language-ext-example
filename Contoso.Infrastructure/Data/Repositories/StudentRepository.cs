using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contoso.Core.Domain;
using Contoso.Core.Interfaces.Repositories;
using LanguageExt;

namespace Contoso.Infrastructure.Data.Repositories
{
    public class StudentRepository:IStudentRepository
    {
        IDBContext _dbContext;
        public StudentRepository(IDBContext context)
        {
            _dbContext = context;
        }

        public Task<int> Add(Student student)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Option<Student>> Get(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Student>> GetAll()
        {
            return Task.FromResult(_dbContext.GetStudents());
        }

        public Task Update(Student student)
        {
            throw new NotImplementedException();
        }
    }
}
