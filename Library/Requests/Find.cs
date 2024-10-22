using System.ComponentModel.DataAnnotations;

namespace Library.Requests
{
    public class Find
    {
        public class GetAllBooksName
        {
            public int Id_Books { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public int Year_public { get; set; }
            public string Description { get; set; }
            public int Copies { get; set; }
            public int Id_genre { get; set; }
        }
        public class GetAllBooksId
        {
            public int Id_Books { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public int Year_public { get; set; }
            public string Description { get; set; }
            public int Copies { get; set; }
            public int Id_genre { get; set; }
        }

        public class FindCopies
        {
            public int Copies { get; set; }
        }

        public class GetAllGenres
        {
            public int Id_Genres { get; set; }
            public string Name_genre { get; set; }
        }

        public class GetAllReaders
        {
            public int Id_reader { get; set; }
            public string First_name { get; set; }
            public string Last_name { get; set; }
            public int Birth_year { get; set; }
            public string Contact_info { get; set; }
            public DateTime? RegistrationDate { get; set; }
        }
    }
}
