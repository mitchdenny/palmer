Palmer
======
Palmer is a .NET library that presents a fluent-API which makes it easier to write retry logic. Palmer is developed and maintained by Mitch Denny and is released under the [MIT license](LICENSE).

How do I get my hands on Palmer?
--------------------------------
You can download Palmer is source from GitHub, however as a .NET developer it would be far easier to use NuGet. Simply bring up your package management console and install the package.

```powershell
Install-Package Palmer
```

How to do I use Palmer?
-----------------------
Once you have the library installed using Palmer is simple. Just wrap a call to Palmer around the offending code block and it will manage the retry logic for you.

```c#
Retry.On<WebException>().For(TimeSpan.FromSeconds(15)).With(context =>
  {
	// Code that might periodically fail due to connectivity issues.
  });
```

With these few lines of code we are telling Palmer to keep retrying the code for 15 seconds, or until it succeeds. If the code still fails after 15 seconds then a RetryException is thrown which provides the developer with access to the latest exception, and all previously raised exceptions. If the code throws any other exception that exception is allowed to bubble out of Palmer.

Why is it called "Palmer"?
--------------------------
The origins of the library name are tied to the origins of the quote:

> "If at first you don't succeed, try, try again." - Thomas H. *Palmer*

Thomas H. Palmer was an educator who wrote a teachers manual from which this quote was taken. I felt that the quote was appropriate for what the Palmer library does. At the time I named the library I was tempted to name it after W.C. Fields who had an extended version of the quote.

> "If at first you don't succeed, try, try again. Then quit. There's no point being a damn fool about it." - W.C. Fields.

This is ultimately why you need to use Palmer. Palmer allows you to succinctly declare retry logic and clearly express under which conditions the code should just give up.

More Examples
------------------
Palmer is still a young library and I don't have an exhaustive API reference (feel free to get involved). Rather than provide no documentation I thought I might provide some examples of how you might use Palmer in your code.

###Deadlock Retry
This code shows you to example the last raised exception to determine if it was a deadlock. It retries five times before giving up. This demonstrates a "count-based" retry strategy instead of a duration based retry strategy.

```c#
Retry.On<SqlException>(handle => (handle.Context.LastException as SqlException).Number == 1205).For(5).With(context =>
	{
		// Code that might result in a SQL deadlock.
	});
```

###Retry Forever
Normally you wouldn't retry forever, but the capability to retry forever is included for completeness.

```c#
Retry.On<WebException>().Indefinately().With(context =>
	{
		// Code that might throw a web exception.
	});
```

###Non-Exception Failures
Some APIs don't throw exceptions. Rather than forcing you to throw an exception when you don't need to, Palmer allows you to write an expression to determine whether a retry is necessary.

```c#
var completedSuccessfully = false;

Retry.On(handle => completedSuccessfully).For(5).With(context =>
	{
		completedSuccessfully = SomeApiThatReturnsTrueOrFailsAsSuccessStatus();
	});
```

### Multiple Exceptions
Sometimes the code that you write might throw multiple different exceptions, Palmer allows you to handle this.

```c#
Retry.On<WebException>().For(5).AndOn<SqlException>().For(5).With(context =>
	{
		// Code that might throw a web exception, or a sql exception.
	});
```

In the case above the counts for each exception are independent of each other.

### Post Conditions with Until
Post conditions are very similar to detecting non-exception failures. Where they differ is their intended usage. The On/AndOn methods which take predicates are evaluated against every exception or on every run. The predicate specified with the Until method is only evaulated once an exception has been detected. At this point this method could be used to fail after a total number of exceptions has been reached.

```c#
Retry
	.On<WebException>().Until(handle => handle.Context.Exceptions.Count() > 10)
	.AndOn<SqlException().Until(handle => handle.Context.Exceptions.Count() > 10)
	.With(context =>
	{
		// Code that might throw a web exception.
	});
```

Getting Help
------------
If you are using Palmer I would love to hear about it. The easiest way get help is by [logging an issue on GitHub](https://github.com/MitchDenny/Palmer/issues). Alternatively you can get me on Twitter at [@MitchDenny](http://twitter.com/MitchDenny) or via my blog at http://blog.mitchdenny.com. 

Contributors
------------
I'm always happy to have someone contribute pull requests to the project. If you do I'll add you to the contributors section here.

- [Mitch Denny](http://blog.mitchdenny.com); primary author of the library.
- [Darren Neimke](http://neimke.blogspot.com.au/); helped me out debugging some issues with the library.
- [Maarten Balliauw](http://blog.maartenballiauw.be); helped me out getting the build work on [MyGet](http://myget.org).