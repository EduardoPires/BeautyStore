using BeautyStore.Domain.Entities;
using BeautyStore.Domain.Interfaces.Service;
using BeautyStore.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace BeautyStore.API.Controllers
{
    [Route("api/categorias")]
    [ApiController]
    [Authorize]
    public class CategoriasController : Controller
    {
        private readonly ICategoriaService _categoriaService;
        private readonly IProdutoService _produtoService;

        public CategoriasController(ICategoriaService categoriaService, IProdutoService produtoService)
        {
            _categoriaService = categoriaService;
            _produtoService = produtoService;
        }

        /// <summary>
        ///  Listar todas as Categorias
        /// </summary>
        [Route("ListarCategorias")]
        [HttpGet]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListarCategorias()
        {
            try
            {
                var listaCategorias = await _categoriaService.ListarTodasCategorias();
                if (listaCategorias == null)
                {
                    return NotFound("Nenhuma categoria foi localizada.");
                }
                else
                {
                    return Ok(listaCategorias);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro: " + ex.Message);
            }           
        }

        /// <summary>
        ///   Buscar categoria por Id
        /// </summary>
        /// <param name="id">Id</param>
        [Route("BuscarCategoria/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BuscarCategoria(Guid id)
        {
            try
            {
                var categoria = await _categoriaService.BuscarCategoria(id);
                if (categoria == null)
                {
                    return NotFound("Nenhuma categoria foi localizada com o id fornecido.");
                }
               
                return Ok(categoria);              
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro: " + ex.Message);
            }
        }

        /// <summary>
        ///     Criar categoria
        /// </summary>
        [Route("CriarCategoria")]
        [HttpPost]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CriarCategoria(Categoria categoria)
        {
            try
            {
                if (!ModelState.IsValid)
                    return ValidationProblem(ModelState);

                var categoriaExistente = await _categoriaService.BuscarCategoriaPorDescricao(categoria.Descricao);
                if (categoriaExistente != null)
                {
                    return BadRequest("Categoria informada já está cadastrada. Verifique.");
                }

                var idExistente = await _categoriaService.BuscarCategoria(categoria.Id);
                if (idExistente != null)
                {
                    return BadRequest("Id fornecido já possui categoria cadastrada. Verifique.");
                }

                await _categoriaService.CriarCategoria(categoria);
                return CreatedAtAction(nameof(BuscarCategoria), new {id = categoria.Id}, categoria);                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro: " + ex.Message);
            }
        }

        /// <summary>
        ///     Atualizar categoria
        /// </summary>
        /// <param name="id">Id</param>
        [Route("AtualizarCategoria/{id}")]
        [HttpPut]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AtualizarCategoria(Guid id, Categoria categoria)
        {
            try
            {
                if (id != categoria.Id)
                {
                    return BadRequest("Id da categoria, é diferente do id fornecido.");
                }

                if (!ModelState.IsValid)
                    return ValidationProblem(ModelState);

                var categoriaExistente = await _categoriaService.BuscarCategoriaPorDescricao(categoria.Descricao);
                if (categoriaExistente != null)
                {
                    return BadRequest("Categoria informada já está cadastrada. Verifique.");
                }

                await _categoriaService.AtualizarCategoria(categoria);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro: " + ex.Message);
            }
        }

        /// <summary>
        ///     Excluir categoria
        /// </summary>
        /// <param name="id">Id</param>
        [Route("ExcluirCategoria/{id}")]
        [HttpDelete]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Categoria), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExcluirCategoria(Guid id)
        {
            try
            {
                var categoria = await _categoriaService.BuscarCategoria(id);
                if (categoria == null)
                {
                    return NotFound("Nenhuma categoria foi localizada com o id fornecido.");
                }

                if (_produtoService.ExisteProduto(categoria.Id))
                {
                    return BadRequest("Categoria não pode ser excluída, pois possui produto associado.");

                }

                await _categoriaService.ExcluirCategoria(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro: " + ex.Message);
            }
        }
    }
}
