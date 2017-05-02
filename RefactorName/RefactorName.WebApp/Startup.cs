using Microsoft.Owin;
using Owin;
using RefactorName.Core;
using RefactorName.Core.Workflow;

[assembly: OwinStartupAttribute(typeof(RefactorName.WebApp.Startup))]
namespace RefactorName.WebApp
{
    public partial class Startup
    {
        public static readonly User Jane = new User { UserId = 1, Username = "Jane" };
        public static readonly User Tom = new User { UserId = 2, Username = "Tom" };
        public static readonly User Gary = new User { UserId = 3, Username = "Gary" };

        public static readonly Group Executives = new Group("Executives");

        public static readonly State A = new State("A", null, StateType.Start) { StateId = 1 };
        public static readonly State B = new State("B", null, StateType.Normal) { StateId = 2 };
        public static readonly State C = new State("C", null, StateType.Denied) { StateId = 3 };

        public static readonly Action ApprovedByRequester = new Action("ApprovedByRequester", "", ActionType.Approve) { ActionId = 1 };
        public static readonly Action ApprovedbyExecutives = new Action("ApprovedbyExecutives", "", ActionType.Approve) { ActionId = 2 };
        public static readonly Action DeniedbyExecutives = new Action("DeniedbyExecutives", "", ActionType.Deny) { ActionId = 3 };
        public static readonly Action DeniedbyRequester = new Action("DeniedbyRequester", "", ActionType.Deny) { ActionId = 4 };

        public static readonly Transition T1 = new Transition(A, B) { TransitionId = 1 };
        public static readonly Transition T2 = new Transition(A, C) { TransitionId = 2 };
        public static readonly Transition T3 = new Transition(B, C) { TransitionId = 3 };

        public void Configuration(IAppBuilder app)
        {
            //T1.Actions.Add(ApprovedByRequester);
            //T1.Actions.Add(ApprovedbyExecutives);
            //T2.Actions.Add(DeniedbyExecutives);
            //T3.Actions.Add(DeniedbyRequester);

            //A.Outgoing.Add(T1);
            //A.Outgoing.Add(T2);

            //B.Outgoing.Add(T3);
            //B.Ingoing.Add(T1);

            //C.Ingoing.Add(T2);
            //C.Ingoing.Add(T3);

            //Request request = new Request("My Request", A);
            //request.Perform(ApprovedByRequester);

            //request.Perform(ApprovedbyExecutives);

            //request.Perform(DeniedbyExecutives);

            //request.Perform(DeniedbyRequester);
            ConfigureAuth(app);
        }
    }
}
