using ClientesWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientesWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public IActionResult Index()
        {
            if (TempData["Mensagem"] != null)
            {
                ViewBag.MensagemTitulo = TempData["MensagemTitulo"];
                ViewBag.Mensagem = TempData["Mensagem"];
            }
            else
            {
                ViewBag.MensagemTitulo = "";
                ViewBag.Mensagem = "";
            }
            return View();
        }

        [HttpPost]
        public IActionResult Logar()
        {
            try
            {
                string _login = Request.Form["login"];
                string _senha = Request.Form["senha"];

                Services.Services _services = new Services.Services(_configuration);

                var retornotoken = _services.RetornaToken(_login, _senha);
                TokenModel token = retornotoken.Result;
                if (token.status == 1)
                {
                   
                    HttpContext.Session.SetString("Token", token.token);
                    HttpContext.Session.SetInt32("IdLogado", token.cliente.ID);
                    HttpContext.Session.SetString("NomeLogado", token.cliente.Nome);
                    HttpContext.Session.SetString("perfilsigla", "cli");

                    return RedirectToAction("Index", "Cliente");
                }
                else
                {
                    TempData["MensagemTitulo"] = "Erro!";
                    TempData["Mensagem"] = token.mensagem;

                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = ex.Message;

                return RedirectToAction("Index", "Login");
            }
        }

    }


}
