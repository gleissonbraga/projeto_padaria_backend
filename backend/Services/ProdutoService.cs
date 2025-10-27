using backend.Config.db;
using backend.Controllers.DTO.pedido;
using backend.Controllers.DTO.produto;
using backend.Entities;
using backend.Enums;
using backend.Errors;
using backend.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class ProdutoService : IProduto
    {
        private readonly Conexao _Conexao;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public List<ErrorDetalhe> errors = new List<ErrorDetalhe>();
        public ProdutoService(Conexao _db, IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor)
        {
            _Conexao = _db;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        public Produto Adicionar(Produto produto)
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

            _Conexao.Entry(produto).Reference(p => p.Categoria).Load();

            return produto;
        }

        public Produto Atualizar(Produto produto, int id)
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
            var findCategoria = _Conexao.Categoria.Find(produto.CodigoCategoria);
            if (findProduto == null) errors.Add(new ErrorDetalhe("Produto não encontrado"));

            if (errors.Count > 0) throw new ErroHttp(errors);

            if (string.IsNullOrEmpty(produto.Imagem))
                produto.Imagem = "image.png";
            else
                findProduto.Imagem = produto.Imagem;

            findProduto.Nome = produto.Nome;
            findProduto.Preco = produto.Preco;
            findProduto.Quantidade = produto.Quantidade;
            findProduto.Status = produto.Status;
            findProduto.Categoria = findCategoria;

            _Conexao.Produtos.Update(findProduto);
            _Conexao.SaveChanges();

            _Conexao.Entry(findProduto).Reference(p => p.Categoria).Load();

            return findProduto;
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

        public List<ProdutoDTO> ObterTodosProdutosAtivos()
        {
            var findProdutos = _Conexao.Produtos.Include(p => p.Categoria).ToList();

            List<ProdutoDTO> lstProdutosDTO = new List<ProdutoDTO>();

           foreach(var item in findProdutos)
            {
                var produtoDTO = new ProdutoDTO
                {
                   IdProduto = item.IdProduto,
                   Nome = item.Nome,
                   Preco = item.Preco,
                   Quantidade = item.Quantidade,
                   NomeCategoria = item.Categoria.NomeCategoria,
                   Imagem = item.Imagem,
                   CodigoCategoria = item.CodigoCategoria,
                   Status = (short)item.Status == (short)Status.ATIVO ? Status.ATIVO.ToString() : Status.INATIVO.ToString(),
                };

                lstProdutosDTO.Add(produtoDTO);
            }

            return lstProdutosDTO;
        }

        public Produto ObterProdutoPorId(int id)
        {
            var produto = _Conexao.Produtos.Find(id);

            if (produto == null)
                errors.Add(new ErrorDetalhe("Produto não encontrado"));

            if (errors.Count > 0) throw new ErroHttp(errors);

            return produto;
        }

        public async Task<string> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("Nenhum arquivo enviado.");

            var extensao = Path.GetExtension(file.FileName).ToLower();
            if (extensao != ".jpg" && extensao != ".jpeg" && extensao != ".png")
                throw new Exception("Apenas arquivos JPG e PNG são permitidos.");

            var path = Path.Combine(_environment.WebRootPath, "images", "produtos");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var fileName = $"{Guid.NewGuid()}{extensao}";
            var filePath = Path.Combine(path, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Pega host atual para montar URL pública
            var request = _httpContextAccessor.HttpContext?.Request;
            var imageUrl = $"{request?.Scheme}://{request?.Host}/images/produtos/{fileName}";

            return imageUrl;
        }
    }
}
