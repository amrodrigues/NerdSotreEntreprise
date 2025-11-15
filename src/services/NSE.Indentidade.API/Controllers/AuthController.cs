using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static NSE.Indentidade.API.Models.UserViewModel;

namespace NSE.Indentidade.API.Controllers
{
    [ApiController]
    [Route("api/identidade")]
    public class AuthController : ControllerBase
    {
        public readonly SignInManager <IdentityUser> _signInManager;
        public readonly UserManager <IdentityUser> _userManager;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registar(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = new IdentityUser
            {
                UserName = usuarioRegistro.Email,
                Email = usuarioRegistro.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, usuarioRegistro.Senha);
            if (result.Succeeded)
            { 
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok();
            }

            return BadRequest();
           
        }

        [HttpPost("auntenticar")]
        public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
       {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _signInManager.PasswordSignInAsync(usuarioLogin.Email , usuarioLogin.Senha, false, false);    

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
