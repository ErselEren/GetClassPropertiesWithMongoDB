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
           
            String connectionString = "mongodb://localhost:27017";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase("test2");
            var collection = database.GetCollection<ClassNames>("ClassNames2");

            //InsertClassNames(collection);

            List<ClassNames> list = GetClassNames(collection);

            //print list
            foreach (ClassNames classNames in list)
            {
                Console.WriteLine(classNames.ClassName + " " + classNames.Namespace);
            }

            CreateNewTable(collection, list);


        }

        private static void CreateNewTable(IMongoCollection<ClassNames> collection, List<ClassNames> list)
        {
            //search for each class in the list
            //if the class is found, add it to the new list
            //if the class is not found,do nothing
            //after the list is created, insert it into the new collection


            foreach (ClassNames className in list)
            {
                String fullName = className.Namespace + "." + className.ClassName;
                Type type = Type.GetType(fullName);
                if (type == null)
                {
                    Console.WriteLine("Class : -" + className.ClassName + "- not found");
                }
                else
                {
                    Console.WriteLine("Class : -" + className.ClassName + "- found");
                }

            }

            Type type1 = Type.GetType("asdfasdfd");
            if (type1 == null)
            {
                Console.WriteLine("Class : -" + "RandomClass" + "- not found");
            }
            else
            {
                Console.WriteLine("Class : -" + "RandomClass" + "- found");
            }


            Console.WriteLine("End of InsertClassNames()");




        }

        private static List<ClassNames> GetClassNames (IMongoCollection<ClassNames> collection)
        {
            var documents = collection.Find(new BsonDocument()).ToList();

            List<ClassNames> classNamesList = new List<ClassNames>();

            foreach (var document in documents)
            {
                ClassNames classNames = new ClassNames { ClassName = document.ClassName, Namespace = document.Namespace };
                classNamesList.Add(classNames);
                Console.WriteLine("=="+classNames.ClassName + " " + classNames.Namespace);
            }

            //var filter = Builders<ClassNames>.Filter.Empty;
            //List<ClassNames> list2 = collection.Find(filter).ToList();

            ////print list
            //foreach (ClassNames classNames in list2)
            //{
            //    Console.WriteLine("++" + classNames.ClassName + " " + classNames.Namespace);
            //}

            return classNamesList;
        }

        private static void InsertClassNames(IMongoCollection<ClassNames> collection)
        {
            ClassNames classNames1 = new ClassNames { ClassName = "Student", Namespace = "mongo1" };
            ClassNames classNames2 = new ClassNames { ClassName = "School", Namespace = "mongo1" };
            ClassNames classNames3 = new ClassNames { ClassName = "Garage", Namespace = "mongo1" };
            ClassNames classNames4 = new ClassNames { ClassName = "Author", Namespace = "mongo1" };
            ClassNames classNames5 = new ClassNames { ClassName = "City", Namespace = "mongo1" };
            ClassNames classNames6 = new ClassNames { ClassName = "Car", Namespace = "mongo1" };

            List<ClassNames> classNamesList = new List<ClassNames>();

            classNamesList.Add(classNames1);
            classNamesList.Add(classNames2);
            classNamesList.Add(classNames3);
            classNamesList.Add(classNames4);
            classNamesList.Add(classNames5);
            classNamesList.Add(classNames6);

            //insert documents
            foreach (ClassNames classNames in classNamesList)
            {
                collection.InsertOne(classNames);
            }



            //BsonDocument className1 = new BsonDocument { { "ClassName", "mongo1.Student" } };
            //BsonDocument className2 = new BsonDocument { { "ClassName", "mongo1.School" } };
            //BsonDocument className3 = new BsonDocument { { "ClassName", "mongo1.Garage" } };
            //BsonDocument className4 = new BsonDocument { { "ClassName", "mongo1.Author" } };
            //BsonDocument className5 = new BsonDocument { { "ClassName", "mongo1.City" } };
            //BsonDocument className6 = new BsonDocument { { "ClassName", "mongo1.Car" } };

            //List<BsonDocument> classNames = new List<BsonDocument>();
            //classNames.Add(className1);
            //classNames.Add(className2);
            //classNames.Add(className3);
            //classNames.Add(className4);
            //classNames.Add(className5);
            //classNames.Add(className6);

            //foreach (BsonDocument className in classNames)
            //{
            //    Console.WriteLine(className.GetElement("ClassName").Value);
            //    Type type = Type.GetType(className.GetElement("ClassName").Value.ToString());
            //    if (type == null)
            //    {
            //        Console.WriteLine("Class : -" + className + "- not found");
            //    }
            //    else
            //    {
            //        Console.WriteLine("Class : -" + className + "- found");
            //    }

            //}

            //Type type1 = Type.GetType("asdfasdfd");
            //if(type1 == null)
            //{
            //    Console.WriteLine("Class : -" + "RandomClass" + "- not found");
            //}
            //else
            //{
            //    Console.WriteLine("Class : -" + "RandomClass" + "- found");
            //}


            //Console.WriteLine("End of InsertClassNames()");
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