<%@ Page Language="C#"%>
<%@ Import namespace="SubSonic.Utilities"%>
<%@ Import Namespace="SubSonic" %>
<%
    const string providerName = "#PROVIDER#";
    const string viewName = "#VIEW#";
    //The data we need
    TableSchema.Table view = DataService.GetSchema(viewName, providerName, TableType.View);
    ICodeLanguage language = new CSharpCodeLanguage();
    
    //The main vars we need
    TableSchema.TableColumnCollection cols = view.Columns;
    string className = view.ClassName;
    string nSpace = DataService.Providers[providerName].GeneratedNamespace;

    
%>
namespace <%=nSpace %>{
    /// <summary>
    /// Strongly-typed collection for the <%=className%> class.
    /// </summary>

    [Serializable]
    public partial class <%=className%>Collection : ReadOnlyList[<]<%= className %>, <%=className%>Collection[>]
    {        
        public <%=className%>Collection() {}
    }

    /// <summary>
    /// This is  Read-only wrapper class for the <%=viewName%> view.
    /// </summary>
    [Serializable]
    public partial class <%=className%> : ReadOnlyRecord[<]<%= className %>[>], IReadOnlyRecord
    {
    
	    #region Default Settings
	    protected static void SetSQLProps() 
	    {
		    GetTableSchema();
	    }
	    #endregion

        #region Schema Accessor
	    public static TableSchema.Table Schema
        {
            get
            {
                if (BaseSchema == null)
                {
                    SetSQLProps();
                }
                return BaseSchema;
            }
        }
    	
        private static void GetTableSchema() 
        {
            if(!IsSchemaInitialized)
            {
                //Schema declaration
                TableSchema.Table schema = new TableSchema.Table("<%=viewName%>", TableType.View, DataService.GetInstance("<%=providerName%>"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"<%=view.SchemaName%>";

                //columns
                <%
                foreach(TableSchema.TableColumn col in cols)
				{
                    string varName = "col" + col.ArgumentName;
                %>
                TableSchema.TableColumn <%=varName %> = new TableSchema.TableColumn(schema);
                <%=varName %>.ColumnName = "<%= col.ColumnName %>";
                <%=varName %>.DataType = DbType.<%=col.DataType %>;
                <%=varName %>.MaxLength = <%=col.MaxLength %>;
                <%=varName %>.AutoIncrement = false;
                <%=varName %>.IsNullable = <%=col.IsNullable.ToString().ToLower()%>;
                <%=varName %>.IsPrimaryKey = false;
                <%=varName %>.IsForeignKey = false;
                <%=varName %>.IsReadOnly = <%= col.IsReadOnly.ToString().ToLower() %>;
                <%
				if(col.IsForeignKey)
				{
                %>
				<%=varName %>.ForeignKeyTableName = "<%= col.ForeignKeyTableName %>";
                <% } %>
                schema.Columns.Add(<%=varName%>);

                <%
                }
                %>
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["<%=providerName %>"].AddSchema("<%=viewName%>",schema);
            }
        }
        #endregion
        
        #region Query Accessor
	    public static Query CreateQuery()
	    {
		    return new Query(Schema);
	    }
	    #endregion
	    
	    #region .ctors
	    public <%=className %>()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }

        public <%=className %>(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public <%=className %>(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public <%=className %>(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
        <%
        foreach(TableSchema.TableColumn col in cols){
            string propName = col.PropertyName;
            string varType = Utility.GetVariableType(col.DataType, col.IsNullable, language);
        %>  
        [XmlAttribute("<%=propName%>")]
        [Bindable(true)]
        public <%=varType%> <%=propName%> 
	    {
		    get
		    {
			    return GetColumnValue[<]<%= varType %>[>]("<%= col.ColumnName %>");
		    }
            set 
		    {
			    SetColumnValue("<%=col.ColumnName%>", value);

            }
        }
	    <%
	    }
	    %>
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    <% 
		    foreach (TableSchema.TableColumn col in cols) {
                string propName = col.PropertyName;
            %>
            public static string <%=propName%> = @"<%=col.ColumnName%>";
            <%
              } 
            %>

	    }
	    #endregion
	    
	    
	    #region IAbstractRecord Members


        public new CT GetColumnValue[<]CT[>](string columnName) {
            return base.GetColumnValue[<]CT[>](columnName);
        }

        public object GetColumnValue(string columnName) {
            return base.GetColumnValue[<]object[>](columnName);
        }

        #endregion
	    
    }
}
