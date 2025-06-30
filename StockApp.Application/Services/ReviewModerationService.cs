using StockApp.Domain.Interfaces;

public class ReviewModerationService : IReviewModerationService
{
    public bool ModerateReview(string review)
    {
     
        return !review.Contains("inapropriado", StringComparison.OrdinalIgnoreCase);
    }
}
