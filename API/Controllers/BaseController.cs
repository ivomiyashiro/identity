using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


public class BaseController(IMediator mediator) : ControllerBase
{
    protected readonly IMediator _mediator = mediator;
}