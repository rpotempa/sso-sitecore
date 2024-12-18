using SSOWebApi.Models;

namespace SSOWebApi.Services
{
    public interface IDataServices
    {
        IList<Todo> GetTodos();
        void AddTodo(Todo todo);
        IList<Contact> GetContacts();
        void AddContact(Contact contact);
    }
}
