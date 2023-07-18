﻿using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationApp.Interfaces
{
    public interface InterfaceProductApp : InterfaceGenericaApp<Produto>
    {
        Task AddProduct(Produto produto);
        Task UpdateProduct(Produto produto);
        Task<List<Produto>> ListarProdutosUsuario(string UserId);
        Task<List<Produto>> ListarProdutoComEstoque();
        Task<List<Produto>> ListarProdutosCarrinhoUsuario(string userId);
        Task<Produto> ObterProdutoCarrinho(int idProdutoCarrinho);
    }
}
