using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EfCoreDemo
{
    public class DbTest
    {
        /// <summary>
        /// 简单操作
        /// </summary>
        public static void method1()
        {
            using (var db = new BloggingContext())
            {
                db.Database.EnsureCreated();
                // Create
                Console.WriteLine("Inserting a new blog");
                db.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
                db.SaveChanges();

                // Read
                Console.WriteLine("Querying for a blog");
                var blog = db.Blogs
                    .OrderBy(b => b.BlogId)
                    .First();

                // Update
                Console.WriteLine("Updating the blog and adding a post");
                blog.Url = "https://devblogs.microsoft.com/dotnet";
                blog.Posts.Add(
                    new Post
                    {
                        Title = "Hello World",
                        Content = "I wrote an app using EF Core!"
                    });
                db.SaveChanges();

                // Delete
                Console.WriteLine("Delete the blog");
                db.Remove(blog);
                db.SaveChanges();

            }
            Console.ReadKey();
        }
    }
}
