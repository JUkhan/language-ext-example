using System;
using System.Threading.Tasks;
using Contoso.Application.Students.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using webApi.Extensions;

namespace webApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController:ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentsController(IMediator mediator) => _mediator = mediator;

        [HttpGet()]
        public Task<IActionResult> AllStudent(int studentId) =>
           _mediator.Send(new GetAllStudents()).ToActionResult();
    }
}
