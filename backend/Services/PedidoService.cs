using backend.Config.db;

namespace backend.Services
{
    public class PedidoService
    {

        private readonly Conexao _Conexao;

        public PedidoService(Conexao conexao)
        {
            _Conexao = conexao;
        }


    }
}
