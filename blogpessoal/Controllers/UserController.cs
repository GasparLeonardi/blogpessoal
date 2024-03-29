﻿using blogpessoal.Model;
using blogpessoal.Security;
using blogpessoal.Service;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace blogpessoal.Controllers
{
    [Route("~/usuarios")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<User> _userValidator;
        private readonly IAuthService _authService;

        public UserController(
            IUserService userService, IValidator<User> userValidator, IAuthService authService)
        {
            _userService = userService;
            _userValidator = userValidator;
            _authService = authService;
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _userService.GetAll());
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var Resposta = await _userService.GetById(id);
            if (Resposta is null)
            {
                return NotFound();
            } return Ok(Resposta);
        }

        [AllowAnonymous]
        [HttpPost("cadastrar")]
        public async Task<ActionResult> Create([FromBody] User user)
        {
            var validarUser = await _userValidator.ValidateAsync(user);
            if (!validarUser.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, validarUser);
            } 
            var Resposta = await _userService.Create(user);

            if (Resposta is null)
            {
                return BadRequest("Usuário já está cadastrado!");
            }
              
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [Authorize]
        [HttpPut("atualizar")]
        public async Task<ActionResult> Update([FromBody] User user)
        {
            if (user.Id == 0)
            {
                return BadRequest("Id do User é Inválido!");
            }

            var validarUser = await _userValidator.ValidateAsync(user);
            
            if (!validarUser.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, validarUser);
            }

            var UserUpdate = await _userService.GetByUsuario(user.Usuario);

            if (UserUpdate is not null && UserUpdate.Id != user.Id)
            {
                return BadRequest("O Usuario(e-mail) já está em uso por outro usuario!");
            }

            var Resposta = await _userService.Update(user);

            if (Resposta is null)
            {
                return NotFound("User não encontrado");
            } 
            return Ok(Resposta);
        }

        [AllowAnonymous]
        [HttpPost("logar")]
        public async Task<ActionResult> Autenticar([FromBody] UserLogin userlogin)
        {
            var Resposta = await _authService.Autenticar(userlogin);

            if (Resposta is null)
            {
                return Unauthorized("Usuario e/ou senha são invalidos");
            }

            return Ok(Resposta);
        }
    }
}
