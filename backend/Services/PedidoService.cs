using backend.Config.db;
using backend.Controllers.DTO.pedido;
using backend.Controllers.DTO.produto;
using backend.Entities;
using backend.Errors;
using backend.Interfaces;
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
            DateOnly dateOnly = new DateOnly();
            string strFormatDate = dateOnly.ToString("dd/MM/yyyy");
            if (string.IsNullOrEmpty(pedido.NomePessoa))
                errors.Add(new ErrorDetalhe("O nome não pode ser vazio"));
            if (string.IsNullOrEmpty(pedido.Contato))
                errors.Add(new ErrorDetalhe("O contato não pode ser vazio"));
            if (string.IsNullOrEmpty(pedido.HoraRetirada.ToString()))
                errors.Add(new ErrorDetalhe("A hora não pode ser vazia"));
            if (dateOnly < pedido.DataRetirada)
                errors.Add(new ErrorDetalhe("Data incorreta, insira novamente"));

            string geraChave = GerarChave(6);
            decimal decValorTotal = 0;

            foreach (var produto in produtos)
            {
                var prod = _Conexao.Produtos.Find(produto.IdProduto);
                if (prod == null) 
                    errors.Add(new ErrorDetalhe("Produto não encontrado"));

                decValorTotal += Convert.ToDecimal(prod.Preco * produto.Quantidade);
            }
            DateTime dateNow = DateTime.UtcNow;
            pedido.DataPedido = dateNow;
            pedido.Chave = geraChave;
            pedido.ValorTotal = decValorTotal;

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

        public void Deletar(int id)
        {
            throw new NotImplementedException();
        }

        public Categoria ObterPedidoPorId(int id)
        {
            throw new NotImplementedException();
        }

        public List<Pedido> ObterTodasPedidos()
        {
            throw new NotImplementedException();
        }

        public List<Pedido> ObterTodosProdutosPedido()
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
