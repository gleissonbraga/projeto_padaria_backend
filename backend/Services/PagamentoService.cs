using backend.Config.db;
using backend.Entities;
using backend.Enums;
using backend.Interfaces;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
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
            var pedidoUpdate = _Conexao.Pedidos.Find(intCodPedido);

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
                    Success = "https://projeto-padaria-frontend.vercel.app/pagamento/sucesso",
                    Failure = "https://projeto-padaria-frontend.vercel.app/pagamento/erro",
                    Pending = "https://projeto-padaria-frontend.vercel.app/pagamento/pendente"
                },
                AutoReturn = "approved"
            };

            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(requestPedido);

            pedidoUpdate.PreferenceId = preference.Id;
            _Conexao.Pedidos.Update(pedidoUpdate);

            return preference;
        }

        public async Task PagamentoAprovado(string strPreferenceId, string strPaymentId)
        {
            var pedidoUpdate = await _Conexao.Pedidos.FirstOrDefaultAsync(p => p.PreferenceId == strPreferenceId);
            if (pedidoUpdate == null) throw new Exception("Pedido não encontrado.");

            pedidoUpdate.PreferenceId = strPreferenceId;
            pedidoUpdate.PaymentId = strPaymentId;
            pedidoUpdate.Status = (short)Status.CONFIRMADO;

            _Conexao.Pedidos.Update(pedidoUpdate);
            _Conexao.SaveChanges();
        } 
    }
}
