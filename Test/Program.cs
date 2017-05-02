using RefactorName.Core.SearchEntities;
using RefactorName.Domain.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //var result = ProcessService.Obj.GetAllProcesses();
            //ProcessSearchCriteria searchCriteria = new ProcessSearchCriteria();
            //searchCriteria.Name = "test";
            //var resultSearch = ProcessService.Obj.FindById(4);
            try
            {
                ProcessSearchCriteria search = new ProcessSearchCriteria();
                search.Name = "i";
                var resultSearch = ProcessService.Obj.Find(search);


                //RefactorName.Core.Workflow.Process newprocess = ProcessService.Obj.FindById(2);
                //RefactorName.Core.Workflow.ActionType type = RefactorName.Core.Workflow.ActionType.Resolve;



                //RefactorName.Core.Workflow.Action action = new RefactorName.Core.Workflow.Action("action name", "action description", type);

                //newprocess.AddAction(action);
                //newprocess = ProcessService.Obj.Update(newprocess);

                ////var result = ProcessService.Obj.Create(newprocess);
                //var resultSearch = ProcessService.Obj.FindById(2);
                
                
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message+"\n"+ ex.StackTrace);
                Console.ReadKey();
            }
        }
    }
}
