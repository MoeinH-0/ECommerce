using _01_ShopQuery.Contracts.Article;
using Microsoft.AspNetCore.Mvc;

namespace BlogManagement.Presentation.Api;

[ApiController]
[Route("api/[controller]")]
public class ArticleController
{
    private readonly IArticleQuery _articleQuery;

    public ArticleController(IArticleQuery articleQuery)
    {
        _articleQuery = articleQuery;
    }
    
    [HttpGet]
    public List<ArticleQueryModel> GetLatestArticles()
    {
        return _articleQuery.GetLatestArticles();
    }
}