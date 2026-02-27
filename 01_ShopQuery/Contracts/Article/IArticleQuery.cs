namespace _01_ShopQuery.Contracts.Article;

public interface IArticleQuery
{
    List<ArticleQueryModel> GetLatestArticles();
    ArticleQueryModel GetArticleDetails(string slug);
}