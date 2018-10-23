namespace IntoItIf.Base.Helpers
{
   using System;
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;

   internal static class AsyncHelper
   {
      #region Methods

      /// <summary>
      ///    Execute's an async Task<T /> method which has a void return value synchronously
      /// </summary>
      /// <param name="task">Task<T /> method to execute</param>
      internal static void RunSync(Func<Task> task)
      {
         var oldContext = SynchronizationContext.Current;
         var synch = new ExclusiveSynchronizationContext();
         SynchronizationContext.SetSynchronizationContext(synch);
         synch.Post(
            async _ =>
            {
               try
               {
                  await task();
               }
               catch (Exception e)
               {
                  synch.InnerException = e;
                  throw;
               }
               finally
               {
                  synch.EndMessageLoop();
               }
            },
            null);
         synch.BeginMessageLoop();

         SynchronizationContext.SetSynchronizationContext(oldContext);
      }

      /// <summary>
      ///    Execute's an async Task<T /> method which has a T return type synchronously
      /// </summary>
      /// <typeparam name="T">Return Type</typeparam>
      /// <param name="task">Task<T /> method to execute</param>
      /// <returns></returns>
      internal static T RunSync<T>(Func<Task<T>> task)
      {
         var oldContext = SynchronizationContext.Current;
         var synch = new ExclusiveSynchronizationContext();
         SynchronizationContext.SetSynchronizationContext(synch);
         var ret = default(T);
         synch.Post(
            async _ =>
            {
               try
               {
                  ret = await task();
               }
               catch (Exception e)
               {
                  synch.InnerException = e;
                  throw;
               }
               finally
               {
                  synch.EndMessageLoop();
               }
            },
            null);
         synch.BeginMessageLoop();
         SynchronizationContext.SetSynchronizationContext(oldContext);
         return ret;
      }

      #endregion

      private class ExclusiveSynchronizationContext : SynchronizationContext
      {
         #region Fields

         private readonly Queue<Tuple<SendOrPostCallback, object>> _items =
            new Queue<Tuple<SendOrPostCallback, object>>();

         private readonly AutoResetEvent _workItemsWaiting = new AutoResetEvent(false);
         private bool _done;

         #endregion

         #region Properties

         // ReSharper disable once MemberCanBePrivate.Local
         internal Exception InnerException { get; set; }

         #endregion

         #region Public Methods and Operators

         public override SynchronizationContext CreateCopy()
         {
            return this;
         }

         public override void Post(SendOrPostCallback d, object state)
         {
            lock (_items)
            {
               _items.Enqueue(Tuple.Create(d, state));
            }

            _workItemsWaiting.Set();
         }

         public override void Send(SendOrPostCallback d, object state)
         {
            throw new NotSupportedException("We cannot send to our same thread");
         }

         #endregion

         #region Methods

         internal void BeginMessageLoop()
         {
            while (!_done)
            {
               Tuple<SendOrPostCallback, object> task = null;
               lock (_items)
               {
                  if (_items.Count > 0) task = _items.Dequeue();
               }

               if (task != null)
               {
                  task.Item1(task.Item2);
                  if (InnerException != null) // the method threw an exeption
                     throw new AggregateException("AsyncHelper.Run method threw an exception.", InnerException);
               }
               else
               {
                  _workItemsWaiting.WaitOne();
               }
            }
         }

         internal void EndMessageLoop()
         {
            Post(_ => _done = true, null);
         }

         #endregion
      }
   }
}