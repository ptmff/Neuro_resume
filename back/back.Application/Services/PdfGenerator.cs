using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using back.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using back.Application.Interfaces;

namespace back.Application.Services;

public class PdfGenerator : IPdfGenerator
{
    public async Task<byte[]> GeneratePdfAsync(string htmlContent)
    {
        await new BrowserFetcher().DownloadAsync();
        var launchOptions = new LaunchOptions { Headless = true };
        using (var browser = await Puppeteer.LaunchAsync(launchOptions))
        using (var page = await browser.NewPageAsync())
        {
            await page.SetContentAsync(htmlContent);
            return await page.PdfDataAsync();
        }
    }
}