using backend.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Interfaces
{
    public interface ICategoria
    {
        List<Categoria> ObterTodasCategorias();
        Categoria ObterCategoriaPorId(int id);
        void Adicionar(Categoria categoria);
        Categoria Atualizar(Categoria categoria, int id);
        void Deletar(int id);
    }
}
