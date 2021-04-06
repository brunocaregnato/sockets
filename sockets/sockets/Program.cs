using System;
using System.Collections.Generic;
using System.Linq;

namespace sockets
{
    class Program
    {

        public static Book Book;

        static void Main(string[] args)
        {           
            while(true)
            {
                PrintMenu();
                int option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        if (Insert())
                            Console.WriteLine("Livro inserido com sucesso.");
                        else
                            Console.WriteLine("Não foi possível inserir o livro.");
                        break;
                    case 2:
                    case 3:                         
                        if(!Search(option))
                        {
                            Console.WriteLine("Não foi possível encontrar nenhum livro com essa busca.");
                        }
                        break;
                    case 4:
                        if(Remove())
                            Console.WriteLine("Livro removido com sucesso.");
                        else
                            Console.WriteLine("Não foi possível remover o livro.");
                        break;
                    case 5:
                        if (Update())
                            Console.WriteLine("Livro alterado com sucesso.");
                        else
                            Console.WriteLine("Não foi possível alterar o livro.");
                        break;
                    case 6:
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
            Console.WriteLine("1 - Criar Livro");
            Console.WriteLine("2 - Consultar Livro");
            Console.WriteLine("3 - Consultar por ano e número da edição");
            Console.WriteLine("4 - Remover livro");
            Console.WriteLine("5 - Alteração do livro");
            Console.WriteLine("6 - Sair");
            Console.WriteLine("-------------------------------");
            Console.Write("Digite sua opção: ");
        }

        private static bool Insert()
        {
            Console.WriteLine("Título do Livro:");
            var title = Console.ReadLine();
            Console.WriteLine("Autor do Livro:");
            var author = Console.ReadLine();
            Console.WriteLine("Ano do Livro:");
            var year = int.Parse(Console.ReadLine());
            Console.WriteLine("Edição do Livro:");
            var edition = int.Parse(Console.ReadLine());

            Book = new Book();

            return Book.Insert(new Book
            {
                Title = title,
                Author = author,
                Year = year,
                Edition = edition
            });
        }

        private static bool Search(int option)
        {
            Book = new Book();
            var books = new List<Book>();
            if(option.Equals(2))
            {
                Console.WriteLine("Título do Livro:");
                var title = Console.ReadLine();
                Console.WriteLine("Autor do Livro:");
                var author = Console.ReadLine();
                books = Book.Search(title, author);
            }
            else 
            {
                Console.WriteLine("Ano do Livro:");
                var year = int.Parse(Console.ReadLine());
                Console.WriteLine("Edição do Livro:");
                var edition = int.Parse(Console.ReadLine());
                books = Book.Search(year, edition, false);
            }

            if (books.Count.Equals(0))
                return false;
            
            books.OrderBy(x => x.Title).ToList().ForEach(book =>
                Console.WriteLine($"Livro: {book.Title} - Autor: {book.Author} - Ano: {book.Year} - Edição: {book.Edition}"));

            return true;
        }

        private static bool Remove()
        {
            Book = new Book();
            Console.WriteLine("Título completo do livro para remover:");
            var title = Console.ReadLine();
            var book = Book.GetByTitle(title);
            if(!book.Equals(null))
            {
                Book.Remove(book.Id);
                return true;
            }
            return false;

        }

        private static bool Update()
        {
            Book = new Book();
            Console.WriteLine("Título do Livro para alterar:");
            var title = Console.ReadLine();
            var book = Book.GetByTitle(title);

            if(!book.Equals(null))
            {
                Console.WriteLine("Novo título do Livro:");
                title = Console.ReadLine();
                Console.WriteLine("Novo autor do Livro:");
                var author = Console.ReadLine();
                Console.WriteLine("Novo ano do Livro:");
                var year = int.Parse(Console.ReadLine());
                Console.WriteLine("Nova edição do Livro:");
                var edition = int.Parse(Console.ReadLine());

                Book.Update(new Book
                {
                    Title = title,
                    Author = author,
                    Year = year,
                    Edition = edition
                });

                return true;
            }

            return false;
                
        }
    }
}
