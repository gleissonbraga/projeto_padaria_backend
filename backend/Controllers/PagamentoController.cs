using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/pagamento")]
    public class PagamentoController : ControllerBase
    {
        public readonly PagamentoService _service;

        public PagamentoController(PagamentoService service)
        {
            _service = service;
        }

        [HttpPost("{id}")]
        public IActionResult CriarPagamento(int id)
        {
            try
            {
                var preference = _service.CriarPreferencia(id);

                return Ok(new { preference });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("sucesso/{preferenceId}/{paymentId}")]
        public async Task<IActionResult> PagamentoAprovado([FromRoute] string preferenceId, [FromRoute] string paymentId)
        {
            try
            {
                await _service.PagamentoAprovado(preferenceId, paymentId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

    }
}
