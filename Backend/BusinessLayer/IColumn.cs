using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{/// <summary>
/// for mock 
/// </summary>
	public interface IColumn
	{
        int Id { get; }
        string Name { get ; }
        int MaxTaskLimit { get ; set; }
        Dictionary<int, Task> DictTask { get ; }
        bool isEmpty();
    }
}
