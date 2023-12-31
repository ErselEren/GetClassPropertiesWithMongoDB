using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using mongo1;
using mongo1.MSSQL_Classes;

namespace mongo1
{
    
    public class Program
    {
        private static IList<ClassNames> listOfClassNames;
        private static IList<Classes> classTable;
        private static IList<Methods> MethodTable;
        private static IList<Parameters> ParametersTable;
        private static IList<ParameterTypes> ParameterTypesTable;
       
        public static void Main(string[] args)
        {

            listOfClassNames = GetClassesFromMongo();
            MethodTable = new List<Methods>();
            ParametersTable = new List<Parameters>();
            ParameterTypesTable = new List<ParameterTypes>();
            classTable = new List<Classes>();


            for(int i = 0; i < listOfClassNames.Count; i++)
            {
                fill_MSSQL_tables(listOfClassNames[i].Namespace, listOfClassNames[i].ClassName);
            }

            writeTable();


            Console.WriteLine("!!!!!!! --- END OF MAIN --- !!!!!!!!");
            Console.ReadLine();
        }

        private static void fill_MSSQL_tables(String classNamespace, String className)
        {   
            string fullClassName = classNamespace + "." + className;
            //if first char is dot, remove it
            if (fullClassName[0] == '.')
            {
                fullClassName = fullClassName.Substring(1);
            }
            
            Console.WriteLine(fullClassName);    
            Type type = Type.GetType(fullClassName);
                
            if (type == null) //Class not found
              return;
               
            if (checkExist(classTable, className) == false) //Class already exist in table
              return;
                    
            Classes newClass = new Classes(className);
            classTable.Add(newClass); //Add class to table if not exist
                                        
            //Get method info
            MethodInfo[] MethodInfoArr = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(method => !method.IsSpecialName).ToArray();
                             
            foreach (MethodInfo method in MethodInfoArr) // Get each method from the class
      {
                Methods methodTableItem = new Methods(method.Name, newClass, method.ReturnType.Name);
                MethodTable.Add(methodTableItem);
                        
                //Get each parameter from the method
                foreach (ParameterInfo param in method.GetParameters())
                {
          
                    ParameterTypes parameterTypesTableItem = new ParameterTypes(param.ParameterType.ToString());
                    Parameters parameterTableItem = new Parameters(parameterTypesTableItem, methodTableItem,param.Name);
                                                       
                    int resultOfCheck = checkParameterExist(ParameterTypesTable, param.ParameterType.ToString());
                    if (resultOfCheck != -1)
                    {
                        parameterTypesTableItem = ParameterTypesTable[resultOfCheck];
                        parameterTableItem.parameterType = parameterTypesTableItem;
                    }
                    
                    
                    if (param.ParameterType.IsPrimitive == true || param.ParameterType.ToString().Equals("System.String"))
                    {
                        ParametersTable.Add(parameterTableItem);
                        ParameterTypesTable.Add(parameterTypesTableItem);
                    }
                    else
                    {
                        Console.WriteLine("Class: " + param.ParameterType.Namespace);
                        
                        ParametersTable.Add(parameterTableItem);
                        ClassNames recursiveItem = new ClassNames();
                        fill_MSSQL_tables(param.ParameterType.Namespace.ToString(),param.ParameterType.Name.ToString());
                        ParameterTypesTable.Add(parameterTypesTableItem);
                    }
                            
                }

            }
        }

        private static int checkParameterExist(IList<ParameterTypes> parameterTypesTable, string paramTypeName)
        {
            for (int i = 0; i < parameterTypesTable.Count; i++)
            {
                if (parameterTypesTable[i].Name.Equals(paramTypeName))
                {
                    return i;
                }
            }

            return -1;
        }

        private static bool checkExist(IList<Classes> classTable,string ClassName)
        {

            //if (classTable.Any(x=>x.Name == ClassName))
            //{

            //}

            for (int i = 0; i < classTable.Count; i++)
            {
                if (classTable[i].Name==ClassName)
                {
                    return false;
                }
            }

            return true;
        }

        private static void handleRelational()
        {

            string connString = @"Server=localhost,1433;Database=CLASSES;User ID=sa;Password=password";
            //string query = @"SELECT * FROM Student";

            SqlConnection sqlConnection = new SqlConnection(connString);
            string query = @"create table classNamespaces(
                                namespaceId int,
                                name VARCHAR(50),
                                primary key(namespaceId)
                            );";
            //SqlCommand cmd = sqlConnection.CreateCommand();
            SqlCommand cmd = new SqlCommand(query, sqlConnection);

            sqlConnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();


        }

        private static SqlCommand getSqlCommand()
        {
            string connString = @"Server=localhost,1433;Database=CLASSES;User ID=sa;Password=password";
            string query = @"SELECT * FROM Student";

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    //access SQL Server and run your command
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    //check if there are records
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            //display retrieved record (first column only/string value)
                            Console.WriteLine(dr.GetInt32(0));
                            Console.WriteLine(dr.GetString(1));
                            Console.WriteLine(dr.GetInt32(2));
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found.");
                    }
                    dr.Close();

                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }

            return null;
        }

        private static void writeTable()
        {
            
            string connString = @"Server=localhost,1433;Database=CLASSES;User ID=sa;Password=password;Encrypt=False;";

            var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlServer(connString).Options;

            using (var dbContext = new MyDbContext(options))
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();

                dbContext.AddRange(classTable);
                dbContext.SaveChanges();

                dbContext.AddRange(MethodTable);
                dbContext.SaveChanges();

                dbContext.AddRange(ParameterTypesTable);
                dbContext.SaveChanges();

                dbContext.AddRange(ParametersTable);
                dbContext.SaveChanges();
            }
        }

        private static List<ClassNames> GetClassesFromMongo() 
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase database = client.GetDatabase("test2");
            var collection = database.GetCollection<ClassNames>("ClassNames2");

            //InsertClassNames(collection);

            List<ClassNames> list = GetClassNames(collection);

            //create new collection
            IMongoCollection<ClassNames> collectionWithProperties = database.GetCollection<ClassNames>("ClassNamesWithProperties");

            //CreateNewMongoTable(collectionWithProperties, list);

            return list;

        }

        private static void CreateNewMongoTable(IMongoCollection<ClassNames> collection, List<ClassNames> list)
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