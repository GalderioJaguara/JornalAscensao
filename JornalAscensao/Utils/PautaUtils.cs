using Microsoft.AspNetCore.Mvc.Rendering;

namespace JornalAscensao.Utils;

public class PautaUtils
{
    public static SelectList GetTipoSelectList()
    {
        var TipoLista = new List<SelectListItem>()
        {
            new SelectListItem { Value = "Analise", Text = "Pauta de Análise" },
            new SelectListItem { Value = "Noticia", Text = "Pauta de Notícia" }
        };
        return new SelectList(TipoLista, "Value", "Text");
    }

    public static SelectList GetCategoriasSelectList()
    {
        var CategoriaLista = new List<SelectListItem>()
        {
            new SelectListItem() { Value = "Politica", Text = "Política" },
            new SelectListItem { Value = "Economia", Text = "Economia" },
            new SelectListItem { Value = "Tecnologia", Text = "Tecnologia" },
            new SelectListItem { Value = "Ciencia", Text = "Ciência" },
            new SelectListItem { Value = "Saude", Text = "Saúde" },
            new SelectListItem { Value = "Financas", Text = "Finanças" },
            new SelectListItem { Value = "Regiliao", Text = "Religião" },
            new SelectListItem { Value = "Mundo", Text = "Mundo" },
            new SelectListItem { Value = "Filosofia", Text = "Filosofia" },
            new SelectListItem { Value = "Teologia", Text = "Teologia" },
            new SelectListItem { Value = "Sociologia", Text = "Sociologia" },
            new SelectListItem { Value = "Distopia", Text = "Distopia" },
            new SelectListItem { Value = "Outro", Text = "Outro" }
        };
        return new SelectList(CategoriaLista, "Value", "Text");
    }
}