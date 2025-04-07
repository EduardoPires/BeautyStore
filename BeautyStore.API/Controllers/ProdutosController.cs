using BeautyStore.Domain.Entities;
using BeautyStore.Domain.Interfaces.Service;
using BeautyStore.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BeautyStore.API.Controllers
{
    [Route("api/produtos")]
    [ApiController]
    [Authorize]
    public class ProdutosController : Controller
    {
        private readonly IProdutoService _produtoService;
        private readonly IVendedorService _vendedorService;

        public ProdutosController(IProdutoService produtoService, IVendedorService vendedorService)
        {
            _produtoService = produtoService;
            _vendedorService = vendedorService;
        }

        /// <summary>
        ///  Listar todos os produtos
        /// </summary>
        [Route("ListarProdutos")]
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListarProdutos()
        {
            try
            {
                var listaProdutos = await _produtoService.ListarTodosProdutos();
                if (listaProdutos == null)
                {
                    return NotFound("Nenhum produto foi localizado.");
                }
                else
                {
                    return Ok(listaProdutos);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro: " + ex.Message);
            }
        }

        /// <summary>
        ///  Listar todos os produtos por categoria
        /// </summary>
        /// <param name="categoriaId">Id</param>
        [Route("ListarProdutosPorCategoria/{categoriaId}")]
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListarProdutosPorCategoria(Guid categoriaId)
        {
            try
            {
                var listaProdutos = await _produtoService.ListarTodosProdutosPorCategoria(categoriaId);
                if (listaProdutos == null)
                {
                    return NotFound("Nenhum produto foi localizado com a categoria fornecida.");
                }
                else
                {
                    return Ok(listaProdutos);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro: " + ex.Message);
            }
        }

        /// <summary>
        ///   Buscar produto por Id
        /// </summary>
        /// <param name="id">Id</param>
        [Route("BuscarProduto/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BuscarProduto(Guid id)
        {
            try
            {                                
                var produto = await _produtoService.BuscarProduto(id);
                if (produto == null)
                {
                    return NotFound("Nenhum produto foi localizado com o id fornecido.");
                }

                var usuarioLogado = await ObterUsuarioLogado(User);
                var vendedor = await _vendedorService.BuscarVendedorPorNome(usuarioLogado);
                if (produto.VendedorId != vendedor.Id)
                {
                    return BadRequest("Vendedor logado, não é o proprietário do produto. Visualização não permitida.");
                }

                return Ok(produto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro: " + ex.Message);
            }
        }
 
        /// <summary>
        ///     Criar produto
        /// </summary>
        [Route("CriarProduto")]
        [HttpPost]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CriarProduto(Produto produto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return ValidationProblem(ModelState);

                var produtoExistente = await _produtoService.BuscarProdutoPorDescricao(produto.Descricao);
                if (produtoExistente != null)
                {
                    return BadRequest("Produto informado já está cadastrado. Verifique.");
                }

                var idExistente = await _produtoService.BuscarProduto(produto.Id);
                if (idExistente != null) 
                {
                    return BadRequest("Id fornecido já possui produto cadastrado. Verifique.");
                }

                await _produtoService.CriarProduto(produto);
                return CreatedAtAction(nameof(BuscarProduto), new { id = produto.Id }, produto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro: " + ex.Message);
            }
        }

        /// <summary>
        ///     Atualizar produto
        /// </summary>
        /// <param name="id">Id</param>
        [Route("AtualizarProduto/{id}")]
        [HttpPut]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AtualizarProduto(Guid id, Produto produto)
        {
            try
            {
                if (id != produto.Id)
                {
                    return BadRequest("Id do produto, é diferente do id fornecido.");
                }

                if (!ModelState.IsValid)
                    return ValidationProblem(ModelState);

                var produtoExistente = await _produtoService.BuscarProdutoPorDescricao(produto.Descricao);
                if (produtoExistente != null && produtoExistente.Id != produto.Id)
                {
                    return BadRequest("Produto informado já está cadastrado. Verifique.");
                }

                var usuarioLogado = await ObterUsuarioLogado(User);
                var vendedor = await _vendedorService.BuscarVendedorPorNome(usuarioLogado);
                if (produto.VendedorId != vendedor.Id)
                {
                    return BadRequest("Vendedor logado, não é o proprietário do produto. Alteração não permitida.");
                }


                await _produtoService.AtualizarProduto(produto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro: " + ex.Message);
            }
        }

        /// <summary>
        ///     Excluir produto
        /// </summary>
        /// <param name="id">Id</param>
        [Route("ExcluirProduto/{id}")]
        [HttpPut]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExcluirProduto(Guid id)
        {
            try
            {
                var produto = await _produtoService.BuscarProduto(id);
                if (produto == null)
                {
                    return NotFound("Nenhum produto foi localizado com o id fornecido.");
                }

                var usuarioLogado = await ObterUsuarioLogado(User);
                var vendedor = await _vendedorService.BuscarVendedorPorNome(usuarioLogado);
                if (produto.VendedorId != vendedor.Id)
                {
                    return BadRequest("Vendedor logado, não é o proprietário do produto. Exclusão não permitida.");
                }

                await _produtoService.ExcluirProduto(produto.Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro: " + ex.Message);
            }
        }

        private async Task<string> ObterUsuarioLogado(ClaimsPrincipal user)
        {
            foreach (var claim in user.Claims)
            {
                var emailClaim = User.Claims.FirstOrDefault(c => c.Type.Contains("emailaddress"));
                if (emailClaim != null && emailClaim.Value.Contains("@"))
                {
                    return emailClaim.Value;
                }
            }
            return null;
        }
    }
}
