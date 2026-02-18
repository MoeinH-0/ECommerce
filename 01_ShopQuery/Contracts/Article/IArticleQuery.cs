namespace _01_ShopQuery.Contracts.Article;

public interface IArticleQuery
{
    List<ArticleQueryModel> LatestArticles();
    ArticleQueryModel GetArticleDetails(string slug);
}