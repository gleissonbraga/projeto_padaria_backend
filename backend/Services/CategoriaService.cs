using backend.Config.db;
using backend.Entities;
using backend.Errors;
using backend.Interfaces;

namespace backend.Services
{
    public class CategoriaService : ICatgoria
    {
        private readonly Conexao _Conexao;
        public List<ErrorDetalhe> errors = new List<ErrorDetalhe>();
        public CategoriaService(Conexao _db)
        {
            _Conexao = _db;
        }

        public void Adicionar(Categoria categoria)
        {
            if (string.IsNullOrEmpty(categoria.NomeCategoria)) 
                errors.Add(new ErrorDetalhe("Nome da categoria não pode ser vazia."));

            var vericaNomecategoria = _Conexao.Categoria.Any(u => u.NomeCategoria == categoria.NomeCategoria);

            if (vericaNomecategoria)
                errors.Add(new ErrorDetalhe("Categoria já existe"));

            if (errors.Count > 0)
                throw new ErroHttp(errors);

            _Conexao.Categoria.Add(categoria);
            _Conexao.SaveChanges();
        }

        public Categoria Atualizar(Categoria categoria, int id)
        {
            var findCategoria = _Conexao.Categoria.Find(id);

            if (string.IsNullOrEmpty(categoria.NomeCategoria))
                errors.Add(new ErrorDetalhe("Nome da categoria não pode ser vazia."));

            if (findCategoria == null)
                errors.Add(new ErrorDetalhe("Esta categoria não existe"));

            if(findCategoria.NomeCategoria == categoria.NomeCategoria)
                errors.Add(new ErrorDetalhe("Nome da categoria não pode ser vazia."));

            if (errors.Count > 0)
                throw new ErroHttp(errors);

            findCategoria.NomeCategoria = categoria.NomeCategoria;
            _Conexao.Categoria.Update(findCategoria);
            _Conexao.SaveChanges();

            return findCategoria;
        }

        public void Deletar(int id)
        {
            var findCategoria = _Conexao.Categoria.Find(id);

            if (findCategoria == null) throw new Exception("Esta categoria não existe");

            _Conexao.Categoria.Remove(findCategoria);
            _Conexao.SaveChanges();
        }

        public Categoria ObterCategoriaPorId(int id)
        {
            var findCategoria = _Conexao.Categoria.Find(id);

            if (findCategoria == null) throw new Exception("Esta categoria não existe");

            return findCategoria;
        }

        public List<Categoria> ObterTodasCategorias()
        {
            var categorias = _Conexao.Categoria.ToList();

            return categorias;
        }
    }
}
