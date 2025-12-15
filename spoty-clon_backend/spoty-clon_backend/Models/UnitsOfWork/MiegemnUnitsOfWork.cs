using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using spoty_clon_backend.Models.Context;

namespace spoty_clon_backend.Models.UnitsOfWork
{
    public sealed class MeigemnUnitOfWork : IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        private readonly MeigemnDbContext _context;

        // Repositorios
        private MeigemnRepository<IdentityUser>? _usersRepository;
        private MeigemnRepository<Product>? _productsRepository;
        private MeigemnRepository<Order>? _ordersRepository;
        private MeigemnRepository<OrderDetail>? _orderDetailsRepository;

        // Propiedad de acceso a la base de datos
        public Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade Database => _context.Database;

        // Constructor
        public MeigemnUnitOfWork(MeigemnDbContext context, IServiceProvider serviceProvider, ILogger logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // --- Repositorios ---
        public MeigemnRepository<IdentityUser> Users => _usersRepository ??= GetRepository<IdentityUser>();
        public MeigemnRepository<Product> Products => _productsRepository ??= GetRepository<Product>();
        public MeigemnRepository<Order> Orders => _ordersRepository ??= GetRepository<Order>();
        public MeigemnRepository<OrderDetail> OrderDetails => _orderDetailsRepository ??= GetRepository<OrderDetail>();

        // --- Métodos de Transacción y Guardado ---

        /// <summary>
        /// Inicia una nueva transacción de base de datos especificando el Nivel de Aislamiento.
        /// </summary>

        public Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel)
        {
            // Usamos CancellationToken.None para satisfacer la sobrecarga de tres argumentos requerida por EF Core:
            // BeginTransactionAsync(IsolationLevel, CancellationToken)
            return _context.Database.BeginTransactionAsync(isolationLevel, CancellationToken.None);
        }

        public async Task Complete()
        {
            await _context.SaveChangesAsync();
        }

        // --- Método Auxiliar ---
        private MeigemnRepository<T> GetRepository<T>() where T : class
        {
            return _serviceProvider.GetRequiredService(typeof(MeigemnRepository<T>)) as MeigemnRepository<T>;
        }

        // --- Implementación IDisposable ---
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
