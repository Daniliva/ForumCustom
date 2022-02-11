namespace ForumCustom.WEB.Domain.Transform
{
    public interface ITransform<T1, T2>
        where T1 : class
    {
        T1 Transform(T2 item);
    }
}