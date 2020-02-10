using System;
namespace OOPTest_NadyaRF
{
    public interface ICart
    {
        ICart addItem(int item_ID, int price, int quantity = 1);
        ICart removeItem(int item_ID);
        ICart addDiscount(string discount);
    }
}
