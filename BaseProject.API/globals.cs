global using AutoMapper;
global using MediatR;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;
global using BaseProject.Application.Services.Authentication.Commands;
global using BaseProject.Domain.Models;
global using BaseProject.Domain.Models.Context;
global using BaseProject.Domain.Models.Dtos.Authorization;
global using BaseProject.Domain.Models.ViewModels;
global using BaseProject.Service.Middlewares;
global using BaseProject.Service.Utilities;
global using BaseProject.Service.Utilities.DependencyResolvers;
global using BaseProject.Service.Utilities.Helpers;
global using System.Text;
global using System.Threading.Tasks;
global using BaseProject.Service.Services.Authentication.Commands;
global using BaseProject.Service.Services.Authentication.Queries;
global using Microsoft.AspNetCore.Authorization;
global using BaseProject.Domain.Models.Dtos.User;
global using BaseProject.Service.Services.Admin.Commands;
global using BaseProject.Service.Services.Admin.Queries;
global using Kendo.Mvc.UI;
global using Microsoft.AspNetCore.Http;
