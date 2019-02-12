using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Microsoft.Ccf.QuickStarts.QsAutomationProject
{
	partial class ProcessFavoriteColor
	{
		#region Designer generated code
		
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
		private void InitializeComponent()
		{
			this.CanModifyActivities = true;
			System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference1 = new System.Workflow.Activities.Rules.RuleConditionReference();
			System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference2 = new System.Workflow.Activities.Rules.RuleConditionReference();
			System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference3 = new System.Workflow.Activities.Rules.RuleConditionReference();
			System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference4 = new System.Workflow.Activities.Rules.RuleConditionReference();
			System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference5 = new System.Workflow.Activities.Rules.RuleConditionReference();
			System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference6 = new System.Workflow.Activities.Rules.RuleConditionReference();
			System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference7 = new System.Workflow.Activities.Rules.RuleConditionReference();
			System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
			this.codeActivity7 = new System.Workflow.Activities.CodeActivity();
			this.codeActivity6 = new System.Workflow.Activities.CodeActivity();
			this.codeActivity5 = new System.Workflow.Activities.CodeActivity();
			this.codeActivity4 = new System.Workflow.Activities.CodeActivity();
			this.codeActivity3 = new System.Workflow.Activities.CodeActivity();
			this.codeActivity2 = new System.Workflow.Activities.CodeActivity();
			this.codeActivity1 = new System.Workflow.Activities.CodeActivity();
			this.ifIsYellow = new System.Workflow.Activities.IfElseBranchActivity();
			this.ifIsWhite = new System.Workflow.Activities.IfElseBranchActivity();
			this.ifIsRed = new System.Workflow.Activities.IfElseBranchActivity();
			this.ifIsPurple = new System.Workflow.Activities.IfElseBranchActivity();
			this.ifIsGreen = new System.Workflow.Activities.IfElseBranchActivity();
			this.ifIsBlue = new System.Workflow.Activities.IfElseBranchActivity();
			this.ifIsBlack = new System.Workflow.Activities.IfElseBranchActivity();
			this.executeControlAction1 = new Microsoft.Ccf.HostedApplicationToolkit.Activity.ExecuteControlAction();
			this.setControlValue1 = new Microsoft.Ccf.HostedApplicationToolkit.Activity.SetControlValue();
			this.ifElseActivity1 = new System.Workflow.Activities.IfElseActivity();
			this.getControlValue1 = new Microsoft.Ccf.HostedApplicationToolkit.Activity.GetControlValue();
			// 
			// codeActivity7
			// 
			this.codeActivity7.Name = "codeActivity7";
			this.codeActivity7.ExecuteCode += new System.EventHandler(this.ExecuteCode4Yellow);
			// 
			// codeActivity6
			// 
			this.codeActivity6.Name = "codeActivity6";
			this.codeActivity6.ExecuteCode += new System.EventHandler(this.ExecuteCode4White);
			// 
			// codeActivity5
			// 
			this.codeActivity5.Name = "codeActivity5";
			this.codeActivity5.ExecuteCode += new System.EventHandler(this.ExecuteCode4Red);
			// 
			// codeActivity4
			// 
			this.codeActivity4.Name = "codeActivity4";
			this.codeActivity4.ExecuteCode += new System.EventHandler(this.ExecuteCode4Purple);
			// 
			// codeActivity3
			// 
			this.codeActivity3.Name = "codeActivity3";
			this.codeActivity3.ExecuteCode += new System.EventHandler(this.ExecuteCode4Green);
			// 
			// codeActivity2
			// 
			this.codeActivity2.Name = "codeActivity2";
			this.codeActivity2.ExecuteCode += new System.EventHandler(this.ExecuteCode4Blue);
			// 
			// codeActivity1
			// 
			this.codeActivity1.Name = "codeActivity1";
			this.codeActivity1.ExecuteCode += new System.EventHandler(this.ExecuteCode4Black);
			// 
			// ifIsYellow
			// 
			this.ifIsYellow.Activities.Add(this.codeActivity7);
			ruleconditionreference1.ConditionName = "IsYellow";
			this.ifIsYellow.Condition = ruleconditionreference1;
			this.ifIsYellow.Name = "ifIsYellow";
			// 
			// ifIsWhite
			// 
			this.ifIsWhite.Activities.Add(this.codeActivity6);
			ruleconditionreference2.ConditionName = "IsWhite";
			this.ifIsWhite.Condition = ruleconditionreference2;
			this.ifIsWhite.Name = "ifIsWhite";
			// 
			// ifIsRed
			// 
			this.ifIsRed.Activities.Add(this.codeActivity5);
			ruleconditionreference3.ConditionName = "IsRed";
			this.ifIsRed.Condition = ruleconditionreference3;
			this.ifIsRed.Name = "ifIsRed";
			// 
			// ifIsPurple
			// 
			this.ifIsPurple.Activities.Add(this.codeActivity4);
			ruleconditionreference4.ConditionName = "IsPurple";
			this.ifIsPurple.Condition = ruleconditionreference4;
			this.ifIsPurple.Name = "ifIsPurple";
			// 
			// ifIsGreen
			// 
			this.ifIsGreen.Activities.Add(this.codeActivity3);
			ruleconditionreference5.ConditionName = "IsGreen";
			this.ifIsGreen.Condition = ruleconditionreference5;
			this.ifIsGreen.Name = "ifIsGreen";
			// 
			// ifIsBlue
			// 
			this.ifIsBlue.Activities.Add(this.codeActivity2);
			ruleconditionreference6.ConditionName = "IsBlue";
			this.ifIsBlue.Condition = ruleconditionreference6;
			this.ifIsBlue.Name = "ifIsBlue";
			// 
			// ifIsBlack
			// 
			this.ifIsBlack.Activities.Add(this.codeActivity1);
			ruleconditionreference7.ConditionName = "IsBlack";
			this.ifIsBlack.Condition = ruleconditionreference7;
			this.ifIsBlack.Name = "ifIsBlack";
			// 
			// executeControlAction1
			// 
			this.executeControlAction1.ApplicationName = "StandaloneTestApp";
			this.executeControlAction1.ControlName = "button_acc";
			this.executeControlAction1.Name = "executeControlAction1";
			// 
			// setControlValue1
			// 
			this.setControlValue1.ApplicationName = "StandaloneTestApp";
			this.setControlValue1.ControlName = "textbox_acc";
			activitybind1.Name = "ProcessFavoriteColor";
			activitybind1.Path = "generatedPhrase";
			this.setControlValue1.Name = "setControlValue1";
			this.setControlValue1.SetBinding(Microsoft.Ccf.HostedApplicationToolkit.Activity.SetControlValue.ControlValueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
			// 
			// ifElseActivity1
			// 
			this.ifElseActivity1.Activities.Add(this.ifIsBlack);
			this.ifElseActivity1.Activities.Add(this.ifIsBlue);
			this.ifElseActivity1.Activities.Add(this.ifIsGreen);
			this.ifElseActivity1.Activities.Add(this.ifIsPurple);
			this.ifElseActivity1.Activities.Add(this.ifIsRed);
			this.ifElseActivity1.Activities.Add(this.ifIsWhite);
			this.ifElseActivity1.Activities.Add(this.ifIsYellow);
			this.ifElseActivity1.Name = "ifElseActivity1";
			// 
			// getControlValue1
			// 
			this.getControlValue1.ApplicationName = "Sample Web App";
			this.getControlValue1.ControlName = "favorite color";
			this.getControlValue1.Name = "getControlValue1";
			// 
			// ProcessFavoriteColor
			// 
			this.Activities.Add(this.getControlValue1);
			this.Activities.Add(this.ifElseActivity1);
			this.Activities.Add(this.setControlValue1);
			this.Activities.Add(this.executeControlAction1);
			this.Name = "ProcessFavoriteColor";
			this.CanModifyActivities = false;

		}

		#endregion

		private CodeActivity codeActivity7;
		private CodeActivity codeActivity6;
		private CodeActivity codeActivity5;
		private CodeActivity codeActivity4;
		private CodeActivity codeActivity3;
		private CodeActivity codeActivity2;
		private CodeActivity codeActivity1;
		private Microsoft.Ccf.HostedApplicationToolkit.Activity.ExecuteControlAction executeControlAction1;
		private IfElseBranchActivity ifIsBlue;
		private IfElseBranchActivity ifIsBlack;
		private IfElseActivity ifElseActivity1;
		private IfElseBranchActivity ifIsGreen;
		private IfElseBranchActivity ifIsYellow;
		private IfElseBranchActivity ifIsWhite;
		private IfElseBranchActivity ifIsRed;
		private IfElseBranchActivity ifIsPurple;
		private Microsoft.Ccf.HostedApplicationToolkit.Activity.SetControlValue setControlValue1;
		private Microsoft.Ccf.HostedApplicationToolkit.Activity.GetControlValue getControlValue1;










































	}
}
