using MarketLink.Data;
using MarketLink.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace MarketLink.Services;

public interface IAuthService
{
    Task<User?> LoginAsync(string email, string password);
    Task<User> RegisterAsync(string name, string email, string password, UserRole role, string? phone, string? address);
}
public class AuthService : IAuthService
{
    private readonly MarketLinkContext _db;
    public AuthService(MarketLinkContext db) => _db=db;
    public async Task<User?> LoginAsync(string email,string password)
    {
        var hash=Hash(password);
        return await _db.Users.FirstOrDefaultAsync(x=>x.Email.ToLower()==email.ToLower() && x.PasswordHash==hash);
    }
    public async Task<User> RegisterAsync(string name,string email,string password,UserRole role,string? phone,string? address)
    {
        if(await _db.Users.AnyAsync(x=>x.Email.ToLower()==email.ToLower())) throw new InvalidOperationException("Email is already registered.");
        var user=new User{FullName=name,Email=email.Trim(),PasswordHash=Hash(password),Role=role,Phone=phone,Address=address};
        _db.Users.Add(user); await _db.SaveChangesAsync(); return user;
    }
    public static string Hash(string value){using var sha=SHA256.Create(); return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(value)));}
}

public interface IProductService
{
    Task<List<Product>> GetAsync(string? search=null,string? category=null);
    Task<Product?> GetByIdAsync(int id);
    Task<Product> AddAsync(Product p);
    Task UpdateAsync(Product p);
    Task DeleteAsync(int id);
}
public class ProductService : IProductService
{
    private readonly MarketLinkContext _db; public ProductService(MarketLinkContext db)=>_db=db;
    public async Task<List<Product>> GetAsync(string? search=null,string? category=null)
    {
        var q=_db.Products.Include(x=>x.Farmer).Where(x=>x.IsActive).AsQueryable();
        if(!string.IsNullOrWhiteSpace(search)) q=q.Where(x=>x.Name.Contains(search)||x.Category.Contains(search));
        if(!string.IsNullOrWhiteSpace(category)) q=q.Where(x=>x.Category==category);
        return await q.OrderByDescending(x=>x.CreatedAt).ToListAsync();
    }
    public Task<Product?> GetByIdAsync(int id)=>_db.Products.Include(x=>x.Farmer).Include(x=>x.Reviews).ThenInclude(x=>x.Customer).FirstOrDefaultAsync(x=>x.Id==id);
    public async Task<Product> AddAsync(Product p){_db.Products.Add(p);await _db.SaveChangesAsync();return p;}
    public async Task UpdateAsync(Product p){_db.Products.Update(p);await _db.SaveChangesAsync();}
    public async Task DeleteAsync(int id){var p=await _db.Products.FindAsync(id);if(p!=null){p.IsActive=false;await _db.SaveChangesAsync();}}
}

public interface IOrderService
{
    Task<Order> CreateAsync(int customerId, List<(int productId,int quantity)> items, string address);
    Task<List<Order>> ForCustomerAsync(int customerId);
    Task<List<Order>> ForFarmerAsync(int farmerId);
    Task<List<Order>> AllAsync();
    Task UpdateStatusAsync(int id, OrderStatus status);
}
public class OrderService : IOrderService
{
    private readonly MarketLinkContext _db; public OrderService(MarketLinkContext db)=>_db=db;
    public async Task<Order> CreateAsync(int customerId,List<(int productId,int quantity)> items,string address)
    {
        if(items.Count==0) throw new InvalidOperationException("Cart is empty.");
        var ids=items.Select(x=>x.productId).ToList();
        var products=await _db.Products.Where(x=>ids.Contains(x.Id)&&x.IsActive).ToListAsync();
        if(products.Count!=items.Select(x=>x.productId).Distinct().Count()) throw new InvalidOperationException("One or more products are unavailable.");
        var order=new Order{CustomerId=customerId,DeliveryAddress=address};
        foreach(var item in items)
        {
            var p=products.First(x=>x.Id==item.productId);
            if(item.quantity<=0 || p.StockQuantity<item.quantity) throw new InvalidOperationException($"Insufficient stock for {p.Name}.");
            p.StockQuantity-=item.quantity;
            order.Items.Add(new OrderItem{ProductId=p.Id,Quantity=item.quantity,UnitPrice=p.Price});
            order.TotalAmount+=p.Price*item.quantity;
        }
        _db.Orders.Add(order); await _db.SaveChangesAsync(); return order;
    }
    public Task<List<Order>> ForCustomerAsync(int id)=>_db.Orders.Include(x=>x.Items).ThenInclude(x=>x.Product).Where(x=>x.CustomerId==id).OrderByDescending(x=>x.OrderDate).ToListAsync();
    public Task<List<Order>> ForFarmerAsync(int id)=>_db.Orders.Include(x=>x.Items).ThenInclude(x=>x.Product).Where(x=>x.Items.Any(i=>i.Product!.FarmerId==id)).OrderByDescending(x=>x.OrderDate).ToListAsync();
    public Task<List<Order>> AllAsync()=>_db.Orders.Include(x=>x.Customer).Include(x=>x.Items).ThenInclude(x=>x.Product).OrderByDescending(x=>x.OrderDate).ToListAsync();
    public async Task UpdateStatusAsync(int id,OrderStatus status){var o=await _db.Orders.FindAsync(id);if(o==null)throw new InvalidOperationException("Order not found.");o.Status=status;await _db.SaveChangesAsync();}
}