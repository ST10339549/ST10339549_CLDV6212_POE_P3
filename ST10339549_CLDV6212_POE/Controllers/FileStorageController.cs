﻿using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ST10339549_CLDV6212_POE.Controllers
{
    public class FileStorageController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _azureFunctionBaseUrl;
        private readonly string _fileUploadFunctionKey;
        private readonly string _fileDeleteFunctionKey;
        private readonly string _fileGetFunctionKey;


        public FileStorageController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _azureFunctionBaseUrl = configuration["AzureFunctionSettings:BaseUrl"];
            _fileUploadFunctionKey = configuration["AzureFunctionSettings:UploadFileFunctionKey"];
            _fileDeleteFunctionKey = configuration["AzureFunctionSettings:DeleteFileFunctionKey"];
            _fileGetFunctionKey = configuration["AzureFunctionSettings:GetFileFunctionKey"];
        }

        public async Task<IActionResult> Index()
        {
            var files = await GetFileUrlsAsync();
            ViewBag.Files = files;
            return View(files);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile formFile)
        {
            if (formFile != null && formFile.Length > 0)
            {
                var uploadUrl = $"{_azureFunctionBaseUrl}api/upload-file?code={_fileUploadFunctionKey}";
                var formFileName = formFile.FileName;

                using (var stream = formFile.OpenReadStream())
                {
                    var content = new MultipartFormDataContent();
                    var streamContent = new StreamContent(stream);
                    content.Add(streamContent, "file", formFileName);

                    var response = await _httpClient.PostAsync(uploadUrl, content);
                    response.EnsureSuccessStatusCode();
                }

                TempData["Message"] = "File uploaded successfully!";
            }
            else
            {
                TempData["Error"] = "Please select a valid file.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var deleteUrl = $"{_azureFunctionBaseUrl}api/delete-file/{fileName}?code={_fileDeleteFunctionKey}";
                var response = await _httpClient.DeleteAsync(deleteUrl);
                response.EnsureSuccessStatusCode();

                TempData["Message"] = $"File '{fileName}' deleted successfully!";
            }
            else
            {
                TempData["Error"] = "File name cannot be empty.";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<List<string>> GetFileUrlsAsync()
        {
            var getUrl = $"{_azureFunctionBaseUrl}api/get-file-urls?code={_fileGetFunctionKey}";
            var response = await _httpClient.GetAsync(getUrl);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var fileUrls = JsonSerializer.Deserialize<List<string>>(jsonResponse);

            return fileUrls ?? new List<string>();
        }
    }
}
