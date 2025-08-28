using backend.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Interfaces
{
    public interface InterfaceProduto
    {
        List<Produto> ObterTodosProdutos();
        Produto ObterProdutoPorId(int id);
        void Adicionar(Produto produto);
        void Atualizar(Produto produto, int id);
        void Deletar(int id);
    }
}
