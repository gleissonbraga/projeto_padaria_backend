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
                    throw new Exception("Produto não encontrado");

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

            string strStatusPedido = null;
            if (pedido.Status == (short)Status.PENDENTE)
                strStatusPedido = Status.PENDENTE.ToString();
            if (pedido.Status == (short)Status.CONFIRMADO)
                strStatusPedido = Status.CONFIRMADO.ToString();
            if (pedido.Status == (short)Status.CANCELADO)
                strStatusPedido = Status.CANCELADO.ToString();
            if (pedido.Status == (short)Status.ENTREGUE)
                strStatusPedido = Status.ENTREGUE.ToString();

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
                Status = strStatusPedido,
                Produtos = pedido.ProdutosPedido.Select(pp => new ProdutoResponseDTO
                {
                    IdProduto = pp.CodigoProduto,
                    Quantidade = pp.QuantidadeProduto,
                    Nome = _Conexao.Produtos.Find(pp.CodigoProduto).Nome,
                    Preco = _Conexao.Produtos.Find(pp.CodigoProduto).Preco
                }).ToList()
            };

            return pedidoDTO;
        }
        public void ConfirmarPagamento(int id)
        {
            var findPedido = _Conexao.Pedidos.Find(id);
            if (findPedido == null)
                throw new Exception("Pedido não encontrado");

            findPedido.Status = (short)Status.CONFIRMADO;
            _Conexao.Pedidos.Update(findPedido);
            _Conexao.SaveChanges();
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

            if (findPedido.Status != (short)Status.PENDENTE)
                throw new Exception("Não é possivel excluir este pedido");

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

            var pedidosDTO = new PedidoRequestDTO
            {
                CodigoPedido = findPedido.CodigoPedido,
                NomePessoa = findPedido.NomePessoa,
                Contato = findPedido.Contato,
                DataRetirada = findPedido.DataRetirada,
                HoraRetirada = findPedido.HoraRetirada,
                ValorTotal = findPedido.ValorTotal,
                Status = strStatusPedido,
                DataPedido = findPedido.DataPedido,
                Produtos = findPedido.ProdutosPedido.Select(pp => new ProdutoResponseDTO
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
        public async Task<PedidoRequestDTO> ObterPedidoPorPreferenceId(string id)
        {
            var findPedido = await _Conexao.Pedidos.Include(p => p.ProdutosPedido).ThenInclude(pp => pp.Produto).FirstOrDefaultAsync(p => p.PreferenceId == id);
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

            var pedidosDTO = new PedidoRequestDTO
            {
                CodigoPedido = findPedido.CodigoPedido,
                NomePessoa = findPedido.NomePessoa,
                Contato = findPedido.Contato,
                DataRetirada = findPedido.DataRetirada,
                HoraRetirada = findPedido.HoraRetirada,
                ValorTotal = findPedido.ValorTotal,
                Status = strStatusPedido,
                DataPedido = findPedido.DataPedido,
                Chave = findPedido.Chave,
                Produtos = findPedido.ProdutosPedido.Select(pp => new ProdutoResponseDTO
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

            //var pedidosDTO = findpedidos.Select(p => new PedidoRequestDTO
            //{
            //    CodigoPedido = p.CodigoPedido,
            //    NomePessoa = p.NomePessoa,
            //    ValorTotal = p.ValorTotal,
            //    Produtos = p.ProdutosPedido.Select(pp => new ProdutoDTO
            //    {
            //        IdProduto = pp.Produto.IdProduto,
            //        Nome = pp.Produto.Nome,
            //        Preco = pp.Produto.Preco,
            //        Quantidade = pp.QuantidadeProduto
            //    }).ToList()
            //}).ToList();

            List<PedidoRequestDTO> lstPedidos = new List<PedidoRequestDTO>();

            foreach (var item in findpedidos)
            {
                string strStatusPedido = null;
                if (item.Status == (short)Status.PENDENTE)
                    strStatusPedido = Status.PENDENTE.ToString();
                if (item.Status == (short)Status.CONFIRMADO)
                    strStatusPedido = Status.CONFIRMADO.ToString();
                if (item.Status == (short)Status.CANCELADO)
                    strStatusPedido = Status.CANCELADO.ToString();
                if (item.Status == (short)Status.ENTREGUE)
                    strStatusPedido = Status.ENTREGUE.ToString();

                var pedidoDTO = new PedidoRequestDTO
                {
                    CodigoPedido = item.CodigoPedido,
                    NomePessoa = item.NomePessoa,
                    Contato = item.Contato,
                    DataPedido = item.DataPedido,
                    DataRetirada = item.DataRetirada,
                    HoraRetirada = item.HoraRetirada,
                    ValorTotal = item.ValorTotal,
                    Chave = item.Chave,
                    Status = strStatusPedido,
                    Produtos = item.ProdutosPedido.Select(pp => new ProdutoResponseDTO
                    {
                        IdProduto = pp.Produto.IdProduto,
                        Quantidade = pp.QuantidadeProduto,
                        Nome = pp.Produto.Nome,
                        Preco = pp.Produto.Preco,
                        Imagem = pp.Produto.Imagem,
                        Categoria = pp.Produto.Categoria != null ? pp.Produto.Categoria.NomeCategoria : null
                    }).ToList()
                };

                lstPedidos.Add(pedidoDTO);
            }

            return lstPedidos;
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
