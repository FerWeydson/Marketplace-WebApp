using Domain.Interfaces.InterfaceCompraUsuario;
using Entities.Entities;
using Entities.Entities.Enums;
using Infrastructure.Configuration;
using Infrastructure.Repository.Generics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;


namespace Infrastructure.Repository.Repositories
{
    public class RepositoryCompraUsuario : RepositoryGenerics<CompraUsuario>, ICompraUsuario
    {

        private readonly DbContextOptions<ContextBase> _OptionsBuilder;
        public RepositoryCompraUsuario()
        {
            _OptionsBuilder = new DbContextOptions<ContextBase>();
        }

        public async Task<bool> ConfirmaCompraCarinhoUsuario(string userId)
        {
            using var banco = new ContextBase(_OptionsBuilder);
            var compraUsuario = new CompraUsuario();
            compraUsuario.ListaProdutos = new List<Produto>();
            try
            {
                var produtosCarrinhoUsuario = await (from p in banco.Produto
                                                     join c in banco.CompraUsuario on p.Id equals c.IdProduto
                                                     where c.UserId.Equals(userId) && c.Estado == EnumEstadoCompra.Produto_Carrinho
                                                     select c).AsNoTracking().ToListAsync();

                produtosCarrinhoUsuario.ForEach(p =>
                {
                    p.Estado = EnumEstadoCompra.Produto_Comprado;
                });
                banco.UpdateRange(produtosCarrinhoUsuario);
                await banco.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {

                return false;
            }
            
        }

        public async Task<CompraUsuario> ProdutosCompradosPorEstado(string userId, EnumEstadoCompra estado)
        {
            using var banco = new ContextBase(_OptionsBuilder);
            var compraUsuario = new CompraUsuario();
            compraUsuario.ListaProdutos = new List<Produto>();

            var produtosCarrinhoUsuario = await (from p in banco.Produto
                                                 join c in banco.CompraUsuario on p.Id equals c.IdProduto
                                                 where c.UserId.Equals(userId) && c.Estado == estado
                                                 select new Produto
                                                 {
                                                     Id = p.Id,
                                                     Nome = p.Nome,
                                                     Descricao = p.Descricao,
                                                     Observacao = p.Observacao,
                                                     Valor = p.Valor,
                                                     QtdCompra = p.QtdCompra,
                                                     IdProdutoCarrinho = p.IdProdutoCarrinho,
                                                     Url = p.Url,
                                                 }).AsNoTracking().ToListAsync();
            compraUsuario.ListaProdutos = produtosCarrinhoUsuario;
            compraUsuario.ApplicationUser = await banco.ApplicationUser.FirstOrDefaultAsync(u => u.Id.Equals(userId));
            compraUsuario.QuantidadeProduto = produtosCarrinhoUsuario.Count();
            compraUsuario.EndereçoCompleto = string.Concat(compraUsuario.ApplicationUser.Endereco, " - ", compraUsuario.ApplicationUser.ComplementoEndereco, " - CEP ", compraUsuario.ApplicationUser.CEP);
            compraUsuario.ValorTotal = produtosCarrinhoUsuario.Sum(v => v.Valor);
            compraUsuario.Estado = estado;
            return compraUsuario;
        }


        public async Task<int> QuantidadeProdutoCarrinhoUsuario(string userId)
        {
            using (var banco = new ContextBase(_OptionsBuilder)) 
            {
                return await banco.CompraUsuario.CountAsync(c => c.UserId.Equals(userId) && c.Estado == EnumEstadoCompra.Produto_Carrinho);
            }
        }
    }
}
