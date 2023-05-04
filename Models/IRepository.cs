using System.Linq;
using System.Threading.Tasks;

namespace VKR_Poom_Reserving.Models
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Возвращает запрос
        /// </summary>
        /// <returns></returns>
        IQueryable<T> Query();
        /// <summary>
        /// Возвращает объект в базе данных с заданным id
        /// </summary>
        /// <param name="id">Id нужного объекта</param>
        /// <returns></returns>
        T Get(int id);
        Task<T> GetAsync(int id);
        /// <summary>
        /// Добавляет нового пользователя в базу данных
        /// </summary>
        /// <param name="obj">объект</param>
        void Create(T obj);
        Task CreateAsync(T obj);
        /// <summary>
        /// Удаляет из базы объект
        /// </summary>
        /// <param name="id">Id нужного объекта</param>
        void Delete(int id);
    }
}
