using backend.Config.db;
using backend.Controllers.DTO.produto;
using backend.Entities;
using backend.Enums;
using backend.Errors;
using backend.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace backend.Services
{
    public class ProdutoService : IProduto
    {
        private readonly Conexao _Conexao;
        public List<ErrorDetalhe> errors = new List<ErrorDetalhe>();
        public ProdutoService(Conexao _db)
        {
            _Conexao = _db;
        }

        public void Adicionar(Produto produto)
        {
            decimal result;

            if (string.IsNullOrEmpty(produto.Nome))
                errors.Add(new ErrorDetalhe("O nome não pode ser vazio"));

            else if (string.IsNullOrEmpty(produto.Quantidade.ToString()) || produto.Quantidade < 0)
                produto.Quantidade = 0;
            if (string.IsNullOrEmpty(produto.Preco.ToString()) || produto.Preco < 0)
                produto.Preco = 0.01M;
            if (!decimal.TryParse(produto.Preco.ToString(), out result))
                errors.Add(new ErrorDetalhe("O preco deve ser somente valor numérico"));
            if (!decimal.TryParse(produto.Quantidade.ToString(), out result))
                errors.Add(new ErrorDetalhe("A quantidade deve ser somente valor numérico"));
            if (string.IsNullOrEmpty(produto.Imagem))
                produto.Imagem = "image.png";


            if (errors.Count > 0) throw new ErroHttp(errors);

            produto.DateNow = DateTime.UtcNow;
            produto.Status = (short)Status.ATIVO;
            _Conexao.Produtos.Add(produto);
            _Conexao.SaveChanges();
        }

        public void Atualizar(Produto produto, int id)
        {
            decimal result;

            if (string.IsNullOrEmpty(produto.Nome))
                errors.Add(new ErrorDetalhe("O nome não pode ser vazio"));

            else if (string.IsNullOrEmpty(produto.Quantidade.ToString()) || produto.Quantidade < 0)
                produto.Quantidade = 0;
            if (string.IsNullOrEmpty(produto.Preco.ToString()) || produto.Preco < 0)
                produto.Preco = 0.01M;
            if (!decimal.TryParse(produto.Preco.ToString(), out result))
                errors.Add(new ErrorDetalhe("O preco deve ser somente valor numérico"));
            if (!decimal.TryParse(produto.Quantidade.ToString(), out result) || produto.Quantidade < 0)
                errors.Add(new ErrorDetalhe("O valor inserido é invalido"));

            var findProduto = _Conexao.Produtos.Find(id);
            if (findProduto == null) errors.Add(new ErrorDetalhe("Produto não encontrado"));

            if (string.IsNullOrEmpty(produto.Imagem))
                produto.Imagem = "image.png";
            else
                findProduto.Imagem = produto.Imagem;


            if (errors.Count > 0) throw new ErroHttp(errors);


            findProduto.Nome = produto.Nome;
            findProduto.Preco = produto.Preco;
            findProduto.Quantidade += produto.Quantidade;
            findProduto.Status = produto.Status;

            _Conexao.Produtos.Update(findProduto);
            _Conexao.SaveChanges();
        }

        public void Deletar(int id)
        {
            var produto = _Conexao.Produtos.Find(id);

            if (produto == null)
                errors.Add(new ErrorDetalhe("Produto não encontrado"));

            if (errors.Count > 0) throw new ErroHttp(errors);

            _Conexao.Produtos.Remove(produto);
            _Conexao.SaveChanges();
        }

        public List<Produto> ObterTodosProdutos()
        {
            var lsttprodutos = _Conexao.Produtos.ToList();

            return lsttprodutos;
        }

        public List<Produto> ObterTodosProdutosInativos()
        {
            var lsttprodutos = _Conexao.Produtos.Where(p => p.Status == (short)Status.INATIVO).ToList();

            return lsttprodutos;
        }

        public List<Produto> ObterTodosProdutosAtivos()
        {
            var lsttprodutos = _Conexao.Produtos.Where(p => p.Status == (short)Status.ATIVO).ToList();

            return lsttprodutos;
        }

        public Produto ObterProdutoPorId(int id)
        {
            var produto = _Conexao.Produtos.Find(id);

            if (produto == null)
                errors.Add(new ErrorDetalhe("Produto não encontrado"));

            if (errors.Count > 0) throw new ErroHttp(errors);

            return produto;
        }
    }
}
