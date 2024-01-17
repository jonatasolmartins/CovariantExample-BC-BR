namespace Aula7;

public class Publication
{
    public string Name { get; set; }

    public Publication(string name)
    {
        Name = name;
    }
}

public class Book : Publication
{
    public string ISBN { get; set; }

    public Book(string name, string isbn) : base(name)
    {
        ISBN = isbn;
    }
}

public class Magazine : Publication
{
    public int Issue { get; set; }

    public Magazine(string name, int issue) : base(name)
    {
        Issue = issue;
    }
}