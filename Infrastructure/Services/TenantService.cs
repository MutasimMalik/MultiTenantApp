using Core.Interfaces;
using Core.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Infrastructure.Services
{
    public class TenantService : ITenantService
    {
        private readonly TenantSettings _tenantSettings;
        private HttpContext _httpContext;
        private Tenant _currentTenant;

        public TenantService(IOptions<TenantSettings> tenantSettings, IHttpContextAccessor contextAccessor)
        {
            _tenantSettings = tenantSettings.Value;
            _httpContext = contextAccessor.HttpContext;

            if(_httpContext != null)
            {
                if (_httpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
                    SetTenant(tenantId);
                else
                    throw new Exception("Invalid Tenant!");
            }
        }

        public string GetConnectionString()
        {
            return _currentTenant?.ConnectionString;
        }

        public string GetDatabaseProvider()
        {
            return _tenantSettings.DefaultConfiguration?.DBProvider;
        }

        public Tenant GetTenant()
        {
            return _currentTenant;
        }

        private void SetTenant(string tenantId)
        {
            _currentTenant = _tenantSettings.Tenants.Where(x=>x.TID == tenantId).FirstOrDefault();
            if(_currentTenant == null)
                throw new Exception("Invalid Tenant!");
            if (string.IsNullOrEmpty(_currentTenant.ConnectionString))
                SetDefaultConnectionString();
        }

        private void SetDefaultConnectionString()
        {
            _currentTenant.ConnectionString = _tenantSettings.DefaultConfiguration.ConnectionString;
        }
    }
}
