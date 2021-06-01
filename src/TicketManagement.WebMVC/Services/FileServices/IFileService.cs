﻿using Microsoft.AspNetCore.Http;

namespace TicketManagement.WebMVC.Services.FileServices
{
    public interface IFileService
    {
        string ConvertFileToString(IFormFile file);

        string SaveImageToFolder(string base64String);
    }
}