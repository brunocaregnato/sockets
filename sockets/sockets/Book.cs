using System;
using System.Collections.Generic;
using System.Text;

namespace sockets
{
    public class Book
    {
        public int Id { get; private set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public int Edition { get; set; }

        public bool Insert(Book book)
        {
            //@TODO 
            return true;
        }

        public List<Book> Search(dynamic value1, dynamic value2, bool byBook = true)
        {
            if(byBook)
            {
                //@TODO consultar por livro e autor
            }
            else
            {
                //@TODO consultar por ano e edição
            }

            var books = new List<Book>();
            books.Add(new Book { Id = 1, Title = "One Piece", Author = "Eichiro Oda", Year = 1999, Edition = 90 });
            books.Add(new Book { Id = 2, Title = "Berserk", Author = "Kentarou Miura", Year = 1989, Edition = 50 });

            return books;
        }

        public Book GetByTitle(string title)
        {
            ///@TODO fazer busca do livro para atualizar
            return new Book();
        }

        public void Remove(int id)
        {
            //@TODO
            return;
        }

        public void Update(Book book)
        {
            //@TODO
            return;
        }

    }
}
