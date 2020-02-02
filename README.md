# Data2Go

### What is Data2Go?

It's a quick wrapper on top of a data access provider that allows you to perform CRUD operations.

### Why would you need such a thing?

Well... first of all you don't. 

You can get a full feature ORM library like EF and start doing the rest work yourself. However if you are lazy like I am, and you realise you'll basically do the same work over and over again for every project, at some point you'll start wondering if maybe... just maybe, you need to do something a little bit more reusable, plain and simple, that just works...

That's how Data2Go was born.

### Ok, but what exactly can it do?

This super tiny library is focused around the [unit of work and repository patterns](https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application). It allows you to do CRUD operations. It also has support for transactions if the underlying data provier supports it

### Show me the code

The sample code below uses EF as the underlying data provider. Calling the `ToGo` extension method on the EF data context creates the 2GO `UnitOfWork` instance. The rest is just code completion:

```C#

var myDbContext = ... //create and setup the DbContext

//to create a the unit of work instance, just call ToGo on your EF DbContext
var toGo = myDbContext.ToGo();

//creating a repository and adding data
toGo
    .GetRepository<ToGoOrderItem>()
    .Add(new ToGoOrderItem
     {
        Name = "Beef Burger",
        Size = Size.Big
     });
     
await toGo.SaveAsync();
     
//creating a repository and reading data     
var big = toGo
    .GetRepository<ToGoOrderItem>()
    .Query()
    .Single(i => i.Size == Size.Big);

//updating data
big.Name = "Big Beef Burger";

await toGo.SaveAsync();

//creating a repository and removing data
toGo
    .GetRepository<ToGoOrderItem>()
    .Remove(big);

await toGo.SaveAsync();

```

Check the tests package of each provider implementation to get more info on what is possible.

### Supported providers

Support for EntityFramework and EntityFrameworkCore is out-of-the-box. However, given that Data2Go was designed to be provider agnostic, other providers can be easily added. 
Take a look at the 2 EF providers as an example of how easy it is to add a new one. Pull requests are welcomed ;)
