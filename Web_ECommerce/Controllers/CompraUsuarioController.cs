using ApplicationApp.Interfaces;
using Entities.Entities;
using Entities.Entities.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Web_ECommerce.Models;

namespace Web_ECommerce.Controllers
{
    public class CompraUsuarioController : HelpQrCode
    {
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly InterfaceCompraUsuarioApp _InterfaceCompraUsuarioApp;
        private readonly IWebHostEnvironment _environment;


        public CompraUsuarioController(UserManager<ApplicationUser> userManager, InterfaceCompraUsuarioApp InterfaceCompraUsuarioApp, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _InterfaceCompraUsuarioApp = InterfaceCompraUsuarioApp;
            _environment = environment;
        }

        public async Task<IActionResult> FinalizarCompra()
        {
            var usuario = await _userManager.GetUserAsync(User);
            var compraUsuario = await _InterfaceCompraUsuarioApp.CarrinhoCompras(usuario.Id);

            return View(compraUsuario);
        }

        public async Task<IActionResult> MinhasCompras(bool mensagem = false)
        {
            var usuario = await _userManager.GetUserAsync(User);

            var compraUsuario = await _InterfaceCompraUsuarioApp.ProdutosComprados(usuario.Id);

            if (mensagem)
            {
                ViewBag.Sucesso = true;
                ViewBag.sucesso = "Compra efetivada com sucesso. Pague o boleto para garantir o produto";
            }
            return View(compraUsuario);
        }

        public async Task<IActionResult> ConfirmaCompra()
        {
            var usuario = await _userManager.GetUserAsync(User);
            var sucesso = await _InterfaceCompraUsuarioApp.ConfirmaCompraCarrinhoUsuario(usuario.Id);

            if(sucesso)
            {
                return RedirectToAction("MinhasCompras", new { message = true });

            }
            else
            {
                return RedirectToAction("FinalizarCompra");
            }
        }



        [HttpPost("/api/AdicionarProdutoCarrinho")]
        public async Task<JsonResult> AdicionarProdutoCarrinho(string id, string nome, string qtd)
        {
            var usuario = await _userManager.GetUserAsync(User);

            if (usuario != null)
            {
                await _InterfaceCompraUsuarioApp.Add(new CompraUsuario
                {
                    IdProduto = Convert.ToInt32(id),
                    QtdCompra = Convert.ToInt32(qtd),
                    Estado = EnumEstadoCompra.Produto_Carrinho,
                    UserId = usuario.Id

                });
                return Json(new { sucesso = true });

            }
            return Json(new { sucesso = false });
        }
        [HttpGet("/api/QtdProdutoCarrinho")]
        public async Task<JsonResult> QtdProdutoCarrinho()
        {
            var usuario = await _userManager.GetUserAsync(User);
            var qtd = 0;
            if (usuario != null)
            {
                qtd = await _InterfaceCompraUsuarioApp.QuantidadeProdutoCarrinhoUsuario(usuario.Id);

                return Json(new { sucesso = true, qtd = qtd });
            }
            return Json(new { sucesso = false, qtd = qtd });
        }

        public async Task<IActionResult> Imprimir()
        {
            var usuario = await _userManager.GetUserAsync(User);

            var compraUsuario = await _InterfaceCompraUsuarioApp.ProdutosComprados(usuario.Id);
             
            return await DownloadQrCode(compraUsuario, _environment);

        }
    }
}
