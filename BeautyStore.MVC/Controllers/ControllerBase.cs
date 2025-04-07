using Microsoft.AspNetCore.Mvc;

namespace BeautyStore.MVC.Controllers
{
    public class ControllerBase : Controller
    {
        protected async Task<ActionResult> ExecFuncaoNaoTratada(Func<Task<ActionResult>> funcao, ActionResult view = null, bool retornaJson = false)
        {
            try
            {
                return await funcao();
            }
            catch (Exception e)
            {
                if (retornaJson)
                {
                    return Json(new { Sucess = false, Mensagem = e.Message.Replace("A seguinte exceção ocorreu:", "") });
                }
                else
                {
                    TempData["Erro"] = e.Message.Replace("A seguinte exceção ocorreu:", "");
                    if (view != null)
                    {
                        return view;
                    }
                    else
                    {
                        return View("~/Views/Shared/Error.cshtml");
                    }
                }
            }
        }
    }
}
