using Xunit;

public class SentimentAnalysisServiceTests
{
    [Theory]
    [InlineData("Este produto é ótimo!", "Positivo")]
    [InlineData("O serviço foi excelente e maravilhoso.", "Positivo")]
    [InlineData("Produto ruim e horrível.", "Negativo")]
    [InlineData("Não gostei, foi péssimo.", "Negativo")]
    [InlineData("Produto comum, nada demais.", "Neutro")]
    [InlineData("", "Neutro")]
    [InlineData(null, "Neutro")]
    [InlineData("Gostei e odiei ao mesmo tempo.", "Neutro")]
    public void AnalyzeSentiment_DeveRetornarSentimentoCorreto(string review, string esperado)
    {
        var service = new SentimentAnalysisService();

        var resultado = service.AnalyzeSentiment(review);

        Assert.Equal(esperado, resultado);
    }
}