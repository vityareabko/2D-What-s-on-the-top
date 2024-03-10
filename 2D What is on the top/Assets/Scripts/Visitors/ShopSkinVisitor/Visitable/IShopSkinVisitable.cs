namespace ShopSkinVisitor.Visitable
{
    public interface IShopSkinVisitable
    {
        public void Accept(IShopSkinVisitor visitor);
    }
}