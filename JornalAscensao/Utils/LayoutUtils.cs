namespace JornalAscensao.Utils;

public class LayoutUtils
{
    public static string GetData()
    {
        var data = DateTime.Now;
        string dataComDiaMesExtenso = data.ToString("dddd, dd 'de' MMMM 'de' yyyy");
        return dataComDiaMesExtenso;
    }

    public static string GetData(DateTime data)
    {
        return data.ToString("MM/dd/yyyy");

    }

    public static string GetMesAno(DateTime data)
    {
        return data.ToString("MMMM 'de' yyyy");
    }
}