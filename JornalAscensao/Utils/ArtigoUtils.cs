using JornalAscensao.Models;

namespace JornalAscensao.Utils;

public class ArtigoUtils
{
    public static string RetornarStatus(StatusArtigo status)
    {
        var tipo = "";
        switch (status)
        {
                
            case StatusArtigo.Escrevendo:
                tipo = "Escrevendo";
                break;
            case StatusArtigo.Revisando:
                tipo = "Revisando";
                break;
            case StatusArtigo.Corrigindo:
                tipo = "Corrigindo";
                break;
            case StatusArtigo.Publicado:
                tipo = "Publicado";
                break;
        }

        return tipo;
    }
}