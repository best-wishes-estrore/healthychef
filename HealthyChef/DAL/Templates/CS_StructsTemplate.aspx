<%@ Page Language="C#"%>
<%@ Import Namespace="SubSonic" %>
<%@ Import Namespace="System.Data" %>

<%foreach(DataProvider p in DataService.Providers)
 {
     TableSchema.Table[] tables = DataService.GetTables(p.Name);
     TableSchema.Table[] views = DataService.GetViews(p.Name);

%>
namespace <%=p.GeneratedNamespace%>
{
	#region Tables Struct
	public partial struct Tables
	{
		<%
     foreach (TableSchema.Table t in tables)
     {
         if (CodeService.ShouldGenerate(t.Name, p.Name))
         {
%>
		public static string <%=t.ClassName%> = @"<%=t.Name%>";
        <%
         }
     }
%>
	}
	#endregion


    #region Schemas
    public partial class Schemas {

		<%
     foreach (TableSchema.Table t in tables)
     {
         if (CodeService.ShouldGenerate(t.Name, p.Name))
         {
%>
		public static TableSchema.Table <%=t.ClassName%>{
            get { return DataService.GetSchema("<%=t.Name%>","<%=p.Name%>"); }
		}
        <%
         }
     }
%>
	
    }
    #endregion



    #region View Struct
    public partial struct Views 
    {
		<%
     foreach (TableSchema.Table v in views)
     {
         if (CodeService.ShouldGenerate(v.Name, p.Name))
         {
%>
		public static string <%=v.ClassName%> = @"<%=v.Name%>";
        <%
         }
     }
%>
    }
    #endregion
    
    #region Query Factories
	public static partial class DB
	{
        public static DataProvider _provider = DataService.Providers["<%=p.Name%>"];
        static ISubSonicRepository _repository;

        public static ISubSonicRepository Repository {
            get {
                if (_repository == null)
                    return new SubSonicRepository(_provider);

                return _repository; 
            }
            set { _repository = value; }
        }
	

        public static Select SelectAllColumnsFrom<T>() where T : RecordBase<T>, new()
	    {

            return Repository.SelectAllColumnsFrom<T>();
            
	    }

	    public static Select Select()
	    {
            return Repository.Select();
	    }

	    
		public static Select Select(params string[] columns)
		{
            return Repository.Select(columns);
        }

	    
		public static Select Select(params Aggregate[] aggregates)
		{
            return Repository.Select(aggregates);
        }

   
	    public static Update Update<T>() where T : RecordBase<T>, new()
	    {
            return Repository.Update<T>();
	    }
     
	    
	    public static Insert Insert()
	    {
            return Repository.Insert();
	    }

	    
	    public static Delete Delete()
	    {
            
            return Repository.Delete();
	    }

	    
	    public static InlineQuery Query()
	    {
            
            return Repository.Query();
	    }
	    	    
	    <%if (p.TableBaseClass=="RepositoryRecord"){%>
	    #region Repository Compliance
	    
        public static T Get<T>(object primaryKeyValue) where T : RepositoryRecord<T>, new() 
        {
            return Repository.Get<T>(primaryKeyValue);
        }

        public static T Get<T>(string columnName, object columnValue) where T : RepositoryRecord<T>, new()
        {
            
            return Repository.Get<T>(columnName,columnValue);

        }

        
        public static void Delete<T>(string columnName, object columnValue) where T : RepositoryRecord<T>, new() 
        {

            Repository.Delete<T>(columnName, columnValue);            

        }

        public static void Delete<T>(RepositoryRecord<T> item) where T : RepositoryRecord<T>, new() 
        {
            
            Repository.Delete<T>(item);
        }

        
        public static void Destroy<T>(RepositoryRecord<T> item) where T : RepositoryRecord<T>, new() 
        {
            
            Repository.Destroy<T>(item);
        }

        
        public static void Destroy<T>(string columnName, object value) where T : RepositoryRecord<T>, new() 
        {
            
            Repository.Destroy<T>(columnName,value);
        }

        public static int Save<T>(RepositoryRecord<T> item) where T : RepositoryRecord<T>, new() 
        {
            
            return Repository.Save<T>(item);
        }

        
        public static int Save<T>(RepositoryRecord<T> item, string userName) where T : RepositoryRecord<T>, new()
        {
            
            return Repository.Save<T>(item,userName);

        }

        public static int SaveAll<ItemType, ListType>(RepositoryList<ItemType, ListType> itemList)
          where ItemType : RepositoryRecord<ItemType>, new()
          where ListType : RepositoryList<ItemType, ListType>, new()
        {
            return Repository.SaveAll<ItemType, ListType>(itemList);
        }
 
        public static int SaveAll<ItemType, ListType>(RepositoryList<ItemType, ListType> itemList, string userName)
          where ItemType : RepositoryRecord<ItemType>, new()
          where ListType : RepositoryList<ItemType, ListType>, new()
        {
            return Repository.SaveAll<ItemType, ListType>(itemList, userName);
        }


        public static int Update<T>(RepositoryRecord<T> item) where T : RepositoryRecord<T>, new() 
        {
            
            return Repository.Update<T>(item, "");
        }

        public static int Update<T>(RepositoryRecord<T> item, string userName) where T : RepositoryRecord<T>, new() 
        {
            
            return Repository.Update<T>(item, userName);

        }

        public static int Insert<T>(RepositoryRecord<T> item) where T : RepositoryRecord<T>, new() 
        {
            
            return Repository.Insert<T>(item);
        }

        public static int Insert<T>(RepositoryRecord<T> item, string userName) where T : RepositoryRecord<T>, new() 
        {
            
            return Repository.Insert<T>(item,userName);

        }

	    #endregion
        <%}%>
	}
    #endregion
    
}
<%} %>


#region Databases
public partial struct Databases 
{
	<%foreach (DataProvider p in DataService.Providers) { %>
	public static string <%= p.Name %> = @"<%= p.Name%>";
    <%}%>
}
#endregion