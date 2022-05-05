using Microsoft.AspNetCore.Mvc;
using ShoppingCart.EventFeed;

namespace ShoppingCart.ShoppingCart;

[Route("/shoppingcart")]
public class ShoppingCartController : Controller
{
    private readonly IShoppingCartStore _shoppingCartStore;
    private readonly IProductCatalogClient _productCatalogClient;
    private readonly IEventStore _eventStore;

    public ShoppingCartController(IShoppingCartStore shoppingCartStore, IEventStore eventStore, IProductCatalogClient productCatalogClient)
    {
        _shoppingCartStore = shoppingCartStore;
        _eventStore = eventStore;
        _productCatalogClient = productCatalogClient;
    }

    [HttpGet("{userId:int}")]
    public ShoppingCart Get(int userId) => _shoppingCartStore.Get(userId);

    [HttpPost("{userId:int}/items")]
    public async Task<ShoppingCart> Post(int userId, [FromBody] int[] productIds)
    {
        var shoppingCart = _shoppingCartStore.Get(userId);
        var shoppingCartItems = await _productCatalogClient.GetShoppingCartItem(productIds);
        
        shoppingCart.AddItems(shoppingCartItems, _eventStore);
        _shoppingCartStore.Save(shoppingCart);

        return shoppingCart;
    }

    [HttpDelete("{userId:int}/items")]
    public ShoppingCart Delete(int userId, [FromBody] int[] productIds)
    {
        var shoppingCart = _shoppingCartStore.Get(userId);
        shoppingCart.RemoveItems(productIds, _eventStore);
        
        _shoppingCartStore.Save(shoppingCart);

        return shoppingCart;
    }
}
