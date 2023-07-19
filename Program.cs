using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Reflection;
using System.Collections.Generic;


namespace mongo1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //func1();

            func2();
            
            
        
        }   


        private static void func1()
        {
            Author author = new Author { FirstName = "John", LastName = "Doe" };
            Author author2 = new Author { Id = 2, FirstName = "Jane", LastName = "Doe" };
            Author author3 = new Author { Id = 3, FirstName = "Michael", LastName = "Smith" };
            Author author4 = new Author { Id = 4, FirstName = "Kate", LastName = "Mell" };

            // create a client connection
            var client = new MongoClient("mongodb://localhost:27017");

            // create a database
            IMongoDatabase db = client.GetDatabase("test");

            // create a collection
            IMongoCollection<BsonDocument> collection2 = db.GetCollection<BsonDocument>("authors");

            //InsertDocument(collection, author);

            //ListDocuments(collection2);

            Console.WriteLine("-----------------------------------------");

            //DeleteDocumentWithId(collection2, 1);

            List<String> fieldList = GetFields(collection2, "FirstName");


            Console.ReadLine();
        }

        private static void func2() 
        {
            InsertClassNames();

            String connectionString = "mongodb://localhost:27017";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase("test2");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("ClassNames");
 
        }

        private static void InsertClassNames()
        {
            BsonDocument className1 = new BsonDocument { { "ClassName", "mongo1.Student" } };
            BsonDocument className2 = new BsonDocument { { "ClassName", "mongo1.School" } };
            BsonDocument className3 = new BsonDocument { { "ClassName", "mongo1.Garage" } };
            BsonDocument className4 = new BsonDocument { { "ClassName", "mongo1.Author" } };
            BsonDocument className5 = new BsonDocument { { "ClassName", "mongo1.City" } };
            BsonDocument className6 = new BsonDocument { { "ClassName", "mongo1.Car" } };

            List<BsonDocument> classNames = new List<BsonDocument>();
            classNames.Add(className1);
            classNames.Add(className2);
            classNames.Add(className3);
            classNames.Add(className4);
            classNames.Add(className5);
            classNames.Add(className6);

            foreach (BsonDocument className in classNames)
            {
                Console.WriteLine(className.GetElement("ClassName").Value);
                Type type = Type.GetType(className.GetElement("ClassName").Value.ToString());
                if (type == null)
                {
                    Console.WriteLine("Class : -" + className + "- not found");
                }
                else
                {
                    Console.WriteLine("Class : -" + className + "- found");
                }

            }

            Type type1 = Type.GetType("asdfasdfd");
            if(type1 == null)
            {
                Console.WriteLine("Class : -" + "RandomClass" + "- not found");
            }
            else
            {
                Console.WriteLine("Class : -" + "RandomClass" + "- found");
            }


            Console.WriteLine("End of InsertClassNames()");
        }

        private static void ListCollections(IMongoDatabase database)
        {
            var filter = new BsonDocument();
            var options = new ListCollectionsOptions { Filter = filter };
            var collections = database.ListCollections(options).ToList();

            foreach (var collection in collections)
            {
                Console.WriteLine(collection.ToString());
            }
        }

        private static void ListDocuments(IMongoCollection<BsonDocument> collection)
        {
            var documents = collection.Find(new BsonDocument()).ToList();

            foreach (var document in documents)
            {
                Console.WriteLine(document.ToString());
            }
        }
        
        private static List<String> GetFields(IMongoCollection<BsonDocument> collection, String str)
        {
            var projection = Builders<BsonDocument>.Projection.Include(str);

            var documents = collection.Find(new BsonDocument()).Project(projection).ToList();

            List<String> fieldList = new List<String>();

            foreach (var document in documents)
            {
                string field = document.GetElement(str).Value.ToString();
                fieldList.Add(field);
                Console.WriteLine(field);
            }

            return fieldList;

        }

        private static void DeleteDocumentWithId(IMongoCollection<BsonDocument> collection, int id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            collection.DeleteOne(filter);
        }

        private static void InsertDocument(IMongoCollection<Author> collection, Author author)
        {
            collection.InsertOne(author);
        }

        private static void SearchClass(List<String> list)
        {
            foreach (String className in list)
            {
                Type type = Type.GetType(className);
                if(type != null)
                {
                    Console.WriteLine("Class : -" + className + "- not found" );
                }
                else
                {
                    Console.WriteLine("Class : -" + className + "- found" );
                }


            }
        }


    }

}