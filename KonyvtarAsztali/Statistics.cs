using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace KonyvtarAsztali
{
    internal static class Statistics
    {
        static List<Book> books;
        public static void Run()
        {
            try
            {
                ReadAllBooks();
                //ellenőrizzük, hogy az adatokat sikerült-e beaolvasni az adatbázisból a könyvcímek kiírásával
                //ez nem volt része a feladatnak, így ezt utána törölni kell
                //books.ForEach(book => Console.WriteLine(book.Title));
                Console.WriteLine("500 oldalnál hosszabb könyvek száma: {0}", CountLongerThan500Pages());
                Console.WriteLine("{0} 1950-nél régebbi könyv", OldaerThan1950IsPresent() ? "Van" : "Nincs");
                Book longest = GetLongestBook();
                Console.WriteLine("A leghosszabb könyv:" +
                    "\r\n\tSzerző: {0} " +
                    "\r\n\tCím: {1}" +
                    "\r\n\tKiadás éve: {2}" +
                    "\r\n\tOldalszám: {3}", longest.Author, longest.Title, longest.Publish_year, longest.Page_count);
                Console.WriteLine("A legtöbb könyvvel rendelkező szerző: {0}", GetAuthorWithMostBooks());
                GetAuthorsBooks();
                Console.Write("Adjon meg egy könyv címet: ");
                string title = Console.ReadLine();
                PrintAuthor(title);
                Console.ReadKey();
            }
            catch(MySqlException ex)
            {
                Console.WriteLine("Hiba történt az adatbáziskapcsolat kiépítésekor:");
                Console.WriteLine(ex.Message);
                
            }
        }

        private static void PrintAuthor(string title)
        {
            int index = 0;
            while(index < books.Count && books[index].Title != title)
            {
                index++;
            }
            if(index < books.Count)
            {
                Console.WriteLine("A megadott könyv szerzője: {0}", books[index].Author);
            }
            else
            {
                Console.WriteLine("Nincs ilyen könyv");
            }
        }

        //Bekért könyv szerzője másik megoldás
        public static void whoIsTheAuthor()
        {
            Console.Write("Adjon meg egy könyv címet: ");
            string bookTitle = Console.ReadLine();
            string author = "";
            foreach (Book item in books)
            {
                if (bookTitle == item.Title)
                {
                    author = item.Author;
                }
            }
            if (author.Length != 0)
            {
                Console.WriteLine($"A megdott könyv szerzője: {author}");
            }
            else
            {
                Console.WriteLine("Nincs ilyen könyv");
            }

        }
    

    private static string GetAuthorWithMostBooks()
        {
           Dictionary<string, int> authorbookCount = new Dictionary<string, int>();
            foreach(Book book in books)
            {
                if (authorbookCount.ContainsKey(book.Author))
                {
                    authorbookCount[book.Author]++;
                }
                else
                {
                    authorbookCount[book.Author] = 1;
                }
            }
            string author = null;
            foreach(KeyValuePair<string,int> item in authorbookCount)
            {
                if(author == null)
                {
                    author = item.Key;
                }
                if(item.Value > authorbookCount[author])
                {
                    author = item.Key;
                }
            }
            return author;
        }

        //minden szerző minden könyve
        private static void GetAuthorsBooks()
        {
            Dictionary<string, int> authorbookCount = new Dictionary<string, int>();
            foreach (Book book in books)
            {
                if (authorbookCount.ContainsKey(book.Author))
                {
                    authorbookCount[book.Author]++;
                }
                else
                {
                    authorbookCount[book.Author] = 1;
                }
               
            }
            foreach (var item in authorbookCount)
            {
                Console.WriteLine(item.Key + ": " + item.Value);
            }

        }

     
        private static Book GetLongestBook()
        {
            Book longest = books[0];
            for(int i =1; i < books.Count; i++)
            {
                if (books[i].Page_count > longest.Page_count)
                {
                    longest = books[i];
                }
            }
            return longest;
        }

        //másik megoldás a leghosszabb könyv adataira
        public static void longestBook()
        {
            int maxPage = 0;
            string author = "";
            string title = "";
            int published = 0;
            int page = 0;
            foreach (var item in books)
            {
                if (item.Page_count > maxPage)
                {
                    maxPage = item.Page_count;
                    author = item.Author;
                    title = item.Title;
                    published = item.Publish_year;
                    page = item.Page_count;
                }
            }
            Console.WriteLine($"A leghosszabb könyv: \n" +
                                $"\tSzerző: {author} \n" +
                                $"\tCím: {title} \n" +
                                $"\tKiadás éve: {published} \n" +
                                $"\tOldalszám: {page}");
        }

        private static bool OldaerThan1950IsPresent()
        {
            int index = 0;
            while (index < books.Count && !(books[index].Publish_year <1950))
            {
                index++;
            }
            return index < books.Count;
        }

        //még egy megoldás az 1950-nél régebbi könyvek megtalálására
        public static void olderThenNineteenFifty()
        {
            int count = 0;
            foreach (Book item in books)
            {
                if (item.Publish_year < 1950)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                Console.WriteLine("Van 1950-nél régebbi könyv.");
            }
            else
            {
                Console.WriteLine("Nincs 1950-nél régebbi könyv.");
            }
        }

        private static int CountLongerThan500Pages()
        {
           int count = 0;
            foreach(Book item in books)
            {
                if(item.Page_count > 500)
                {
                    count++;
                }
            }
            return count;
        }

       

        private static void ReadAllBooks()
        {
            BookService bookService = new BookService();
            books = bookService.GetBooks();
        }
    }
}
