using Microsoft.EntityFrameworkCore;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using DatingApp.API.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using DatingApp.API.Dto;
using System.Collections.Generic;
using System.Security.Claims;
using System;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repository;
        private readonly IMapper _mapper;

        public UsersController(IDatingRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repository.GetUser(id);
            var userToReturn = _mapper.Map<UserForDetailDto>(user);
            return Ok(userToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repository.GetUsers();
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok(usersToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            // if (!(id == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)))
            // {
            //     return Unauthorized();
            // }

            var user = await _repository.GetUser(id);
            _mapper.Map(userForUpdateDto, user);

            if (true)//await _repository.SaveAll())
            {
                return NoContent();
            }

            throw new Exception($"Updating user id {id} faild to save");
        }
    }
}