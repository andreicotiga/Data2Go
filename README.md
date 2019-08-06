# Data2Go

### So what is Data2Go?

To put it simply, it's a quick (I would not say dirty) wrapper on top of a data access provider that allows get/add/change the persisted data.

### Why would you need such a thing?

Well... first of all you don't. 

You can get a full feature ORM library like EF and start doing the rest work yourself. However if you are lazy like I am, and you realise you'll basically do the same work over and over again for every project, at some point you'll start wondering if maybe... just maybe, you need to do something a little bit more reusable, plain and simple, that just works...

Hence...Data2Go

### Ok, but what exactly can it do?

This super tiny library is focused around the unit of work and repository patterns. It allows you to do CRUD operations. It also has support for transactions if you want to go there :)

### Show me the code

Ok, so once you have your EF DbContext, just call the `ToGo` extension method on it to obtain an `IUnitOfWork` instance. The rest is just VS intellisense:

```C#

var myDbContext = ... //create and setup the DbContext

//get the unit of work, just call ToGo on your DbContext
var unitOfWork = myDbContext.ToGo();

//add some data
unitOfWork
  .GetRepository<ToGoOrderItem>()
  .Add(new ToGoOrderItem
  {
    Name = "Beef Burger",
    Size = Size.Big
  });
 
//save it    
await _unitOfWork.SaveAsync();

//retrieve it
var small = unitOfWork
  .GetRepository<ToGoOrderItem>()
  .Query()
  .Where(i => i.Size == Size.Big);

```

Check the tests package of each provider implementation to get more info on what is possible.

### Supported providers

For now, the concrete providers are EntityFramework and EntityFrameworkCore. However the core Data2Go project is provider agnostic and others can be easily added.
