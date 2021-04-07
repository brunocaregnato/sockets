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
    }
}
