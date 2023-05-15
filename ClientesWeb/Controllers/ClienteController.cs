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
    public class ClienteController : Controller
    {
        private readonly IConfiguration _configuration;

        public ClienteController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Cadastro(ClienteModel _usuario)
        {
            Services.Services _services = new Services.Services(_configuration);
            string _token = HttpContext.Session.GetString("Token");

            ViewBag.NomeUsuario = HttpContext.Session.GetString("NomeLogado");

            ClienteModel cliente = new ClienteModel();

            try
            {
                var retornoServiceCadastroUsuario = _services.CadastraCliente(_usuario, _token);
                bool retornoCadastroUsuario = retornoServiceCadastroUsuario.Result;

                TempData["MensagemTitulo"] = "Ok";
                TempData["Mensagem"] = "Usuário cadastrado com sucesso!";
                return RedirectToAction("Index", "Usuario");
            }
            catch (Exception ex)
            {
                TempData["MensagemTitulo"] = "Erro!";
                TempData["Mensagem"] = "Erro ao cadastrar Cliente. Detalhes do erro: " + ex.Message;
                return RedirectToAction("Index", "Cliente");
            }
        }

        public JsonResult BuscaCep(string? cep)
        {
            Services.Services _services = new Services.Services(_configuration);
            string _token = HttpContext.Session.GetString("Token");
            CepModel _cep = new CepModel();

            try
            {
                var retornocep = _services.RetornaCep(cep);
                CepModel resultDiasSemana = retornocep.Result;
                return Json(resultDiasSemana);
            }
            catch (Exception ex)
            {
                
            }
            return Json(_cep);
        }

    }
}
