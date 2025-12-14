using Cartify.Domain.Entities;
using Cartify.Domain.Interfaces.Repositories;
using Cartify.Domain.Models;
using Cartify.Infrastructure.Persistence;

namespace Cartify.Infrastructure.Implementation.Services
{
    public class UnitOfWork : IUnitOfWork
	{

		private readonly AppDbContext _context;
		public IRepository<PasswordResetCode> PasswordResetCodess { get; }
        public IProfileRepository ProfileRepository { get; }


        public IRepository<TblUserStore> UserStorerepository { get; }

        public IRepository<TblProduct> ProductRepository { get;  }

        public IRepository<TblType>  SubCategoryRepository { get; }

        public IRepository<TblCategory> CategoryRepository { get; }

        public IRepository<TblInventory> InventoryRepository { get; }

        public IRepository<TblOrder> OrderRepository { get; }

        public IRepository<TblOrderDetail> OrderDetailsRepository { get; }

        public IRepository<TblProductImage> ImagesRepository { get; }
        public IRepository<TblProductDetail> ProductDetails { get; }
        public IRepository<TblProductDetail> ProductDetailsRepository { get; }

        public IRepository<TblReview> ReviewRepository { get; }

        public IRepository<lkpAttribute> AttributeRepository { get; }

        public IRepository<LkpMeasureUnite> MeasureUnitRepository { get; }

        public IRepository<LkpOrderStatue> OrderStatusRepository { get; }

        public ICheckoutRepository CheckoutRepository { get; }

        public IOrdertrackingRepository OrdertrackingRepository { get; }

		public IRepository<Ticket> TicketRepository { get; }
		public IRepository<Ticket>WishlistRepository { get; }

        public IRepository<TblCart> CartRepository { get; }

        public UnitOfWork(
			AppDbContext context,
            IRepository<LkpOrderStatue> OrderStatusRepository,
            IRepository<PasswordResetCode> passwordResetCodess,
			IProfileRepository ProfileRepository,
            IRepository<TblUserStore> UserStorerepository,
			IRepository<TblProduct> productRepository,
			IRepository<TblType> subCategoryRepository,
			IRepository<TblCategory> categoryRepository,
			IRepository<TblInventory> inventoryRepository,
			IRepository<TblOrder> orderRepository,
			IRepository<TblOrderDetail> orderDetailsRepository,
			IRepository<TblProductImage> imagesRepository,
			IRepository<TblProductDetail> productDetails,
			IRepository<TblReview> reviewRepository,
			IRepository<lkpAttribute> attributeRepository,
			IRepository<LkpMeasureUnite> measureUnitRepository,
			ICheckoutRepository checkoutRepository,
			IOrdertrackingRepository ordertrackingRepository,
			IRepository<Ticket> TicketRepository,
            IRepository<Ticket> WishlistRepository,
            IRepository<TblCart> cartRepository
			)
		{

            _context = context;
			this.PasswordResetCodess = passwordResetCodess;
			this.ProfileRepository = ProfileRepository;
			this.AttributeRepository = attributeRepository;
			this.UserStorerepository = UserStorerepository;
			this.ProductRepository = productRepository;
			this.SubCategoryRepository = subCategoryRepository;
			this.CategoryRepository = categoryRepository;
			this.InventoryRepository = inventoryRepository;
			this.OrderRepository = orderRepository;
			this.OrderDetailsRepository = orderDetailsRepository;
			this.ImagesRepository = imagesRepository;
			this.ProductDetailsRepository = productDetails;
			this.ReviewRepository = reviewRepository;
			this.MeasureUnitRepository = measureUnitRepository;
			this.OrderStatusRepository = OrderStatusRepository;
			this.CheckoutRepository = checkoutRepository;
			this.OrdertrackingRepository = ordertrackingRepository;
			this.CartRepository = cartRepository;
			this.TicketRepository=TicketRepository;
			this.WishlistRepository=WishlistRepository;
        }

		public void Dispose()
		{
			_context.Dispose();
		}

		public async Task<int> SaveChanges()
		{
			return await _context.SaveChangesAsync();

		}
	}
}
