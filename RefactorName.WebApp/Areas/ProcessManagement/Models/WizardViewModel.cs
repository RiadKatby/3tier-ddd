using RefactorName.Core;
using RefactorName.WebApp;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class WizardViewModel
{
    public int CurrentStepIndex { get; set; }
    public IList<IStepViewModel> Steps { get; set; }

    public Process Process { get; set; }

    public int LastOrder { get; set; }

    public void Initialize()
    {
        Steps = typeof(IStepViewModel)
            .Assembly
            .GetTypes()
            .Where(t => !t.IsAbstract && typeof(IStepViewModel).IsAssignableFrom(t))
            .Select(t => (IStepViewModel)Activator.CreateInstance(t))
            .ToList();
        Process = new Process();
        LastOrder = 0;
        //Steps[0] = (IStepViewModel)((ProcessModel)Steps[0]).FillDDLsWithUsers();
        //Steps[1] = (IStepViewModel)((GroupModel)Steps[1]).FillDDLsWithUsers();
    }
}