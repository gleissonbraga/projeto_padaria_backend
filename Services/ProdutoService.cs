using backend.Config.db;
using backend.Entities;
using backend.Errors;
using backend.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace backend.Services
{
    public class ProdutoService : InterfaceProduto
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


            if (errors.Count > 0) 
                throw new ErroHttp(errors);

            produto.DateNow = DateTime.UtcNow;
            _Conexao.Produtos.Add(produto);
            _Conexao.SaveChanges();
        }

        public void Atualizar(Produto produto, int id)
        {
            throw new NotImplementedException();
        }

        public void Deletar(int id)
        {
            throw new NotImplementedException();
        }

        public List<Produto> ObterTodosProdutos()
        {
            var lsttprodutos = _Conexao.Produtos.ToList();

            return lsttprodutos;
        }

        public Produto ObterUsuarioPorId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
