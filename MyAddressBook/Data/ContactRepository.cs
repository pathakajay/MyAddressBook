using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using MyAddressBook.Models;

namespace MyAddressBook.Data
{
    public class ContactRepository : IContactRepository
    {
        private IDbConnection db;

        public ContactRepository()
        {
            var connectionstring = ConfigurationManager.ConnectionStrings["SqlDataConnection"].ConnectionString;
            db = new SqlConnection(connectionstring);
        }
        
        public int AddContact(Contacts Contacts)
        {
            var sql = "INSERT INTO dbo.[Contacts] ([Name] ,[Email] ,[Phone] ,[Address] ,[PictureName]) VALUES" +
                "(@Name, @Email, @Phone, @Address, @Picturename); " +
                "SELECT CAST(SCOPE_IDENTITY() AS INT)";
            var id = this.db.Query<int>(sql, Contacts).Single();
            Contacts.Id = id;
            return id;
        }

        public bool DeleteContact(int id)
        {
            var sql = "DELETE FROM dbo.[Contacts] WHERE id = @id";
            var result = db.Execute(sql, new { Id = id });

            return true;
        }

        public Contacts GetContact(int id)
        {
            var sql = "SELECT * FROM dbo.[Contacts] WHERE id = @id";
            var result = db.Query<Contacts>(sql, new { Id = id })
                .SingleOrDefault();

            return result;
        }

        public List<Contacts> GetContacts()
        {
            var sql = "SELECT * FROM dbo.[Contacts] order by id";
            var result = db.Query<Contacts>(sql).ToList();

            return result;
        }
    }
}