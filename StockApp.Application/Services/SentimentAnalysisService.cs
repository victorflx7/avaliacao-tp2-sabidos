using System;
using System.Collections.Generic;
using System.Linq;

public interface ISentimentAnalysisService
{
    string AnalyzeSentiment(string review);
}

public class SentimentAnalysisService : ISentimentAnalysisService
{
    private static readonly string[] PositiveWords = { "ótimo", "excelente", "bom", "maravilhoso", "recomendo", "gostei", "perfeito", "adoro" };
    private static readonly string[] NegativeWords = { "ruim", "péssimo", "horrível", "odiei", "terrível", "inapropriado", "detestei" };

    public string AnalyzeSentiment(string review)
    {
        if (string.IsNullOrWhiteSpace(review))
            return "Neutro";

        var reviewLower = review.ToLower();

        int positiveScore = PositiveWords.Count(word => reviewLower.Contains(word));
        int negativeScore = NegativeWords.Count(word => reviewLower.Contains(word));

        if (positiveScore > negativeScore)
            return "Positivo";
        if (negativeScore > positiveScore)
            return "Negativo";
        if (negativeScore > 0) 
            return "Negativo";
        return "Neutro";
    }
}