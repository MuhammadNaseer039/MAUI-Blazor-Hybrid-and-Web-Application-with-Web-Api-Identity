using LayoutPracticeMAUIWEB.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayoutPracticeMAUIWEB.Shared.Services
{
    public class BlogService : IBlogService
    {
        private readonly List<Blog> listofblogs;

        public BlogService()
        {
            listofblogs = new List<Blog>()
            {
                new Blog() { Id = 1, Name = "C#", Description = "Learn C# from basics to advanced.", Author = "Muhammad Naseer", Title = "CSharp", Url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRFzV4Ww4dzvXIxVyhLI0hw2J7DfZ4kYDlHLg&s" },
                new Blog() { Id = 2, Name = "ASP.NET Core", Description = "Web development using ASP.NET Core.", Author = "John Doe", Title = "AspNetCore Guide", Url = "" },
                new Blog() { Id = 3, Name = "JavaScript", Description = "Master JavaScript step-by-step.", Author = "Jane Smith", Title = "JavaScript for Beginners", Url = "https://example.com/image2.jpg" },
                new Blog() { Id = 4, Name = "ReactJS", Description = "Build UIs with React.", Author = "Alex Johnson", Title = "React Fundamentals", Url = "https://example.com/image3.jpg" },
                new Blog() { Id = 5, Name = "Python", Description = "Python programming made easy.", Author = "Emily Brown", Title = "Intro to Python", Url = "" },
                new Blog() { Id = 6, Name = "Laravel", Description = "PHP framework for web artisans.", Author = "Chris White", Title = "Laravel Tips", Url = "https://example.com/image5.jpg" },
                new Blog() { Id = 7, Name = "Node.js", Description = "Server-side JavaScript with Node.", Author = "Robert Green", Title = "NodeJS Crash Course", Url = "https://example.com/image6.jpg" },
                new Blog() { Id = 8, Name = "SQL", Description = "Database queries and tips.", Author = "Natalie Black", Title = "SQL Essentials", Url = "https://example.com/image7.jpg" },
                new Blog() { Id = 9, Name = "Angular", Description = "Develop SPAs using Angular.", Author = "David Blue", Title = "Angular Basics", Url = "https://example.com/image8.jpg" },
                new Blog() { Id = 10, Name = "DevOps", Description = "CI/CD pipelines and tools.", Author = "Sophia King", Title = "DevOps Tools Overview", Url = "https://example.com/image9.jpg" },
                new Blog() { Id = 11, Name = "Cloud Computing", Description = "Exploring AWS, Azure, GCP.", Author = "Liam Clark", Title = "Cloud Essentials", Url = "https://example.com/image10.jpg" },
                new Blog() { Id = 12, Name = "Docker", Description = "Containerize your apps.", Author = "Olivia Martinez", Title = "Docker 101", Url = "https://example.com/image11.jpg" },
                new Blog() { Id = 13, Name = "Kubernetes", Description = "Orchestrate your containers.", Author = "William Lewis", Title = "K8s Crash Course", Url = "https://example.com/image12.jpg" },
                new Blog() { Id = 14, Name = "Java", Description = "OOP concepts and more.", Author = "Isabella Walker", Title = "Java for Everyone", Url = "https://example.com/image13.jpg" },
                new Blog() { Id = 15, Name = "AI & ML", Description = "Getting started with AI and ML.", Author = "James Hall", Title = "Intro to AI", Url = "https://example.com/image14.jpg" },
                new Blog() { Id = 16, Name = "HTML & CSS", Description = "Frontend design skills.", Author = "Grace Allen", Title = "Design with HTML/CSS", Url = "https://example.com/image15.jpg" },
                new Blog() { Id = 17, Name = "TypeScript", Description = "Typed JS for large apps.", Author = "Benjamin Young", Title = "TypeScript Guide", Url = "https://example.com/image16.jpg" },
                new Blog() { Id = 18, Name = "Blazor", Description = "Web apps using C# and .NET.", Author = "Chloe Scott", Title = "Blazor Demo", Url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQep2UTrlWNVO-f_KbsKipNCl40KRJzxxrpgQ&s" },
                new Blog() { Id = 19, Name = "Cyber Security", Description = "Stay secure in the digital world.", Author = "Logan Wright", Title = "Cyber Security Basics", Url = "https://example.com/image18.jpg" },
                new Blog() { Id = 20, Name = "Data Structures", Description = "Fundamentals of data structures.", Author = "Mia Baker", Title = "DSA Introduction", Url = "https://example.com/image19.jpg" }
            };

        }
        public async Task<Blog> CreateAsync(Blog blog)
        {

            if (blog != null)
            {
                blog.Id = listofblogs.Count + 1;
                listofblogs.Add(blog);
            }
            return blog;
        }

        public async Task<int> DeleteAsync(int Id)
        {
            var producttodelete = listofblogs.Where(b => b.Id == Id).FirstOrDefault();
            if (producttodelete != null)
            {
                listofblogs.Remove(producttodelete);
                Id = producttodelete.Id;
            }
            return Id > 0 ? Id : 0;
        }

        public async Task<List<Blog>> GetAllBlogs()
        {
            return listofblogs;
        }

        public async Task<Blog> GetByIdAsync(int Id)
        {
            return listofblogs.Where(b => b.Id == Id).FirstOrDefault();
        }

        public async Task<int> UpdateAsync(int Id, Blog blog)
        {
            var existingblog = listofblogs.Where(b => b.Id == Id).FirstOrDefault();
            if (existingblog != null)
            {
                existingblog.Name = blog.Name;
                existingblog.Description = blog.Description;
            }
            return existingblog.Id;
        }
    }
}
