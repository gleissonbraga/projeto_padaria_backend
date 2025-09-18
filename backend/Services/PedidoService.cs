using backend.Config.db;
using backend.Controllers.DTO.pedido;
using backend.Controllers.DTO.produto;
using backend.Entities;
using backend.Enums;
using backend.Errors;
using backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using Sprache;
using System;

namespace backend.Services
{
    public class PedidoService : IPedido
    {

        private readonly Conexao _Conexao;
        public List<ErrorDetalhe> errors = new List<ErrorDetalhe>();
        public PedidoService(Conexao conexao)
        {
            _Conexao = conexao;
        }

        public PedidoRequestDTO Adicionar(Pedido pedido, List<Produto> produtos)
        {
            //var dateOnly = new DateTime;
            //string strFormatDate = dateOnly.ToString("dd/MM/yyyy");
            if (string.IsNullOrEmpty(pedido.NomePessoa))
                errors.Add(new ErrorDetalhe("O nome não pode ser vazio"));
            if (string.IsNullOrEmpty(pedido.Contato))
                errors.Add(new ErrorDetalhe("O contato não pode ser vazio"));
            if (string.IsNullOrEmpty(pedido.HoraRetirada.ToString()))
                errors.Add(new ErrorDetalhe("A hora não pode ser vazia"));
            //if (dateOnly < pedido.DataRetirada)
            //    errors.Add(new ErrorDetalhe("Data incorreta, insira novamente"));

            string geraChave = GerarChave(6);
            decimal decValorTotal = 0;

            foreach (var produto in produtos)
            {
                var prod = _Conexao.Produtos.Find(produto.IdProduto);
                if (prod == null) 
                    errors.Add(new ErrorDetalhe("Produto não encontrado"));

                if(prod.Quantidade < produto.Quantidade)
                    errors.Add(new ErrorDetalhe($"O produto {prod.Nome} possui somente {prod.Quantidade} em estoque"));

                if (errors.Count > 0)
                    throw new ErroHttp(errors);

                prod.Quantidade -= produto.Quantidade;

                decValorTotal += Convert.ToDecimal(prod.Preco * produto.Quantidade);
                _Conexao.Produtos.Update(prod);
            }

            DateTime dateNow = DateTime.UtcNow;
            pedido.DataPedido = dateNow;
            pedido.Chave = geraChave;
            pedido.ValorTotal = decValorTotal;
            pedido.Status = (short)Status.PENDENTE;

            foreach (var item in produtos)
            {
                pedido.ProdutosPedido.Add(new ProdutoPedido
                {
                    CodigoProduto = item.IdProduto,
                    QuantidadeProduto = Convert.ToInt32(item.Quantidade),
                });
            }

            _Conexao.Pedidos.Add(pedido);
            _Conexao.SaveChanges();

            var pedidoDTO = new PedidoRequestDTO
            {
                CodigoPedido = pedido.CodigoPedido,
                NomePessoa = pedido.NomePessoa,
                Contato = pedido.Contato,
                DataPedido = pedido.DataPedido,
                DataRetirada = pedido.DataRetirada,
                HoraRetirada = pedido.HoraRetirada,
                ValorTotal = pedido.ValorTotal,
                Chave = pedido.Chave,
                Produtos = pedido.ProdutosPedido.Select(pp => new ProdutoDTO
                {
                    IdProduto = pp.CodigoProduto,
                    Quantidade = pp.QuantidadeProduto,
                    Nome = _Conexao.Produtos.Find(pp.CodigoProduto).Nome,
                    Preco = _Conexao.Produtos.Find(pp.CodigoProduto).Preco
                }).ToList()
            };

            return pedidoDTO;
        }

        public void ConfirmarEntrega(int id, string chave)
        {
            var findPedido = _Conexao.Pedidos.Find(id);
            if (findPedido == null)
                throw new Exception("Pedido não encontrado");

            if (findPedido.Chave != chave)
                throw new Exception("Chave incorreta, insira um valor válido");

            findPedido.Status = (short)Status.ENTREGUE;
            _Conexao.Pedidos.Update(findPedido);
            _Conexao.SaveChanges();
        }
        public void Deletar(int id)
        {
            var findPedido = _Conexao.Pedidos.Find(id);
            if (findPedido == null)
                throw new Exception("Pedido não encontrado");

            _Conexao.Pedidos.Remove(findPedido);
            _Conexao.SaveChanges();
        }

        public PedidoRequestDTO ObterPedidoPorId(int id)
        {
            var findPedido = _Conexao.Pedidos.Include(p => p.ProdutosPedido).ThenInclude(pp => pp.Produto).FirstOrDefault(p => p.CodigoPedido == id);
            string strStatusPedido = null;
            if (findPedido == null)
                throw new Exception("Pedido não encontrado");

            if (findPedido.Status == (short)Status.PENDENTE)
                strStatusPedido = Status.PENDENTE.ToString();
            if (findPedido.Status == (short)Status.CONFIRMADO)
                strStatusPedido = Status.CONFIRMADO.ToString();
            if (findPedido.Status == (short)Status.CANCELADO)
                strStatusPedido = Status.CANCELADO.ToString();
            if (findPedido.Status == (short)Status.ENTREGUE)
                strStatusPedido = Status.ENTREGUE.ToString();

            var pedidosDTO =  new PedidoRequestDTO
            {
                CodigoPedido = findPedido.CodigoPedido,
                NomePessoa = findPedido.NomePessoa,
                Contato = findPedido.Contato,
                Chave = findPedido.Chave,
                DataRetirada = findPedido.DataRetirada,
                HoraRetirada = findPedido.HoraRetirada,
                ValorTotal = findPedido.ValorTotal,
                Status = strStatusPedido,
                DataPedido = findPedido.DataPedido,
                Produtos = findPedido.ProdutosPedido.Select(pp => new ProdutoDTO
                {
                    IdProduto = pp.Produto.IdProduto,
                    Nome = pp.Produto.Nome,
                    Preco = pp.Produto.Preco,
                    Quantidade = pp.QuantidadeProduto,
                    Status = pp.Produto.Status == (short)Status.ATIVO ? Status.ATIVO.ToString() : Status.INATIVO.ToString(),
                }).ToList()
            };

            return pedidosDTO;
        }

        public List<PedidoRequestDTO> ObterTodasPedidos()
        {
            var findpedidos = _Conexao.Pedidos.Include(p => p.ProdutosPedido).ThenInclude(pp => pp.Produto).ToList();

            var pedidosDTO = findpedidos.Select(p => new PedidoRequestDTO
            {
                CodigoPedido = p.CodigoPedido,
                NomePessoa = p.NomePessoa,
                ValorTotal = p.ValorTotal,
                Produtos = p.ProdutosPedido.Select(pp => new ProdutoDTO
                {
                    IdProduto = pp.Produto.IdProduto,
                    Nome = pp.Produto.Nome,
                    Preco = pp.Produto.Preco,
                    Quantidade = pp.QuantidadeProduto
                }).ToList()
            }).ToList();

            return pedidosDTO;
        }

        public List<PedidoRequestDTO> ObterTodosProdutosPedido()
        {
            throw new NotImplementedException();
        }

        static string GerarChave(int tamanho = 6)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var chave = new char[tamanho];

            for (int i = 0; i < tamanho; i++)
                chave[i] = chars[random.Next(chars.Length)];

            return new string(chave);
        }

    }
}
