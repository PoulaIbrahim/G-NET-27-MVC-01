using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using GymMangement.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _context;
        private readonly ISessionRepository _sessionRepository;
        private readonly Dictionary<string, object> _repositories = [];

        public UnitOfWork(GymDbContext context, ISessionRepository sessionRepository)
        {
            _context = context;
            _sessionRepository = sessionRepository;
        }

        public ISessionRepository sessionRepository => _sessionRepository;

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            // Generate Repository Of TEntity
            // Check IEntity If Exists Or Not in Memory
            var typeName = typeof(TEntity).Name;

            if (_repositories.TryGetValue(typeName, out object? value))
                return (IGenericRepository<TEntity>)value!;

            var repo = new GenericRepository<TEntity>(_context);
            _repositories.Add(typeName, repo);

            return repo;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
           => await _context.SaveChangesAsync(ct);
    }
}
