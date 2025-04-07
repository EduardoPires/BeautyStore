using BeautyStore.Domain.Entities;
using BeautyStore.Domain.Interfaces.Service;
using BeautyStore.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BeautyStore.MVC.Controllers
{
    [Authorize]
    [Route("Produtos")]
    public class ProdutosController : Controller
    {
        private readonly IProdutoService _produtoService;
        private readonly ICategoriaService _categoriaService;
        private readonly IVendedorService _vendedorService;
        private readonly IWebHostEnvironment _environment;

        public ProdutosController(IProdutoService produtoService, ICategoriaService categoriaService, IVendedorService vendedorService, IWebHostEnvironment environment)
        {
            _produtoService = produtoService;
            _categoriaService = categoriaService;
            _vendedorService= vendedorService;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            Guid usuarioId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value.ToString());

            var listaProdutos = new List<Produto>();
            listaProdutos = await _produtoService.ListarTodosProdutos(usuarioId);

            return View(listaProdutos);
        }

        [Route("criar")]
        public async Task<IActionResult> Create()
        {
            await CarregaSelectLists();

            if (ViewBag.ListCategorias == null)
            {
                TempData["Erro"] = "Não existem categorias cadastradas. É necessário cadastrar categoria, para depois incluir produtos.";
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost("criar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Descricao,Preco,Estoque,CategoriaId")] Produto produto, IFormFile Imagem)
        {           
            ModelState.Remove(nameof(produto.Imagem));
            ModelState.Remove(nameof(produto.Vendedor));
            ModelState.Remove(nameof(produto.VendedorId));
            ModelState.Remove(nameof(produto.Categoria));

            if (ModelState.IsValid)
            {
                var produtoExistente = await _produtoService.BuscarProdutoPorDescricao(produto.Descricao);
                if (produtoExistente != null)
                {
                    TempData["Erro"] = "Produto informado já está cadastrado. Verifique.";
                    return RedirectToAction("Index");
                }

                if (Imagem == null)
                {                  
                    ModelState.AddModelError("Imagem", "É necessário informar a imagem do produto.");
                    await CarregaSelectLists();
                    return View(produto);
                }

                Guid usuarioId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value.ToString());
                var nomeUsuario = User.Identity?.Name;

                var vendedorExiste = await _vendedorService.BuscarVendedor(usuarioId);
                if (vendedorExiste == null)
                {
                    var vendedor = new Vendedor()
                    {
                        Id = usuarioId,
                        Nome = nomeUsuario,
                        IsProprietario = true
                    };

                    produto.Vendedor = vendedor;
                }

                produto.VendedorId = usuarioId;
                produto.Imagem = await UploadImage(Imagem);
                await _produtoService.CriarProduto(produto);
                TempData["Sucesso"] = "Produto cadastrado com sucesso.";
                return RedirectToAction(nameof(Index));
            }

            await CarregaSelectLists();
            return View(produto);
        }

        [Route("detalhes/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var produto = await _produtoService.BuscarProduto(id);
            if (produto == null)
            {
                TempData["Erro"] = "Nenhum produto foi localizado com o id fornecido.";
                return RedirectToAction("Index");
            }

            Guid usuarioId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value.ToString());
            if (produto.VendedorId != usuarioId)
            {
                TempData["Erro"] = "Vendedor logado, não é o proprietário do produto. Visualização não permitida.";
                return RedirectToAction("Index");
            }

            return View(produto);
        }

        [Route("excluir/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var produto = await _produtoService.BuscarProduto(id);
            if (produto == null)
            {
                TempData["Erro"] = "Nenhum produto foi localizado com o id fornecido.";
                return RedirectToAction("Index");
            }

            Guid usuarioId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value.ToString());
            if (produto.VendedorId != usuarioId)
            {
                TempData["Erro"] = "Vendedor logado, não é o proprietário do produto. Exclusão não permitida.";
                return RedirectToAction("Index");
            }

            return View(produto);
        }

        [HttpPost("excluir/{id:guid}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produto = await _produtoService.BuscarProduto(id);
            if (produto == null)
            {
                TempData["Erro"] = "Nenhum produto foi localizado com o id fornecido.";
                return RedirectToAction("Index");
            }

            await _produtoService.ExcluirProduto(produto.Id);

            TempData["Sucesso"] = "Produto excluído com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [Route("atualizar/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var produto = await _produtoService.BuscarProduto(id);
            if (produto == null)
            {
                TempData["Erro"] = "Nenhum produto foi localizado com o id fornecido.";
                return RedirectToAction("Index");
            }

            var produtoExistente = await _produtoService.BuscarProdutoPorDescricao(produto.Descricao);
            if (produtoExistente != null && produtoExistente.Id != produto.Id)
            {
                TempData["Erro"] = "Produto informado já está cadastrado. Verifique.";
                return RedirectToAction("Index");
            }

            Guid usuarioId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value.ToString());
            if (produto.VendedorId != usuarioId)
            {
                TempData["Erro"] = "Vendedor logado, não é o proprietário do produto. Alteração não permitida.";
                return RedirectToAction("Index");
            }

            await CarregaSelectLists();
            return View(produto);
        }

        [HttpPost("atualizar/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nome,Descricao,Preco,Estoque,VendedorId,CategoriaId")] Produto produto, IFormFile Imagem, string ImagemAtual)
        {
            if (id != produto.Id)
            {
                TempData["Erro"] = "Id do produto, é diferente do id fornecido.";
                return RedirectToAction("Index");
            }

            ModelState.Remove(nameof(produto.Categoria));
            ModelState.Remove(nameof(produto.Vendedor));
            ModelState.Remove(nameof(produto.Imagem));

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImagemAtual == null)
                        produto.Imagem = await UploadImage(Imagem);
                    else
                        produto.Imagem = ImagemAtual;

                    await _produtoService.AtualizarProduto(produto);
                    TempData["Sucesso"] = "Produto alterado com sucesso.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!_produtoService.ExisteProduto(produto.Id))
                    {
                        TempData["Erro"] = "Nenhum produto foi localizado com o id fornecido.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        throw e;
                    }
                }
            }

            await CarregaSelectLists();
            return View(produto);
        }

        private async Task<string> UploadImage(IFormFile imagem)
        {
            if (imagem != null && imagem.Length > 0)
            {
                var fileName = Path.GetFileName(imagem.FileName);
                var uploadsDirectory = Path.Combine(_environment.WebRootPath, "uploads");

                if (!Directory.Exists(uploadsDirectory))
                {
                    Directory.CreateDirectory(uploadsDirectory);
                }

                var filePath = Path.Combine(uploadsDirectory, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imagem.CopyToAsync(stream);
                }

                return "/uploads/" + fileName;
            }

            return null;
        }

        private async Task CarregaSelectLists()
        {
            var ListCategorias = new SelectList(await _categoriaService.ListarTodasCategorias(), "Id", "Descricao");
            ViewBag.ListCategorias = ListCategorias.Any() ? ListCategorias : null;
        }
    }
}
