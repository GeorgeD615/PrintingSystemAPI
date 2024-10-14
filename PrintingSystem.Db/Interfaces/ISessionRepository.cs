﻿using Microsoft.AspNetCore.Http;
using PrintingSystem.Db.Models;

namespace PrintingSystem.Db.Interfaces
{
    public interface ISessionRepository
    {
        Task<bool> Create(Session session, int? installationOrderNumber, bool simulateDelay = true);
        Task<int> ProcessSessionsFromCsvAsync(IFormFile file);
    }
}