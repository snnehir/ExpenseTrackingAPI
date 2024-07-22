global using Microsoft.AspNetCore.Mvc;
global using ExpenseTrackingApp.Infrastructure.Data;
global using Microsoft.EntityFrameworkCore;
global using ExpenseTrackingApp.WebAPI.Extensions;
global using ExpenseTrackingApp.Infrastructure.Repositories.UserRepository;
global using ExpenseTrackingApp.Services.Services.AppUser;
global using ExpenseTrackingApp.Services.Services.Auth;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using Microsoft.IdentityModel.Tokens;
global using ExpenseTrackingApp.WebAPI.Models;
global using System.Security.Cryptography;
global using System.Text;
global using static ExpenseTrackingApp.WebAPI.Models.TokenModel;
global using ExpenseTrackingApp.DataTransferObjects.Responses;
global using ExpenseTrackingApp.Services.Helpers;
global using ExpenseTrackingApp.WebAPI.Methods;
global using System.Text.Json;
global using Mapster;
global using Microsoft.Extensions.Caching.Distributed;
global using Microsoft.Extensions.Caching.Memory;
global using ExpenseTrackingApp.DataTransferObjects.Requests;
global using Microsoft.OpenApi.Models;
global using System.Reflection;
global using ExpenseTrackingApp.Services.Services.Mail;








