using ApplicationApp.Interfaces;
using Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace Web_ECommerce.Controllers
{

    [Authorize]
    public class ProdutosController : Controller
    {

        public readonly UserManager<ApplicationUser> _userManager;

        public readonly InterfaceProductApp _InterfaceProductApp;

        public readonly InterfaceCompraUsuarioApp _interfaceCompraUsuarioApp;

        private IWebHostEnvironment _environment;
        public ProdutosController(InterfaceProductApp InterfaceProductApp, UserManager<ApplicationUser> userManager,
                                   InterfaceCompraUsuarioApp interfaceCompraUsuarioApp, IWebHostEnvironment environment)
        {
            _InterfaceProductApp = InterfaceProductApp;
            _userManager = userManager;
            _interfaceCompraUsuarioApp = interfaceCompraUsuarioApp;
            _environment =  environment;

        }
        // GET: ProdutosController
        public async Task<IActionResult> Index()
        {
            var IdUsuario = await RetornarIdUsuarioLogado();

            return View(await _InterfaceProductApp.ListarProdutosUsuario(IdUsuario));
        }

        // GET: ProdutosController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            return View(await _InterfaceProductApp.GetEntityById(id));
        }

        // GET: ProdutosController/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: ProdutosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produto produto)
        {
            try
            {
                var IdUsuario = await RetornarIdUsuarioLogado();

                produto.UserId = IdUsuario;

                await _InterfaceProductApp.AddProduct(produto);
                if (produto.Notitycoes.Any())
                {
                    foreach (var item in produto.Notitycoes)
                    {
                        ModelState.AddModelError(item.NomePropriedade, item.mensagem);
                    }

                }
                await SalvarImagemProduto(produto);

            }
            catch
            {
                return View("Create", produto);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: ProdutosController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            return View(await _InterfaceProductApp.GetEntityById(id));
        }

        // POST: ProdutosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Produto produto)
        {
            try
            {
                await _InterfaceProductApp.UpdateProduct(produto);
                if (produto.Notitycoes.Any())
                {
                    foreach (var item in produto.Notitycoes)
                    {
                        ModelState.AddModelError(item.NomePropriedade, item.mensagem);
                    }

                    return View("Edit", produto);
                }

            }
            catch
            {
                return View("Edit", produto);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: ProdutosController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            return View(await _InterfaceProductApp.GetEntityById(id));
        }

        // POST: ProdutosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Produto produto)
        {
            try
            {
                var produtoDeletar = await _InterfaceProductApp.GetEntityById(id);
                await _InterfaceProductApp.Delete(produtoDeletar);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        private async Task<string> RetornarIdUsuarioLogado()
        {
            var IdUsuario = await _userManager.GetUserAsync(User);

            return IdUsuario.Id;
        }

        [AllowAnonymous]
        [HttpGet("/api/ListarProdutoComEstoque")]
        public async Task<JsonResult> ListaProdutosComEstoque()
        {
            return Json(await _InterfaceProductApp.ListarProdutoComEstoque());
        }

        public async Task<IActionResult> ListaProdutosCarrinhoUsuario()
        {
            var idUsuario = await RetornarIdUsuarioLogado();
            return View( await _InterfaceProductApp.ListarProdutosCarrinhoUsuario(idUsuario));
        }

        // GET: ProdutosController/Delete/5
        public async Task<IActionResult> RemoverCarrinho(int id)
        {
            return View(await _InterfaceProductApp.ObterProdutoCarrinho(id));
        }

        // POST: ProdutosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoverCarrinho(int id, Produto produto)
        {
            try
            {
                var produtoDeletar = await _interfaceCompraUsuarioApp.GetEntityById(id);
                await _interfaceCompraUsuarioApp.Delete(produtoDeletar);

                return RedirectToAction(nameof(ListaProdutosCarrinhoUsuario));
            }
            catch
            {
                return View();
            }
        }
        public async Task SalvarImagemProduto(Produto produtoTela)
        {
            var produto = await _interfaceCompraUsuarioApp.GetEntityById(produtoTela.Id);

            try
            {
                if (produtoTela.Imagem != null)
                {
                    var webRoot = _environment.WebRootPath;
                    var permissionSet = new PermissionSet(PermissionState.Unrestricted);
                    var writePermission = new FileIOPermission(FileIOPermissionAccess.Append, string.Concat(webRoot, "/imgProdutos"));
                    permissionSet.AddPermission(writePermission);

                    var extension = System.IO.Path.GetExtension(produtoTela.Imagem.FileName);
                    var nomeArquivo = string.Concat(produtoTela.Id.ToString(), extension);
                    var diretorioSalvar = string.Concat(webRoot, "\\imgProdutos\\", nomeArquivo);

                    produtoTela.Imagem.CopyTo(new FileStream(diretorioSalvar, FileMode.Create));
                    produtoTela.Url = string.Concat("https://localhost:44346", "/imgProdutos/", nomeArquivo);

                    await _InterfaceProductApp.UpdateProduct(produtoTela);
                }
            }
            catch (Exception erro)
            {

            }
            
        }
    }
}
