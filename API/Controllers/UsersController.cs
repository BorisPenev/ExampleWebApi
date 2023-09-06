using API.Dtos;
using Application.Users;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
public class UsersController : BaseApiController
{
    private readonly ILogger<SwiftMessagesController> _logger;

    public UsersController(ILogger<SwiftMessagesController> logger)
    {
        _logger = logger;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllAsync()
    {

        return HandleResult(await Mediator.Send(new GetAll.Query()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        return HandleResult(await Mediator.Send(new GetById.Query() { Id = id }));
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateAsync(UserDto userDto)
    {
        return HandleResult(await Mediator.Send(new Create.Command 
        { 
            Title = userDto.Title,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email,
            Password = userDto.Password,
            ConfirmPassword = userDto.ConfirmPassword
        }));
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, UserDto userDto)
    {
        return HandleResult(await Mediator.Send(new Update.Command 
        {
            Id = id,
            Title = userDto.Title,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email,
            Password = userDto.Password,
            ConfirmPassword = userDto.ConfirmPassword
        }));
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
    }
}
