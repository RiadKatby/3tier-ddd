namespace RefactorName.SqlServerRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitializeDatabase : DbMigration
    {
        public override void Up()
        {  
            CreateTable(
                "dbo.Actions",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        ProcessId = c.Int(nullable: false),
                        ActionTypeId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 255),
                        Transition_TransitionId = c.Int(),
                    })
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.ActionTypes", t => t.ActionTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Processes", t => t.ProcessId, cascadeDelete: true)
                .ForeignKey("dbo.Transitions", t => t.Transition_TransitionId)
                .Index(t => t.ProcessId)
                .Index(t => t.ActionTypeId)
                .Index(t => t.Transition_TransitionId);
            
            CreateTable(
                "dbo.ActionTypes",
                c => new
                    {
                        ActionTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ActionTypeId);
            
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        ActivityId = c.Int(nullable: false, identity: true),
                        ProcessId = c.Int(nullable: false),
                        ActivityTypeId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 255),
                        State_StateId = c.Int(),
                        Transition_TransitionId = c.Int(),
                    })
                .PrimaryKey(t => t.ActivityId)
                .ForeignKey("dbo.ActivityTypes", t => t.ActivityTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Processes", t => t.ProcessId, cascadeDelete: true)
                .ForeignKey("dbo.States", t => t.State_StateId)
                .ForeignKey("dbo.Transitions", t => t.Transition_TransitionId)
                .Index(t => t.ProcessId)
                .Index(t => t.ActivityTypeId)
                .Index(t => t.State_StateId)
                .Index(t => t.Transition_TransitionId);
            
            CreateTable(
                "dbo.ActivityTypes",
                c => new
                    {
                        ActivityTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ActivityTypeId);
            
            CreateTable(
                "dbo.Fields",
                c => new
                    {
                        FieldId = c.Int(nullable: false, identity: true),
                        ProcessId = c.Int(nullable: false),
                        Name = c.String(),
                        Length = c.Int(nullable: false),
                        Type_FieldTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.FieldId)
                .ForeignKey("dbo.FieldTypes", t => t.Type_FieldTypeId)
                .ForeignKey("dbo.Processes", t => t.ProcessId, cascadeDelete: true)
                .Index(t => t.ProcessId)
                .Index(t => t.Type_FieldTypeId);
            
            CreateTable(
                "dbo.FieldTypes",
                c => new
                    {
                        FieldTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TypeFullName = c.String(),
                    })
                .PrimaryKey(t => t.FieldTypeId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        ProcessId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.GroupId)
                .ForeignKey("dbo.Processes", t => t.ProcessId, cascadeDelete: true)
                .Index(t => t.ProcessId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Group_GroupId = c.Int(),
                        Process_ProcessId = c.Int(),
                        Request_RequestId = c.Int(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Groups", t => t.Group_GroupId)
                .ForeignKey("dbo.Processes", t => t.Process_ProcessId)
                .ForeignKey("dbo.Requests", t => t.Request_RequestId)
                .Index(t => t.Group_GroupId)
                .Index(t => t.Process_ProcessId)
                .Index(t => t.Request_RequestId);
            
            CreateTable(
                "dbo.Processes",
                c => new
                    {
                        ProcessId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ProcessId);
            
            CreateTable(
                "dbo.States",
                c => new
                    {
                        StateId = c.Int(nullable: false, identity: true),
                        ProcessId = c.Int(nullable: false),
                        StateTypeId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 255),
                        Fields_StateFieldId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StateId)
                .ForeignKey("dbo.StateFields", t => t.Fields_StateFieldId)
                .ForeignKey("dbo.StateTypes", t => t.StateTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Processes", t => t.ProcessId, cascadeDelete: true)
                .Index(t => t.ProcessId)
                .Index(t => t.StateTypeId)
                .Index(t => t.Fields_StateFieldId);
            
            CreateTable(
                "dbo.StateFields",
                c => new
                    {
                        StateFieldId = c.Int(nullable: false, identity: true),
                        StateId = c.Int(nullable: false),
                        FieldId = c.Int(nullable: false),
                        IsEditable = c.Boolean(nullable: false),
                        IsRequired = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StateFieldId)
                .ForeignKey("dbo.Fields", t => t.FieldId, cascadeDelete: true)
                .Index(t => t.FieldId);
            
            CreateTable(
                "dbo.StateTypes",
                c => new
                    {
                        StateTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.StateTypeId);
            
            CreateTable(
                "dbo.Transitions",
                c => new
                    {
                        TransitionId = c.Int(nullable: false, identity: true),
                        ProcessId = c.Int(nullable: false),
                        CurrentStateId = c.Int(nullable: false),
                        NextStateId = c.Int(nullable: false),
                        CurrentState_StateId = c.Int(),
                        NextState_StateId = c.Int(),
                    })
                .PrimaryKey(t => t.TransitionId)
                .ForeignKey("dbo.States", t => t.CurrentState_StateId)
                .ForeignKey("dbo.States", t => t.NextState_StateId)
                .ForeignKey("dbo.Processes", t => t.ProcessId, cascadeDelete: true)
                .Index(t => t.ProcessId)
                .Index(t => t.CurrentState_StateId)
                .Index(t => t.NextState_StateId);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        RequestId = c.Int(nullable: false, identity: true),
                        UserRequestedId = c.Int(nullable: false),
                        DateRequested = c.DateTime(nullable: false),
                        UserRequested_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.RequestId)
                .ForeignKey("dbo.Users", t => t.UserRequested_UserId)
                .Index(t => t.UserRequested_UserId);
            
            CreateTable(
                "dbo.RequestDatas",
                c => new
                    {
                        RequestDataId = c.Int(nullable: false, identity: true),
                        RequestId = c.Int(nullable: false),
                        FieldId = c.Int(nullable: false),
                        Value = c.String(),
                        ValueIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RequestDataId)
                .ForeignKey("dbo.Fields", t => t.FieldId, cascadeDelete: true)
                .ForeignKey("dbo.Requests", t => t.RequestId, cascadeDelete: true)
                .Index(t => t.RequestId)
                .Index(t => t.FieldId);
            
            CreateTable(
                "dbo.RequestActions",
                c => new
                    {
                        RequestActionId = c.Int(nullable: false, identity: true),
                        RequestId = c.Int(nullable: false),
                        ActionId = c.Int(nullable: false),
                        TransitionId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsComplete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RequestActionId)
                .ForeignKey("dbo.Actions", t => t.ActionId, cascadeDelete: true)
                .ForeignKey("dbo.Transitions", t => t.TransitionId)
                .ForeignKey("dbo.Requests", t => t.RequestId, cascadeDelete: true)
                .Index(t => t.RequestId)
                .Index(t => t.ActionId)
                .Index(t => t.TransitionId);
            
            CreateTable(
                "dbo.RequestNotes",
                c => new
                    {
                        RequestNoteId = c.Int(nullable: false, identity: true),
                        RequestId = c.Int(nullable: false),
                        StateId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.RequestNoteId)
                .ForeignKey("dbo.States", t => t.StateId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Requests", t => t.RequestId, cascadeDelete: true)
                .Index(t => t.RequestId)
                .Index(t => t.StateId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "UserRequested_UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "Request_RequestId", "dbo.Requests");
            DropForeignKey("dbo.RequestNotes", "RequestId", "dbo.Requests");
            DropForeignKey("dbo.RequestNotes", "UserId", "dbo.Users");
            DropForeignKey("dbo.RequestNotes", "StateId", "dbo.States");
            DropForeignKey("dbo.RequestActions", "RequestId", "dbo.Requests");
            DropForeignKey("dbo.RequestActions", "TransitionId", "dbo.Transitions");
            DropForeignKey("dbo.RequestActions", "ActionId", "dbo.Actions");
            DropForeignKey("dbo.RequestDatas", "RequestId", "dbo.Requests");
            DropForeignKey("dbo.RequestDatas", "FieldId", "dbo.Fields");
            DropForeignKey("dbo.Transitions", "ProcessId", "dbo.Processes");
            DropForeignKey("dbo.Transitions", "NextState_StateId", "dbo.States");
            DropForeignKey("dbo.Transitions", "CurrentState_StateId", "dbo.States");
            DropForeignKey("dbo.Activities", "Transition_TransitionId", "dbo.Transitions");
            DropForeignKey("dbo.Actions", "Transition_TransitionId", "dbo.Transitions");
            DropForeignKey("dbo.States", "ProcessId", "dbo.Processes");
            DropForeignKey("dbo.States", "StateTypeId", "dbo.StateTypes");
            DropForeignKey("dbo.States", "Fields_StateFieldId", "dbo.StateFields");
            DropForeignKey("dbo.StateFields", "FieldId", "dbo.Fields");
            DropForeignKey("dbo.Activities", "State_StateId", "dbo.States");
            DropForeignKey("dbo.Groups", "ProcessId", "dbo.Processes");
            DropForeignKey("dbo.Fields", "ProcessId", "dbo.Processes");
            DropForeignKey("dbo.Users", "Process_ProcessId", "dbo.Processes");
            DropForeignKey("dbo.Activities", "ProcessId", "dbo.Processes");
            DropForeignKey("dbo.Actions", "ProcessId", "dbo.Processes");
            DropForeignKey("dbo.Users", "Group_GroupId", "dbo.Groups");
            DropForeignKey("dbo.Fields", "Type_FieldTypeId", "dbo.FieldTypes");
            DropForeignKey("dbo.Activities", "ActivityTypeId", "dbo.ActivityTypes");
            DropForeignKey("dbo.Actions", "ActionTypeId", "dbo.ActionTypes");
            DropIndex("dbo.RequestNotes", new[] { "UserId" });
            DropIndex("dbo.RequestNotes", new[] { "StateId" });
            DropIndex("dbo.RequestNotes", new[] { "RequestId" });
            DropIndex("dbo.RequestActions", new[] { "TransitionId" });
            DropIndex("dbo.RequestActions", new[] { "ActionId" });
            DropIndex("dbo.RequestActions", new[] { "RequestId" });
            DropIndex("dbo.RequestDatas", new[] { "FieldId" });
            DropIndex("dbo.RequestDatas", new[] { "RequestId" });
            DropIndex("dbo.Requests", new[] { "UserRequested_UserId" });
            DropIndex("dbo.Transitions", new[] { "NextState_StateId" });
            DropIndex("dbo.Transitions", new[] { "CurrentState_StateId" });
            DropIndex("dbo.Transitions", new[] { "ProcessId" });
            DropIndex("dbo.StateFields", new[] { "FieldId" });
            DropIndex("dbo.States", new[] { "Fields_StateFieldId" });
            DropIndex("dbo.States", new[] { "StateTypeId" });
            DropIndex("dbo.States", new[] { "ProcessId" });
            DropIndex("dbo.Users", new[] { "Request_RequestId" });
            DropIndex("dbo.Users", new[] { "Process_ProcessId" });
            DropIndex("dbo.Users", new[] { "Group_GroupId" });
            DropIndex("dbo.Groups", new[] { "ProcessId" });
            DropIndex("dbo.Fields", new[] { "Type_FieldTypeId" });
            DropIndex("dbo.Fields", new[] { "ProcessId" });
            DropIndex("dbo.Activities", new[] { "Transition_TransitionId" });
            DropIndex("dbo.Activities", new[] { "State_StateId" });
            DropIndex("dbo.Activities", new[] { "ActivityTypeId" });
            DropIndex("dbo.Activities", new[] { "ProcessId" });
            DropIndex("dbo.Actions", new[] { "Transition_TransitionId" });
            DropIndex("dbo.Actions", new[] { "ActionTypeId" });
            DropIndex("dbo.Actions", new[] { "ProcessId" });
            DropTable("dbo.RequestNotes");
            DropTable("dbo.RequestActions");
            DropTable("dbo.RequestDatas");
            DropTable("dbo.Requests");
            DropTable("dbo.Transitions");
            DropTable("dbo.StateTypes");
            DropTable("dbo.StateFields");
            DropTable("dbo.States");
            DropTable("dbo.Processes");
            DropTable("dbo.Users");
            DropTable("dbo.Groups");
            DropTable("dbo.FieldTypes");
            DropTable("dbo.Fields");
            DropTable("dbo.ActivityTypes");
            DropTable("dbo.Activities");
            DropTable("dbo.ActionTypes");
            DropTable("dbo.Actions");
        }
    }
}
