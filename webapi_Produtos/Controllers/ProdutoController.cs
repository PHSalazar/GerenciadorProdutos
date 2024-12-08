using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi_Produtos.Models.Produto;
using webapi_Produtos.Services;

namespace webapi_Produtos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private IProduto _iproduto;
        public ProdutoController(IProduto iproduto)
        {
            _iproduto = iproduto;
        }


        /// <summary>
        /// Obtem todos os produtos cadastrados
        /// </summary>
        /// <returns>Lista de Produtos cadastrados</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var produtosBD = await _iproduto.GetAll();
            if (produtosBD.Sucesso == false)
            {
                return NotFound(produtosBD);
            }

            return Ok(produtosBD);
        }

        /// <summary>
        /// Obter produto através do ID.
        /// </summary>
        /// <param name="id">ID do Produto</param>
        /// <returns>Produto com ID determinado</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProdutoById([FromRoute] int id)
        {
            var produtosBD = await _iproduto.GetProdutoById(id);
            if (produtosBD.Sucesso == false)
            {
                return NotFound(produtosBD);
            }

            return Ok(produtosBD);
        }
        /// <summary>
        /// Busca produtos com determinado nome ou parte do nome.
        /// </summary>
        /// <param name="nome">Termo de busca</param>
        /// <returns>Lista de produtos com nome completo ou parte dele.</returns>
        [HttpGet("buscaNome/{name}")]
        public async Task<IActionResult> GetProdutoByName([FromRoute] string name)
        {
            var produtosBD = await _iproduto.GetProdutoByName(name);
            if (produtosBD.Sucesso == false)
            {
                return NotFound(produtosBD);
            }

            return Ok(produtosBD);
        }
        /// <summary>
        /// Cria um novo produto
        /// </summary>
        /// <param name="produto">Corpo do novo produto</param>
        /// <returns>Retorna o produto adicionado.</returns>
        [HttpPost]
        public async Task<IActionResult> AdicionarNovoProduto([FromBody] ProdutoRequest novoProduto)
        {
            var produtosBD = await _iproduto.AdicionarNovoProduto(novoProduto);
            if (produtosBD.Sucesso == false)
            {
                return BadRequest(produtosBD);
            }

            return Ok(produtosBD);
        }
        /// <summary>
        /// Atualiza o estoque de um produto
        /// </summary>
        /// <param name="id">ID do produto com o estoque a ser atualizado</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("atualizarEstoque/{id}")]
        [Authorize(Roles = "ger, func")]
        public async Task<IActionResult> AtualizarEstoqueProdutoById([FromRoute] int id, [FromBody] ProdutoEstoqueRequest request)
        {
            var produtosBD = await _iproduto.AtualizarEstoqueProdutoById(id, request);
            if (produtosBD.Sucesso == false)
            {
                return BadRequest(produtosBD);
            }

            return Ok(produtosBD);
        }
        /// <summary>
        /// Obtem lista de produtos em estoque
        /// </summary>
        /// <returns>Lista de Produtos com Estoque disponível.</returns>
        [HttpGet("EmEstoque")]
        public async Task<ActionResult> GetProdutosDisponiveisEmEstoque()
        {
            var result = await _iproduto.GetProdutosDisponiveisEmEstoque();
            if (result.Sucesso == false)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Remove um produto.
        /// </summary>
        /// <param name="id">ID do produto a ser removido.</param>
        /// <returns>Lista de Produtos atualizada após remoção.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "ger")]
        public async Task<ActionResult> RemoverProdutoPorId([FromRoute] int id)
        {
            var result = await _iproduto.RemoverProdutoPorId(id);
            if (result.Sucesso == false)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }


}
