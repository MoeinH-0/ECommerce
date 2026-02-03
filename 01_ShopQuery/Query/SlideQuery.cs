using _01_ShopQuery.Contracts.Slide;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Infrastructure.EFCore;

namespace _01_ShopQuery.Query;

public class SlideQuery : ISlideQuery
{
    private readonly ShopContext _context;

    public SlideQuery(ShopContext context)
    {
        _context = context;
    }

    public List<SlideQueryModel> GetSlides()
    {
        return _context.Slides
            .Where(x => !x.IsRemoved)
            .Select(x => new SlideQueryModel
            {
                Link = x.Link,
                Title = x.Title,
                Text = x.Text,
                BtnText = x.BtnText,
                Heading = x.Heading,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle
            }).AsNoTracking().ToList();
    }
}