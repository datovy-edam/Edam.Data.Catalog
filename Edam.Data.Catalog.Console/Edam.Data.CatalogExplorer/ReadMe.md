# Catalog Explorer

This (Uno Platform) console application is provided to demonstrate how to use
the Edam.Data.Catalog (DB and Model) libraries.

The main objective is to have a Catalog of items such as folders and files that
will work both in Windows or a Browser (WebApplication) with the minimum
dependencies to other external (paid) services.  The (TO-BE) Catalog Services
API will include all the expected functionality to allow access to file and
folders for EDAM Projects but still these services should be independent and its
functionality applicable to any application.

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

## What is next?

- Currently the enphasis is on the Windows platform and will keep advancing the
libraries to that end.
- As important, libraries to support the API Catalog services hopefully that
will fully implment the same functionality as made available through the
ICatalogService interface will be developed next or in parralel.


