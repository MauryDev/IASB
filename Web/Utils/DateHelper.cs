using System.Globalization;

namespace Web.Utils;

public static class DateHelper
{
    private static readonly CultureInfo CulturaPtBr = new ("pt-BR");

    public static string GetMesAnoName(int mes, int ano)
    {
        var data = new DateTime(ano, mes, 1);

        return data.ToString("MMMM 'de' yyyy", CulturaPtBr);
    }
    public static IEnumerable<DateTime> GetSabados(int ano, int trimestre)
    {
        int mesInicio = (trimestre - 1) * 3 + 1;
        int mesFim = mesInicio + 2;
        for (int mes = mesInicio; mes <= mesFim; mes++)
        {
            var diasNoMes = DateTime.DaysInMonth(ano, mes);
            for (int dia = 1; dia <= diasNoMes; dia++)
            {
                var data = new DateTime(ano, mes, dia);
                if (data.DayOfWeek == DayOfWeek.Saturday)
                {
                    yield return data;
                }
            }
        }
    }
}
