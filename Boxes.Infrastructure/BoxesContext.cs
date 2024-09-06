using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrationEventLogEF;
using Boxes.Domain.AggregatesModel.BoxAggregate;
using MediatR;
using Boxes.Infrastructure.Repositories;
using System.Data;
using Boxes.Infrastructure.EntityConfigurations;
using Boxes.Domain.AggregatesModel.UserAggregate;

namespace Boxes.Infrastructure;

public class BoxesContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;
    private IDbContextTransaction _currentTransaction;
    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;                 
    public bool HasActiveTransaction => _currentTransaction != null;


    //"Host=localhost;Database=boxesservicedb;Username=postgres;Password=postgres"
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseNpgsql("Host=localhost;Database=boxesservicedb;Username=postgres;Password=postgres");

    
    public BoxesContext(DbContextOptions<BoxesContext> options) : base(options) { }
    public BoxesContext(DbContextOptions<BoxesContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        System.Diagnostics.Debug.WriteLine("OrderingContext::ctor ->" + this.GetHashCode());
    }
    
    public virtual DbSet<Box> Boxes { get; set; }

    public virtual DbSet<BoxContent> BoxContents { get; set; }

    //public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.HasDefaultSchema("boxesservicedb");
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ClientRequestEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UserInfoEntityTypeConfiguration());
        modelBuilder.UseIntegrationEventLogs();
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEventsAsync(this);
        _ = await base.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null) return null;

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken token)
    {
        if (_currentTransaction == null) throw new Exception();
        if (transaction != _currentTransaction) throw new Exception();

        try
        {
            await SaveChangesAsync();
            await _currentTransaction.CommitAsync(token);
        }
        catch (Exception ex) {

            RollbackTransaction();
        }
        finally{
            _currentTransaction?.Dispose();
        }

    }
    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (HasActiveTransaction)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        };

    }
    public void CreateSavepoint(string name,CancellationToken token)
    {
        if (_currentTransaction == null) return;
        _currentTransaction.CreateSavepoint(name);
    }

    public void RollbackToSavepoint(string name)
    {
        if (_currentTransaction == null) return;
        _currentTransaction.RollbackToSavepoint(name);
    }


}
