namespace ForumCustom.BLL.Contract.Transform
{
    public interface ITransform<T1, T2>
        where T1 : class
    {
        T1 Transform(T2 item);

        T2 Transform(T1 item);
    }
}