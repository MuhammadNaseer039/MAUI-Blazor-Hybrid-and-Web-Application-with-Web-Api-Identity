using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using LayoutPracticeMAUIWEB.Shared.Models;

namespace LayoutPracticeMAUIWEB.Shared.Services
{
    public interface IBlogService
    {
        Task<List<Blog>> GetAllBlogs();

        Task<Blog> GetByIdAsync(int Id);
        Task<Blog> CreateAsync(Blog blog);

        Task<int> UpdateAsync(int Id, Blog blog);
        Task<int> DeleteAsync(int Id);
    }
}
