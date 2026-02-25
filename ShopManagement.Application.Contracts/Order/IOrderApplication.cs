namespace ShopManagement.Application.Contracts.Order;

public interface IOrderApplication
{
    long PlaceOrder(Cart cart);
    string PaymentSucceeded(long orderId, long refId);
    void Cancel(long id);
    double GetAmountBy(long id);
    List<OrderItemViewModel> GetItems(long orderId);
    List<OrderViewModel> Search(OrderSearchModel searchModel);
}