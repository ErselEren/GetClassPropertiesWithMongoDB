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
            func2();
        }   

        private static void func2() 
        {
           
            String connectionString = "mongodb://localhost:27017";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase("test2");
            var collection = database.GetCollection<ClassNames>("ClassNames2");

            //InsertClassNames(collection);

            List<ClassNames> list = GetClassNames(collection);

            //create new collection
            IMongoCollection<ClassNames> collectionWithProperties = database.GetCollection<ClassNames>("ClassNamesWithProperties");

            CreateNewTable(collectionWithProperties, list);


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
                    List<PropertyInfo> propertyList = new List<PropertyInfo>();
                    ClassEntry Entry = new ClassEntry();
                    propertyList = type.GetProperties().ToList();
                    
                    foreach (PropertyInfo property in propertyList)
                    {
                        Console.WriteLine(property.Name + " " + property.PropertyType);
                        Entry.ClassName = className.ClassName;
                        Entry.Namespace = className.Namespace;
                        Entry.types.Add((property.Name, property.PropertyType.Name));
                    }
                    collection.InsertOne(Entry);
                }
                Console.WriteLine("-------------------------------------------------");
            }




        }

        private static List<ClassNames> GetClassNames (IMongoCollection<ClassNames> collection)
        {
            var documents = collection.Find(new BsonDocument()).ToList();

            List<ClassNames> classNamesList = new List<ClassNames>();

            foreach (var document in documents)
            {
                ClassNames classNames = new ClassNames { ClassName = document.ClassName, Namespace = document.Namespace };
                classNamesList.Add(classNames);
                //Console.WriteLine("=="+classNames.ClassName + " " + classNames.Namespace);
            }

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