2009-10-28 rread
====================================
This DAL folder exists for two purposes.

1) To provide the "partial struct Databases" class for all modules using Subsonic 2.10.

   By default, Subsonic wants to generate this partial class in each instance of the AllStructs.cs file.
   This causes a conflict when more than one module uses Subsonic.  The Subsonic templates in the
   FormBuilder and MasterDetail modules have been modified to not generate the partial class, and therefore
   prevent compile-time errors.
   
2) To provide a place to generate Subsonic model classes for custom database objects in the project.