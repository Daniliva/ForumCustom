namespace ForumCustom.BLL.Contract
{
    public interface IManagerFactory
    {
        T GetManager<T>();
    }
}