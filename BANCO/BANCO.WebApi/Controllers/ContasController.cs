using BANCO.Core;
using BANCO.Core.Exceptions;
using BANCO.Core.ModelView;
using BANCO.Manager.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BANCO.WebApi.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class ContasController : ControllerBase
    {
        private readonly IContaManager _contaManager;
        //private readonly ILogger _logger;

        public ContasController(IContaManager contaManager)
        {
            _contaManager = contaManager;
        }

        /// <summary>
        /// Consulta uma conta conforme parâmetros passados
        /// </summary>
        /// <param name="numeroConta">Número da conta.</param>
        /// <param name="nomeBanco">Nome do banco.</param>
        /// <returns></returns>
        // GET: api/Tipo
        [HttpGet("api/[controller]")]
        [ProducesResponseType(typeof(Conta), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<Conta>>> GetContaAsync(string numeroConta, string nomeBanco)
        {
            try
            {
                var conta = await _contaManager.GetContaBancoAsync(numeroConta, nomeBanco.ToUpper());
                if (conta != null)
                {
                    return Ok(conta);
                }
                else
                {
                    return NotFound("Conta não encontrada.");
                }
            }
            catch (BusinessException bex)
            {
                return BadRequest(bex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Efetua um saque na conta e banco passados.
        /// </summary>
        /// <param name="numeroConta">Número da conta onde será feito o saque.</param>
        /// <param name="nomeBanco">Nome do banco.</param>
        /// <param name="valorSaque">Valor a ser sacado.</param>
        /// <returns></returns>
        // GET: api/Conta
        [HttpGet("api/[controller]/Sacar")]
        [ProducesResponseType(typeof(ContaView), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<ContaView>>> SacarAsync(string numeroConta, string nomeBanco, decimal valorSaque)
        {
            try
            {
                var conta = await _contaManager.SacarAsync(numeroConta, nomeBanco.ToUpper(), valorSaque);
                if (conta != null)
                {
                    return Ok("Saque efetuado com sucesso!");
                }
                else
                {
                    return BadRequest("Não foi possível realizar o saque.");
                }
            }
            catch (BusinessException bex)
            {
                return BadRequest(bex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Efetua um depósito na conta e banco passados.
        /// </summary>
        /// <param name="numeroConta">Número da conta onde será feito o saque.</param>
        /// <param name="nomeBanco">Nome do banco.</param>
        /// <param name="valorDeposito">Valor a ser depositado.</param>
        /// <returns></returns>
        // GET: api/Conta
        [HttpGet("api/[controller]/Depositar")]
        [ProducesResponseType(typeof(ContaView), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<ContaView>>> DepositarAsync(string numeroConta, string nomeBanco, decimal valorDeposito)
        {
            try
            {
                var conta = await _contaManager.DepositarAsync(numeroConta, nomeBanco.ToUpper(), valorDeposito);
                if (conta != null)
                {
                    return Ok("Depósito efetuado com sucesso!");
                }
                else
                {
                    return BadRequest("Não foi possível realizar o depósito.");
                }
            }
            catch (BusinessException bex)
            {
                return BadRequest(bex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Transferir valores entre contas
        /// </summary>
        /// <param name="numeroContaOrigem"></param>
        /// <param name="nomeBancoOrigem"></param>
        /// <param name="numeroContaDestino"></param>
        /// <param name="nomeBancoDestino"></param>
        /// <param name="valorTransferir"></param>
        /// <returns></returns>
        // GET: api/Conta
        [HttpGet("api/[controller]/Transferir")]
        [ProducesResponseType(typeof(ContaView), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<ContaView>>> TransferirAsync(string numeroContaOrigem, string nomeBancoOrigem, string numeroContaDestino, string nomeBancoDestino, decimal valorTransferir)
        {
            try
            {
                var conta = await _contaManager.TransferirAsync(numeroContaOrigem, nomeBancoOrigem, numeroContaDestino, nomeBancoDestino, valorTransferir);
                if (conta != null)
                {
                    return Ok("Transferência efetuada com sucesso!");
                }
                else
                {
                    return BadRequest("Não foi possível realizar a transferência.");
                }
            }
            catch (BusinessException bex)
            {
                return BadRequest(bex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///<summary>
        /// Cadastra uma nova conta
        ///</summary>
        /// <param name="conta">Objeto Conta</param>
        [ProducesResponseType(typeof(ContaView), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        [HttpPost("api/[controller]")]
        public async Task<ActionResult<Conta>> CadastararConta(ContaView conta)
        {
            try
            {
                //var resultValidator = new ContaValidator().Validate(conta);
                //if (!resultValidator.IsValid)
                //    return BadRequest(resultValidator.Errors);

                Conta result = await _contaManager.CadastrarAsync(new Conta(conta));
                return CreatedAtAction("GetContaAsync", new { numeroConta = result.NumeroConta, nomeBanco = result.NomeBanco }, result);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(409, $"Conta já cadastrada.\n {ex.InnerException.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro inesperado. - {ex.Message}");
                throw;
            }
        }
    }
}
