using backend.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Interfaces
{
    public interface IProduto
    {
        List<Produto> ObterTodosProdutos();
        Produto ObterProdutoPorId(int id);
        Produto Adicionar(Produto produto);
        Produto Atualizar(Produto produto, int id);
        void Deletar(int id);
    }
}
