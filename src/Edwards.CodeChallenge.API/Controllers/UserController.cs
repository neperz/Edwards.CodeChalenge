﻿using Edwards.CodeChallenge.API.Services.Interfaces;
using Edwards.CodeChallenge.API.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edwards.CodeChallenge.API.Controllers;
// TODO: Host a web API for the user to call to manage the users
[ApiController]
[Produces("application/json")]
[Route("api/v1/users")]
[OpenApiTag("User")]
public class UserController : ControllerBase
{
    private readonly IEdwardsUserService _edwardsUserService;

    public UserController(IEdwardsUserService edwardsUserService)
    {
        _edwardsUserService = edwardsUserService;
    }

    /// <summary>
    /// List of users.
    /// </summary>
    /// <remarks>
    /// Returns a list of all users.
    /// </remarks>
    /// <response code="200">Returns a list of users.</response>
    /// <response code="400">Request error.</response>
    /// <response code="401">Access denied.</response>
    /// <response code="500">API internal error.</response>

    [ProducesResponseType(typeof(IEnumerable<EdwardsUserViewModel>), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 401)]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EdwardsUserViewModel>>> GetAll() => Ok(await _edwardsUserService.GetAllAsync());


    /// <summary>
    /// User by Id.
    /// </summary>
    /// <remarks>
    /// Returns a User by Id.
    /// </remarks>
    /// <param name="edwardsUser">The "id" parameter of the User.</param>
    /// <response code="200">Returns an user.</response>
    /// <response code="204">User not found.</response>
    /// <response code="400">Request error.</response>
    /// <response code="401">Access denied.</response>
    /// <response code="500">API internal error.</response>

    [ProducesResponseType(typeof(EdwardsUserViewModel), 200)]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 401)]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    [HttpGet("{id}")]
    public async Task<ActionResult<EdwardsUserViewModel>> GetById([FromQuery] EdwardsUserIdViewModel edwardsUser)
    {
        var edwardsUserVM = await _edwardsUserService.GetByIdAsync(edwardsUser);

        if (edwardsUserVM == null)
        {
            return NotFound();
        }

        return Ok(edwardsUserVM);
    }

    /// <summary>
    /// User by name.
    /// </summary>
    /// <remarks>
    /// Returns a User by name.
    /// </remarks>
    /// <param name="edwardsUser">The "name" parameter of the User.</param>
    /// <response code="200">Returns a list of users.</response>
    /// <response code="204">Not found.</response>
    /// <response code="400">Request error.</response>
    /// <response code="401">Access denied.</response>
    /// <response code="500">API internal error.</response>

    [ProducesResponseType(typeof(IEnumerable<EdwardsUserViewModel>), 200)]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 401)]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    [HttpGet("name/{name}")]
    public async Task<ActionResult<EdwardsUserViewModel>> GetByName([FromQuery] EdwardsUserNameViewModel edwardsUser)
    {
        var edwardsUserVM = await _edwardsUserService.GetByNameAsync(edwardsUser);

        if (edwardsUserVM == null)
        {
            return NoContent();
        }

        return Ok(edwardsUserVM);
    }
    /// <summary>
    /// User by e-mail.
    /// </summary>
    /// <remarks>
    /// Returns a User by e-mail.
    /// </remarks>
    /// <param name="edwardsUser">The "email" parameter of the User.</param>
    /// <response code="200">Returns a list of users.</response>
    /// <response code="204">Not found.</response>
    /// <response code="400">Request error.</response>
    /// <response code="401">Access denied.</response>
    /// <response code="500">API internal error.</response>

    [ProducesResponseType(typeof(IEnumerable<EdwardsUserViewModel>), 200)]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 401)]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    [HttpGet("email/{email}")]
    public async Task<ActionResult<EdwardsUserViewModel>> GetByEmail([FromQuery] EdwardsUserEmailViewModel edwardsUser)
    {
        // TODO: Get a list of users with filter from the storage
        var edwardsUserVM = await _edwardsUserService.GetByEmailAsync(edwardsUser);

        if (edwardsUserVM == null)
        {
            return NoContent();
        }

        return Ok(edwardsUserVM);
    }

    /// <summary>
    /// User creation.
    /// </summary>
    /// <remarks>
    /// Creates a new User.
    /// </remarks>
    /// <param name="edwardsUser">The "User" parameter.</param>
    /// <response code="201">Record created.</response>
    /// <response code="204">User not found.</response>
    /// <response code="400">Request error.</response>
    /// <response code="401">Access denied.</response>
    /// <response code="500">API internal error.</response>
    [ProducesResponseType(typeof(EdwardsUserViewModel), 201)]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 401)]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    [HttpPost]
    public async Task<ActionResult<EdwardsUserViewModel>> PostUserAsync([FromBody] EdwardsUserViewModel edwardsUser)
    {
        // TODO: Adding a new user to the storage.
        if (edwardsUser == null || string.IsNullOrWhiteSpace(edwardsUser.Email))
        {
            return NoContent();
        }

        return Created(nameof(PostUserAsync), await _edwardsUserService.AddAsync(edwardsUser).ConfigureAwait(false));
    }

    /// <summary>
    /// User update.
    /// </summary>
    /// <remarks>
    /// Updates a User.
    /// </remarks>
    /// <param name="id">The "id" parameter of the User.</param>
    /// <param name="edwardsUser">The "User" parameter.</param>
    /// <response code="202">Record created.</response>
    /// <response code="204">User not found.</response>
    /// <response code="400">Request error.</response>
    /// <response code="401">Access denied.</response>
    /// <response code="500">API internal error.</response>

    [ProducesResponseType(typeof(void), 202)]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 401)]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutUser(string id, [FromBody] EdwardsUserViewModel edwardsUser)
    {
        // TODO: Update a user in the storage
        if (!Guid.TryParse(id, out Guid userId))
            return BadRequest();
        if (edwardsUser == null || edwardsUser.Id.ToUpper() != userId.ToString().ToUpper())
            return BadRequest();


        var edwardsUserVM = await _edwardsUserService.GetByIdAsync(new EdwardsUserIdViewModel(userId.ToString().ToUpper()));

        if (edwardsUserVM == null)
            return NoContent();


        await _edwardsUserService.UpdateAsync(edwardsUser).ConfigureAwait(false);

        return Accepted();
    }

    /// <summary>
    /// User deletion.
    /// </summary>
    /// <remarks>
    /// Deletes a User.
    /// </remarks>
    /// <param name="edwardsUser">The "id" parameter of the User.</param>
    /// <response code="202">Record created.</response>
    /// <response code="204">User not found.</response>
    /// <response code="400">Request error.</response>
    /// <response code="401">Access denied.</response>
    /// <response code="500">API internal error.</response>
    [ProducesResponseType(typeof(void), 202)]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 401)]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser([FromQuery] EdwardsUserIdViewModel edwardsUser)
    {
        // TODO: Deleting a user from the storage.
        var edwardsUserVM = await _edwardsUserService.GetByIdAsync(edwardsUser);

        if (edwardsUserVM == null)
        {
            return NoContent();
        }

        await _edwardsUserService.RemoveAsync(edwardsUserVM).ConfigureAwait(false);

        return Accepted();
    }
}