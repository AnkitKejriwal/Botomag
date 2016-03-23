namespace Botomag.DAL.Model
{
    public interface IBaseEntity<TKey> where TKey : struct
    {
        TKey Id { get; set; }
    }
}
