namespace _01_ShopQuery.Contracts.ArticleCategory;

public interface IArticleCategoryQuery
{
    ArticleCategoryQueryModel GetArticleCategory(string slug);
    List<ArticleCategoryQueryModel> GetArticleCategories();
}