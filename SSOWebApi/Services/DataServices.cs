using SSOWebApi.Models;

namespace SSOWebApi.Services
{
    public class DataServices : IDataServices
    {
        private List<Todo> _todos;
        private List<Contact> _contacts;

        public DataServices()
        {
            _todos = new List<Todo>()
            {
                new Todo("Todo1", "Description1", DateTime.Today.AddDays(4)),
                new Todo("Todo2", "Description2", DateTime.Today.AddDays(6)),
                new Todo("Todo3", "Description3", DateTime.Today.AddDays(8)),
                new Todo("Todo4", "Description4", DateTime.Today.AddDays(9))
            };

            _contacts = new List<Contact>()
            {
                new Contact("Name1", "LastName1", "PhoneNumber1"),
                new Contact("Name2", "LastName2", "PhoneNumber2"),
                new Contact("Name3", "LastName3", "PhoneNumber3"),
                new Contact("Name4", "LastName4", "PhoneNumber4")
            };

        }
        public void AddContact(Contact contact)
        {
            _contacts.Add(contact);
        }

        public void AddTodo(Todo todo)
        {
            _todos.Add(todo);
        }

        public IList<Contact> GetContacts()
        {
            return _contacts;
        }

        public IList<Todo> GetTodos()
        {
            return _todos;
        }
    }
}
