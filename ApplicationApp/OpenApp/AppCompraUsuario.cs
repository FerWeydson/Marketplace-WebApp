using ApplicationApp.Interfaces;
using Domain.Interfaces.InterfaceCompraUsuario;
using Domain.Interfaces.InterfaceServices;
using Domain.Services;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationApp.OpenApp
{
    public class AppCompraUsuario : InterfaceCompraUsuarioApp
    {
        private readonly ICompraUsuario _ICompraUsuario;
        private readonly IServiceCompraUsuario _IserviceCompraUsuario;
        public AppCompraUsuario(ICompraUsuario ICompraUsuario, IServiceCompraUsuario IserviceCompraUsuario)
        {
            _ICompraUsuario = ICompraUsuario;
            _IserviceCompraUsuario = IserviceCompraUsuario;
        }

        public async Task<CompraUsuario> CarrinhoCompras(string userId)
        {
            return await _IserviceCompraUsuario.CarrinhoCompras(userId);
        }   

        public async Task<CompraUsuario> ProdutosComprados(string userId)
        {
            return await _IserviceCompraUsuario.ProdutosComprados(userId);
        }

        public async Task<bool> ConfirmaCompraCarrinhoUsuario(string userId)
        {
            return await _ICompraUsuario.ConfirmaCompraCarinhoUsuario(userId);
        }

        public async Task<int> QuantidadeProdutoCarrinhoUsuario(string userId)
        {
            return await _ICompraUsuario.QuantidadeProdutoCarrinhoUsuario(userId);
        }

        public async Task Add(CompraUsuario Objeto)
        {
            await _ICompraUsuario.Add(Objeto);
        }

        public async Task Delete(CompraUsuario Objeto)
        {
            await _ICompraUsuario.Delete(Objeto);
        }

        public async Task<CompraUsuario> GetEntityById(int Id)
        {
           return await _ICompraUsuario.GetEntityById(Id);
        }

        public async Task<List<CompraUsuario>> List()
        {
            return await _ICompraUsuario.List();
        }

        public async Task Update(CompraUsuario Objeto)
        {
            await _ICompraUsuario.Update(Objeto);
        }

    }
}
