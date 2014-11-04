
namespace Hook.Models
{
    interface IHookRepo
    {
        void Create(string id);

        Hook Get(string id);

        void Save(Hook hook);
    }
}
