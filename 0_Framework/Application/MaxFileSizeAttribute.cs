using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace _0_Framework.Application;

public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly int _maxFileSize;

    public MaxFileSizeAttribute(int maxFileSize)
    {
        _maxFileSize = maxFileSize;
    }

    public override bool IsValid(object value)
    {
        var file = value as IFormFile;
        return file != null && file.Length <= _maxFileSize;
    }
}