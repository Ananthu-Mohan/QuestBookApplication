using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestLibraryApplication.Abstraction
{
    public interface IHttpUtility
    {
        Task<string> GetAsyncMethod();
        Task<string> EditAsyncMethod(string EditResponse);
        Task<string> CreateAsyncMethod(string NewResponse);
        Task<string> DeleteAsyncMethod();

    }
}
