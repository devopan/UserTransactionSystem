using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserTransactionSystem.Infrastructure.Data
{
    public class ApplicationDbScopedFactory : IDbContextFactory<ApplicationDbContext>
    {
        private const int DefaultTenantId = -1;

        private readonly IDbContextFactory<ApplicationDbContext> _pooledFactory;

        public ApplicationDbScopedFactory(
            IDbContextFactory<ApplicationDbContext> pooledFactory)
        {
            _pooledFactory = pooledFactory;
        }

        public ApplicationDbContext CreateDbContext()
        {
            return _pooledFactory.CreateDbContext();
        }
    }
}
