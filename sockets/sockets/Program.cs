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

            while (true)
            {
                using (var clientAccept = server.AcceptTcpClient())
                {
                    Console.WriteLine("Client has connected.");
                    var output = clientAccept.GetStream();

                    using (var send = new BinaryWriter(output))
                    using (var receive = new BinaryReader(output))
                    {
                        string title, author;
                        int year, edition;
                        var run = true;

                        while (run)
                        {
                            switch (receive.ReadInt32())
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
                                    title = receive.ReadString();
                                    send.Write(VerifiyBook(title));
                                    break;

                                case 8:
                                    run = false;
                                    break;

                                case 9:
                                    var oldTitle = receive.ReadString();
                                    var newTitle = receive.ReadString();
                                    author = receive.ReadString();
                                    year = receive.ReadInt32();
                                    edition = receive.ReadInt32();
                                    send.Write(Update(oldTitle, newTitle, author, year, edition));
                                    break;

                                default:
                                    Console.WriteLine("Opção inexistente!");
                                    break;
                            }
                        }
                    }
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
                Console.WriteLine($"Falha durante a operação: {e}");
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
                    returnMessage = "Livro inserido com sucesso.\n";
                }
                catch(Exception)
                {
                    returnMessage = "Não foi possível inserir o livro.\n";
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
                    returnMessage = "Nenhum livro encontrado.\n";

                foreach(var book in books)
                {
                    returnMessage += $"Livro: {book.Title} - Autor: {book.Author} - Ano: {book.Year} - Edição: {book.Edition}\n";
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
                    returnMessage = "Nenhum livro encontrado.\n";
                }

                foreach(var book in books)
                {
                    returnMessage += $"Livro: {book.Title} - Autor: {book.Author} - Ano: {book.Year} - Edição: {book.Edition}\n";
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
                    returnMessage = "Nenhum livro encontrado.\n";
                }

                foreach(var book in books)
                {
                    returnMessage += $"Livro: {book.Title} - Autor: {book.Author} - Ano: {book.Year} - Edição: {book.Edition}\n";
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
                    returnMessage = "Nenhum livro encontrado.\n";
                }

                foreach(var book in books)
                {
                    returnMessage += $"Livro: {book.Title} - Autor: {book.Author} - Ano: {book.Year} - Edição: {book.Edition}\n";
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
                    .Where(b => b.Title == title)
                    .OrderBy(b => b.Title)
                    .AsEnumerable();

                if (!books.Any())
                {
                    returnMessage = "Nenhum livro encontrado.\n";
                }
                else if (books.Count() > 1)
                {
                    returnMessage = "Mais de um livro encontrado, livros:\n";

                    foreach(var b in books)
                    {
                        returnMessage += $"    Livro: {b.Title} - Autor: {b.Author} - Ano: {b.Year} - Edição: {b.Edition}\n";
                    }
                }
                else
                {
                    var book = books.Single();
                    context.Books.Remove(book);
                    try
                    {
                        context.SaveChanges();
                        returnMessage = $"Livro {book.Title} removido\n";
                    }
                    catch (Exception)
                    {
                        returnMessage = $"Não foi possivel remover o livro: {book.Title}\n";
                    }
                }
            });

            return returnMessage;
        }

        private static string VerifiyBook(string title)
        {
            var returnMessage = string.Empty;
            Execute(context =>
            {
                var books = context.Books.AsQueryable()
                    .Where(b => b.Title == title)
                    .OrderBy(b => b.Title)
                    .AsEnumerable();

                if (!books.Any())
                {
                    returnMessage = "Nenhum livro encontrado.\n";
                }
                else if (books.Count() > 1)
                {
                    returnMessage = "Mais de um livro encontrado, livros:\n";

                    foreach (var b in books)
                    {
                        returnMessage += $"    Livro: {b.Title} - Autor: {b.Author} - Ano: {b.Year} - Edição: {b.Edition}\n";
                    }
                }
            });

            return returnMessage;
        }

        private static string Update(string oldTitle, string newTitle, string author, int year, int edition)
        {
            var returnMessage = string.Empty;

            Execute(context =>
            {
                var books = context.Books.AsQueryable()
                    .Where(b => b.Title == oldTitle)
                    .OrderBy(b => b.Title)
                    .AsEnumerable();

                var book = books.Single();

                book.Title = newTitle;
                book.Author = author;
                book.Year = year;
                book.Edition = edition;

                try
                {
                    context.Books.Update(book);
                    context.SaveChanges();
                    returnMessage = $"Livro {book.Title} atualizado\n";
                }
                catch (Exception)
                {
                    returnMessage = "Não foi possível atualizar o livro.\n";
                }
            });

            return returnMessage;
        }
    }
}
