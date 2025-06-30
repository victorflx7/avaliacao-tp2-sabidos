using StockApp.Application.Services;
using Xunit;

public class ReviewModerationServiceTests
{
    [Theory]
    [InlineData("Este produto é ótimo!", true)]
    [InlineData("Este review é inapropriado", false)]
    [InlineData("Nada de inapropriado aqui", false)]
    [InlineData("Excelente qualidade", true)]
    public void ModerateReview_DeveDetectarConteudoInapropriado(string review, bool esperado)
    {
        
        var service = new ReviewModerationService();

        
        var resultado = service.ModerateReview(review);

        
        Assert.Equal(esperado, resultado);
    }
}