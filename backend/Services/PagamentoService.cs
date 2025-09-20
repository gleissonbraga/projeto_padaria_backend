using backend.Config.db;
using backend.Entities;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Threading.Tasks;

namespace backend.Services
{
    public class PagamentoService
    {
        private readonly Conexao _Conexao;
        private readonly PedidoService _PedidoService;
        public PagamentoService(Conexao conexao, PedidoService pedidoService) 
        { 
            _Conexao = conexao;
            _PedidoService = pedidoService;
            MercadoPagoConfig.AccessToken = Environment.GetEnvironmentVariable("ACCESS_TOKEN_MP");
        }

        public async Task<Preference> CriarPreferencia(int intCodPedido) 
        {
            var pedido = _PedidoService.ObterPedidoPorId(intCodPedido);
            if (pedido == null) throw new Exception("Pedido não encontrado.");

            var requestPedido = new PreferenceRequest
            {
                Items = pedido.Produtos.Select(i => new PreferenceItemRequest
                {
                    Id = i.IdProduto.ToString(),
                    Title = i.Nome,
                    Quantity = Convert.ToInt32(i.Quantidade),
                    UnitPrice = i.Preco,
                    CurrencyId = "BRL",
                }).ToList(),

                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = "https://padaria-api-sui1.onrender.com/pagamento/sucesso",
                    Failure = "https://padaria-api-sui1.onrender.com/pagamento/erro",
                    Pending = "https://padaria-api-sui1.onrender.com/pagamento/pendente"
                },
                AutoReturn = "approved"
            };

            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(requestPedido);

            return preference;
        }
    }
}
