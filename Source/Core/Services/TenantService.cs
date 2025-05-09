﻿using Contracts;
using Entities.Entities;
using Entities.Exceptions;
using Entities.LinkModels;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Services;

internal sealed class TenantService : ITenantService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IDataShaper<TenantDto> _dataShaper;
    private readonly ITenantLinks _tenantLinks;

    public TenantService(
        IRepositoryManager repository,
        ILoggerManager logger,
        IDataShaper<TenantDto> dataShaper,
        ITenantLinks tenantLinks)
    {
        _repository = repository;
        _logger = logger;
        _dataShaper = dataShaper;
        _tenantLinks = tenantLinks;
    }

    public async Task<(IEnumerable<TenantDto> tenants, MetaData metaData)> GetAllTenantsAsync(TenantParameters tenantParameters,
        bool trackChanges)
    {
        var tenantsWithMetaData = await _repository.Tenant.GetAllTenantsAsync(tenantParameters, trackChanges);
        var tenantsDto = tenantsWithMetaData.Select(t => new TenantDto
        {
            Id = t.Id,
            Title = t.Title,
            Address = t.Address
        }).ToList();

        return (tenants: tenantsDto, metaData: tenantsWithMetaData.MetaData);
    }

    public async Task<TenantDto> GetTenantAsync(Guid tenantId, bool trackChanges)
    {
        var tenant = await _repository.Tenant.GetTenantAsync(tenantId, trackChanges);
        if (tenant is null)
        {
            throw new TenantNotFoundException(tenantId);
        }

        return new TenantDto
        {
            Id = tenant.Id,
            Title = tenant.Title,
            Address = tenant.Address
        };
    }

    public async Task<TenantDto> CreateTenantAsync(TenantForCreationDto tenant)
    {
        var entity = new Tenant
        {
            Title = tenant.Title,
            Address = tenant.Address,
            IsActive = true
        };

        _repository.Tenant.CreateTenant(entity);
        await _repository.SaveChangesAsync();

        return new TenantDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Address = entity.Address
        };
    }

    public async Task<IEnumerable<TenantDto>> GetTenantsByIdAsync(IEnumerable<Guid> ids, bool trackChanges)
    {
        var tenants = await _repository.Tenant.GetTenantsByIdAsync(ids, trackChanges);
        if (tenants.Count() != ids.Count())
            throw new CollectionByIdsBadRequestException();

        return tenants.Select(t => new TenantDto
        {
            Id = t.Id,
            Title = t.Title,
            Address = t.Address
        }).ToList();
    }

    public async Task<(IEnumerable<TenantDto> tenants, string ids)> CreateTenantCollection(IEnumerable<TenantForCreationDto> tenantCollection)
    {
        if (tenantCollection is null)
            throw new TenantCollectionBadRequestException();

        var entities = new List<Tenant>();

        foreach (var tenant in tenantCollection)
        {
            var entity = new Tenant
            {
                Title = tenant.Title,
                Address = tenant.Address,
                IsActive = true
            };
            entities.Add(entity);

            _repository.Tenant.CreateTenant(entity);
        }

        await _repository.SaveChangesAsync();

        var tenantCollectionToReturn = entities.Select(t => new TenantDto
        {
            Id = t.Id,
            Title = t.Title,
            Address = t.Address
        }).ToList();

        var idsToReturn = string.Join(", ", tenantCollectionToReturn.Select(t => t.Id));

        return (tenants: tenantCollectionToReturn, ids: idsToReturn);
    }

    public async Task DeleteTenantAsync(Guid tenantId, bool trackChanges)
    {
        var tenant = await _repository.Tenant.GetTenantAsync(tenantId, trackChanges)
            ?? throw new TenantNotFoundException(tenantId);

        _repository.Tenant.DeleteTenant(tenant);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateTenantAsync(Guid tenantId, TenantForUpdateDto tenantForUpdate, bool trackChanges)
    {
        var tenantEntity = await _repository.Tenant.GetTenantAsync(tenantId, trackChanges)
            ?? throw new TenantNotFoundException(tenantId);

        tenantEntity.Title = tenantForUpdate.Title;
        tenantEntity.Address = tenantForUpdate.Address;
        await _repository.SaveChangesAsync();
    }

    public async Task<(IEnumerable<ShapedEntity> tenants, MetaData metaData)> GetAllDataShapedTenantsAsync(
        TenantParameters tenantParameters, bool trackChanges)
    {
        var tenantsWithMetaData = await _repository.Tenant.GetAllTenantsAsync(tenantParameters, trackChanges);
        var tenantsDto = tenantsWithMetaData.Select(t => new TenantDto
        {
            Id = t.Id,
            Title = t.Title,
            Address = t.Address
        }).ToList();

        var shapedData = _dataShaper.ShapeData(tenantsDto, tenantParameters.Fields);

        return (tenants: shapedData, metaData: tenantsWithMetaData.MetaData);
    }

    public async Task<(LinkResponse linkResponse, MetaData metaData)> GetHATEOASAllTenantsAsync(TenantLinkParameters linkParameters, bool trackChanges)
    {
        var tenantsWithMetaData = await _repository.Tenant.GetAllTenantsAsync(linkParameters.TenantParameters, trackChanges);
        var tenantsDto = tenantsWithMetaData.Select(t => new TenantDto
        {
            Id = t.Id,
            Title = t.Title,
            Address = t.Address
        }).ToList();

        var links = _tenantLinks.TryGenerateLinks(tenantsDto, linkParameters.TenantParameters.Fields, linkParameters.Context);

        return (linkResponse: links, metaData: tenantsWithMetaData.MetaData);
    }
}
