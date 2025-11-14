using Cartify.Domain.Entities;
using Cartify.Domain.Models;

namespace Cartify.Domain.Interfaces.Repositories;
public interface IUnitOfWork : IDisposable
{
    public IRepository<LkpOrderStatue> OrderStatusRepository { get; }
    public IRepository<PasswordResetCode> PasswordResetCodess { get; }
	public IRepository<TblUserStore> UserStorerepository { get; }
    public IProfileRepository ProfileRepository { get; }
    public IRepository<TblProduct> ProductRepository { get; }
    public IRepository<TblType> SubCategoryRepository { get; }
    public IRepository<TblCategory> CategoryRepository { get; }
    public IRepository<TblInventory> InventoryRepository { get; }
    public IRepository<TblOrder> OrderRepository { get; }
    public IRepository<TblOrderDetail> OrderDetailsRepository { get; }
    public IRepository<TblProductImage> ImagesRepository { get; }
    public IRepository<TblProductDetail> ProductDetailsRepository { get; }
    public IRepository<TblReview> ReviewRepository { get; }
    public IRepository<LkpPromotion> PromotionsRepository { get; }
    public IRepository<lkpAttribute> AttributeRepository { get; }
    public IRepository<LkpMeasureUnite> MeasureUnitRepository { get; }
    public ICheckoutRepository CheckoutRepository { get; }
    public IOrdertrackingRepository OrdertrackingRepository { get; }
	public IRepository<Ticket> TicketRepository {  get; }

	Task<int> SaveChanges();

}
