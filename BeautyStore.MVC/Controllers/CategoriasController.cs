using BeautyStore.Domain.Entities;
using BeautyStore.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeautyStore.MVC.Controllers
{
    [Authorize]
    [Route("Categorias")]
    public class CategoriasController : Controller
    {
        private readonly ICategoriaService _categoriaService;
        private readonly IProdutoService _produtoService;

        public CategoriasController(ICategoriaService categoriaService, IProdutoService produtoService)
        {
            _categoriaService = categoriaService;
            _produtoService = produtoService;
        }

        public async Task<IActionResult> Index()
        {
            var listaCategorias = new List<Categoria>();
            listaCategorias = await _categoriaService.ListarTodasCategorias();

            return View(listaCategorias);
        }

        [Route("criar")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("criar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Descricao")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                var categoriaExistente = await _categoriaService.BuscarCategoriaPorDescricao(categoria.Descricao);
                if (categoriaExistente != null)
                {
                    TempData["Erro"] = "Categoria informada já está cadastrada. Verifique.";
                    return RedirectToAction("Index");
                }

                await _categoriaService.CriarCategoria(categoria);
                TempData["Sucesso"] = "Categoria cadastrada com sucesso.";
                return RedirectToAction(nameof(Index));
            }

           
            return View(categoria);
        }

        [Route("detalhes/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var categoria = await _categoriaService.BuscarCategoria(id);
            if (categoria == null)
            {
                TempData["Erro"] = "Nenhuma categoria foi localizada com o id fornecido.";
                return RedirectToAction("Index");
            }
            return View(categoria);
        }

        [Route("excluir/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var categoria = await _categoriaService.BuscarCategoria(id);
            if (categoria == null)
            {
                TempData["Erro"] = "Nenhuma categoria foi localizada com o id fornecido.";
                return RedirectToAction("Index");
            }

            if (_produtoService.ExisteProduto(categoria.Id))
            {
                TempData["Erro"] = "Categoria não pode ser excluída, pois possui produto associado.";
                return RedirectToAction("Index");
            }

            return View(categoria);
        }

        [HttpPost("excluir/{id:guid}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var categoria = await _categoriaService.BuscarCategoria(id);
            if (categoria == null)
            {
                TempData["Erro"] = "Nenhuma categoria foi localizada com o id fornecido.";
                return RedirectToAction("Index");
            }

            await _categoriaService.ExcluirCategoria(categoria.Id);

            TempData["Sucesso"] = "Categoria excluída com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [Route("atualizar/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var categoria = await _categoriaService.BuscarCategoria(id);
            if (categoria == null)
            {
                TempData["Erro"] = "Nenhuma categoria foi localizada com o id fornecido.";
                return RedirectToAction("Index");
            }
            return View(categoria);
        }

        [HttpPost("atualizar/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nome,Descricao")] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                TempData["Erro"] = "Id da categoria, é diferente do id fornecido.";
                return RedirectToAction("Index");
            }                    

            if (ModelState.IsValid)
            {
                try
                {
                    await _categoriaService.AtualizarCategoria(categoria);
                    TempData["Sucesso"] = "Categoria alterada com sucesso.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!_categoriaService.ExisteCategoria(categoria.Id))
                    {
                        TempData["Erro"] = "Nenhuma categoria foi localizada com o id fornecido.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
            return View(categoria);
        }
    }
}
