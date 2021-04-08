using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace sockets
{
    class Program
    {
        static void Main(string[] args)
        {

            var server = new TcpListener(IPAddress.Parse("127.0.0.1"), 9000);
            server.Start();
            Console.WriteLine("Server initialized");

            var clientAccept = server.AcceptTcpClient();
            Console.WriteLine("Client has connected.");

            var output = clientAccept.GetStream();            
            var send = new BinaryWriter(output);
                        
            int option;
            BinaryReader receive;

            string title, author;
            int year, edition;

            using (receive = new BinaryReader(output))
            {
                option = receive.ReadInt32();

                switch (option)
                {
                    case 1:
                        title = receive.ReadString();
                        author = receive.ReadString();
                        year = receive.ReadInt32(); 
                        edition = receive.ReadInt32();
                        send.Write(AddBook(title, author, year, edition));
                        break;

                    case 2:
                        title = receive.ReadString();
                        send.Write(SearchByTitle(title));
                        break;

                    case 3:
                        author = receive.ReadString();
                        send.Write(SearchByAuthor(author));
                        break;

                    case 4:
                        year = receive.ReadInt32();
                        send.Write(SearchByYear(year));
                        break;

                    case 5:
                        edition = receive.ReadInt32();
                        send.Write(SearchByEdition(edition));
                        break;

                    case 6:
                        title = receive.ReadString();
                        send.Write(Remove(title));
                        break;

                    case 7:
                        Update();
                        break;

                    case 8:
                        output.Close();
                        receive.Close();
                        send.Close();
                        clientAccept.Close();
                        server.Stop();
                        break; 

                    default:
                        Console.WriteLine("Opção inexistente!");
                        break;
                }
            }
        }

        private static void Execute(Action<ApplicationDbContext> callback)
        {
            try
            {
                using var context = new ApplicationDbContext();
                callback(context);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Falha Durante a operação: {e}");
            }
        }

        private static string AddBook(string title, string author, int year, int edition)
        {
            var returnMessage = string.Empty;
            Execute(context =>
            {
                context.Books.Add(new Book
                {
                    Title = title,
                    Author = author,
                    Year = year,
                    Edition = edition
                });

                try
                {
                    context.SaveChanges();
                    returnMessage = "Livro inserido com sucesso.";
                }
                catch(Exception)
                {
                    returnMessage = "Não foi possível inserir o livro.";
                }

            });

            return returnMessage;
        }

        private static string SearchByTitle(string title)
        {
            var returnMessage = string.Empty;

            Execute(context =>
            {
                var books = context.Books.AsQueryable()
                    .Where(b => b.Title.ToUpper().Contains(title.ToUpper()))
                    .OrderBy(b => b.Title)
                    .AsEnumerable();

                if (!books.Any())
                    returnMessage = "Nenhum livro encontrado";

                foreach(var book in books)
                {
                    returnMessage += $"Livro: {book.Title} - Autor: {book.Author} - Ano: {book.Year} - Edição: {book.Edition}";
                }
            });

            return returnMessage;
        }

        private static string SearchByAuthor(string author)
        {
            var returnMessage = string.Empty;

            Execute(context =>
            {
                var books = context.Books.AsQueryable()
                    .Where(b => b.Author.ToUpper().Contains(author.ToUpper()))
                    .OrderBy(b => b.Author)
                    .AsEnumerable();

                if (!books.Any())
                {
                    returnMessage = "Nenhum livro encontrado";
                }

                foreach(var book in books)
                {
                    returnMessage += $"Livro: {book.Title} - Autor: {book.Author} - Ano: {book.Year} - Edição: {book.Edition}";
                }
            });

            return returnMessage;
        }

        private static string SearchByYear(int year)
        {
            var returnMessage = string.Empty;

            Execute(context =>
            {
                var books = context.Books.AsQueryable()
                    .Where(b => b.Year == year)
                    .OrderBy(b => b.Title)
                    .AsEnumerable();

                if (!books.Any())
                {
                    returnMessage = "Nenhum livro encontrado";
                }

                foreach(var book in books)
                {
                    returnMessage += $"Livro: {book.Title} - Autor: {book.Author} - Ano: {book.Year} - Edição: {book.Edition}";
                }
            });

            return returnMessage;
        }

        private static string SearchByEdition(int edition)
        {
            var returnMessage = string.Empty;

            Execute(context =>
            {
                var books = context.Books.AsQueryable()
                    .Where(b => b.Edition == edition)
                    .OrderBy(b => b.Title)
                    .AsEnumerable();

                if (!books.Any())
                {
                    returnMessage = "Nenhum livro encontrado";
                }

                foreach(var book in books)
                {
                    returnMessage += $"Livro: {book.Title} - Autor: {book.Author} - Ano: {book.Year} - Edição: {book.Edition}";
                }
            });

            return returnMessage;
        }

        private static string Remove(string title)
        {
            var returnMessage = string.Empty;

            Execute(context =>
            {
                var books = context.Books.AsQueryable()
                    .Where(b => b.Title.ToUpper().Contains(title.ToUpper()))
                    .OrderBy(b => b.Title)
                    .AsEnumerable();

                if (!books.Any())
                {
                    returnMessage = "Nenhum livro encontrado.";
                }
                else if (books.Count() > 1)
                {
                    returnMessage = "Mais de um livro encontrado, livros:\n";

                    foreach(var b in books)
                    {
                        returnMessage += $"    Livro: {b.Title} - Autor: {b.Author} - Ano: {b.Year} - Edição: {b.Edition}";
                    }
                }
                else
                {
                    var book = books.Single();
                    context.Books.Remove(book);
                    try
                    {
                        context.SaveChanges();
                        returnMessage += $"Livro {book.Title} removido";
                    }
                    catch (Exception)
                    {
                        returnMessage += $"Não foi possivel remover o livro: {book.Title}";
                    }
                }                
            });

            return returnMessage;
        }

        private static void Update()
        {
            Console.WriteLine("Título do Livro:");
            var title = Console.ReadLine();

            Execute(context =>
            {
                var books = context.Books.AsQueryable()
                    .Where(b => b.Title.ToUpper().Contains(title.ToUpper()))
                    .OrderBy(b => b.Title)
                    .AsEnumerable();

                if (!books.Any())
                {
                    Console.WriteLine("Nenhum livro encontrado");
                    return;
                }

                if (books.Count() > 1)
                {
                    Console.WriteLine("Mais de um livro encontrado, livros:");

                    foreach(var b in books)
                    {
                        Console.WriteLine($"    Livro: {b.Title} - Autor: {b.Author} - Ano: {b.Year} - Edição: {b.Edition}");
                    }
                }

                var book = books.Single();

                Console.WriteLine("Novo título do Livro:");
                book.Title = Console.ReadLine();

                Console.WriteLine("Novo autor do Livro:");
                book.Author = Console.ReadLine();

                Console.WriteLine("Novo ano do Livro:");
                book.Year = int.Parse(Console.ReadLine());

                Console.WriteLine("Nova edição do Livro:");
                book.Edition = int.Parse(Console.ReadLine());

                context.Books.Update(book);
                context.SaveChanges();
                Console.WriteLine($"Livro {book.Title} atualizado");
            });
        }
    }
}
