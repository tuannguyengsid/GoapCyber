using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GSID.Web.Core.Helpers
{
    /// <summary>
    /// Helper class to run async methods within a sync process.
    /// </summary>
    public static class AsyncUtil
    {
        private static readonly TaskFactory _taskFactory = new
            TaskFactory(CancellationToken.None,
                        TaskCreationOptions.None,
                        TaskContinuationOptions.None,
                        TaskScheduler.Default);

        /// <summary>
        /// Executes an async Task method which has a void return value synchronously
        /// USAGE: AsyncUtil.RunSync(() => AsyncMethod());
        /// </summary>
        /// <param name="task">Task method to execute</param>
        public static void RunSync(Func<Task> task)
            => _taskFactory
                .StartNew(task)
                .Unwrap()
                .GetAwaiter()
                .GetResult();

        /// <summary>
        /// Executes an async Task<T> method which has a T return type synchronously
        /// USAGE: T result = AsyncUtil.RunSync(() => AsyncMethod<T>());
        /// </summary>
        /// <typeparam name="TResult">Return Type</typeparam>
        /// <param name="task">Task<T> method to execute</param>
        /// <returns></returns>
        public static TResult RunSync<TResult>(Func<Task<TResult>> task)
            => _taskFactory
                .StartNew(task)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
    }
}
//https://www.ryadel.com/en/asyncutil-c-helper-class-async-method-sync-result-wait/
//https://docs.microsoft.com/en-us/aspnet/mvc/overview/performance/using-asynchronous-methods-in-aspnet-mvc-4
//The asynchronous programming concept has become a widely used standard in ASP.NET since the.NET Framework 4 thanks to the introduction of the Task class: ASP.NET MVC 4 leverages this approach with the Task<ActionResult> return type, which lets developers write asynchronous action methods.The Tasks object instances are represented by the Task type and related types in the System.Threading.Tasks namespace: working with these kind of objects has been made much easier starting from.NET Framework 4.5 and its await and async keywords, which are arguably less complex from the average developer than previous asynchronous approaches.

//To briefly summarize their purpose, we could say that:


//The await keyword is syntactical shorthand for indicating that a piece of code should asynchronously wait on some other piece of code.
//The async keyword can be used to mark methods as task-based asynchronous methods and keep the compiler aware of this.
//The combination of await, async, and the Task object is called the Task-based Asynchronous Pattern (TAP). For more info about such approach, we strongly recommend reading this Microsoft article about async programming in .NET 4. 


//In this post we’ll take for granted that the reader knows all the basics of TAP programming in ASP.NET and deal with some specific tips and tricks for working with async methods in C#.

//Running sync method as async
//Any standard sync method can be run inside a Task by using the following one-liner:

//Task.Run(() => SyncMethod());
//1
//Task.Run(() => SyncMethod());
//Why should we do that? Well, the answer is simple: the main difference between sync and async methods is that the former ones blocks their execution thread while the latter don’t.With this workaround, the “blocking” sync method in a background thread, leaving the foreground one unblocked.This can be very useful in desktop and/or mobile applications, where you would avoid blocking the UI thread while executing resource-intensive and/or time-consuming tasks; it has little use within a web application context, since all threads will spawn from the same thread pool of the request thread and the context will expect them all to complete – or to timeout – before returning the HTTP response.

//Run async method as sync
//There are a number of ways to run async methods in a synchronous context: let’s see them all, from the best one to the worst possible way of do that.

//The Good
//The best way to run any async method and wait for it to complete is to use the await keyword in the following way:

//var t = await AsyncMethod<T>();
//1
//var t = await AsyncMethod<T>();
//What await does here is to return the result of the operation immediately and synchronously if the operation has already completed or, if it hasn’t, to schedule a continuation to execute the remainder of the async keyword before returning the control to the caller.When the asynchronous operation completes, the scheduled completion will then execute. This is great to avoid deadlocks and can also be used within a try/catch block to get the exception raised by the AsyncMethod itself.

//The only downside of using the async keyword is that it requires the containing method to be flagged as async as well.If you are unable to do that, you can only opt for a less secure workaround to achieve the same goal such as those explained below.
//Then you can use it that way:

//C#
//var t = AsyncUtil.RunSync<T>(() => AsyncMethod<T>());
//1
//var t = AsyncUtil.RunSync<T>(() => AsyncMethod<T>());
//This workaround is definitely more complex than the previous one-liner, but it’s a decent way to perform an async-within-sync call.As we can see, the helper class basically creates, configure and starts an async task on-the-fly, then unwraps it and synchronously wait for its result: just like the await method above, this approach will prevent deadlocks and could be used within a try/catch block to get the exception raised by the AsyncMethod itself.


//UPDATE: if our asynchronous method doesn’t need to synchronize back to its context, then we can use an one-liner here as well by making use of the Task.WaitAndUnwrapException method in the following way:


//var result = AsyncMethod().WaitAndUnwrapException();
//1
//var result = AsyncMethod().WaitAndUnwrapException();
//Or the following alternative:


//Task.Run(async() => await AsyncMethod()).WaitAndUnwrapException();
//1
//Task.Run(async() => await AsyncMethod()).WaitAndUnwrapException();
//It’s worth noting that we do not want to use Task.Wait or Task.Result here, because they both wrap exceptions in AggregateException, which can be very hard to debug and/or test.

//IMPORTANT: the above solution is only appropriate if the AsyncMethod does not synchronize back to its context: in other words, every await in the AsyncMethod should end with ConfigureAwait(false). This basically means that it can’t update UI elements, access the ASP.NET request context or perform other context-aware activities.
//The Bad
//Among the various worst ways to run async tasks in a sync way, let’s consider the following:

//var t = AsyncMethod().GetAwaiter().GetResult();
//var t = AsyncMethod().Result;
//var t = AsyncMethod().Wait;
//1
//2
//3
//var t = AsyncMethod().GetAwaiter().GetResult();
//var t = AsyncMethod().Result;
//var t = AsyncMethod().Wait;
//The problem of the above approaches lies in the fact that they all configure a synchronous wait on the execution thread. This basically mean that the main thread cannot be doing any other work while it is synchronously waiting for the task to complete: this is not only unefficient, but it will also pose a serious risk of causing a deadlock the main thread.

//To better understand this, consider the following scenario:

//AsyncMethod() is a method that will only do something whenever its main execution thread is free.
//SyncMethod() performs an AsyncMethod().Result operation, thus posing itself in a synchronous wait.
//That’s it: we can clearly see how AsyncMethod() will never be able to return anything, since the main thread is locked by its container SyncMethod() thanks to the synchronous wait configured by Result. In other words, the main thread will be permanently deadlocked by itself.

//Related Links
//If you want to know more about asynchronous programming in C#, you should definitely take a look at the following must-read posts:

//Await, and UI, and deadlocks! Oh my!, an excellent MSDN article by Stephen Toub explaining how to use async/await when dealing with the UI and with event handlers.
//Getting started with async/await, another amazing article by Jon Goldberger published on the Xamarin official blog, explaining how to properly use async/await.
