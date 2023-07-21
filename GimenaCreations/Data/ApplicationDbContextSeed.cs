using Microsoft.Data.SqlClient;
using Polly.Retry;
using Polly;
using System.IO.Compression;
using Microsoft.AspNetCore.Identity;
using GimenaCreations.Entities;
using GimenaCreations.Constants;

namespace GimenaCreations.Data;

public class ApplicationDbContextSeed
{
    public async Task SeedAsync(ApplicationDbContext context,
        IWebHostEnvironment environment,
        ILogger<ApplicationDbContextSeed> logger,
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        var policy = CreatePolicy(logger, nameof(ApplicationDbContextSeed));

        await policy.ExecuteAsync(async () =>
        {
            var contentRootPath = environment.ContentRootPath;
            var picturePath = environment.WebRootPath;

            if (!context.CatalogTypes.Any())
            {
                await context.CatalogTypes.AddRangeAsync(GetPreconfiguredCatalogTypes());
                await context.SaveChangesAsync();
            }

            if (!context.CatalogItems.Any())
            {
                await context.CatalogItems.AddRangeAsync(GetPreconfiguredItems());
                await context.SaveChangesAsync();
                GetCatalogItemPictures(contentRootPath, picturePath);
            }

            if (!context.Roles.Any())
            {
                GetIdentityRoles().ToList().ForEach(x => roleManager.CreateAsync(x).Wait());
            }            
        });
    }

    private static void GetCatalogItemPictures(string contentRootPath, string picturePath)
    {
        if (picturePath != null)
        {
            DirectoryInfo directory = new(picturePath);
            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }

            string zipFileCatalogItemPictures = Path.Combine(contentRootPath, "Setup", "CatalogItems.zip");
            ZipFile.ExtractToDirectory(zipFileCatalogItemPictures, picturePath);
        }
    }

    private static IEnumerable<CatalogItem> GetPreconfiguredItems() => new List<CatalogItem>()
        {
            new() { CatalogTypeId = 2, AvailableStock = 100, Description = ".NET Bot Black Hoodie", Name = ".NET Bot Black Hoodie", Price = 19.5M, PictureFileName = "1.png" },
            new() { CatalogTypeId = 1, AvailableStock = 100, Description = ".NET Black & White Mug", Name = ".NET Black & White Mug", Price= 8.50M, PictureFileName = "2.png" },
            new() { CatalogTypeId = 2, AvailableStock = 100, Description = "Prism White T-Shirt", Name = "Prism White T-Shirt", Price = 12, PictureFileName = "3.png" },
            new() { CatalogTypeId = 2, AvailableStock = 100, Description = ".NET Foundation T-shirt", Name = ".NET Foundation T-shirt", Price = 12, PictureFileName = "4.png" },
            new() { CatalogTypeId = 3, AvailableStock = 100, Description = "Roslyn Red Sheet", Name = "Roslyn Red Sheet", Price = 8.5M, PictureFileName = "5.png" },
            new() { CatalogTypeId = 2, AvailableStock = 100, Description = ".NET Blue Hoodie", Name = ".NET Blue Hoodie", Price = 12, PictureFileName = "6.png" },
            new() { CatalogTypeId = 2, AvailableStock = 100, Description = "Roslyn Red T-Shirt", Name = "Roslyn Red T-Shirt", Price = 12, PictureFileName = "7.png" },
            new() { CatalogTypeId = 2, AvailableStock = 100, Description = "Kudu Purple Hoodie", Name = "Kudu Purple Hoodie", Price = 8.5M, PictureFileName = "8.png" },
            new() { CatalogTypeId = 1, AvailableStock = 100, Description = "Cup<T> White Mug", Name = "Cup<T> White Mug", Price = 12, PictureFileName = "9.png" },
            new() { CatalogTypeId = 3, AvailableStock = 100, Description = ".NET Foundation Sheet", Name = ".NET Foundation Sheet", Price = 12, PictureFileName = "10.png" },
            new() { CatalogTypeId = 3, AvailableStock = 100, Description = "Cup<T> Sheet", Name = "Cup<T> Sheet", Price = 8.5M, PictureFileName = "11.png" },
            new() { CatalogTypeId = 2, AvailableStock = 100, Description = "Prism White TShirt", Name = "Prism White TShirt", Price = 12, PictureFileName = "12.png" },
        };

    private static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes() => new List<CatalogType>()
        {
            new() { Type = "Mug"},
            new() { Type = "T-Shirt" },
            new() { Type = "Sheet" },
            new() { Type = "USB Memory Stick" }
        };

    private static IEnumerable<IdentityRole> GetIdentityRoles() => new List<IdentityRole>
        {
            new(){Name="Admin"},
            new(){Name="Manager"}
        };

    private static AsyncRetryPolicy CreatePolicy(ILogger<ApplicationDbContextSeed> logger, string prefix, int retries = 3) => Policy.Handle<SqlException>().
            WaitAndRetryAsync(
                retryCount: retries,
                sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                onRetry: (exception, timeSpan, retry, ctx) =>
                {
                    logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}",
                        prefix, exception.GetType().Name, exception.Message, retry, retries);
                }
            );
}
