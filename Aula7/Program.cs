using Aula7;
//Covariant
// A more derived class can be asign to a less derived class

Book book = new Book("My book", "11237374");
//This works because Book derived from Publication
//It's following the Liskov Substitution Principle
Publication publication = book;

Magazine magazine = new Magazine("My magazine", 123);
//Same here
publication = magazine;

List<Publication> publications = new List<Publication>() { book, magazine };

//Call this method with a list of publications to be printed
PrintPublication(publications);
//************************************************************************************************************//


/*
 This doesn't work because List<Book> is not a List<Publication>
 and the compiler doesn't know if it's safe to do this.
 We can potentially add a magazine object to a list of books inside this method and it would break the code.
 Uncomment this line below to see the error
*/
List<Magazine> magazines = new List<Magazine>() {magazine};
List<Book> books = new List<Book>() {book};
// PrintPublication(books);


//************************************************************************************************************//

//This works because we are using a generic method, and we are telling to compiler that T in List<T> is a Publication (T : Publication) and 
PrintGenericPublication(books);
PrintGenericPublication(magazines);


//************************************************************************************************************//

//This doesn't work because we are passing an array of books and the method is expecting a list of publications
/* SEE COMMENT #1 IN THE END OF THIS FILE TO A MORE COMPLETE EXPLANATION OS THE ISSUE */

Book[] booksArray = new Book[1]; //Set the size of the array
//This is also valid : Book[] booksArray = new Book[1] {book}; and this Book[] booksArray = {book};
booksArray[0] = book;
/* PrintPublication(booksArray);
 PrintGenericPublication(booksArray); */

//************************************************************************************************************//

/*
 This works because we are using IEnumerable<T> instead of List<T>
 List<T> and Array<T> implements IEnumerable<T>
 So we can substitute List<T> and Array<T> for IEnumerable<T> in the method signature as long as T is a Publication
  */
PrintPublicationEnumerable(booksArray);
var magazinesArray  = new Magazine[]{magazine};
PrintPublicationEnumerable(magazinesArray);

//************************************************************************************************************//

void PrintPublication(List<Publication> publications)
{
    Console.WriteLine("Printing publications");
    foreach (var pub in publications)
    {
        Console.WriteLine($"{pub.Name}");
    }
}

void PrintGenericPublication<T>(List<T> publications) where T : Publication
{
    
    /*
     This is not allowed
        publications.Add(new Publication("My book", "11237374"));
        publications.Add(new Book("My book", "11237374"));
        
        Even with casting because the compiler doesn't know if publications is a List<Book> or List<Magazine>
        publications.Add((T) new Book("My book", "11237374"));
        //This one will fail in runtime
        publications.Add((T) new Publication("My book"));
    */
    
    Console.WriteLine("Printing generic publications");
    foreach (var pub in publications)
    {
        Console.WriteLine($"{pub.Name}");
    }
}

void PrintPublicationEnumerable<T>(IEnumerable<T> publications) where T : Publication
{
    Console.WriteLine("Printing generic IEnumerable publications");
    foreach (var pub in publications)
    {
        Console.WriteLine($"{pub.Name}");
    }
}

/*
 This work but as we are telling to the compiler that T is any Class (T : class) we can not use the property Name as it's not defined in the class object
 void PrintPublicationEnumerable<T>(IEnumerable<T> publications) where T : class
 For instance, a string is a class, so we can pass a List<string> to this method, but string doesn't have a property Name
*/

ExampleOfTwoGenericParameterUandT<List<Publication>, Publication>(new List<Publication>(){magazine});

void ExampleOfTwoGenericParameterUandT<U, T>(U publications) where U : IEnumerable<T> where T :Publication
{
    foreach (var pub in publications)
    {
        Console.WriteLine($"{pub.Name} from ExampleOfTwoGenericParameterUandT");
    }
}

// #1 - Why this doesn't work?
//************************************************************************************************************//

/* Consider this this object hierarchy
 
    public interface IEnumerable<T> {}
    public class List<T> : IEnumerable<T> {}
    public class Array<T> : IEnumerable<T> {}
    
    Book[] booksArray = {book};
    
    //We are trying substitute List<T> for an Array<T> in the method call
    PrintGenericPublication(bookArray);
    
    For the same reason that we can't do this:
    Magazine magazine = new Book("My book", "11237374");
    We can't do this:
    List<T> myList = new Array<T>();

    As Magazine and Book implements Publication (They are derived from Publication), List<T> and Array<T> implements IEnumerable<T> (They are derived from IEnumerable<T>)
    But we can't substitute List<T> for Array<T> because List<T> and Array<T> are not derived from each other.
    
    Because of the Liskok substitution principle We can do this:
    IEnumerable<T> myList = new Array<T>();
    IEnumerable<T> myList = new List<T>();
    As well as we can do this:
    Publication publication = new Book("My book", "11237374");
    Publication publication = new Magazine("My magazine", 123);
    
    
    IEnumerable<T> is the base class of List<T> and Array<T> 
    so we can substitute List<T> and Array<T> for IEnumerable<T> in the method signature
   
    void PrintPublicationEnumerable<T>(IEnumerable<T> publications) where T : Publication {}
   
    and them we can pass List<T> and Array<T> to this method
    PrintPublicationEnumerable(booksArray);

*/
