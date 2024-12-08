# Catalog Explorer

This (Uno Platform) console application is provided to demonstrate how to use
the Edam.Data.Catalog (DB and Model) libraries.

The main objective is to have a Catalog of items such as folders and files that
will work both in Windows or a Browser (WebApplication) with the minimum
dependencies to other external (paid) services.  The Catalog Services
API include all the expected functionality to allow access to file and
folders for EDAM Projects but still these services are independent and its
functionality applicable to any application.

## Dependencies

As of this post the Catalog Explorer is dependent on the `Edam.Common` and
`Edam.UI.Common` libraries that provides generic or common application functionality
such as Diagnostics results log and others.

## Catalog Explorer Console (Solution)
The solution and project can be found in:

```
Edam.Libraries/Edam.Data.Catalog/Edam.CatalogExplorer/Edam.CatalogExplorer.Console.sln
```

## Catalog Repository
This first implementation uses an EF based repository to manage Containers,
(File) Items, and Item Data (Files), details follow:

- Containers are similar to a file-system drive.  They have a "root" item that
is automatically created and will have its independent tree representation that
can be built by submitting file paths as you will do in a File System.

- A container (File) Item can be a branch or leaf of a Tree and may have zero,
one, or more data (Files) that are defined independently and stored in the
Data Store as binary (blob) files.

- A Data (File) is a child of an Item and will hold the binary blob.

## Catalog Model
The Catalog Model define the C# classes used in the related Catalog Repository
implementation of a database to manage Containers, (File) Items, and Item Data
(Files), details follow:

- Containers are similar to a file-system drive.  They have a "root" item that
is automatically created and will have its independent tree representation that
can be built by submitting file paths as you will do in a File System.

- A container (File) Item can be a branch or leaf of a Tree and may have zero,
one, or more data (Files) that are defined independently and stored in the Data
Store as binary (blob) files.

- A Data (File) is a child of an Item and will hold the binary blob.
  
The Catalog Tree Builder can be used to prepare a Tree that could be viewed in
Web or other related targets.

## Catalog Service Interfaces
As of this version there are 2 available implentations to support the Catalog
Service instance including:

- local service - implemented by instancing the Catalog Service within the
host.
- web service - implemented by firing web-service that implements the
interface.

The selection of one or the other is done by detecting the Uno Platform
`Environment.OSVersion.Platform` variable that will conditionally instance the
appropiate instance (local or other).  Get an instance of the Catalog Service by
calling:

```
var catalogService = CatalogServiceHelper.GetCatalog([connection-uri]);
```

or

```
var catalogService = await CatalogServiceHelper.GetCatalogAsync([connection-uri]);
```

Note that the optional connection URI can be a connection string if local or an
http URI if a web service is required.  If not provided it must be set using
the main project appsettings.json setting the `DefaultConnectionString` and/or
the `CatalogServiceBaseUri`.

## Prepare the Catalog DB
To initialize or access the Catalog database (DB) make sure to update the
windows application and Catalog Service projects `appsettings.json`
ConnectionStrings or other keys, therefore make sure to update all pointing to
your MS-SQL database instance.

When running the Catalog Explorer under the windows app or webassembly browser
the Catalog database (DB) will be created if it does not exists.

## Testing the Catalog Explorer in WebAssembly
In the Windows Explorer find the CatalogService solution that should be relative
to:

```
...Edam.Libraries/Edam.Data.Catalog/Edam.Data.CatalogService/
```

Open the Edam.Data.CatalogService.sln solution in VisualStudio and Debug or run
it.  A Browser instance will open or you could search for:

```
https://localhost:7069/catalogservice
```

After searching for the above resource you will get a message about a page that
can't be found. To submit requests to the service run the CatalogExplorer app in
Windows or a Browser. Or browse for:

```
https://localhost:7069/catalogservice/container/list?sessionId=&containerId
```
Note that this request takes 2 arguments "SessionId" and "ContainerId".  For now
The session ID's are not validated and subsequently it will return the list of
availables containers, for example:
```
[
    {
        "id": "9a269989-5673-4a15-a1fc-12f4660c62ae",
        "containerId": "default",
        "description": "Default",
        "contentType": "application/json",
        "catalog": "{}",
        "createdDate": "2024-09-29T17:30:59.8181393-04:00",
        "updatedDate": "2024-09-29T17:30:59.8181467-04:00",
        "recordStatusDate": "2024-09-29T17:30:59.8181475-04:00",
        "recordStatusCode": "Active"
    }
]
```

Also, note that every Catalog have a "default" container.

# Libraries

Some notes about used Libraries follow.

## Monaco Editor

The Monaco Editor library is based on a project that was downlooaded from GitHub
package named `WinUI.Monaco-Editor`.

This library was modified to allow multiple instances of the Editor (see updates
in the `Editor Core` region in the `MonacoEditor.xaml.cs` file).  As
downloaded when a the control is revisited the editor reinitialized resources and
previously editor context is not preserve.  This was fixed by calling the
`InitializeEditor` method only once and not every time is revisited.  To understand
what was modified you need to compare the original project from GitHub to the
`MonacoEditor` code as available in this solution.

# What is next?

- Continue adding functionality to the Catalog Explorer.
- 


