using System;
using System.Collections.Generic;
using System.Linq;

namespace sockets
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                PrintMenu();
                int option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        AddBook();
                        break;

                    case 2:
                        SearchByTitle();
                        break;

                    case 3:
                        SearchByAuthor();
                        break;

                    case 4:
                        SearchByYear();
                        break;

                    case 5:
                        SearchByEdition();
                        break;

                    case 6:
                        Remove();
                        break;

                    case 7:
                        Update();
                        break;

                    case 8:
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Opção não existente");
                        break;
                }
            }
        }

        private static void PrintMenu()
        {
            Console.WriteLine("-------------------------------");
            Console.WriteLine("1 - Criar livro");
            Console.WriteLine("2 - Consultar livro por título");
            Console.WriteLine("3 - Consultar livro por autor");
            Console.WriteLine("4 - Consultar livro por ano");
            Console.WriteLine("5 - Consultar livro por edição");
            Console.WriteLine("6 - Remover livro");
            Console.WriteLine("7 - Atualizar livro");
            Console.WriteLine("8 - Sair");
            Console.WriteLine("-------------------------------");
            Console.Write("Digite sua opção: ");
        }

        private static void Execute(Action<ApplicationDbContext> callback)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    callback(context);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Falha Durante a operação: {e}");
            }
        }

        private static void AddBook()
        {

            Console.WriteLine("Título do Livro:");
            var title = Console.ReadLine();

            Console.WriteLine("Autor do Livro:");
            var author = Console.ReadLine();

            Console.WriteLine("Ano do Livro:");
            var year = int.Parse(Console.ReadLine());

            Console.WriteLine("Edição do Livro:");
            var edition = int.Parse(Console.ReadLine());

            Execute(context =>
            {
                context.Books.Add(new Book
                {
                    Title = title,
                    Author = author,
                    Year = year,
                    Edition = edition
                });
                context.SaveChanges();
                Console.WriteLine("Livro inserido com sucesso.");
            });
        }

        private static void SearchByTitle()
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

                foreach(var book in books)
                {
                    Console.WriteLine($"Livro: {book.Title} - Autor: {book.Author} - Ano: {book.Year} - Edição: {book.Edition}");
                }
            });
        }

        private static void SearchByAuthor()
        {
            Console.WriteLine("Autor do Livro:");
            var author = Console.ReadLine();

            Execute(context =>
            {
                var books = context.Books.AsQueryable()
                    .Where(b => b.Author.ToUpper().Contains(author.ToUpper()))
                    .OrderBy(b => b.Author)
                    .AsEnumerable();

                if (!books.Any())
                {
                    Console.WriteLine("Nenhum livro encontrado");
                    return;
                }

                foreach(var book in books)
                {
                    Console.WriteLine($"Livro: {book.Title} - Autor: {book.Author} - Ano: {book.Year} - Edição: {book.Edition}");
                }
            });
        }

        private static void SearchByYear()
        {
            Console.WriteLine("Ano do Livro:");
            var year = int.Parse(Console.ReadLine());

            Execute(context =>
            {
                var books = context.Books.AsQueryable()
                    .Where(b => b.Year == year)
                    .OrderBy(b => b.Author)
                    .AsEnumerable();

                if (!books.Any())
                {
                    Console.WriteLine("Nenhum livro encontrado");
                    return;
                }

                foreach(var book in books)
                {
                    Console.WriteLine($"Livro: {book.Title} - Autor: {book.Author} - Ano: {book.Year} - Edição: {book.Edition}");
                }
            });
        }

        private static void SearchByEdition()
        {
            Console.WriteLine("Edição do Livro:");
            var edition = int.Parse(Console.ReadLine());

            Execute(context =>
            {
                var books = context.Books.AsQueryable()
                    .Where(b => b.Edition == edition)
                    .OrderBy(b => b.Author)
                    .AsEnumerable();

                if (!books.Any())
                {
                    Console.WriteLine("Nenhum livro encontrado");
                    return;
                }

                foreach(var book in books)
                {
                    Console.WriteLine($"Livro: {book.Title} - Autor: {book.Author} - Ano: {book.Year} - Edição: {book.Edition}");
                }
            });
        }

        private static void Remove()
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
                context.Books.Remove(book);
                context.SaveChanges();
                Console.WriteLine($"Livro {book.Title} removido");
            });
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

        // private static bool Update()
        // {
        //     // Book = new Book();
        //     // Console.WriteLine("Título do Livro para alterar:");
        //     // var title = Console.ReadLine();
        //     // var book = Book.GetByTitle(title);

        //     // if(!book.Equals(null))
        //     // {
        //     //     Console.WriteLine("Novo título do Livro:");
        //     //     title = Console.ReadLine();
        //     //     Console.WriteLine("Novo autor do Livro:");
        //     //     var author = Console.ReadLine();
        //     //     Console.WriteLine("Novo ano do Livro:");
        //     //     var year = int.Parse(Console.ReadLine());
        //     //     Console.WriteLine("Nova edição do Livro:");
        //     //     var edition = int.Parse(Console.ReadLine());

        //     //     Book.Update(new Book
        //     //     {
        //     //         Title = title,
        //     //         Author = author,
        //     //         Year = year,
        //     //         Edition = edition
        //     //     });

        //     //     return true;
        //     // }

        //     return false;
                
        // }
    }
}
